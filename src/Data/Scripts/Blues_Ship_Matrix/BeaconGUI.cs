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
        public static void AddControls(IMyModContext context)
        {
            if (controlsAdded) {
                return;
            }

            if(waitTicks < 30)//TODO I don't know why I need this, but 1 didn't work, 30 does - I'm going to leave this for now
            {
                waitTicks++;

                return;
            }

            controlsAdded = true;

            //Create Seperator
            IMyTerminalControlSeparator mySeparator = MyAPIGateway.TerminalControls.CreateControl<IMyTerminalControlSeparator, IMyBeacon>("SeperateMyDumbEyes");
            mySeparator.SupportsMultipleBlocks = false;

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
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(mySeparator);
            MyAPIGateway.TerminalControls.AddControl<IMyBeacon>(combobox);
        }


        private static bool SetVisible(IMyTerminalBlock block)
        {
            return true;
        }

        private static void SetComboboxContent(List<MyTerminalControlComboBoxItem> list)
        {
            list.Add(new MyTerminalControlComboBoxItem { Key = 1L, Value = VRage.Utils.MyStringId.GetOrCompute("Hello") });
            list.Add(new MyTerminalControlComboBoxItem { Key = 2L, Value = VRage.Utils.MyStringId.GetOrCompute("World") });

        }
        private static long GetShipClass(IMyTerminalBlock block)
        {
            GridData gridData = ModSessionManager.GridData.GetGridData(block);

            return gridData.ShipClassId;
        }
        private static void SetShipClass(IMyTerminalBlock block, long key)
        {
            GridData gridData = ModSessionManager.GridData.GetGridData(block);
            gridData.SetShipClass(key);
        }
    }
}
