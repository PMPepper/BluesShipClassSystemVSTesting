using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    static class DefaultShipClassConfig
    {
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
        private static BlockType Drill = new BlockType() { CountWeight = 1, TypeId = "Drill" };
        private static BlockType Grinder = new BlockType() { CountWeight = 1, TypeId = "ShipGrinder" };
        private static BlockType Welder = new BlockType() { CountWeight = 1, TypeId = "ShipWelder" };

        private static BlockType[] Tools = new BlockType[] { Drill, Grinder, Welder };

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

        public static ShipClass DefaultShipClassDefinition = new ShipClass()
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

        public static ModConfig DefaultModConfig = new ModConfig()
        {
            DefaultShipClass = DefaultShipClassDefinition,
            ShipClasses = new ShipClass[] {
            new ShipClass() {
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
                } },
            new ShipClass() {
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
            ,
            new ShipClass() {
                Id = 3,
                Name = "Faction outpost",
                LargeGridStatic = true,
                ForceBroadCast = true,
                ForceBroadCastRange = 1000,
                Modifiers = DefaultGridModifiers

            },
            new ShipClass() {
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

            },
            new ShipClass() {
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
            },
            new ShipClass() {
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
            },
            new ShipClass() {
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
            },
            new ShipClass() {
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
        };
    }
}
