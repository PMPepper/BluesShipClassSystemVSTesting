﻿using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO better unknown config handling

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
	[ProtoContract]
	public class ModConfig
    {
		const string VariableId = nameof(ModConfig); // IMPORTANT: must be unique as it gets written in a shared space (sandbox.sbc)
		
		[ProtoMember(1)]
		public List<GridLimit> GridLimits;

		public static ModConfig LoadOrGetDefaultConfig(string filename) {
			return LoadConfig(filename) ?? DefaultModConfig;
		}

		private static string GetVariableName(string filename)
        {
			return $"{VariableId}/{filename}";
		}

		public static ModConfig LoadConfig(string filename)
		{
			string fileContent;

			if (!MyAPIGateway.Utilities.GetVariable<string>(GetVariableName(filename), out fileContent)) {
				return null;
			}

			ModConfig loadedConfig = MyAPIGateway.Utilities.SerializeFromXML<ModConfig>(fileContent);

			if (loadedConfig == null)
			{
				Utils.Log($"Failed to load ModConfig from {filename}", 2);

				return null;
			}

			return loadedConfig;
		}

		public static void SaveConfig(ModConfig config, string filename)
        {
			Utils.SaveConfig(GetVariableName(filename), filename, config);
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

		public static GridModifiers DefaultGridModifiers = new GridModifiers()
		{
			ThrusterForce = 1,
			ThrusterEfficiency = 1,
			GyroForce = 1,
			GyroEfficiency = 1,
			AssemblerSpeed = 1,
			DrillHarvestMutiplier = 1,
			PowerProducersOutput = 1,
			RefineEfficiency = 1,
			RefineSpeed = 1
		};

		public static GridLimit DefaultGridLimit = new GridLimit()
		{
			Id = 0,
			Name = "Unknown",
			SmallGridShip = true,
			SmallGridStatic = true,
			LargeGridShip = true,
			LargeGridStatic = true,
			ForceBroadCast = true,
			ForceBroadCastRange = 20000,
			Modifiers = DefaultGridModifiers
		};

		public static ModConfig DefaultModConfig = new ModConfig() { GridLimits = new List<GridLimit>() {
			{ DefaultGridLimit }, 
			{ new GridLimit() { 
				Id = 1, 
				Name = "Fighter", 
				SmallGridShip = true,
				LargeGridShip = true,
				MaxBlocks = 1500, 
				ForceBroadCast = true, 
				ForceBroadCastRange = 1500, 
				MaxPerFaction = 10, 
				Modifiers = new GridModifiers() { 
					ThrusterForce = 2.5f, 
					ThrusterEfficiency = 2.5f, 
					GyroForce = 2f, 
					GyroEfficiency = 2f, 
					AssemblerSpeed = 1, 
					DrillHarvestMutiplier = 0, 
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
				ForceBroadCastRange = 2500,
				MaxPerFaction = 12,
				Modifiers = new GridModifiers() {
					ThrusterForce = 1,
					ThrusterEfficiency = 3,
					GyroForce = 2,
					GyroEfficiency = 2,
					AssemblerSpeed = 1,
					DrillHarvestMutiplier = 3,
					PowerProducersOutput = 1,
					RefineEfficiency = 1,
					RefineSpeed = 1
				},
				BlockLimits = new BlockLimit[]{
					new BlockLimit() { Name = "Weapons", MaxCount = 4, BlockTypes = SmallGridWeapons },
					new BlockLimit() { Name = "Drills", MaxCount = 80, BlockTypes = new BlockType[] { Drill } },
				} }
			},
			{ new GridLimit() {
				Id = 3,
				Name = "Faction outpost",
				LargeGridStatic = true,
				ForceBroadCast = true,
				ForceBroadCastRange = 1000,
				Modifiers = DefaultGridModifiers

			} },
			{ new GridLimit() {
				Id = 4,
				Name = "Faction base",
				LargeGridStatic = true,
				ForceBroadCast = true,
				ForceBroadCastRange = 20000,
				Modifiers = new GridModifiers() {
					ThrusterForce = 1,
					ThrusterEfficiency = 1,
					GyroForce = 1,
					GyroEfficiency = 1,
					RefineEfficiency = 3,
					RefineSpeed = 50,
					PowerProducersOutput = 1,
					DrillHarvestMutiplier = 1,
				}

			} },
            {
				new GridLimit() {
					Id = 5,
					Name = "Frigate",
					LargeGridShip = true,
					ForceBroadCast = true,
					ForceBroadCastRange = 3000,
					Modifiers = new GridModifiers() {
						ThrusterForce = 2,
						ThrusterEfficiency = 2,
						GyroForce = 2,
						GyroEfficiency = 2,
						RefineEfficiency = 1,
						RefineSpeed = 1,
						PowerProducersOutput = 1,
						DrillHarvestMutiplier = 0,
					}
				}
            },
			{
				new GridLimit() {
					Id = 6,
					Name = "Destroyer",
					LargeGridShip = true,
					ForceBroadCast = true,
					ForceBroadCastRange = 4000,
					Modifiers = new GridModifiers() {
						ThrusterForce = 1.2f,
						ThrusterEfficiency = 1.5f,
						GyroForce = 2,
						GyroEfficiency = 2,
						RefineEfficiency = 1,
						RefineSpeed = 1,
						PowerProducersOutput = 1,
						DrillHarvestMutiplier = 0,
					}
				}
			},
			{
				new GridLimit() {
					Id = 7,
					Name = "Battleship",
					LargeGridShip = true,
					ForceBroadCast = true,
					ForceBroadCastRange = 6500,
					Modifiers = new GridModifiers() {
						ThrusterForce = 2f,
						ThrusterEfficiency = 2f,
						GyroForce = 4,
						GyroEfficiency = 2,
						RefineEfficiency = 1,
						RefineSpeed = 1,
						PowerProducersOutput = 1,
						DrillHarvestMutiplier = 0,
					}
				}
			},
			{
				new GridLimit() {
					Id = 8,
					Name = "Capital",
					LargeGridShip = true,
					ForceBroadCast = true,
					ForceBroadCastRange = 10000,
					Modifiers = new GridModifiers() {
						ThrusterForce = 3f,
						ThrusterEfficiency = 7f,
						GyroForce = 5,
						GyroEfficiency = 10,
						RefineEfficiency = 5,
						RefineSpeed = 5,
						PowerProducersOutput = 2,
						DrillHarvestMutiplier = 0,
						AssemblerSpeed = 5,
					}
				}
			}
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
		public GridModifiers Modifiers;
		[ProtoMember(16)]
		public BlockLimit[] BlockLimits;
	}


	[ProtoContract]
	public struct GridModifiers
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


		public override string ToString()
        {
			return $"<GridModifiers ThrusterForce={ThrusterForce} ThrusterEfficiency={ThrusterEfficiency} GyroForce={GyroForce} GyroEfficiency={GyroEfficiency} RefineEfficiency={RefineEfficiency} RefineSpeed={RefineSpeed} AssemblerSpeed={AssemblerSpeed} PowerProducersOutput={PowerProducersOutput} DrillHarvestMutiplier={DrillHarvestMutiplier} />";
        }
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
