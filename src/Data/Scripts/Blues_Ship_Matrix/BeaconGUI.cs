using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
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

            if(waitTicks < 40)//TODO I don't know why I need this, but 1 didn't work, 40 seems to work - I'm going to leave this for now
            {
                waitTicks++;

                return;
            }

            controlsAdded = true;

            //Create Drop Down Menu
            var combobox = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlCombobox, IMyBeacon>("SetShipClass");
            combobox.Visible = SetVisible;
            combobox.Enabled = SetVisible;
            combobox.Title = VRage.Utils.MyStringId.GetOrCompute("Ship Class");
            combobox.Tooltip = VRage.Utils.MyStringId.GetOrCompute("Select Your Desired Ship Class");
            combobox.SupportsMultipleBlocks = false;
            combobox.Getter = GetShipClass;
            combobox.Setter = SetShipClass;
            combobox.ComboBoxContent = SetComboboxContent;


            // Add the control to the ship controller's terminal
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(combobox);


            List<IMyTerminalControl> controls = new List<IMyTerminalControl>();
            MyAPIGateway.TerminalControls.GetControls<IMyBeacon>(out controls);
            Utils.ClientDebug($"beacon controls: {controls.Count}");

            foreach (var control in controls)
            {
                if (ControlsToRemove.Contains(control.Id))
                {
                    MyAPIGateway.TerminalControls.RemoveControl<IMyBeacon>(control);
                }
            }
        }

        private static bool SetVisible(IMyTerminalBlock block)
        {
            return true;
        }

        private static void SetComboboxContent(List<MyTerminalControlComboBoxItem> list)
        {
            foreach(var gridLimit in ModSessionManager.Instance.Config.GridLimits)
            {
                list.Add(new MyTerminalControlComboBoxItem { Key = gridLimit.Id, Value = VRage.Utils.MyStringId.GetOrCompute(gridLimit.Name) });
            }
        }
        private static long GetShipClass(IMyTerminalBlock block)
        {
            CubeGridLogic cubeGridLogic = block.CubeGrid.GameLogic?.GetAs<CubeGridLogic>();

            return cubeGridLogic.ShipClassId;
        }
        private static void SetShipClass(IMyTerminalBlock block, long key)
        {
            CubeGridLogic cubeGridLogic = block.CubeGrid.GameLogic?.GetAs<CubeGridLogic>();

            cubeGridLogic.ShipClassId = key;
        }
    }
}
