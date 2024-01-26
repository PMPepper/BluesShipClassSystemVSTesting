using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
	public class ModConfig
    {
		public List<GridLimit> GridLimits;

		public static ModConfig LoadConfig(string filename)
		{
			try
			{
				if (MyAPIGateway.Utilities.FileExistsInWorldStorage(filename, typeof(ModConfig)))
				{
					TextReader Reader = MyAPIGateway.Utilities.ReadFileInWorldStorage(filename, typeof(ModConfig));
					string fileContent = Reader.ReadToEnd();
					Reader.Close();
					ModConfig loadedConfig = MyAPIGateway.Utilities.SerializeFromXML<ModConfig>(fileContent);

					if (loadedConfig == null) {
						Utils.Log($"Failed to load ModConfig from {filename}", 2);

						return null;
					}

					return loadedConfig;
				}

			}
			catch (Exception e)
			{
				Utils.Log($"Failed to load ModConfig from {filename}, reason = {e.Message}", 2);
			}

			return null;
		}

		public static void SaveConfig(ModConfig config, string filename)
        {
			try
			{
				TextWriter writer = MyAPIGateway.Utilities.WriteFileInWorldStorage(filename, typeof(ModConfig));
				writer.Write(MyAPIGateway.Utilities.SerializeToXML(config));
				writer.Close();
			}
			catch (Exception e)
			{
				Utils.Log($"Failed to save ModConfig file, reason {e.Message}", 3);
			}
		}

		private static BlockType InteriorTurret = new BlockType() { CountWeight = 1, TypeId = "InteriorTurret", SubtypeId = "LargeInteriorTurret" };

		private static BlockType SmallGatlingTurret = new BlockType() { CountWeight = 1, TypeId = "SmallGatlingTurret" };
		private static BlockType LargeGatlingTurret = new BlockType() { CountWeight = 1, TypeId = "LargeGatlingTurret" };
		private static BlockType LargeMissileTurret = new BlockType() { CountWeight = 1, TypeId = "LargeMissileTurret" };
		private static BlockType SmallMissileLauncher = new BlockType() { CountWeight = 1, TypeId = "SmallMissileLauncher" };
		private static BlockType LargeMissileLauncher = new BlockType() { CountWeight = 1, TypeId = "LargeMissileLauncher" };
		private static BlockType SmallMissileLauncherReload = new BlockType() { CountWeight = 1, TypeId = "SmallMissileLauncherReload" };
		private static BlockType SmallGatlingGun = new BlockType() { CountWeight = 1, TypeId = "SmallGatlingGun" };
		//TODO missing artillery/assault weapons + modded blocks?

		private static BlockType[] Weapons = new BlockType[] { SmallGatlingTurret, LargeGatlingTurret, LargeMissileTurret, SmallMissileLauncher, LargeMissileLauncher, SmallMissileLauncherReload, SmallGatlingGun };
		private static BlockType[] SmallGridWeapons = new BlockType[] { SmallGatlingTurret, SmallMissileLauncher, SmallMissileLauncherReload, SmallGatlingGun };
		private static BlockType[] LargeGridWeapons = new BlockType[] { LargeGatlingTurret, LargeMissileTurret, LargeMissileLauncher };

		//TODO other tools? large/small?
		private static BlockType Drill = new BlockType() { CountWeight = 1, TypeId = "ShipDrill" };

		private static BlockType[] Tools = new BlockType[] { Drill };

		public static ModConfig DefaultModConfig = new ModConfig() { GridLimits = new List<GridLimit>() {
			{ new GridLimit() { 
				Id = 1, 
				Name = "Fighter", 
				SmallGridShip = true, 
				MaxBlocks = 2000, 
				ForceBroadCast = true, 
				ForceBroadCastRange = 2500, 
				MaxPerFaction = 10, 
				Modifiers = new GridAttributes() { 
					ThrusterForce = 3, 
					ThrusterEfficiency = 2, 
					GyroForce = 1.5f, 
					GyroEfficiency = 1.5f, 
					AssemblerSpeed = 1, 
					DrillHarvestMutiplier = 1, 
					PowerProducersOutput = 1, 
					RefineEfficiency = 1, 
					RefineSpeed = 1 
				}, 
				BlockLimits = new BlockLimit[]{ 
					new BlockLimit() { Name = "Weapons", MaxCount = 8, BlockTypes = SmallGridWeapons },
					new BlockLimit() { Name = "Tools", MaxCount = 0, BlockTypes = Tools },
				} } },
			{ new GridLimit() {
				Id = 2,
				Name = "Miner",
				SmallGridShip = true,
				LargeGridShip = true,
				//MaxBlocks = 2000,
				ForceBroadCast = true,
				ForceBroadCastRange = 1500,
				MaxPerFaction = 8,
				Modifiers = new GridAttributes() {
					ThrusterForce = 1,
					ThrusterEfficiency = 1,
					GyroForce = 1,
					GyroEfficiency = 1,
					AssemblerSpeed = 1,
					DrillHarvestMutiplier = 3,
					PowerProducersOutput = 1,
					RefineEfficiency = 1,
					RefineSpeed = 1
				},
				BlockLimits = new BlockLimit[]{
					new BlockLimit() { Name = "Weapons", MaxCount = 4, BlockTypes = SmallGridWeapons },
					new BlockLimit() { Name = "Drills", MaxCount = 80, BlockTypes = new BlockType[] { Drill } },
				} } },
		} };
	}
	
	[ProtoContract]
	public struct GridLimit
	{
		[ProtoMember(1)]
		public int Id;
		[ProtoMember(2)]
		public string Name;
		[ProtoMember(3)]
		public bool SmallGridStatic;
		[ProtoMember(4)]
		public bool SmallGridShip;
		[ProtoMember(5)]
		public bool LargeGridStatic;
		[ProtoMember(6)]
		public bool LargeGridShip;
		[ProtoMember(7)]
		public int MaxBlocks;
		[ProtoMember(8)]
		public int MaxPCU;
		[ProtoMember(9)]
		public double MaxMass;
		[ProtoMember(10)]
		public bool ForceBroadCast;
		[ProtoMember(11)]
		public float ForceBroadCastRange;
		[ProtoMember(12)]
		public int MaxPerFaction;
		[ProtoMember(13)]
		public int MaxPerPlayer;
		[ProtoMember(14)]
		public GridAttributes Modifiers;
		[ProtoMember(16)]
		public BlockLimit[] BlockLimits;
	}


	[ProtoContract]
	public struct GridAttributes
	{
		[ProtoMember(1)]
		public float ThrusterForce;
		[ProtoMember(2)]
		public float ThrusterEfficiency;
		[ProtoMember(3)]
		public float GyroForce;
		[ProtoMember(4)]
		public float GyroEfficiency;
		[ProtoMember(5)]
		public float RefineEfficiency;
		[ProtoMember(6)]
		public float RefineSpeed;
		[ProtoMember(7)]
		public float AssemblerSpeed;
		[ProtoMember(8)]
		public float PowerProducersOutput;
		[ProtoMember(9)]
		public float DrillHarvestMutiplier;

	}

	[ProtoContract]
	public struct BlockLimit
	{
		[ProtoMember(1)]
		public string Name;
		[ProtoMember(2)]
		public BlockType[] BlockTypes;
		[ProtoMember(4)]
		public int MaxCount;
	}

	[ProtoContract]
	public struct BlockType
    {
		[ProtoMember(1)]
		public string TypeId;
		[ProtoMember(2)]
		public string SubtypeId;
		[ProtoMember(3)]
		public int CountWeight;
	}
}
