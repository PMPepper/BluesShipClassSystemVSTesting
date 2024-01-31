using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;

namespace RedVsBlueClassSystem
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class ModSessionManager : MySessionComponentBase, IMyEventProxy
    {
        private static ModSessionManager Instance;


        public ModConfig Config;

        private MyEntity lastControlledEntity = null;

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

            if(Constants.IsClient)
            {
                // Existing code for controlled entities and predictions
                MyEntity controlledEntity = Utils.GetControlledGrid();
                MyEntity cockpitEntity = Utils.GetControlledCockpit(controlledEntity);

                if (controlledEntity != null && !controlledEntity.Equals(lastControlledEntity))
                {
                    lastControlledEntity = controlledEntity;
                    MyCubeGrid controlled = controlledEntity as MyCubeGrid;

                    if (controlled != null)
                    {
                        var cubeGridLogic = controlled.GetGridLogic();

                        if (cubeGridLogic != null && !cubeGridLogic.GridMeetsGridClassRestrictions)
                        {
                            var gridClass = cubeGridLogic.GridClass;

                            if (gridClass != null)
                            {
                                Utils.ShowNotification($"Class \"{gridClass.Name}\" not valid for grid \"{controlled.DisplayName}\"");
                            }
                            else
                            {
                                Utils.ShowNotification($"Unknown class assigned to grid \"{controlled.DisplayName}\"");
                            }
                        }
                        else if(cubeGridLogic == null)
                        {
                            Utils.Log($"Grid missing CubeGridLogic: \"{controlled.DisplayName}\"", 1);
                        }
                    }
                }
                else if (controlledEntity == null)
                {
                    lastControlledEntity = null;
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
