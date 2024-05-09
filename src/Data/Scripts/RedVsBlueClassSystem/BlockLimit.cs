using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RedVsBlueClassSystem
{
    public class BlockLimit
    {
        public string Name;
        public BlockType[] BlockTypes;
        public float MinCount;
        public float MaxCount;

        public bool IsLimitedBlock(IMyTerminalBlock block, out float blockCountWeight)
        {
            blockCountWeight = 0;

            foreach (var blockType in BlockTypes)
            {
                if (blockType.IsBlockOfType(block, out blockCountWeight))
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
        public string TypeId;
        public string SubtypeId;
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
            if (Utils.GetBlockId(block) == TypeId && (String.IsNullOrEmpty(SubtypeId) || Convert.ToString(block.BlockDefinition.SubtypeId) == SubtypeId))
            {
                blockCountWeight = CountWeight;

                return true;
            }
            else
            {
                blockCountWeight = 0;

                return false;
            }
        }
    }

    public class BlockGroup
    {
        public string Id;

        public SingleBlockType[] BlockTypes;
    }
}
