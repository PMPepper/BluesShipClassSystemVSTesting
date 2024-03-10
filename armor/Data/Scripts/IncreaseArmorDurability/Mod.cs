using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;

namespace RvB.BoostArmorDurability
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class ModSessionManager : MySessionComponentBase
    {
		public const float LightArmourGeneralDamageModifier = 0.25f;
		public const float LightArmorDeformationRatio = 0.25f;
		public const float HeavyArmourGeneralDamageModifier = 0.25f;
		public const float HeavyArmorDeformationRatio = 0.25f;
		public override void LoadData()
        {
            base.LoadData();

			var lightArmorBlocks = MyDefinitionManager.Static.GetAllDefinitions()
				.Where(def => def is MyCubeBlockDefinition
					&& (((MyCubeBlockDefinition)def).Id.TypeId == typeof(MyObjectBuilder_CubeBlock))
					&& (((MyCubeBlockDefinition)def).Id.ToString().Contains("Armor") || def.Id.SubtypeName.StartsWith("AQD_LG_LA_") || def.Id.SubtypeName.StartsWith("AQD_SG_LA_"))
					&& (((MyCubeBlockDefinition)def).EdgeType != null && ((MyCubeBlockDefinition)def).EdgeType.Equals("Light")) // order of execution is important, EdgeType is only defined for armor blocks!														
				);
			foreach (MyCubeBlockDefinition lightArmorBlock in lightArmorBlocks)
			{
				// Heavy Armor Resistance:
				lightArmorBlock.GeneralDamageMultiplier = LightArmourGeneralDamageModifier;
				// Heavy Armor Deformation Ratio:
				lightArmorBlock.DeformationRatio = LightArmorDeformationRatio;
			}

			var heavyArmorBlocks = MyDefinitionManager.Static.GetAllDefinitions()
				.Where(def => def is MyCubeBlockDefinition
					&& (((MyCubeBlockDefinition)def).Id.TypeId == typeof(MyObjectBuilder_CubeBlock))
					&& (((MyCubeBlockDefinition)def).Id.ToString().Contains("Armor") || def.Id.SubtypeName.StartsWith("AQD_LG_HA_") || def.Id.SubtypeName.StartsWith("AQD_SG_HA_"))
					&& (((MyCubeBlockDefinition)def).EdgeType != null && ((MyCubeBlockDefinition)def).EdgeType.Equals("Heavy")) // order of execution is important, EdgeType is only defined for armor blocks!																											
				);
			foreach (MyCubeBlockDefinition heavyArmorBlock in heavyArmorBlocks)
			{
				// Heavy Armor Resistance:
				heavyArmorBlock.GeneralDamageMultiplier = HeavyArmourGeneralDamageModifier;
				// Heavy Armor Deformation Ratio:
				heavyArmorBlock.DeformationRatio = HeavyArmorDeformationRatio;
			}
		}
    }
}
