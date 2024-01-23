﻿using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    public static class Constants
    {
        
        public static bool IsDedicated => MyAPIGateway.Utilities.IsDedicated;
        public static bool IsServer => MyAPIGateway.Multiplayer.IsServer;
        public static bool IsMultiplayer => MyAPIGateway.Multiplayer.MultiplayerActive;
        public static bool IsClient => !(IsServer && IsDedicated);
    }
}