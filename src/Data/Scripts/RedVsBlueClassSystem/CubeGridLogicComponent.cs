using RedVsBlueClassSystem;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Network;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;

namespace RedVsBlueClassSystem
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_CubeGrid), false)]
    public class CubeGridLogicComponent : MyGameLogicComponent, IMyEventProxy
    {
        public bool IsMaster;
        public IMyCubeGrid Grid;
        public GridGroup GridGroup;

        //Synced values
        public MySync<long, SyncDirection.FromServer> GridClassSync = null;

        public long GridClassId
        {
            get { return GridClassSync.Value; }
            set
            {
                //TODO is this a good idea?
                if (!Constants.IsServer)
                {
                    throw new Exception("CubeGridLgic:: set GridClassId: Grid class Id can only be set on the server");
                }

                if (!ModSessionManager.IsValidGridClass(value))
                {
                    throw new Exception($"CubeGridLgic:: set GridClassId: invalid grid class id {value}");
                }

                Utils.Log($"CubeGridLogic::GridClassId setting grid class to {value}", 1);

                GridClassSync.Value = value;
            }
        }

        public MySync<SimpleGridGroupCheckResult, SyncDirection.FromServer> GridCheckResultsSync = null;
        public SimpleGridGroupCheckResult GridCheckResults { get { return GridCheckResultsSync.Value; } }

        public int BlocksCount { get { return (Grid as MyCubeGrid)?.BlocksCount ?? 0; } }
        public string GridName { get { return Grid?.CustomName; } }

        //Static props
        private static Dictionary<long, CubeGridLogicComponent> CubeGridLogicComponentByEntityId = new Dictionary<long, CubeGridLogicComponent>();
        private static List<CubeGridLogicComponent> AllCubeGridLogicComponents = new List<CubeGridLogicComponent>();


        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            // the base methods are usually empty, except for OnAddedToContainer()'s, which has some sync stuff making it required to be called.
            base.Init(objectBuilder);

            Grid = (IMyCubeGrid)Entity;

            NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            base.UpdateOnceBeforeFrame();

            //Utils.Log("[CubeGridLogic] FirstUpdate");

            if (Grid?.Physics == null) // ignore projected and other non-physical grids
            {
                return;
            }

            AddCubeGridLogicComponent(this);

            if (Entity.Storage == null)
            {
                Entity.Storage = new MyModStorageComponent();
            }

            GridGroup = GridGroup.GetGridGroupFor(Grid);

            /*AddGridLogic(this);

            if (Entity.Storage == null)
            {
                Entity.Storage = new MyModStorageComponent();
            }

            //Init event handlers
            GridClassSync.ValueChanged += OnGridClassChanged;
            GridCheckResultsSync.ValueChanged += GridCheckResultsSync_ValueChanged;

            Grid.OnBlockOwnershipChanged += OnBlockOwnershipChanged;
            Grid.OnIsStaticChanged += Grid_OnIsStaticChanged;

            if (Constants.IsClient)
            {
                Grid.OnBlockAdded += ClientOnBlockChanged;
                Grid.OnBlockRemoved += ClientOnBlockChanged;
            }

            //If server, init persistant storage + apply grid class
            if (Constants.IsServer)
            {
                IsServerGridClassDirty = true;

                Grid.OnBlockAdded += ServerOnBlockAdded;
                Grid.OnBlockRemoved += ServerOnBlockRemoved;

                //Utils.WriteToClient($"Entity storage: {Entity.Storage[Constants.GridClassStorageGUID]}, {Grid.EntityId}");
                //Load persisted grid class id from storage (if server)
                if (Entity.Storage.ContainsKey(Constants.GridClassStorageGUID))
                {
                    long gridClassId = 0;

                    try
                    {
                        gridClassId = long.Parse(Entity.Storage[Constants.GridClassStorageGUID]);
                    }
                    catch (Exception e)
                    {
                        string msg = $"[CubeGridLogic] Error parsing serialised GridClassId: {Entity.Storage[Constants.GridClassStorageGUID]}, EntityId = {Grid.EntityId}, Name = {Grid.DisplayName}";
                        //Utils.WriteToClient(msg);
                        Utils.Log(msg, 1);
                        Utils.Log(e.Message, 1);
                    }

                    //TODO validate gridClassId
                    Utils.Log($"[CubeGridLogic] Assigning GridClassId = {gridClassId}, EntityId = {Grid.EntityId}, Name = {Grid.DisplayName}", 2);
                    GridClassSync.Value = gridClassId;
                }

                if (GridClassSync.Value <= 0)
                {
                    //Utils.WriteToClient($"[CubeGridLogic] Checking beacon GridClassId = {GridClassSync.Value}, EntityId = {Grid.EntityId}, Name = {Grid.DisplayName}");
                    foreach (var beacon in GetBeacons())
                    {
                        long gridClassId;
                        //Utils.WriteToClient($"beacon custom data = {beacon.CustomData}");
                        if (!string.IsNullOrEmpty(beacon.CustomData) && long.TryParse(beacon.CustomData, out gridClassId))
                        {
                            //Utils.WriteToClient($"beacon grid class id = {gridClassId}");
                            if (ModSessionManager.Instance.Config.IsValidGridClassId(gridClassId))
                            {
                                GridClassSync.Value = gridClassId;
                                //Utils.WriteToClient($"[CubeGridLogic] Using beacon GridClassId = {gridClassId}, EntityId = {Grid.EntityId}, Name = {Grid.DisplayName}");
                                Utils.Log($"[CubeGridLogic] Using beacon GridClassId = {gridClassId}, EntityId = {Grid.EntityId}, Name = {Grid.DisplayName}", 2);
                                break;
                            }
                        }
                    }
                }
            }

            ApplyModifiers();*/

            // NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            // NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
            // NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }



        public override void MarkForClose()
        {
            base.MarkForClose();

            GridGroup?.RemoveGrid(Grid);

            RemoveCubeGridLogicComponent(this);

            // called when entity is about to be removed for whatever reason (block destroyed, entity deleted, grid despawn because of sync range, etc)
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
                        if(GridGroup != null)
                        {
                            // serialise state here
                            Entity.Storage[Constants.GridClassStorageGUID] = GridGroup.GridClassId.ToString();
                        }
                        
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

        //Public methods
        public long GetGridClassIdFromStorage()
        {
            if (Entity.Storage.ContainsKey(Constants.GridClassStorageGUID))
            {
                long gridClassId = 0;

                try
                {
                    gridClassId = long.Parse(Entity.Storage[Constants.GridClassStorageGUID]);
                }
                catch (Exception e)
                {
                    string msg = $"CubeGridLogicComponent:GetGridClassIdFromStorage() Error parsing serialised GridClassId: {Entity.Storage[Constants.GridClassStorageGUID]}, EntityId = {Grid.EntityId}, Name = {Grid.DisplayName}";
                    //Utils.WriteToClient(msg);
                    Utils.Log(msg, 1);
                    Utils.Log(e.Message, 1);
                }

                if(ModSessionManager.Instance.Config.IsValidGridClassId(gridClassId))
                {
                    return gridClassId;
                }
            }

            return 0;
        }

        //Static methods

        public static CubeGridLogicComponent GetCubeGridLogicComponentByEntityId(long entityId)
        {
            if (CubeGridLogicComponentByEntityId.ContainsKey(entityId))
            {
                return CubeGridLogicComponentByEntityId[entityId];
            }

            return null;
        }

        //Internal/private static methods
        private static void AddCubeGridLogicComponent(CubeGridLogicComponent gridLogic)
        {
            try
            {
                if (gridLogic == null)
                {
                    throw new Exception("gridLogic cannot be null");
                }

                if (gridLogic.Grid == null)
                {
                    throw new Exception("gridLogic.Grid cannot be null");
                }

                if (AllCubeGridLogicComponents == null)
                {
                    throw new Exception("AllCubeGridLogics cannot be null");
                }

                /*if (gridsPerFactionClassManager == null)//TODO
                {
                    throw new Exception("gridsPerFactionClassManager cannot be null");
                }*/

                if (CubeGridLogicComponentByEntityId == null)
                {
                    throw new Exception("CubeGridLogics cannot be null");
                }

                if (!AllCubeGridLogicComponents.Contains(gridLogic))
                {
                    AllCubeGridLogicComponents.Add(gridLogic);
                }

                //gridsPerFactionClassManager.AddCubeGrid(gridLogic);//TODO
                CubeGridLogicComponentByEntityId[gridLogic.Grid.EntityId] = gridLogic;
            }
            catch (Exception e)
            {
                Utils.Log($"CubeGridLogic::AddGridLogic: caught error", 3);
                Utils.LogException(e);
            }
        }

        private static void RemoveCubeGridLogicComponent(CubeGridLogicComponent gridLogic)
        {
            CubeGridLogicComponentByEntityId.Remove(gridLogic.Grid.EntityId);
            AllCubeGridLogicComponents.RemoveAll((item) => item == gridLogic);
        }

    }
}
