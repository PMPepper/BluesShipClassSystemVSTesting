using VRage.Game;
using VRage.Game.Components;
using VRage.Network;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class ModSessionManager : MySessionComponentBase, IMyEventProxy
    {
        private static ModSessionManager Instance;


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
                //Save whatever config you're using
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

            if (Constants.IsServer)
            {
                var gridsToCheck = CubeGridLogic.GetGridsToBeChecked(Settings.MAX_GRID_PROCESSED_PER_TICK);

                foreach (var gridLogic in gridsToCheck)
                {
                    gridLogic.CheckGridLimits();
                }
            }
        }

        public static ShipClass GetShipClassById(long ShipClassId)
        {
            return Instance.Config.GetShipClassById(ShipClassId);
        }

        public static ShipClass[] GetAllShipClasses()
        {
            return Instance.Config.ShipClasses ?? new ShipClass[0];
        }
    }
}
