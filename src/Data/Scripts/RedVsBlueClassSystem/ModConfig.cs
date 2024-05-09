using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//TODO better unknown config handling

namespace RedVsBlueClassSystem
{
    public class ModConfig
    {
        private static bool ForceRegenerateConfig = false;
        private static readonly string VariableId = nameof(ModConfig); // IMPORTANT: must be unique as it gets written in a shared space (sandbox.sbc)

        private GridClass[] _GridClasses;
        private GridClass _DefaultGridClass = DefaultGridClassConfig.DefaultGridClassDefinition;
        private Dictionary<long, GridClass> _GridClassesById = new Dictionary<long, GridClass>();

        public bool IncludeAIFactions = false;
        public string[] IgnoreFactionTags = new string[0];

        public string[] ExcludeBeaconSubTypeId = new string[] { "ArtifactOfRespect", "ArtifactOfCourage", "ArtifactOfModeration" };

        public static bool IsExcludedSubTypeId(IMyTerminalBlock block)
        {
            string subTypeId = Utils.GetBlockSubtypeId(block);

            return ModSessionManager.Instance.Config.ExcludeBeaconSubTypeId.Contains(subTypeId);
        }

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

    
}
