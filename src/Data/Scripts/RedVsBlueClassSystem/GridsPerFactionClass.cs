using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedVsBlueClassSystem
{
    [ProtoContract]
    public class GridsPerFactionClass
    {
        [ProtoMember(1)]
        private Dictionary<long, Dictionary<long, List<CubeGridLogic>>> PerFaction = new Dictionary<long, Dictionary<long, List<CubeGridLogic>>>();

        public void AddCubeGrid(CubeGridLogic gridLogic)
        {
            var factionId = gridLogic.OwningFaction == null ? -1 : gridLogic.OwningFaction.FactionId;
            var gridClassId = gridLogic.GridClassId;

            if (!PerFaction.ContainsKey(factionId))
            {
                PerFaction.Add(factionId, new Dictionary<long, List<CubeGridLogic>>());
            }

            var perGridClass = PerFaction[factionId];

            if (!perGridClass.ContainsKey(gridClassId))
            {
                perGridClass.Add(gridClassId, new List<CubeGridLogic>());
            }

            perGridClass[gridClassId].Add(gridLogic);
        }

        public Dictionary<long, List<CubeGridLogic>> GetFactionGridsByClass(long factionId)
        {
            if (PerFaction.ContainsKey(factionId))
            {
                return PerFaction[factionId];
            }

            return null;
        }

        public void Reset()
        {
            PerFaction.Clear();
        }

        public static GridsPerFactionClass FromBytes(byte[] data)
        {
            return MyAPIGateway.Utilities.SerializeFromBinary<GridsPerFactionClass>(data);
        }
    }
}
