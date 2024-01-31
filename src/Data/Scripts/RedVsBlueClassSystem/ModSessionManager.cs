using Sandbox.Game;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Network;

namespace RedVsBlueClassSystem
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
                //Other scripts can use this...
                if(MyVisualScriptLogicProvider.PlayerEnteredCockpit != null)
                {
                    BasePlayerEnteredCockpit = MyVisualScriptLogicProvider.PlayerEnteredCockpit;
                }

                //Sadly, poorly written scripts might overwrite my handler later, soo...?
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

        //private Action<string, long, string> BasePlayerEnteredCockpit;
        private DoubleKeyPlayerEvent BasePlayerEnteredCockpit;

        private void PlayerEnteredCockpit(string entityName, long playerId, string gridName) {
            Utils.WriteToClient($"PlayerEnteredCockpit Getting Called!");

            if(BasePlayerEnteredCockpit != null)
            {
                BasePlayerEnteredCockpit(entityName, playerId, gridName);
            }

            Utils.ShowNotification($"PlayerEnteredCockpit Getting Called!");

            if (playerId == MyAPIGateway.Session?.Player.IdentityId) {//TODO check that this is actually working
                VRage.ModAPI.IMyEntity myEntity = MyAPIGateway.Entities.GetEntityByName(gridName);

                if(myEntity is IMyCubeGrid)
                {
                    var grid = myEntity as IMyCubeGrid;
                    var cubeGridLogic = grid.GetGridLogic();

                    if(cubeGridLogic != null && !cubeGridLogic.GridMeetsGridClassRestrictions)
                    {
                        var gridClass = cubeGridLogic.GridClass;

                        if(gridClass != null)
                        {
                            Utils.ShowNotification($"Class \"{gridClass.Name}\" not valid for grid \"{grid.DisplayName}\"");
                        }
                        else
                        {
                            Utils.ShowNotification($"Unknown class assigned to grid \"{grid.DisplayName}\"");
                        }
                    } else
                    {
                        Utils.ShowNotification($"Grid missing CubeGridLogic: \"{grid.DisplayName}\"");
                    }

                        
                }
            }
                
        }

        public static GridClass GetGridClassById(long GridClassId)
        {
            return Instance.Config.GetGridClassById(GridClassId);
        }

        public static GridClass[] GetAllGridClasses()
        {
            return Instance.Config.GridClasses ?? new GridClass[0];
        }
    }
}
