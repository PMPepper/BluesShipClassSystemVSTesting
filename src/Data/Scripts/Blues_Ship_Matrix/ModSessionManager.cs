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

        public override void LoadData()
        {
            base.LoadData();

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

            
        }


        public override void UpdateAfterSimulation()
        {
            base.UpdateAfterSimulation();

            BeaconGUI.AddControls(ModContext);

            //TODO put core game logic here


        }

        protected override void UnloadData()
        {
            base.UnloadData();

            GridData.UnloadData();
        }
    }
}
