using ProtoBuf;
using RedVsBlueClassSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedVsBlueClassSystem
{
    [ProtoContract]
    public struct SimpleGridGroupCheckResult
    {
        [ProtoMember(1)]
        public bool MaxBlocks;
        [ProtoMember(2)]
        public bool MinBlocks;
        [ProtoMember(3)]
        public bool MaxPCU;
        [ProtoMember(4)]
        public bool MaxMass;
        [ProtoMember(5)]
        public ulong BlockLimits;
        [ProtoMember(6)]
        public long GridClassId;
        [ProtoMember(7)]
        public bool ValidGridType;

        public bool CheckPassedForGridClass(GridClass gridClass)
        {
            if (gridClass == null)
            {
                return false;
            }

            if (gridClass.Id == 0)
            {
                return true;//default/unknown grid class always passes
            }

            if (gridClass.Id != GridClassId)
            {
                return false;//this GridCheckResult is for a different grid class, so always fails
            }

            if (!MinBlocks || !MaxBlocks || !MaxPCU || !MaxMass || !ValidGridType)
            {
                return false;
            }

            if (BlockLimits != 0)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"[GridCheckResults GridClassId={GridClassId} MaxBlocks={MaxBlocks} MaxPCU={MaxPCU} MaxMass={MaxMass} BlockLimits={BlockLimits} ]";
        }

        public static SimpleGridGroupCheckResult FromDetailedGridClassCheckResult(DetailedGridClassCheckResult result, long gridClassId)
        {
            ulong BlockLimits = 0;

            if (result.BlockLimits != null)
            {
                for (int i = 0; i < result.BlockLimits.Length; i++)
                {
                    if (!result.BlockLimits[i].Passed)
                    {
                        BlockLimits += 1UL << i;
                    }
                }
            }

            return new SimpleGridGroupCheckResult()
            {
                MaxBlocks = result.MaxBlocks.Passed,
                MinBlocks = result.MinBlocks.Passed,
                MaxPCU = result.MaxPCU.Passed,
                MaxMass = result.MaxMass.Passed,
                BlockLimits = BlockLimits,
                GridClassId = gridClassId,
                ValidGridType = result.ValidGridType
            };
        }
    }
}
