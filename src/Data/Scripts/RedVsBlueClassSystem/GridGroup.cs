using Sandbox.Common.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Game.ModAPI.Network;
using VRage.Network;
using Sandbox.ModAPI;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using ProtoBuf;

namespace RedVsBlueClassSystem
{
    public class GridGroup
    {
        
        private static Queue<GridGroup> ToBeCheckedOnServerQueue = new Queue<GridGroup>();
        //private static GridsPerFactionClassManager gridsPerFactionClassManager = new GridsPerFactionClassManager(ModSessionManager.Instance.Config);

        private long _GridClassId;

        public CubeGridLogicComponent Master;
        public List<CubeGridLogicComponent> AllGrids = new List<CubeGridLogicComponent>();
        public long GridClassId { get { return _GridClassId; } }


        private bool _IsServerGridClassDirty = false;
        public bool IsServerGridClassDirty {
            get { return _IsServerGridClassDirty; }

            protected set {
                if (value != _IsServerGridClassDirty) {
                    if (value)
                    {
                        ToBeCheckedOnServerQueue.Enqueue(this);
                    }

                    _IsServerGridClassDirty = value;
                }
            } }

        private bool IsOwnerDirty = true;

        private IMyFaction _OwningFaction = null;
        public IMyFaction OwningFaction { get {
                if (IsOwnerDirty) {
                    _OwningFaction = GetOwningFaction();
                    IsOwnerDirty = false;
                }

                return _OwningFaction;
            } }

        private bool _isClientGridClassCheckDirty = true;

        private DetailedGridClassCheckResult _detailedGridClassCheckResult;
        public DetailedGridClassCheckResult DetailedGridClassCheckResult { get {
                if (_isClientGridClassCheckDirty) {
                    _detailedGridClassCheckResult = GridClass?.CheckGridGroupIsValid(this);
                    _isClientGridClassCheckDirty = false;
                }

                return _detailedGridClassCheckResult;
            } }

        public GridClass GridClass { get { return ModSessionManager.GetGridClassById(GridClassId); } }
        public long MasterEntityId { get { return Master.Entity.EntityId; } }
        public bool GridGroupMeetsGridClassRestrictions { get { return Master != null ? Master.GridCheckResults.CheckPassedForGridClass(GridClass) : false; } }
        public GridModifiers Modifiers { get
            {
                return GridGroupMeetsGridClassRestrictions ? GridClass.Modifiers : ModSessionManager.GetGridClassById(0).Modifiers;
            } }

        private IMyGridGroupData GroupData;
        
        public GridGroup(IMyGridGroupData groupData)
        {
            

            GroupData = groupData;

            //init event listeners
            groupData.OnReleased += GroupData_OnReleased;
            groupData.OnGridAdded += GroupData_OnGridAdded;
            groupData.OnGridRemoved += GroupData_OnGridRemoved;

            //record link to this object in the grid group
            groupData.SetVariable(Constants.GridGroupVarGUID, this);

            //init grids
            var allGridsInGroup = new List<IMyCubeGrid>();
            groupData.GetGrids(allGridsInGroup);

            Utils.WriteToClient($"GridGroup: created (type = {groupData.LinkType.ToString()}, girds# = {allGridsInGroup.Count})");

            foreach (var grid in allGridsInGroup)
            {
                AddGrid(grid);
            }
        }

        private void GroupData_OnGridRemoved(IMyGridGroupData groupData, IMyCubeGrid grid, IMyGridGroupData newGroupData)
        {
            Utils.WriteToClient($"GroupData_OnGridRemoved: {grid.CustomName}");

            if (groupData != GroupData)
            {
                Utils.WriteToClient($"different group data?!");
            }

            if (newGroupData == GroupData)
            {
                Utils.WriteToClient($"to the same group?!");
            }

            if (newGroupData == null)
            {
                Utils.WriteToClient($"no new group?!");
            }

            RemoveGrid(grid);

            if (newGroupData != null)
            {
                GetGridGroupFor(newGroupData);
            }
        }

        private void GroupData_OnGridAdded(IMyGridGroupData groupData, IMyCubeGrid grid, IMyGridGroupData oldGroupData)
        {
            Utils.WriteToClient($"GroupData_OnGridAdded: {grid.CustomName}");

            if (groupData != GroupData) {
                Utils.WriteToClient($"different group data?!");
            }

            if(oldGroupData == GroupData)
            {
                Utils.WriteToClient($"from the same group?!");
            }

            if (oldGroupData == null) {
                Utils.WriteToClient($"no old group?!");
            }

            AddGrid(grid);
        }

        private void GroupData_OnReleased(IMyGridGroupData groupData)
        {
            Utils.WriteToClient($"GroupData_OnReleased");
            
            if(GroupData == groupData)
            {
                groupData.OnReleased -= GroupData_OnReleased;
                groupData.OnGridAdded -= GroupData_OnGridAdded;
                groupData.OnGridRemoved -= GroupData_OnGridRemoved;

                GroupData = null;
            } else
            {
                Utils.WriteToClient($"Wrong groupdata?!");
            }
        }

        public void AddGrid(IMyCubeGrid grid)
        {
            Utils.WriteToClient($"GridGroup:AddGrid() {grid?.CustomName}");
            if (grid == null || grid.Physics == null)
            {
                Utils.WriteToClient($"Ignoring grid (non-physical?)");
                return;
            }

            var cubeGridLogicComponent = grid.GetCubeGridLogicComponent();
            if(cubeGridLogicComponent == null)
            {
                throw new ArgumentException("GridGroup:AddGrid() grid does not have a cubeGridLogicComponent");
            }

            if(AllGrids.Contains(cubeGridLogicComponent))
            {
                return;//ignore already added grid
            }

            cubeGridLogicComponent.GridGroup = this;

            AllGrids.Add(cubeGridLogicComponent);

            UpdateMaster();

            bool isMaster = cubeGridLogicComponent == Master;
            cubeGridLogicComponent.IsMaster = isMaster;

            //event handlers
            cubeGridLogicComponent.GridClassSync.ValueChanged += OnGridClassChanged;
            cubeGridLogicComponent.GridCheckResultsSync.ValueChanged += GridCheckResultsSync_ValueChanged;

            cubeGridLogicComponent.Grid.OnBlockOwnershipChanged += OnBlockOwnershipChanged;
            cubeGridLogicComponent.Grid.OnIsStaticChanged += Grid_OnIsStaticChanged;

            if (Constants.IsClient)
            {
                _isClientGridClassCheckDirty = true;
                grid.OnBlockAdded += ClientOnBlockChanged;
                grid.OnBlockRemoved += ClientOnBlockChanged;
            }

            if (Constants.IsServer)
            {
                IsServerGridClassDirty = true;

                grid.OnBlockAdded += ServerOnBlockAdded;
                grid.OnBlockRemoved += ServerOnBlockRemoved;

                //TODO which grids to do this for?
                //If server, load persisted grid class
                /*if(isMaster)//is this correct? Probably not...
                {
                    //Load persisted grid class id from storage
                    long gridClassId = cubeGridLogicComponent.GetGridClassIdFromStorage();

                    //If not found, check beacons
                    if (gridClassId <= 0)
                    {
                        gridClassId = GetGridClassIdFromBeacons();
                    }

                    cubeGridLogicComponent.GridClassSync.Value = gridClassId;
                }*/
            }
        }

        public void RemoveGrid(IMyCubeGrid grid)
        {
            Utils.WriteToClient($"GridGroup:RemoveGrid() {grid?.CustomName}");

            if (grid == null || grid.Physics == null)
            {
                Utils.WriteToClient($"Ignoring grid (non-physical?)");
                return;
            }

            var cubeGridLogicComponent = grid.GetCubeGridLogicComponent();

            if (cubeGridLogicComponent == null)
            {
                throw new ArgumentException("GridGroup:AddGrid() grid does not have a cubeGridLogicComponent");
            }

            if (!AllGrids.Contains(cubeGridLogicComponent))
            {
                Utils.Log($"GridGroup:RemoveGrid() grid \"{grid.CustomName}\" not part of group?");
                return;//ignore grid not part of group
            }
            
            bool isMaster = Master == cubeGridLogicComponent;
            AllGrids.Remove(cubeGridLogicComponent);

            if(isMaster)
            {
                UpdateMaster();
            }

            cubeGridLogicComponent.GridClassSync.ValueChanged -= OnGridClassChanged;
            cubeGridLogicComponent.GridCheckResultsSync.ValueChanged -= GridCheckResultsSync_ValueChanged;

            cubeGridLogicComponent.Grid.OnBlockOwnershipChanged -= OnBlockOwnershipChanged;
            cubeGridLogicComponent.Grid.OnIsStaticChanged -= Grid_OnIsStaticChanged;

            if (Constants.IsClient)
            {
                _isClientGridClassCheckDirty = true;
                grid.OnBlockAdded -= ClientOnBlockChanged;
                grid.OnBlockRemoved -= ClientOnBlockChanged;
            }

            if (Constants.IsServer)
            {
                IsServerGridClassDirty = true;

                grid.OnBlockAdded -= ServerOnBlockAdded;
                grid.OnBlockRemoved -= ServerOnBlockRemoved;
            }

            if(cubeGridLogicComponent.GridGroup == this)
            {
                cubeGridLogicComponent.GridGroup = null;
            }
        }

        public static GridGroup GetGridGroupFor(IMyGridGroupData groupData)
        {
            var gridGroup = groupData.GetVariable<GridGroup>(Constants.GridGroupVarGUID);

            if (gridGroup == null)
            {
                gridGroup = new GridGroup(groupData);
            }

            return gridGroup;
        }

        public static GridGroup GetGridGroupFor(IMyCubeGrid grid)
        {
            return GetGridGroupFor(grid.GetGridGroup(GridLinkTypeEnum.Mechanical));
        }

        public void CheckGridGroupClassLimits()
        {
            if (Constants.IsServer)
            {
                IsServerGridClassDirty = false;

                if (Master == null)
                {
                    Utils.Log("Missing master CubeGridLogicComponent");
                    return;
                }

                if (Master.Grid == null)
                {
                    Utils.Log("Missing master MyCubeGrid");
                    return;
                }

                var gridClass = GridClass;

                if (gridClass == null) {
                    Utils.Log("Missing grid class");
                    return;
                }

                if (Master.GridCheckResultsSync == null)
                {
                    Utils.Log("Missing grid grid check results sync");
                    return;
                }

                var checkResult = gridClass.CheckGridGroupIsValid(this);
                Master.GridCheckResultsSync.Value = SimpleGridGroupCheckResult.FromDetailedGridClassCheckResult(checkResult, gridClass.Id);
            }
        }

        public List<T> GetFatBlocks<T>() where T : class, IMyCubeBlock
        {
            var allFatBlocks = new List<T>();

            foreach (var cubeGridLogicComponent in AllGrids)
            {
                allFatBlocks.AddRange(cubeGridLogicComponent.Grid.GetFatBlocks<T>());
            }

            return allFatBlocks;
        }

        private IEnumerable<IMyBeacon> GetBeacons()
        {
            return GetFatBlocks<IMyBeacon>();
        }

        private long GetGridClassIdFromBeacons()
        {
            foreach (var beacon in GetBeacons())
            {
                long gridClassId;
                //Utils.WriteToClient($"beacon custom data = {beacon.CustomData}");
                if (!string.IsNullOrEmpty(beacon.CustomData) && long.TryParse(beacon.CustomData, out gridClassId))
                {
                    //Utils.WriteToClient($"beacon grid class id = {gridClassId}");
                    if (ModSessionManager.Instance.Config.IsValidGridClassId(gridClassId))
                    {
                        return gridClassId;
                    }
                }
            }

            return 0;
        }

        private void ApplyModifiers()
        {
            Utils.Log($"Applying modifiers {Modifiers}");

            foreach (var block in GetFatBlocks<IMyTerminalBlock>())
            {
                CubeGridModifiers.ApplyModifiers(block, Modifiers);
            }
        }

        private IMyFaction GetOwningFaction()
        {
            var Grid = Master?.Grid;

            if(Grid == null)
            {
                return null;
            } 

            if (Grid.BigOwners.Count == 0)
            {
                return null;
            }

            if (Grid.BigOwners.Count == 1)
            {
                return MyAPIGateway.Session.Factions.TryGetPlayerFaction(Grid.BigOwners[0]);
            }

            var ownersPerFaction = new Dictionary<IMyFaction, int>();

            //Find the faction with the most owners
            foreach (var owner in Grid.BigOwners)
            {
                var OwnerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(Grid.BigOwners[0]);
                
                if (OwnerFaction != null)
                {
                    if (!ownersPerFaction.ContainsKey(OwnerFaction))
                    {
                        ownersPerFaction[OwnerFaction] = 1;
                    }
                    else
                    {
                        ownersPerFaction[OwnerFaction]++;
                    }
                }
            }

            if (ownersPerFaction.Count == 0)
            {
                return null;
            }

            //new select the faction with the most owners
            return ownersPerFaction.MaxBy(kvp => kvp.Value).Key;
        }

        //Event handlers
        private void ClientOnBlockChanged(IMySlimBlock obj)
        {
            _isClientGridClassCheckDirty = true;
        }

        private void Grid_OnIsStaticChanged(IMyCubeGrid arg1, bool arg2)
        {
            if (Constants.IsServer)
            {
                IsServerGridClassDirty = true;//need to trigger a recheck of grid class
            }

            if (Constants.IsClient)
            {
                _isClientGridClassCheckDirty = true;
            }
        }

        private void OnGridClassChanged(MySync<long, SyncDirection.FromServer> newGridClassId)
        {
            Utils.Log($"CubeGridLogic::OnGridClassChanged: new grid class id = {newGridClassId}, Grid = {Master.Grid.DisplayName}", 2);

            ApplyModifiers();

            if (Constants.IsServer)
            {
                IsServerGridClassDirty = true;

                foreach(var cubeGridLogicComponent in AllGrids)
                {
                    cubeGridLogicComponent.Entity.Storage[Constants.GridClassStorageGUID] = newGridClassId.ToString();
                }

                foreach(var beacon in GetBeacons())
                {
                    beacon.CustomData = newGridClassId.ToString();
                }
            }

            if (Constants.IsClient)
            {
                _isClientGridClassCheckDirty = true;
            }
        }

        private void GridCheckResultsSync_ValueChanged(MySync<SimpleGridGroupCheckResult, SyncDirection.FromServer> obj)
        {
            //Utils.WriteToClient($"GridCheck results = {obj.Value}");

            ApplyModifiers();
        }

        private void ServerOnBlockAdded(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;

            if (fatBlock != null)
            {
                //Utils.WriteToClient($"Added block TypeId = {Utils.GetBlockId(fatBlock)}, Subtype = {Utils.GetBlockSubtypeId(fatBlock)}");

                CubeGridModifiers.ApplyModifiers(fatBlock, Modifiers);
            }

            IsServerGridClassDirty = true;
            IsOwnerDirty = true;
        }

        private void ServerOnBlockRemoved(IMySlimBlock obj)
        {
            IsServerGridClassDirty = true;
            IsOwnerDirty = true;
        }

        private void OnBlockOwnershipChanged(IMyCubeGrid obj)
        {
            IsOwnerDirty = true;
        }

        private void UpdateMaster()
        {
            Utils.Log($"GridGroup:UpdateMaster", 3);

            var newMaster = GetMasterGrid();

            if (newMaster != null && newMaster == Master)
            {
                //Master hasn't changed
                return;
            }

            Utils.Log($"GridGroup:UpdateMaster switching from {Master?.Grid?.CustomName ?? "<none>"} to {newMaster?.GridName ?? "<none>"}", 3);

            var oldMaster = Master;

            if(oldMaster != null && oldMaster.GridGroup == this)
            {
                oldMaster.IsMaster = false;
            }

            if(newMaster != null)
            {
                newMaster.IsMaster = true;
                Master = newMaster;
            }
            else
            {
                Master = null;
            }
        }

        private CubeGridLogicComponent GetMasterGrid()
        {
            var grids = AllGrids;
            Utils.WriteToClient($"GetMasterGrid: num grids: {grids.Count}");
            int biggestGridBlockCount = -1;
            CubeGridLogicComponent biggestGrid = null;

            foreach(var grid in grids)
            {
                if(grid.BlocksCount > biggestGridBlockCount)
                {
                    biggestGrid = grid;
                    biggestGridBlockCount = grid.BlocksCount;
                }
            }

            return biggestGrid;
        }

        public static List<GridGroup> GetGridsToBeChecked(int max)
        {
            if (!Constants.IsServer)
            {
                throw new Exception("This method should only be called on the server");
            }

            var output = new List<GridGroup>();

            while (ToBeCheckedOnServerQueue.Count > 0 && output.Count < max)
            {
                var gridGroup = ToBeCheckedOnServerQueue.Dequeue();
                gridGroup.IsServerGridClassDirty = false;

                if (gridGroup.Master != null)
                {
                    output.Add(gridGroup);
                }

            }

            return output;
        }

        


        //TODO update this
        /*public static void UpdateGridsPerFactionClass()
        {
            gridsPerFactionClassManager.Reset();

            foreach(var gridGroup in AllGridGroups)
            {
                gridsPerFactionClassManager.AddCubeGrid(gridLogic);
            }
        }*/

        /*public static IMyCubeGrid GetMainCubeGrid(IMyCubeGrid grid)
        {
            var grids = GetConnectedPhysicalGrids(grid);

            return grids.OfType<MyCubeGrid>().MaxBy(concrete => concrete.BlocksCount);
        }

        public static IMyCubeGrid GetMainCubeGrid(IMyCubeGrid grid, out List<IMyCubeGrid> subgrids)
        {
            var grids = GetConnectedPhysicalGrids(grid);

            if (!grids.Any())
            {
                subgrids = grids;
                return grid;
            }

            var biggestGrid = grids.OfType<MyCubeGrid>().MaxBy(concrete => concrete.BlocksCount);
            subgrids = grids.Where(g => g.EntityId != biggestGrid.EntityId).ToList();

            return biggestGrid;
        }

        public static List<IMyCubeGrid> GetConnectedPhysicalGrids(IMyCubeGrid grid)
        {
            var group = grid.GetGridGroup(GridLinkTypeEnum.Mechanical);
            var grids = new List<IMyCubeGrid>();

            group?.GetGrids(grids);
            return grids.Where(g => g?.Physics != null).ToList();
        }*/


    }

    

    //MainGrid.GridGeneralDamageModifier.ValidateAndSet(0.75f);
}
