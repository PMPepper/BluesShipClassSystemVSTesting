using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace RedVsBlueClassSystem
{
    public static class BeaconGUI
    {
        private static int waitTicks = 0;
        private static bool controlsAdded = false;
        private static string[] ControlsToRemove = { "Radius", "HudText" };
        public static void AddControls(IMyModContext context)
        {
            if (controlsAdded) {
                return;
            }

            if(waitTicks < 100)//TODO I don't know why I need this, but without it, I lose all vanilla controls on dedicated servers - I'm going to leave this for now
            {
                waitTicks++;

                return;
            }

            controlsAdded = true;

            // Create Drop Down Menu and add the control to the grid controller's terminal
            // Different comboboxes available depending on grid type
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(GetCombobox($"SetGridClassLargeStatic", SetComboboxContentLargeStatic, (IMyTerminalBlock block) => block.CubeGrid.IsStatic && block.CubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Large));
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(GetCombobox($"SetGridClassLargeMobile", SetComboboxContentLargeGrid, (IMyTerminalBlock block) => !block.CubeGrid.IsStatic && block.CubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Large));
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(GetCombobox($"SetGridClassSmallStatic", SetComboboxContentSmallStatic, (IMyTerminalBlock block) => block.CubeGrid.IsStatic && block.CubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Small));
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(GetCombobox($"SetGridClassSmallMobile", SetComboboxContentSmallMobile, (IMyTerminalBlock block) => !block.CubeGrid.IsStatic && block.CubeGrid.GridSizeEnum == VRage.Game.MyCubeSize.Small));

            List<IMyTerminalControl> controls = new List<IMyTerminalControl>();
            MyAPIGateway.TerminalControls.GetControls<IMyBeacon>(out controls);

            foreach (var control in controls)
            {
                if (ControlsToRemove.Contains(control.Id))
                {
                    MyAPIGateway.TerminalControls.RemoveControl<IMyBeacon>(control);
                }
            }
        }

        private static IMyTerminalControlCombobox GetCombobox(string name, Action<List<MyTerminalControlComboBoxItem>> setComboboxContent, Func<IMyTerminalBlock, bool> isVisible) {
            var combobox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCombobox, IMyBeacon>(name);
            combobox.Visible = isVisible;
            combobox.Enabled = isVisible;
            combobox.Title = VRage.Utils.MyStringId.GetOrCompute("Grid class");
            combobox.Tooltip = VRage.Utils.MyStringId.GetOrCompute("Select your desired grid class");
            combobox.SupportsMultipleBlocks = false;
            combobox.Getter = GetGridClass;
            combobox.Setter = SetGridClass;
            combobox.ComboBoxContent = setComboboxContent;

            return combobox;
        }

        private static void SetComboboxContentLargeStatic(List<MyTerminalControlComboBoxItem> list)
        {
            foreach(var gridLimit in ModSessionManager.GetAllGridClasses())
            {
                if(gridLimit.LargeGridStatic)
                {
                    list.Add(new MyTerminalControlComboBoxItem { Key = gridLimit.Id, Value = VRage.Utils.MyStringId.GetOrCompute(gridLimit.Name) });
                }
            }
        }

        private static void SetComboboxContentLargeGrid(List<MyTerminalControlComboBoxItem> list)
        {
            foreach (var gridLimit in ModSessionManager.GetAllGridClasses())
            {
                if(gridLimit.LargeGridMobile)
                {
                    list.Add(new MyTerminalControlComboBoxItem { Key = gridLimit.Id, Value = VRage.Utils.MyStringId.GetOrCompute(gridLimit.Name) });
                }
            }
        }

        private static void SetComboboxContentSmallStatic(List<MyTerminalControlComboBoxItem> list)
        {
            foreach (var gridLimit in ModSessionManager.GetAllGridClasses())
            {
                if(gridLimit.SmallGridStatic)
                {
                    list.Add(new MyTerminalControlComboBoxItem { Key = gridLimit.Id, Value = VRage.Utils.MyStringId.GetOrCompute(gridLimit.Name) });
                }
            }
        }

        private static void SetComboboxContentSmallMobile(List<MyTerminalControlComboBoxItem> list)
        {
            foreach (var gridLimit in ModSessionManager.GetAllGridClasses())
            {
                if(gridLimit.SmallGridMobile)
                {
                    list.Add(new MyTerminalControlComboBoxItem { Key = gridLimit.Id, Value = VRage.Utils.MyStringId.GetOrCompute(gridLimit.Name) });
                }
            }
        }
        private static long GetGridClass(IMyTerminalBlock block)
        {
            CubeGridLogic cubeGridLogic = block.GetGridLogic();

            return cubeGridLogic.GridClassId;
        }
        private static void SetGridClass(IMyTerminalBlock block, long key)
        {
            CubeGridLogic cubeGridLogic = block.GetGridLogic();

            cubeGridLogic.GridClassId = key;
        }
    }
}
