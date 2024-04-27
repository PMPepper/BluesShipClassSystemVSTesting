using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Extensions;
using VRageMath;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;

namespace YourName.ModName.temp
{
    class Program : MyGridProgram
    {
        /* Example script template*/

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Me.CubeGrid.CustomName = "Red printer #1";
        }
    }
}
