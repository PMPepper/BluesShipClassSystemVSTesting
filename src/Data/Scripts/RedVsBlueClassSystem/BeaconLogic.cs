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

namespace RedVsBlueClassSystem
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

            if (Beacon.CubeGrid?.Physics == null)
                return; // ignore ghost/projected grids

            NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        public override void UpdateAfterSimulation100()
        {
            base.UpdateAfterSimulation100();

            UpdateBeacon();
        }

        public void UpdateBeacon() {
            var gridClass = GridLogic.GridClass;

            if(gridClass.ForceBroadCast)
            {
                Beacon.Enabled = true;
                Beacon.Radius = gridClass.ForceBroadCastRange;
            }
            
            Beacon.HudText = $"{Beacon.CubeGrid.DisplayName} : {gridClass.Name}";
            
            /*if(primaryOwnerId != -1)
            {
                Beacon.own
                Beacon.OwnerId = primaryOwnerId;
            }*/
            
        }
    }
}
