using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;

//TODO better unknown config handling

namespace RedVsBlueClassSystem
{
    public class ModConfig
    {
        private static bool ForceRegenerateConfig = true;
        private static readonly string VariableId = nameof(ModConfig); // IMPORTANT: must be unique as it gets written in a shared space (sandbox.sbc)

        private GridClass[] _GridClasses;
        private GridClass _DefaultGridClass = DefaultGridClassConfig.DefaultGridClassDefinition;
        private Dictionary<long, GridClass> _GridClassesById = new Dictionary<long, GridClass>();

        [XmlAttribute]
        public bool IncludeAIFactions = false;
        public string[] IgnoreFactionTags = new string[0];

        public BlockGroup[] BlockGroups = new BlockGroup[0];

        public BlockGroup GetBlockGroupById(string id)
        {
            if(BlockGroups != null)
            {
                return Array.Find(BlockGroups, (currentBlockGroup) => currentBlockGroup.Id == id);
            }

            return null;
        }
        public GridClass[] GridClasses { get { return _GridClasses; } set { _GridClasses = value; UpdateGridClassesDictionary(); } }
        public GridClass DefaultGridClass { get { return _DefaultGridClass; } set { _DefaultGridClass = value; UpdateGridClassesDictionary(); } }
        
        public GridClass GetGridClassById(long gridClassId)
        {
            if(_GridClassesById.ContainsKey(gridClassId))
            {
                return _GridClassesById[gridClassId];
            }

            Utils.Log($"Unknown grid class {gridClassId}, using default grid class");

            return DefaultGridClass;
        }

        public bool IsValidGridClassId(long gridClassId)
        {
            return _GridClassesById.ContainsKey(gridClassId);
        }

        private void UpdateGridClassesDictionary()
        {
            _GridClassesById.Clear();

            if(_DefaultGridClass != null)
            {
                _GridClassesById[0] = DefaultGridClass;
            } else
            {
                _GridClassesById[0] = DefaultGridClassConfig.DefaultGridClassDefinition;
            }
            
            if(_GridClasses != null)
            {
                foreach (var gridClass in _GridClasses)
                {
                    _GridClassesById[gridClass.Id] = gridClass;
                }
            }
        }

        public static ModConfig LoadOrGetDefaultConfig(string filename)
        {
            if(ForceRegenerateConfig)
            {
                return DefaultGridClassConfig.DefaultModConfig;
            }

            return LoadConfig(filename) ?? DefaultGridClassConfig.DefaultModConfig;
        }

        public static ModConfig LoadConfig(string filename)
        {
            //return null;//TEMP force load default config

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
                        Utils.Log($"Loaded config {filename} from world storage was empty");
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

    public class GridClass
    {
        [XmlAttribute]
        public int Id;
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public bool SmallGridStatic = false;
        [XmlAttribute]
        public bool SmallGridMobile = false;
        [XmlAttribute]
        public bool LargeGridStatic = false;
        [XmlAttribute]
        public bool LargeGridMobile = false;
        [XmlAttribute]
        public int MaxBlocks = -1;
        [XmlAttribute]
        public int MinBlocks = -1;
        [XmlAttribute]
        public int MaxPCU = -1;
        [XmlAttribute]
        public float MaxMass = -1;
        [XmlAttribute]
        public bool ForceBroadCast = false;
        [XmlAttribute]
        public float ForceBroadCastRange = 0;
        [XmlAttribute]
        public int MaxPerFaction = -1;
        [XmlAttribute]
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

        public DetailedGridClassCheckResult CheckGrid(IMyCubeGrid grid) {
            var concreteGrid = (grid as MyCubeGrid);

            GridCheckResult<int> MaxBlocksResult = new GridCheckResult<int>(
                MaxBlocks > 0, 
                MaxBlocks > 0 ? concreteGrid.BlocksCount <= MaxBlocks : true, 
                concreteGrid.BlocksCount, 
                MaxBlocks
            );

            GridCheckResult<int> MinBlocksResult = new GridCheckResult<int>(
                MinBlocks > 0, 
                MinBlocks > 0 ? concreteGrid.BlocksCount >= MinBlocks : true, 
                concreteGrid.BlocksCount, 
                MinBlocks
            );

            GridCheckResult<int> MaxPCUResult = new GridCheckResult<int>(
                MaxPCU > 0, 
                MaxPCU > 0 ? concreteGrid.BlocksPCU <= MaxPCU : true, 
                concreteGrid.BlocksPCU, 
                MaxPCU
            );

            GridCheckResult<float> MaxMassResult = new GridCheckResult<float>(
                MaxMass > 0, 
                MaxMass > 0 ? concreteGrid.Mass <= MaxMass : true, 
                concreteGrid.Mass, 
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

                //Get all blocks to check
                IEnumerable<IMyTerminalBlock> BlocksOnGrid = grid.GetFatBlocks<IMyTerminalBlock>();

                //Check all blocks against the limits
                foreach (var block in BlocksOnGrid)
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
                for(int i = 0; i < BlockLimitResults.Length; i++)
                {
                    BlockLimitResults[i].Passed = BlockLimitCheckResult.ResultsPassed(BlockLimitResults[i]);
                }
            }
            else
            {
                Utils.Log("No blocklimits");
            }

            return new DetailedGridClassCheckResult(
                IsGridEligible(grid),
                MaxBlocksResult,
                MinBlocksResult,
                MaxPCUResult,
                MaxMassResult,
                BlockLimitResults
            );
        }
    }

    public class GridModifiers
    {
        [XmlAttribute]
        public float ThrusterForce = 1;
        [XmlAttribute]
        public float ThrusterEfficiency = 1;
        [XmlAttribute]
        public float GyroForce = 1;
        [XmlAttribute]
        public float GyroEfficiency = 1;
        [XmlAttribute]
        public float RefineEfficiency = 1;
        [XmlAttribute]
        public float RefineSpeed = 1;
        [XmlAttribute]
        public float RefinePowerEfficiency = 1;
        [XmlAttribute]
        public float AssemblerSpeed = 1;
        [XmlAttribute]
        public float AssemblerPowerEfficiency = 1;
        [XmlAttribute]
        public float PowerProducersOutput = 1;
        [XmlAttribute]
        public float DrillHarvestMutiplier = 1;
        [XmlAttribute]
        public float DamageModifier = 1;
        [XmlAttribute]
        public float BulletDamageModifier = 1;
        [XmlAttribute]
        public float DeformationDamageModifier = 1;
        [XmlAttribute]
        public float ExplosionDamageModifier = 1;

        public override string ToString()
        {
            return $"<GridModifiers ThrusterForce={ThrusterForce} ThrusterEfficiency={ThrusterEfficiency} GyroForce={GyroForce} GyroEfficiency={GyroEfficiency} RefineEfficiency={RefineEfficiency} RefineSpeed={RefineSpeed} RefinePowerEfficiency={RefinePowerEfficiency} AssemblerSpeed={AssemblerSpeed} AssemblerPowerEfficiency={AssemblerPowerEfficiency} PowerProducersOutput={PowerProducersOutput} DrillHarvestMutiplier={DrillHarvestMutiplier} DamageModifier={DamageModifier} BulletDamageModifier={BulletDamageModifier} DeformationDamageModifier={DeformationDamageModifier} ExplosionDamageModifier={ExplosionDamageModifier} />";
        }

        public IEnumerable<ModifierNameValue> GetModifierValues()
        {
            yield return new ModifierNameValue("Thruster force", ThrusterForce);
            yield return new ModifierNameValue("Thruster efficiency", ThrusterEfficiency);
            yield return new ModifierNameValue("Gyro force", GyroForce);
            yield return new ModifierNameValue("Gryo efficiency", GyroEfficiency);
            yield return new ModifierNameValue("Refinery efficiency", RefineEfficiency);
            yield return new ModifierNameValue("Refinery speed", RefineSpeed);
            yield return new ModifierNameValue("Refinery power efficiency", RefinePowerEfficiency);
            yield return new ModifierNameValue("Assembler speed", AssemblerSpeed);
            yield return new ModifierNameValue("Assembler power efficiency", AssemblerPowerEfficiency);
            yield return new ModifierNameValue("Power output", PowerProducersOutput);
            yield return new ModifierNameValue("Drill harvest", DrillHarvestMutiplier);
            yield return new ModifierNameValue("Damage modifier", DamageModifier);
            yield return new ModifierNameValue("Bullet damage modifier", BulletDamageModifier);
            yield return new ModifierNameValue("Deformation damage modifier", DeformationDamageModifier);
            yield return new ModifierNameValue("Explosion damage modifier", ExplosionDamageModifier);
        }

        public float GetDamageModifier(MyStringHash type)
        {
            if (type == MyDamageType.Bullet) {
                return DamageModifier * BulletDamageModifier;
            }
            
            if(type == MyDamageType.Explosion)
            {
                return DamageModifier * ExplosionDamageModifier;
            }

            if (type == MyDamageType.Deformation)
            {
                return DamageModifier * DeformationDamageModifier;
            }

            return DamageModifier;
        }
    }

    public struct ModifierNameValue {
        public string Name;
        public float Value;

        public ModifierNameValue(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }

    public class BlockGroup
    {
        [XmlAttribute]
        public string Id;

        public SingleBlockType[] BlockTypes;
    }

    public class BlockLimit
    {
        [XmlAttribute]
        public string Name;
        public BlockType[] BlockTypes;
        [XmlAttribute]
        public float MinCount;
        [XmlAttribute]
        public float MaxCount;

        public bool IsLimitedBlock(IMyTerminalBlock block, out float blockCountWeight)
        {
            blockCountWeight = 0;
            
            foreach (var blockType in BlockTypes)
            {
                if(blockType.IsBlockOfType(block, out blockCountWeight))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [XmlInclude(typeof(BlockTypeGroup))]
    [XmlInclude(typeof(SingleBlockType))]
    public abstract class BlockType
    {
        public abstract bool IsBlockOfType(IMyTerminalBlock block, out float blockCountWeight);
    }

    public class BlockTypeGroup : BlockType
    {
        [XmlAttribute]
        public string GroupId;

        private bool BlockTypesChecked = false;
        private SingleBlockType[] BlockTypes;

        public override bool IsBlockOfType(IMyTerminalBlock block, out float blockCountWeight)
        {
            blockCountWeight = 0;

            if (!BlockTypesChecked)
            {
                BlockTypesChecked = true;
                BlockGroup blockGroup = ModSessionManager.Instance?.Config.GetBlockGroupById(GroupId);

                if (blockGroup != null)
                {
                    BlockTypes = blockGroup.BlockTypes;
                }
            }

            if (BlockTypes != null && BlockTypes.Length > 0)
            {
                foreach (var blockType in BlockTypes)
                {
                    if (blockType.IsBlockOfType(block, out blockCountWeight))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class SingleBlockType : BlockType
    {
        [XmlAttribute]
        public string TypeId;
        [XmlAttribute]
        public string SubtypeId;
        [XmlAttribute]
        public float CountWeight;
        
        public SingleBlockType() { }

        public SingleBlockType(string typeId, string subtypeId = "", float countWeight = 1)
        {
            TypeId = typeId;
            SubtypeId = subtypeId;
            CountWeight = countWeight;
        }
        public override bool IsBlockOfType(IMyTerminalBlock block, out float blockCountWeight)
        {
            if(Utils.GetBlockId(block) == TypeId && (String.IsNullOrEmpty(SubtypeId) || Convert.ToString(block.BlockDefinition.SubtypeId) == SubtypeId))
            {
                blockCountWeight = CountWeight;

                return true;
            } else
            {
                blockCountWeight = 0;

                return false;
            }
        }
    }
}
