using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class ModSessionManager : MySessionComponentBase
    {
        public static GridManager GridData;

        public static bool Debug = true;
        public static int LOG_LEVEL = 0;//messages with logPriority >= this will get logged, less than will be ignored
        
        public static bool IsDedicated => MyAPIGateway.Utilities.IsDedicated;
        public static bool IsServer => MyAPIGateway.Multiplayer.IsServer;
        public static bool IsMultiplayer => MyAPIGateway.Multiplayer.MultiplayerActive;
        public static bool IsClient => !(IsServer && IsDedicated);

        public override void LoadData()
        {
            base.LoadData();

            //ClientDebug("LoadData");
            GridData = new GridManager();
            GridData.LoadData();

            
        }

        /*public override void Init(MyObjectBuilder_SessionComponent SessionComponent)
        {
            base.Init(SessionComponent);

            ClientDebug("Init");
            MyLog.Default.WriteLine("Blues_Ship_Matrix: Init");
            
        }*/

        public override void BeforeStart()
        {
            base.BeforeStart();

            if (IsClient)
            {
                BeaconGUI.AddControls(ModContext);
            }
        }


        public override void UpdateAfterSimulation()
        {
            base.UpdateAfterSimulation();

            //TODO put core game logic here

            
        }

        protected override void UnloadData()
        {
            base.UnloadData();

            GridData.UnloadData();
        }

        public static void ClientDebug(string msg) {
            if(IsClient && Debug)
            {
                MyAPIGateway.Utilities.ShowMessage("[[BSCS]]: ", msg);
            }
        }

        public static void WriteToClient(string msg) {
            if (IsClient)
            {
                MyAPIGateway.Utilities.ShowMessage("[BSCS]: ", msg);
            }
        }

        public static void Log(string msg, int logPriority = 0) {
            if(logPriority >= LOG_LEVEL)
            {
                MyLog.Default.WriteLine($"[BSCS]: {msg}");
            }
        }
    }
}
