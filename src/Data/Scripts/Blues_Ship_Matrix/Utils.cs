using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    public static class Utils
    {
        /*public static void ClientDebug(string msg)
        {
            if (Constants.IsClient && Settings.Debug)
            {
                MyAPIGateway.Utilities.ShowMessage("[[BSCS]]: ", msg);
            }
        }*/

        public static void ShowNotification(string msg, int disappearTime = 10000, string font = MyFontEnum.Red)
        {
            if (MyAPIGateway.Session?.Player != null)
                MyAPIGateway.Utilities.ShowNotification(msg, disappearTime, font);
        }

        public static void WriteToClient(string msg)
        {
            if (Constants.IsClient)
            {
                MyAPIGateway.Utilities.ShowMessage("[BSCS]: ", msg);
            }
        }

        public static void Log(string msg, int logPriority = 0)
        {
            if (logPriority >= Settings.LOG_LEVEL)
            {
                MyLog.Default.WriteLine($"[BSCS]: {msg}");
            }

            if(logPriority >= Settings.CLIENT_OUTPUT_LOG_LEVEL)
            {
                MyAPIGateway.Utilities.ShowMessage($"[B={logPriority}]: ", msg);
            }
        }

        public static void SaveConfig<T>(string variableId, string filename, T data)
        {
            string saveText = MyAPIGateway.Utilities.SerializeToXML(data);

            MyAPIGateway.Utilities.SetVariable(variableId, saveText);

            Log($"Saving config file to: {filename}", 0);

            using (TextWriter file = MyAPIGateway.Utilities.WriteFileInWorldStorage(filename, typeof(string)))
            {
                file.Write(saveText);
            }
        }

        public static string GetBlockId(IMyCubeBlock block)
        {
            return Convert.ToString(block.BlockDefinition.TypeId).Replace("MyObjectBuilder_", "");
        }

        public static CubeGridLogic GetGridLogic(this IMyCubeGrid grid)
        {
            return grid.GameLogic?.GetAs<CubeGridLogic>();
        }

        public static CubeGridLogic GetGridLogic(this MyCubeGrid grid)
        {
            return grid.GameLogic?.GetAs<CubeGridLogic>();
        }

        public static CubeGridLogic GetGridLogic(this IMyCubeBlock block)
        {
            return block.CubeGrid.GameLogic?.GetAs<CubeGridLogic>();
        }

        public static CubeGridLogic GetGridLogic(this IMyTerminalBlock block)
        {
            return block.CubeGrid.GameLogic?.GetAs<CubeGridLogic>();
        }
    }
}
