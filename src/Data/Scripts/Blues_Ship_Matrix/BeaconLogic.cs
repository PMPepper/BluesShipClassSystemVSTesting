﻿using System;
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
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Beacon), false)]
    public class BeaconLogic : MyGameLogicComponent
    {
        private IMyBeacon Beacon;
        private CubeGridLogic GridLogic { get { return Beacon.CubeGrid.GameLogic?.GetAs<CubeGridLogic>(); } }

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

            Beacon.Radius = shipClass.ForceBroadCastRange;//TODO get correct radius from ship class
            Beacon.HudText = $"{Beacon.CubeGrid.DisplayName} : {shipClass.Name}";//TODO get ship class name
            
            /*if(primaryOwnerId != -1)
            {
                Beacon.own
                Beacon.OwnerId = primaryOwnerId;
            }*/
            
        }
    }
}
