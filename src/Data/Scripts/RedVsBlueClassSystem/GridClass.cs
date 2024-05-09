using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;

namespace RedVsBlueClassSystem
{
    public class GridClass
    {
        public int Id;
        public string Name;
        public bool SmallGridStatic = false;
        public bool SmallGridMobile = false;
        public bool LargeGridStatic = false;
        public bool LargeGridMobile = false;
        public int MaxBlocks = -1;
        public int MinBlocks = -1;
        public int MaxPCU = -1;
        public float MaxMass = -1;
        public bool ForceBroadCast = false;
        public float ForceBroadCastRange = 0;
        public int MaxPerFaction = -1;
        public int MaxPerPlayer = -1;
        public GridModifiers Modifiers = new GridModifiers();
        public BlockLimit[] BlockLimits;

        public bool IsGridEligible(IMyCubeGrid grid)
        {
            return grid.IsStatic
                ? grid.GridSizeEnum == VRage.Game.MyCubeSize.Large
                    ? LargeGridStatic
                    : SmallGridStatic
                : grid.GridSizeEnum == VRage.Game.MyCubeSize.Large
                    ? LargeGridMobile
                    : SmallGridMobile;
        }

        public DetailedGridClassCheckResult CheckGridGroupIsValid(GridGroup gridGroup)
        {
            int blocksCountTotal = 0;
            int pcuTotal = 0;
            float massTotal = 0;
            var allGridBlocks = new List<IMyTerminalBlock>();

            foreach(var cubeGridLogicComponent in gridGroup.AllGrids)
            {
                //TODO check constraints on subgrids?
                var concreteGrid = (cubeGridLogicComponent.Grid as MyCubeGrid);

                blocksCountTotal += concreteGrid.BlocksCount;
                pcuTotal += concreteGrid.BlocksPCU;
                massTotal += concreteGrid.Mass;

                var gridBlocks = cubeGridLogicComponent.Grid.GetFatBlocks<IMyTerminalBlock>();

                if(BlockLimits != null)
                {
                    allGridBlocks.AddRange(gridBlocks);
                }
            }

            GridCheckResult<int> MaxBlocksResult = new GridCheckResult<int>(
                MaxBlocks > 0,
                MaxBlocks > 0 ? blocksCountTotal <= MaxBlocks : true,
                blocksCountTotal,
                MaxBlocks
            );

            GridCheckResult<int> MinBlocksResult = new GridCheckResult<int>(
                MinBlocks > 0,
                MinBlocks > 0 ? blocksCountTotal >= MinBlocks : true,
                blocksCountTotal,
                MinBlocks
            );

            //TODO does this already count subgrid PCU?
            GridCheckResult<int> MaxPCUResult = new GridCheckResult<int>(
                MaxPCU > 0,
                MaxPCU > 0 ? pcuTotal <= MaxPCU : true,
                pcuTotal,
                MaxPCU
            );

            //TODO does this already count subgrid mass?
            GridCheckResult<float> MaxMassResult = new GridCheckResult<float>(
                MaxMass > 0,
                MaxMass > 0 ? massTotal <= MaxMass : true,
                massTotal,
                MaxMass
            );

            BlockLimitCheckResult[] BlockLimitResults = null;

            if (BlockLimits != null)
            {
                //Init the result objects
                BlockLimitResults = new BlockLimitCheckResult[BlockLimits.Length];

                for (int i = 0; i < BlockLimits.Length; i++)
                {
                    BlockLimitResults[i] = new BlockLimitCheckResult() { Min = BlockLimits[i].MinCount, Max = BlockLimits[i].MaxCount };
                }

                //Check all blocks against the limits
                foreach (var block in allGridBlocks)
                {
                    for (int i = 0; i < BlockLimits.Length; i++)
                    {
                        float weightedCount;

                        if (BlockLimits[i].IsLimitedBlock(block, out weightedCount))
                        {
                            BlockLimitResults[i].Blocks++;
                            BlockLimitResults[i].Score += weightedCount;
                        }
                    }
                }

                //Check if the limits were exceeded & decide if test was passed
                for (int i = 0; i < BlockLimitResults.Length; i++)
                {
                    BlockLimitResults[i].Passed = BlockLimitCheckResult.ResultsPassed(BlockLimitResults[i]);
                }
            }
            else
            {
                Utils.Log("No blocklimits");
            }

            return new DetailedGridClassCheckResult(
                IsGridEligible(gridGroup.Master.Grid),
                MaxBlocksResult,
                MinBlocksResult,
                MaxPCUResult,
                MaxMassResult,
                BlockLimitResults
            );
        }
    }
}
