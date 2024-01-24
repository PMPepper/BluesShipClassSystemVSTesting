using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    public static class Settings
    {
        public static readonly bool Debug = true;
        public static readonly int LOG_LEVEL = 0;//messages with logPriority >= this will get logged, less than will be ignored
        public static readonly ushort COMMS_MESSAGE_ID = 53642;
    }
}
