using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI.Network;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class ModSessionManager : MySessionComponentBase, IMyEventProxy
    {
        public static ModSessionManager Instance;


        public ModConfig Config;

        /*public override void LoadData()
        {
            base.LoadData();           
        }*/

        public override void Init(MyObjectBuilder_SessionComponent SessionComponent)
        {
            base.Init(SessionComponent);

            Instance = this;

            Utils.Log("Init");

            Config = ModConfig.LoadOrGetDefaultConfig(Constants.ConfigFilename);

            if (Constants.IsServer)
            {
                //Save whatever you're using
                ModConfig.SaveConfig(Config, Constants.ConfigFilename);

            }
        }

        /*public override void BeforeStart()
        {
            base.BeforeStart();

        }*/

        public override void UpdateAfterSimulation()
        {
            base.UpdateAfterSimulation();

            BeaconGUI.AddControls(ModContext);
        }

        public static GridLimit GetShipClassById(long ShipClassId)
        {
            var config = Instance.Config;

            if(config != null)
            {
                return config.GridLimits.Find(gridLimit => gridLimit.Id == ShipClassId);
            }

            //TODO better default handling
            return new GridLimit();
        }

        
    }
}
