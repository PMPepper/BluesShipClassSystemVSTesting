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

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_CubeGrid), false)]
    public class CubeGridLogic : MyGameLogicComponent, IMyEventProxy
    {
        //Apparently, this does not attach for clients in multiplayer?
        private IMyCubeGrid Grid;

        private MySync<long, SyncDirection.BothWays> ShipClassSync = null;

        public long ShipClassId { get { return ShipClassSync.Value; } set { ShipClassSync.Value = value; } }//TODO add validation logic in setter?

        public bool IsApplicableGrid { get {
                return Grid?.Physics != null && Grid is MyCubeGrid && ((MyCubeGrid)Grid).BlocksCount >= 3; 
        } }

        public string OwningFaction { get {
                if(Grid.BigOwners.Count == 0)
                {
                    return null;
                }

                if(Grid.BigOwners.Count == 1)
                {
                    var OwnerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(Grid.BigOwners[0]);

                    return OwnerFaction?.Tag;
                }

                var ownersPerFaction = new Dictionary<string, int>();

                //Find the faction with the most owners
                foreach (var owner in Grid.BigOwners)
                {
                    var OwnerFaction = MyAPIGateway.Session.Factions.TryGetPlayerFaction(Grid.BigOwners[0]);

                    if (OwnerFaction != null)
                    {
                        if (!ownersPerFaction.ContainsKey(OwnerFaction.Tag))
                        {
                            ownersPerFaction[OwnerFaction.Tag] = 1;
                        }
                        else
                        {
                            ownersPerFaction[OwnerFaction.Tag]++;
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

            Utils.ClientDebug($"[CubeGridLogic] EntityId = {Grid.EntityId}");
            Utils.Log($"[CubeGridLogic] EntityId = {Grid.EntityId}");


            ShipClassSync.ValueChanged += ShipClassSync_ValueChanged;

            Grid.OnBlockAdded += Grid_OnBlockAdded;
            Grid.OnBlockRemoved += Grid_OnBlockRemoved;
            Grid.OnBlockOwnershipChanged += Grid_OnBlockOwnershipChanged;



            // makes UpdateOnceBeforeFrame() execute.
            // this is a special flag that gets self-removed after the method is called.
            // it can be used multiple times but mind that there is overhead to setting this so avoid using it for continuous updates.
           // NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }



        /*public override void UpdateOnceBeforeFrame()
        {
            base.UpdateOnceBeforeFrame();

            if (Grid?.Physics == null) // ignore projected and other non-physical grids
                return;


            // do stuff...
            // you can access things from session via Example_Session.Instance.[...]


            // in other places (session, terminal control callbacks, TSS, etc) where you have an entity and you want to get this gamelogic, you can use:
            //   ent.GameLogic?.GetAs<Example_GameLogic>()
            // which will simply return null if it's not found.


            // allow UpdateAfterSimulation() and UpdateAfterSimulation100() to execute, remove if not needed
            // NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
            // NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }*/

        public override void MarkForClose()
        {
            base.MarkForClose();

            // called when entity is about to be removed for whatever reason (block destroyed, entity deleted, ship despawn because of sync range, etc)
        }

        public override void UpdateAfterSimulation()
        {
            base.UpdateAfterSimulation();

            // this and UpdateBeforeSimulation() require NeedsUpdate to contain MyEntityUpdateEnum.EACH_FRAME.
            // gets executed 60 times a second after physics simulation, unless game is paused.
        }

        /*public override void UpdateAfterSimulation100()
        {
            base.UpdateAfterSimulation100();

            // this and UpdateBeforeSimulation100() require NeedsUpdate to contain EACH_100TH_FRAME.
            // executed approximately every 100 ticks (~1.66s), unless game is paused.
            //   why approximately? Explained at the "Important information" in: https://forum.keenswh.com/threads/pb-scripting-guide-how-to-use-self-updating.7398267/

            // there's also 10-tick variants, UpdateBeforeSimulation10() and UpdateAfterSimulation10()
            //   which require NeedsUpdate to contain EACH_10TH_FRAME
        }*/


        // less commonly used methods:

        public override bool IsSerialized()
        {
            // executed when the entity gets serialized (saved, blueprinted, streamed, etc) and asks all
            //   its components whether to be serialized too or not (calling GetObjectBuilder())

            // this can be used for serializing to Storage dictionary for example,
            //   and for reliability I recommend that Storage has at least one key in it before this runs (by adding yours in first update).

            //TODO add serialisation here

            // you cannot add custom OBs to the game so this should always return the base (which currently is always false).
            return base.IsSerialized();
        }

        public override void UpdatingStopped()
        {
            base.UpdatingStopped();

            // only called when game is paused.
        }

        //Event handlers

        private void ShipClassSync_ValueChanged(MySync<long, SyncDirection.BothWays> newShipClassId)
        {
            //TODO handle value changing?
            Utils.Log($"ShipClassSync_ValueChanged {newShipClassId}");
            Utils.ClientDebug($"ShipClassSync_ValueChanged {newShipClassId}");

            /*if (MyAPIGateway.Session.OnlineMode != VRage.Game.MyOnlineModeEnum.OFFLINE && MyAPIGateway.Session.IsServer)
                MyAPIGateway.Utilities.SendMessage($"Synced server value on server: {obj.Value}");
            else
                MyAPIGateway.Utilities.ShowMessage("Test", $"Synced server value on client: {obj.Value}");*/
        }

        private void Grid_OnBlockAdded(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;
        }

        private void Grid_OnBlockRemoved(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;
        }

        private void Grid_OnBlockOwnershipChanged(IMyCubeGrid obj)
        {
            //TODO
        }
    }
}
