﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedVsBlueClassSystem
{
    public class GridClassCheckResult
    {
        public bool Passed { get; private set; }
        public bool ValidGridType { get; private set; }
        public GridCheckResult<int> MaxBlocks { get; private set; }
        public GridCheckResult<int> MinBlocks { get; private set; }
        public GridCheckResult<int> MaxPCU { get; private set; }
        public GridCheckResult<float> MaxMass { get; private set; }
        public BlockLimitCheckResult[] BlockLimits { get; private set; }

        public GridClassCheckResult(bool validGridType, GridCheckResult<int> maxBlocks, GridCheckResult<int> minBlocks, GridCheckResult<int> maxPCU, GridCheckResult<float> maxMass, BlockLimitCheckResult[] blockLimits)
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

    public class BlockLimitCheckResult
    {
        public bool Passed;
        public float Score = 0;
        public int Blocks = 0;
        public float Max = 0;
    }
}