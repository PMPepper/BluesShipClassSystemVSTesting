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

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_CubeGrid), false)]
    public class CubeGridLogic : MyGameLogicComponent, IMyEventProxy
    {
        private static Queue<CubeGridLogic> ToBeCheckedQueue = new Queue<CubeGridLogic>();

        private IMyCubeGrid Grid;

        private MySync<long, SyncDirection.BothWays> ShipClassSync = null;
        private MySync<GridCheckResults, SyncDirection.FromServer> GridCheckResultsSync = null;

        private bool _IsDirty = false;
        public bool IsDirty { get { return _IsDirty; } protected set { if (value != _IsDirty) {
                    if(value)
                    {
                        ToBeCheckedQueue.Enqueue(this);
                    }

                    _IsDirty = value;
        } } }

        public long ShipClassId { get { return ShipClassSync.Value; } set { ShipClassSync.Value = value; } }//TODO add validation logic in setter?
        public GridCheckResults GridCheckResults { get { return GridCheckResultsSync.Value; } }
        public ShipClass ShipClass {get { return ModSessionManager.GetShipClassById(ShipClassId); } }
        public bool GridMeetsShipClassRestrictions { get { return GridCheckResults.CheckPassedForShipClass(ShipClass); } }
        public GridModifiers Modifiers { get
            {
                return GridMeetsShipClassRestrictions ? ShipClass.Modifiers : ModSessionManager.GetShipClassById(0).Modifiers;
            } }

        public bool IsApplicableGrid { get {
                return Grid?.Physics != null && Grid is MyCubeGrid && ((MyCubeGrid)Grid).BlocksCount >= 3; 
        } }

        /*public long PrimaryOwnerId
        {
            get
            {
                if (Grid.BigOwners.Count == 0)
                {
                    return -1;
                }

                if (Grid.BigOwners.Count == 1)
                {
                    return Grid.BigOwners[0];
                }

                string owningFactionTag = OwningFactionTag;

                if(owningFactionTag == null)
                {
                    return -1;
                }

                foreach(var ownerId in Grid.BigOwners)
                {
                    var ownerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(ownerId);

                    if(ownerFaction?.Tag == owningFactionTag)
                    {
                        return ownerId;
                    }
                }

                return -1;
            }
        }*/

        public IMyFaction OwningFaction { get {
                if(Grid.BigOwners.Count == 0)
                {
                    return null;
                }

                if(Grid.BigOwners.Count == 1)
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

                if(ownersPerFaction.Count == 0)
                {
                    return null;
                }

                //new select the faction with the most owners
                return ownersPerFaction.MaxBy(kvp => kvp.Value).Key;
            } }

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            // the base methods are usually empty, except for OnAddedToContainer()'s, which has some sync stuff making it required to be called.
            base.Init(objectBuilder);

            Grid = (IMyCubeGrid)Entity;

            //Utils.Log($"[CubeGridLogic] Init EntityId = {Grid.EntityId}");
            
            // makes UpdateOnceBeforeFrame() execute.
            // this is a special flag that gets self-removed after the method is called.
            // it can be used multiple times but mind that there is overhead to setting this so avoid using it for continuous updates.
            // We need to wait until the first update to check if this is a physical grid that needs initing or not
            NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            base.UpdateOnceBeforeFrame();

            //Utils.Log("[CubeGridLogic] FirstUpdate");

            if (Grid?.Physics == null) // ignore projected and other non-physical grids
            {
                //Utils.Log("[CubeGridLogic] FirstUpdate: ignore non-physical grid");
                return;
            }

            //Init event handlers
            ShipClassSync.ValueChanged += ShipClassSync_ValueChanged;
            GridCheckResultsSync.ValueChanged += GridCheckResultsSync_ValueChanged;

            Grid.OnBlockAdded += Grid_OnBlockAdded;
            Grid.OnBlockRemoved += Grid_OnBlockRemoved;
            //Grid.OnBlockOwnershipChanged += Grid_OnBlockOwnershipChanged;

            if (Entity.Storage == null)
            {
                Entity.Storage = new MyModStorageComponent();
            }

            //If server, init persistant storage + apply ship class
            if (Constants.IsServer)
            {
                //Load persisted ship class id from storage (if server)
                if (Entity.Storage.ContainsKey(Constants.ShipClassStorageGUID))
                {
                    long shipClassId = 0;

                    try
                    {
                        shipClassId = long.Parse(Entity.Storage[Constants.ShipClassStorageGUID]);
                    }
                    catch (Exception e)
                    {
                        string msg = $"[CubeGridLogic] Error parsing serialised ShipClassId: {Entity.Storage[Constants.ShipClassStorageGUID]}, EntityId = {Grid.EntityId}";

                        Utils.Log(msg, 1);
                        Utils.Log(e.Message, 1);
                    }

                    //TODO validate shipClassId
                    Utils.Log($"[CubeGridLogic] Assigning ShipClassId = {shipClassId}");
                    ShipClassSync.Value = shipClassId;
                }
            }
            
            ApplyModifiers();

            IsDirty = true;

            // NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            // NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
            // NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void MarkForClose()
        {
            base.MarkForClose();

            // called when entity is about to be removed for whatever reason (block destroyed, entity deleted, ship despawn because of sync range, etc)
        }

        // less commonly used methods:

        public override bool IsSerialized()
        {
            // executed when the entity gets serialized (saved, blueprinted, streamed, etc) and asks all
            //   its components whether to be serialized too or not (calling GetObjectBuilder())
            if (Grid?.Physics != null)
            {
                if (Constants.IsServer)
                {
                    try
                    {
                        // serialise state here
                        Entity.Storage[Constants.ShipClassStorageGUID] = ShipClassId.ToString();
                    }
                    catch (Exception e)
                    {
                        Utils.Log($"Error serialising CubeGridLogic, {e.Message}");
                    }

                }
            }

            // you cannot add custom OBs to the game so this should always return the base (which currently is always false).
            return base.IsSerialized();
        }

        /*public override void UpdatingStopped()
        {
            base.UpdatingStopped();

            // only called when game is paused.
        }*/

        public void CheckGridLimits()
        {
            //Mark as not dirty
            IsDirty = false;

            if (Constants.IsServer)
            {
                var grid = Grid as MyCubeGrid;
                var shipClass = ShipClass;

                if (shipClass == null) {
                    Utils.Log("Missing ship class");
                    return;
                }

                if (grid == null)
                {
                    Utils.Log("Missing ship grid");
                    return;
                }

                if (GridCheckResultsSync == null)
                {
                    Utils.Log("Missing ship grid check results sync");
                    return;
                }

                var checkResult = shipClass.CheckGrid(grid);
                GridCheckResultsSync.Value = GridCheckResults.FromShipClassCheckResult(checkResult, shipClass.Id);
            }
        }

        private void ApplyModifiers()
        {
            Utils.Log($"Applying modifiers {Modifiers}");

            foreach (var block in Grid.GetFatBlocks<IMyTerminalBlock>())
            {
                CubeGridModifiers.ApplyModifiers(block, Modifiers);
            }
        }

        //Event handlers

        private void ShipClassSync_ValueChanged(MySync<long, SyncDirection.BothWays> newShipClassId)
        {
            Utils.Log($"ShipClassSync_ValueChanged {newShipClassId}");

            ApplyModifiers();

            IsDirty = true;

            /*if (MyAPIGateway.Session.OnlineMode != VRage.Game.MyOnlineModeEnum.OFFLINE && MyAPIGateway.Session.IsServer)
                MyAPIGateway.Utilities.SendMessage($"Synced server value on server: {obj.Value}");
            else
                MyAPIGateway.Utilities.ShowMessage("Test", $"Synced server value on client: {obj.Value}");*/
        }

        private void GridCheckResultsSync_ValueChanged(MySync<GridCheckResults, SyncDirection.FromServer> obj)
        {
            Utils.WriteToClient($"GridCheck results = {obj.Value}");

            ApplyModifiers();
        }

        private void Grid_OnBlockAdded(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;
            

            if(fatBlock != null)
            {
                Utils.WriteToClient($"Added block TypeId = {Utils.GetBlockId(fatBlock)}");

                CubeGridModifiers.ApplyModifiers(fatBlock, Modifiers);
            }

            IsDirty = true;
        }

        private void Grid_OnBlockRemoved(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;

            IsDirty = true;
        }

        /*private void Grid_OnBlockOwnershipChanged(IMyCubeGrid obj)
        {
            //TODO
        }*/

        public static List<CubeGridLogic> GetGridsToBeChecked(int max)
        {
            var output = new List<CubeGridLogic>();

            while(ToBeCheckedQueue.Count > 0 && output.Count < max)
            {
                var grid = ToBeCheckedQueue.Dequeue();
                grid.IsDirty = false;

                if(!grid.MarkedForClose)
                {
                    output.Add(grid);
                }
                
            }

            return output;
        }
    }

    [ProtoContract]
    public struct GridCheckResults
    {
        [ProtoMember(1)]
        public bool MaxBlocks;
        [ProtoMember(2)]
        public bool MaxPCU;
        [ProtoMember(3)]
        public bool MaxMass;
        [ProtoMember(4)]
        public ulong BlockLimits;
        [ProtoMember(5)]
        public long ShipClassId;

        public bool CheckPassedForShipClass(ShipClass shipClass) {
            if(shipClass == null)
            {
                return false;
            }

            if(shipClass.Id == 0)
            {
                return true;//default/unknown ship class always passes
            }

            if(shipClass.Id != ShipClassId)
            {
                return false;//this GridCheckResult is for a different ship class, so always fails
            }

            if(!MaxBlocks || !MaxPCU || !MaxMass)
            {
                return false;
            }

            if(shipClass.BlockLimits != null && shipClass.BlockLimits.Length > 0)
            {
                ulong blockLimitPassedTarget = (1UL << shipClass.BlockLimits.Length) - 1;

                return BlockLimits == blockLimitPassedTarget;
            }
            
            return true;
        }

        public override string ToString()
        {
            return $"[GridCheckResults ShipClassId={ShipClassId} MaxBlocks={MaxBlocks} MaxPCU={MaxPCU} MaxMass={MaxMass} BlockLimits={BlockLimits} ]";
        }

        public static GridCheckResults FromShipClassCheckResult(ShipClassCheckResult result, long shipClassId)
        {
            ulong BlockLimits = 0;

            if(result.BlockLimits != null)
            {
                for(int i = 0; i < result.BlockLimits.Length; i++)
                {
                    if(result.BlockLimits[i] != null && result.BlockLimits[i].Passed)
                    {
                        BlockLimits += 1UL << i;
                    }
                }
            }

            return new GridCheckResults() { MaxBlocks = result.MaxBlocks.Passed, MaxPCU = result.MaxPCU.Passed, MaxMass = result.MaxMass.Passed, BlockLimits = BlockLimits, ShipClassId = shipClassId };
        }
    }
}
