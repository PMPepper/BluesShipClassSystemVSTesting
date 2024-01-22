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
        public static void AddControls(IMyModContext context)
        {
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
            /*if (block?.GameLogic?.GetAs<ShipCore>() != null)
            {
                try
                {
                    var GridOwner = Manager.GetOwner(block.CubeGrid as MyCubeGrid);
                    if (GridOwner == block.OwnerId)
                    {
                        return (true);
                    }
                }
                catch { }
            }*/
            //return false;
        }

        private static void SetComboboxContent(List<MyTerminalControlComboBoxItem> list)
        {
            // list.Add(new MyTerminalControlComboBoxItem {Key=1L, Value=VRage.Utils.MyStringId.GetOrCompute(Manager.MySettings.Station_Basic.Name)});
            // list.Add(new MyTerminalControlComboBoxItem {Key=2L, Value=VRage.Utils.MyStringId.GetOrCompute(Manager.MySettings.LargeShip_Basic.Name)});
            // list.Add(new MyTerminalControlComboBoxItem {Key=3L, Value=VRage.Utils.MyStringId.GetOrCompute(Manager.MySettings.SmallShip_Basic.Name)});
            // long itemKey = 4L;

            /*for (int index = 0; index < Manager.MySettings.GridLimits.Count; index++)
            {
                MyGridLimit ShipClass = Manager.MySettings.GridLimits[index];
                list.Add(new MyTerminalControlComboBoxItem { Key = index, Value = VRage.Utils.MyStringId.GetOrCompute(ShipClass.Name) });
            }*/

            list.Add(new MyTerminalControlComboBoxItem { Key = 1L, Value = VRage.Utils.MyStringId.GetOrCompute("Hello") });
            list.Add(new MyTerminalControlComboBoxItem { Key = 2L, Value = VRage.Utils.MyStringId.GetOrCompute("World") });

        }
        private static long GetShipClass(IMyTerminalBlock block)
        {
            GridData gridData = ModSessionManager.GridData.GetGridData(block);

            return gridData.ShipClassId;
            /*var ShipCore = block?.GameLogic?.GetAs<ShipCore>();
            var CoreGridClass = Globals.GetClass(block.CubeGrid);

            if (ShipCore != null && ShipCore.CoreGridClass != null)
            {
                if (CoreGridClass.Name == (Manager.MySettings.Station_Basic.Name)) { return 1L; }
                if (CoreGridClass.Name == (Manager.MySettings.LargeShip_Basic.Name)) { return 2L; }
                if (CoreGridClass.Name == (Manager.MySettings.SmallShip_Basic.Name)) { return 3L; }
                long itemKey = 4L;
                foreach (MyGridLimit Class in Manager.MySettings.GridLimits)
                {
                    if (CoreGridClass.Name == Class.Name) { return itemKey; }
                    itemKey++;
                }
            }
            return 1L;*/
        }
        private static void SetShipClass(IMyTerminalBlock block, long key)
        {
            GridData gridData = ModSessionManager.GridData.GetGridData(block);
            gridData.SetShipClass(key);
            

            /*var ShipCore = block?.GameLogic?.GetAs<ShipCore>();
            MyGridLimit NewGridClass = null;
            if (ShipCore != null)
            {
                if (key == 1L) { NewGridClass = Manager.MySettings.Station_Basic; }
                if (key == 2L) { NewGridClass = Manager.MySettings.LargeShip_Basic; }
                if (key == 3L) { NewGridClass = Manager.MySettings.SmallShip_Basic; }
                long itemKey = 4L;
                foreach (MyGridLimit Class in Manager.MySettings.GridLimits)
                {
                    if (key == itemKey) { NewGridClass = Class; }
                    itemKey++;
                }
                if (ShipCore.CoreGridClass != null)
                {
                    if (NewGridClass.Name == ShipCore.CoreGridClass.Name) { return; }
                    if (ShipCore.CoreGrid.CustomName.Contains(ShipCore.CoreGridClass.Name)) { ShipCore.CoreGrid.CustomName = ShipCore.CoreGrid.CustomName.Replace(ShipCore.CoreGridClass.Name, NewGridClass.Name); }
                    else { ShipCore.CoreGrid.CustomName += ": " + NewGridClass.Name; }
                    if (ShipCore.CoreBeacon.HudText.Contains(ShipCore.CoreGridClass.Name)) { ShipCore.CoreBeacon.HudText = ShipCore.CoreBeacon.HudText.Replace(ShipCore.CoreGridClass.Name, NewGridClass.Name); }
                }
                //ShipCore.CoreGridClass=NewGridClass;
                //ShipCore.SyncGridClass.ValidateAndSet(NewGridClass);
*/

        }


        static List<IMyTerminalControl> GetControls<T>() where T : IMyTerminalBlock
        {
            List<IMyTerminalControl> controls = new List<IMyTerminalControl>();
            MyAPIGateway.TerminalControls.GetControls<T>(out controls);

            return controls;
        }
    }
}
