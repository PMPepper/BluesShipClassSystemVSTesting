using Sandbox.Game;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
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

            if(Constants.IsClient)
            {
                MyVisualScriptLogicProvider.PlayerEnteredCockpit = PlayerEnteredCockpit;
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

        private void PlayerEnteredCockpit(string entityName, long playerId, string gridName) {
            if (playerId == MyAPIGateway.Session?.Player.IdentityId) {
                VRage.ModAPI.IMyEntity myEntity = MyAPIGateway.Entities.GetEntityByName(gridName);

                if(myEntity is IMyCubeGrid)
                {
                    var grid = myEntity as IMyCubeGrid;
                    var cubeGridLogic = grid.GetGridLogic();

                    if(!cubeGridLogic.GridMeetsShipClassRestrictions)
                    {
                        var shipClass = cubeGridLogic.ShipClass;

                        if(shipClass != null)
                        {
                            Utils.ShowNotification($"Class \"{shipClass.Name}\" not valid for grid \"{grid.DisplayName}\"");
                        }
                        else
                        {
                            Utils.ShowNotification($"Unknown class assigned to grid \"{grid.DisplayName}\"");
                        }
                    }

                        
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
