using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;

//TODO better unknown config handling

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    public class ModConfig
    {
        private static readonly string VariableId = nameof(ModConfig); // IMPORTANT: must be unique as it gets written in a shared space (sandbox.sbc)

        private ShipClass[] _ShipClasses;
        private ShipClass _DefaultShipClass = DefaultShipClassConfig.DefaultShipClassDefinition;
        private Dictionary<long, ShipClass> _ShipClassesById = new Dictionary<long, ShipClass>();

        public ShipClass[] ShipClasses { get { return _ShipClasses; } set { _ShipClasses = value; UpdateShipClassesDictionary(); } }
        public ShipClass DefaultShipClass { get { return _DefaultShipClass; } set { _DefaultShipClass = value; UpdateShipClassesDictionary(); } }
        
        public ShipClass GetShipClassById(long shipClassId)
        {
            if(_ShipClassesById.ContainsKey(shipClassId))
            {
                return _ShipClassesById[shipClassId];
            }

            Utils.Log($"Unknown ship class {shipClassId}, using default ship class");

            return DefaultShipClass;
        }

        private void UpdateShipClassesDictionary()
        {
            _ShipClassesById.Clear();

            if(_DefaultShipClass != null)
            {
                _ShipClassesById[0] = DefaultShipClass;
            } else
            {
                _ShipClassesById[0] = DefaultShipClassConfig.DefaultShipClassDefinition;
            }
            
            if(_ShipClasses != null)
            {
                foreach (var shipClass in _ShipClasses)
                {
                    _ShipClassesById[shipClass.Id] = shipClass;
                }
            }
        }

        public static ModConfig LoadOrGetDefaultConfig(string filename)
        {
            return LoadConfig(filename) ?? DefaultShipClassConfig.DefaultModConfig;
        }

        public static ModConfig LoadConfig(string filename)
        {
            string fileContent = null;

            //If this is the server, initially try loading from world storage
            if (Constants.IsServer)
            {
                if (MyAPIGateway.Utilities.FileExistsInWorldStorage(filename, typeof(ModConfig)))
                {
                    Utils.Log($"Loading config {filename} from world storage");
                    TextReader Reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(filename, typeof(ModConfig));
                    fileContent = Reader.ReadToEnd();
                    Reader.Close();

                    if (string.IsNullOrEmpty(fileContent))
                    {
                        Utils.Log($"Loadied config {filename} from world storage was empty");
                    }
                    else
                    {
                        Utils.Log($"Loaded config {filename} from world storage size = {fileContent.Length}");
                    }
                }
            }

            //If we do not have any data (either not the server, or no config file present on the server)
            //then try loading from the sandbox.sbc
            if (fileContent == null)
            {
                Utils.Log($"Loading config {filename} from sandbox data");
                if (!MyAPIGateway.Utilities.GetVariable<string>(GetVariableName(filename), out fileContent))
                {
                    return null;
                }
            }

            //We didn't find any saved config, so return null
            if (fileContent == null)
            {
                Utils.Log($"No saved config found for {filename}");
                return null;
            }

            //Otherwise, attempt to parse the saved config data
            try
            {
                ModConfig loadedConfig = MyAPIGateway.Utilities.SerializeFromXML<ModConfig>(fileContent);

                if (loadedConfig == null)
                {
                    Utils.Log($"Failed to load ModConfig from {filename}", 2);

                    return null;
                }

                return loadedConfig;
            }
            catch (Exception e)
            {
                Utils.Log($"Failed to parse saved config file {filename}, reason = {e.Message}", 2);
                Utils.Log($"{e.StackTrace}");
            }

            return null;
        }

        public static void SaveConfig(ModConfig config, string filename)
        {
            if (Constants.IsServer)
            {
                try
                {
                    TextWriter writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(filename, typeof(ModConfig));
                    writer.Write(MyAPIGateway.Utilities.SerializeToXML(config));
                    writer.Close();
                }
                catch (Exception e)
                {
                    Utils.Log($"Failed to save ModConfig file {filename}, reason {e.Message}", 3);
                }

                Utils.SaveConfig(GetVariableName(filename), filename, config);
            }
        }

        private static string GetVariableName(string filename)
        {
            return $"{VariableId}/{filename}";
        }
    }

    [ProtoContract]
    public class ShipClass
    {
        [ProtoMember(1)]
        public int Id;
        [ProtoMember(2)]
        public string Name;
        [ProtoMember(3)]
        public bool SmallGridStatic = false;
        [ProtoMember(4)]
        public bool SmallGridShip = false;
        [ProtoMember(5)]
        public bool LargeGridStatic = false;
        [ProtoMember(6)]
        public bool LargeGridShip = false;
        [ProtoMember(7)]
        public int MaxBlocks = -1;
        [ProtoMember(8)]
        public int MaxPCU = -1;
        [ProtoMember(9)]
        public float MaxMass = -1;
        [ProtoMember(10)]
        public bool ForceBroadCast = false;
        [ProtoMember(11)]
        public float ForceBroadCastRange = 0;
        [ProtoMember(12)]
        public int MaxPerFaction = -1;
        [ProtoMember(13)]
        public int MaxPerPlayer = -1;
        [ProtoMember(14)]
        public GridModifiers Modifiers = new GridModifiers();
        [ProtoMember(16)]
        public BlockLimit[] BlockLimits;

        public bool IsGridEligible(IMyCubeGrid grid)
        {
            return grid.IsStatic
                ? grid.GridSizeEnum == VRage.Game.MyCubeSize.Large
                    ? LargeGridStatic
                    : SmallGridStatic
                : grid.GridSizeEnum == VRage.Game.MyCubeSize.Large
                    ? LargeGridShip
                    : SmallGridShip;
        }

        public ShipClassCheckResult CheckGrid(IMyCubeGrid grid) {
            var concreteGrid = (grid as MyCubeGrid);
            GridCheckResult<int> MaxBlocksResult = new GridCheckResult<int>() { 
                Passed = MaxBlocks > 0 ? concreteGrid.BlocksCount <= MaxBlocks : true, 
                Active = MaxBlocks > 0,
                Value = concreteGrid.BlocksCount, 
                Max = MaxBlocks
            };

            GridCheckResult<int> MaxPCUResult = new GridCheckResult<int>()
            {
                Passed = MaxPCU > 0 ? concreteGrid.BlocksPCU <= MaxPCU : true,
                Active = MaxPCU > 0,
                Value = concreteGrid.BlocksPCU,
                Max = MaxPCU
            };

            GridCheckResult<float> MaxMassResult = new GridCheckResult<float>()
            {
                Passed = MaxMass > 0 ? concreteGrid.Mass <= MaxMass : true,
                Active = MaxMass > 0,
                Value = concreteGrid.Mass,
                Max = MaxMass
            };

            BlockLimitCheckResult[] BlockLimitResults = null;

            if (BlockLimits != null)
            {
                //Init the result objects
                BlockLimitResults = new BlockLimitCheckResult[BlockLimits.Length];

                for (int i = 0; i < BlockLimits.Length; i++)
                {
                    BlockLimitResults[i] = new BlockLimitCheckResult() { Max = BlockLimits[i].MaxCount };
                }

                //Get all blocks to check
                IEnumerable<IMyFunctionalBlock> BlocksOnGrid = grid.GetFatBlocks<IMyFunctionalBlock>();

                //Check all blocks against the limits
                foreach (var block in BlocksOnGrid)
                {
                    for (int i = 0; i < BlockLimits.Length; i++)
                    {
                        var limitResults = BlockLimitResults[i];
                        int weightedCount;

                        if (BlockLimits[i].IsLimitedBlock(block, out weightedCount))
                        {
                            limitResults.Blocks++;
                            limitResults.Score += weightedCount;
                        }
                    }
                }

                //Check if the limited were exceeded & decide if test was passed
                foreach(var limitResult in BlockLimitResults)
                {
                    limitResult.Passed = limitResult.Score <= limitResult.Max;
                }
            }
            else
            {
                Utils.Log("No blocklimits");
            }

            return new ShipClassCheckResult() { MaxMass = MaxMassResult, MaxBlocks = MaxBlocksResult, MaxPCU = MaxPCUResult, BlockLimits = BlockLimitResults };
        }
    }

    public class ShipClassCheckResult
    {
        public GridCheckResult<int> MaxBlocks;
        public GridCheckResult<int> MaxPCU;
        public GridCheckResult<float> MaxMass;
        public BlockLimitCheckResult[] BlockLimits;
    }

    public class GridCheckResult<T>
    {
        public bool Active;
        public bool Passed;
        public T Value;
        public T Max;
    }

    [ProtoContract]
    public class GridModifiers
    {
        [ProtoMember(1)]
        public float ThrusterForce = 1;
        [ProtoMember(2)]
        public float ThrusterEfficiency = 1;
        [ProtoMember(3)]
        public float GyroForce = 1;
        [ProtoMember(4)]
        public float GyroEfficiency = 1;
        [ProtoMember(5)]
        public float RefineEfficiency = 1;
        [ProtoMember(6)]
        public float RefineSpeed = 1;
        [ProtoMember(7)]
        public float AssemblerSpeed = 1;
        [ProtoMember(8)]
        public float PowerProducersOutput = 1;
        [ProtoMember(9)]
        public float DrillHarvestMutiplier = 1;


        public override string ToString()
        {
            return $"<GridModifiers ThrusterForce={ThrusterForce} ThrusterEfficiency={ThrusterEfficiency} GyroForce={GyroForce} GyroEfficiency={GyroEfficiency} RefineEfficiency={RefineEfficiency} RefineSpeed={RefineSpeed} AssemblerSpeed={AssemblerSpeed} PowerProducersOutput={PowerProducersOutput} DrillHarvestMutiplier={DrillHarvestMutiplier} />";
        }
    }

    [ProtoContract]
    public class BlockLimit
    {
        [ProtoMember(1)]
        public string Name;
        [ProtoMember(2)]
        public BlockType[] BlockTypes;
        [ProtoMember(4)]
        public int MaxCount;

        public bool IsLimitedBlock(IMyFunctionalBlock block, out int blockCountWeight)
        {
            blockCountWeight = 0;
            
            foreach (var blockType in BlockTypes)
            {
                if(blockType.IsBlockOfType(block))
                {
                    blockCountWeight = blockType.CountWeight;

                    return true;
                }
            }

            return false;
        }
    }

    public class BlockLimitCheckResult
    {
        public bool Passed;
        public int Score = 0;
        public int Blocks = 0;
        public int Max = 0;
    }

    [ProtoContract]
    public class BlockType
    {
        [ProtoMember(1)]
        public string TypeId;
        [ProtoMember(2)]
        public string SubtypeId;
        [ProtoMember(3)]
        public int CountWeight;

        public bool IsBlockOfType(IMyFunctionalBlock block)
        {
            return Utils.GetBlockId(block) == TypeId && (String.IsNullOrEmpty(SubtypeId) || Convert.ToString(block.BlockDefinition.SubtypeId) == SubtypeId);
        }
    }
}
