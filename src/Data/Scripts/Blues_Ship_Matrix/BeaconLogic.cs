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
using Sandbox.Common.ObjectBuilders;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MyEntityComponentDescriptor(typeof(Sandbox.Common.ObjectBuilders.MyObjectBuilder_Beacon), false, new string[] { "SmallBlockBeacon", "LargeBlockBeacon", "SmallBlockBeaconReskin", "LargeBlockBeaconReskin" })]
    public class BeaconLogic : MyGameLogicComponent
    {
        private IMyBeacon Beacon;
        private CubeGridLogic GridLogic { get { return Beacon.GetGridLogic(); } }

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            // the base methods are usually empty, except for OnAddedToContainer()'s, which has some sync stuff making it required to be called.
            base.Init(objectBuilder);

            Beacon = (IMyBeacon)Entity;

            NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            base.UpdateOnceBeforeFrame();

            // do stuff...
            // you can access things from session via Example_Session.Instance.[...]

            NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void UpdateAfterSimulation100()
        {
            base.UpdateAfterSimulation100();

            UpdateBeacon();
        }

        public void UpdateBeacon() {
            var shipClass = GridLogic.ShipClass;

            if(shipClass.ForceBroadCast)
            {
                Beacon.Enabled = true;//TEMP force beacon to always be turned on
                Beacon.Radius = shipClass.ForceBroadCastRange;
            }
            
            Beacon.HudText = $"{Beacon.CubeGrid.DisplayName} : {shipClass.Name}";
            
            /*if(primaryOwnerId != -1)
            {
                Beacon.own
                Beacon.OwnerId = primaryOwnerId;
            }*/
            
        }
    }
}
