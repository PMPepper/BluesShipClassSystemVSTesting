using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedVsBlueClassSystem
{
    public class DetailedGridClassCheckResult
    {
        public bool Passed { get; private set; }
        public bool ValidGridType { get; private set; }
        public GridCheckResult<int> MaxBlocks { get; private set; }
        public GridCheckResult<int> MinBlocks { get; private set; }
        public GridCheckResult<int> MaxPCU { get; private set; }
        public GridCheckResult<float> MaxMass { get; private set; }
        public BlockLimitCheckResult[] BlockLimits { get; private set; }

        public DetailedGridClassCheckResult(bool validGridType, GridCheckResult<int> maxBlocks, GridCheckResult<int> minBlocks, GridCheckResult<int> maxPCU, GridCheckResult<float> maxMass, BlockLimitCheckResult[] blockLimits)
        {
            ValidGridType = validGridType;
            MaxBlocks = maxBlocks;
            MinBlocks = minBlocks;
            MaxPCU = maxPCU;
            MaxMass = maxMass;
            BlockLimits = blockLimits;

            Passed = validGridType && maxBlocks.Passed && minBlocks.Passed && maxPCU.Passed && maxMass.Passed && (blockLimits == null || blockLimits.All(blockLimit => blockLimit.Passed));
        }
    }

    public struct GridCheckResult<T>
    {
        public bool Active;
        public bool Passed;
        public T Value;
        public T Limit;

        public GridCheckResult(bool active, bool passed, T value, T limit)
        {
            Active = active;
            Passed = passed;
            Value = value;
            Limit = limit;
        }
    }

    public struct BlockLimitCheckResult
    {
        public bool Passed;
        public float Score;
        public int Blocks;
        public float Min;
        public float Max;

        public static bool ResultsPassed(BlockLimitCheckResult results)
        {
            return results.Min > 0                      //Has a minimum?
                ? results.Score >= results.Min          //Yes:  Score meets/beats minimum?
                    ? results.Max > 0                       //Yes: Has a maximum?
                        ? results.Score <= results.Max          //Yes: check the maximum
                        : true                                  //No: no maximum, meets/beats minimum, passes
                    : false                                 //No: Does not meet the minimum, fails
                : results.Score <= results.Max;         //No: check the maximum
        }

        public string DescribeRange()
        {
            if (Min == 0)
            {
                //no minimum, just check maximum
                return $"{Max}";
            }
            else if (Max == 0)
            {
                //no maximum, just check the minimum
                return $">= {Min}";
            }
            else
            {
                //minimum and maximum
                return $"{Min} - {Max}";
            }
        }
    }
}
