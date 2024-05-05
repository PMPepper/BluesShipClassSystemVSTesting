using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedVsBlueClassSystem
{
    static class DefaultGridClassConfig
    {
        //vanilla small grid fixed weapons
        private static SingleBlockType SmallMissileLauncher = new SingleBlockType("SmallMissileLauncher");
        private static SingleBlockType SmallWarfareMissileLauncher = new SingleBlockType("SmallMissileLauncher", "SmallMissileLauncherWarfare2");
        private static SingleBlockType SmallMissileLauncherReload = new SingleBlockType("SmallMissileLauncherReload", "SmallMissileLauncherReload");
        private static SingleBlockType SmallAssaultCannon = new SingleBlockType("SmallMissileLauncherReload", "SmallBlockMediumCalibreGun");
        private static SingleBlockType SmallGatlingGun = new SingleBlockType("SmallGatlingGun", "");
        private static SingleBlockType SmallWarfareGatlingGun = new SingleBlockType("SmallGatlingGun", "SmallGatlingGunWarfare2");
        private static SingleBlockType SmallAutocannon = new SingleBlockType("SmallGatlingGun", "SmallBlockAutocannon");
        private static SingleBlockType SmallRailgun = new SingleBlockType("ConveyorSorter", "SmallRailgun");

        private static SingleBlockType[] SmallGridFixedWeapons = new SingleBlockType[] { SmallMissileLauncher, SmallWarfareMissileLauncher, SmallMissileLauncherReload, SmallAssaultCannon, SmallGatlingGun, SmallWarfareGatlingGun, SmallAutocannon, SmallRailgun };

        //vanilla small grid turrets
        private static SingleBlockType AssaultCannonTurret = new SingleBlockType("LargeMissileTurret", "SmallBlockMediumCalibreTurret");
        private static SingleBlockType AutocannonTurret = new SingleBlockType("LargeMissileTurret", "AutoCannonTurret");
        private static SingleBlockType SmallMissileTurret = new SingleBlockType("LargeMissileTurret", "SmallMissileTurret");
        private static SingleBlockType SmallGatlingTurret = new SingleBlockType("LargeGatlingTurret", "SmallGatlingTurret");

        private static SingleBlockType[] SmallGridTurretWeapons = new SingleBlockType[] { AssaultCannonTurret, AutocannonTurret, SmallMissileTurret, SmallGatlingTurret };

        //vanilla large grid fixed weapons
        private static SingleBlockType LargeMissileLauncher = new SingleBlockType("SmallMissileLauncher", "LargeMissileLauncher");
        private static SingleBlockType LargeArtilleryGun = new SingleBlockType("SmallMissileLauncher", "LargeBlockLargeCalibreGun");
        private static SingleBlockType LargeRailgun = new SingleBlockType("ConveyorSorter", "LargeRailgun");

        private static SingleBlockType[] LargeGridFixedWeapons = new SingleBlockType[] { LargeMissileLauncher, LargeArtilleryGun, LargeRailgun };

        //vanilla large grid turrets
        private static SingleBlockType LargeMissileTurret = new SingleBlockType("LargeMissileTurret");
        private static SingleBlockType LargeAssaultCannonTurret = new SingleBlockType("LargeMissileTurret", "LargeBlockMediumCalibreTurret");
        private static SingleBlockType LargeArtilleryTurret = new SingleBlockType("LargeMissileTurret", "LargeCalibreTurret");
        private static SingleBlockType LargeGatlingTurret = new SingleBlockType("LargeGatlingTurret");
        private static SingleBlockType InteriorTurret = new SingleBlockType() { CountWeight = 1, TypeId = "InteriorTurret", SubtypeId = "LargeInteriorTurret" };

        private static SingleBlockType[] LargeGridTurretWeapons = new SingleBlockType[] { LargeMissileTurret, LargeAssaultCannonTurret, LargeArtilleryTurret, LargeGatlingTurret, InteriorTurret };

        private static SingleBlockType[] SmallGridWeapons = SmallGridFixedWeapons.Concat(SmallGridTurretWeapons).ToArray();
        private static SingleBlockType[] LargeGridWeapons = LargeGridFixedWeapons.Concat(LargeGridTurretWeapons).ToArray();
        private static SingleBlockType[] VanillaWeapons = SmallGridWeapons.Concat(LargeGridWeapons).ToArray();

        private static SingleBlockType[] Artillery = new SingleBlockType[] { LargeArtilleryGun, LargeArtilleryTurret };

        //Tools
        private static SingleBlockType SmallDrill = new SingleBlockType("Drill", "SmallBlockDrill");
        private static SingleBlockType LargeDrill = new SingleBlockType("Drill", "LargeBlockDrill");

        private static SingleBlockType[] Drills = new SingleBlockType[] {
            SmallDrill,
            LargeDrill,
        };

        private static SingleBlockType SmallWelder = new SingleBlockType("ShipWelder", "SmallShipWelder");
        private static SingleBlockType LargeWelder = new SingleBlockType("ShipWelder", "LargeShipWelder");

        private static SingleBlockType[] Welders = new SingleBlockType[] {
            SmallWelder,
            LargeWelder,
        };

        //Misc vanilla
        private static SingleBlockType[] ProgrammableBlocks = new SingleBlockType[] {
            new SingleBlockType("MyProgrammableBlock", "LargeProgrammableBlock"),
            new SingleBlockType("MyProgrammableBlock", "LargeProgrammableBlockReskin"),
            new SingleBlockType("MyProgrammableBlock", "SmallProgrammableBlock"),
            new SingleBlockType("MyProgrammableBlock", "SmallProgrammableBlockReskin"),
        };

        private static SingleBlockType[] Assemblers = new SingleBlockType[] {
            new SingleBlockType("Assembler", "BasicAssembler"),
            new SingleBlockType("Assembler", "LargeAssemblerIndustrial"),
            new SingleBlockType("Assembler", "LargeAssembler"),
        };

        private static SingleBlockType[] Refineries = new SingleBlockType[] {
            new SingleBlockType("Refinery", "Blast Furnace"),
            new SingleBlockType("Refinery", "LargeRefineryIndustrial"),
            new SingleBlockType("Refinery", "LargeRefinery"),
        };

        private static SingleBlockType[] O2H2Generators = new SingleBlockType[] {
            new SingleBlockType("OxygenGenerator", "OxygenGeneratorSmall"),
            new SingleBlockType("OxygenGenerator", ""),
        };

        private static SingleBlockType[] Gyros = new SingleBlockType[] {
            new SingleBlockType("Gyro", "SmallBlockGyro"),
            new SingleBlockType("Gyro", "LargeBlockGyro"),
        };

        private static SingleBlockType[] Connectors = new SingleBlockType[] {
            new SingleBlockType("ShipConnector", "Connector"),
            new SingleBlockType("ShipConnector", "ConnectorMedium"),
            new SingleBlockType("ShipConnector", "ConnectorSmall"),
        };

        private static SingleBlockType[] HydrogenTanks = new SingleBlockType[] {
            new SingleBlockType("OxygenTank", "LargeHydrogenTank", 15),
            new SingleBlockType("OxygenTank", "LargeHydrogenTankIndustrial", 15),
            new SingleBlockType("OxygenTank", "LargeHydrogenTankSmall"),
        };

        private static SingleBlockType[] Batteries = new SingleBlockType[] {
            new SingleBlockType("BatteryBlock", "LargeBlockBatteryBlock"),
            new SingleBlockType("BatteryBlock", "LargeBlockBatteryBlockWarfare2"),
        };

        //Build and Repair
        private static SingleBlockType BuildAndRepair = new SingleBlockType("ShipWelder", "SELtdLargeNanobotBuildAndRepairSystem");

        //Energy shields
        private static SingleBlockType[] EnergyShieldGenerators = new SingleBlockType[] {
            new SingleBlockType("Refinery", "LargeShipSmallShieldGeneratorBase"),
            new SingleBlockType("Refinery", "LargeShipLargeShieldGeneratorBase"),
            new SingleBlockType("Refinery", "SmallShipMicroShieldGeneratorBase"),
            new SingleBlockType("UpgradeModule", "ShieldCapacitor"),
        };

        //Star Citizen
        private static SingleBlockType[] SCLargeLasers = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "LG_CF117"),
            new SingleBlockType("ConveyorSorter", "LG_CF227", 2),
            new SingleBlockType("ConveyorSorter", "LG_CF337", 3),
            new SingleBlockType("ConveyorSorter", "LG_M3A"),
            new SingleBlockType("ConveyorSorter", "LG_M4A", 2),
            new SingleBlockType("ConveyorSorter", "LG_M5A", 3),
        };

        private static SingleBlockType[] SCSmallLasers = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "SG_CF117"),
            new SingleBlockType("ConveyorSorter", "SG_CF227", 2),
            new SingleBlockType("ConveyorSorter", "SG_CF337", 3),
            new SingleBlockType("ConveyorSorter", "SG_M3A"),
            new SingleBlockType("ConveyorSorter", "SG_M4A", 2),
            new SingleBlockType("ConveyorSorter", "SG_M5A", 3),
        };

        //TIO
        private static SingleBlockType[] TIOSmallGuns = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "SG_Missile_Bay_Block"),
            new SingleBlockType("ConveyorSorter", "SG_TankCannon_Block"),
            new SingleBlockType("ConveyorSorter", "SG_Vulcan_AutoCannon_Block"),
            new SingleBlockType("ConveyorSorter", "SG_Vulcan_SAMS_Block"),
            new SingleBlockType("ConveyorSorter", "ThunderBoltGatlingGun_Block"),
        };

        private static SingleBlockType[] TIOMissiles = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "SG_Missile_Bay_Block"),
            new SingleBlockType("ConveyorSorter", "VMLS_Block"),
        };

        private static SingleBlockType[] TIOTorpedo = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "Torp_Block", 3),
            new SingleBlockType("ConveyorSorter", "FixedTorpedo_Block", 3),
        };

        private static SingleBlockType[] TIOTorpedoFixed = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "FixedTorpedo_Block", 3),
        };

        private static SingleBlockType[] TIOTorpedoTurret = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "Torp_Block", 3),
        };

        private static SingleBlockType[] TIOSGTorpedo = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "SGTorpedoBay_Block", 3),
            new SingleBlockType("ConveyorSorter", "SGTorpedoBayLeft_Block", 3),
        };

        private static SingleBlockType[] TIOLargeGeneralGuns = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "Laser_Block"),
            new SingleBlockType("ConveyorSorter", "CoilgunMk2_Block"),
            new SingleBlockType("ConveyorSorter", "IronMaiden_Block"),
            new SingleBlockType("ConveyorSorter", "MK1BattleshipGun_Block", 2),
            new SingleBlockType("ConveyorSorter", "MK1Railgun_Block", 2),
            new SingleBlockType("ConveyorSorter", "PDCTurret_Block", 0.5f),
            new SingleBlockType("ConveyorSorter", "PriestReskin_Block", 1),
            new SingleBlockType("ConveyorSorter", "Concordia_Block", 0.5f),
            new SingleBlockType("ConveyorSorter", "MBA57Bofors_Block", 0.5f),
        };

        private static SingleBlockType[] TIOLargeMk2Guns = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "MK2_Battleship_Block", 2),
            new SingleBlockType("ConveyorSorter", "MK2_Railgun_Block", 3),
        };

        private static SingleBlockType[] TIOLargeMk3Guns = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "MK3_Battleship_Block", 3),
            new SingleBlockType("ConveyorSorter", "MK3_Railgun_Block", 4),
        };

        private static SingleBlockType[] Coilgun = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "CoilgunFixedEnd_Block", 1),
            new SingleBlockType("ConveyorSorter", "CoilgunFixedStart_Block", 1),
            new SingleBlockType("ConveyorSorter", "CoilgunFixedCore_Block", 1),
        };

        private static SingleBlockType[] SuperLaser = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "SuperLaserLoader_Block", 1),
            new SingleBlockType("ConveyorSorter", "SuperLaserCore_Block", 2),
            new SingleBlockType("ConveyorSorter", "SuperLaserMuzzle_Block", 1),
            new SingleBlockType("ConveyorSorter", "SuperLaserUpgrade_Block", 1),
        };

        //Stealthdrive
        private static SingleBlockType[] StealthDrives = new SingleBlockType[] {
            new SingleBlockType("UpgradeModule", "StealthDrive"),
            new SingleBlockType("UpgradeModule", "StealthDrive1x1"),
            new SingleBlockType("UpgradeModule", "StealthHeatSink"),
            new SingleBlockType("UpgradeModule", "StealthDriveSmall"),
            new SingleBlockType("UpgradeModule", "StealthHeatSinkSmall"),
        };

        //Misc mods
        private static SingleBlockType[] Barbettes = new SingleBlockType[] {
            new SingleBlockType("CargoContainer", "Ace_CargoContainer_Barbette"),
        };

        private static SingleBlockType[] XLCargo = new SingleBlockType[] {
            new SingleBlockType("CargoContainer", "LargeBlockExtraLargeLongContainer"),
            new SingleBlockType("CargoContainer", "LargeBlockExtraLargeContainer"),
            new SingleBlockType("CargoContainer", "LargeBlockLargeLongContainer"),
        };

        private static SingleBlockType LaserToolFixed = new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool", 1);
        private static SingleBlockType LaserToolTurret = new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool_Turret", 1);

        //XLBlocks
        /*private static BlockType[] XLBlocks = new BlockType[] {
            new BlockType("CubeBlock", "XL_1x"),
            new BlockType("CubeBlock", "XL_1xPlatform"),
            new BlockType("CubeBlock", "XL_1xMount"),
            new BlockType("CubeBlock", "XL_1xFrame"),
            new BlockType("CubeBlock", "XL_Block"),
            new BlockType("CubeBlock", "XL_BlockDetail"),
            new BlockType("CubeBlock", "XL_BlockHazard"),
            new BlockType("CubeBlock", "XL_BlockInvCorner"),
            new BlockType("CubeBlock", "XL_BlockSlope"),
            new BlockType("CubeBlock", "XL_BlockCorner"),
            new BlockType("CubeBlock", "XL_BlockBlockFrame"),
            new BlockType("CubeBlock", "XL_HalfBlock"),
            new BlockType("CubeBlock", "XL_HalfBlockCorner"),
            new BlockType("CubeBlock", "XL_HalfCornerBase"),
            new BlockType("CubeBlock", "XL_HalfCornerTip"),
            new BlockType("CubeBlock", "XL_HalfCornerTipInv"),
            new BlockType("CubeBlock", "XL_HalfCornerBaseInv"),
            new BlockType("CubeBlock", "XL_HalfSlopeBase"),
            new BlockType("CubeBlock", "XL_HalfHalfSlopeTip"),
            new BlockType("CubeBlock", "XL_PassageCorner"),
            new BlockType("CubeBlock", "XL_Passage"),
            new BlockType("CubeBlock", "XL_Hip"),
            new BlockType("CubeBlock", "XL_Brace"),
        };*/

        private static BlockLimit GetHydrogenTankLimit(int max) {
            return new BlockLimit() { Name = "Hydrogen Tanks", MaxCount = max, BlockTypes = HydrogenTanks };
        }
        private static BlockLimit GetBatteryLimit(int max)
        {
            return new BlockLimit() { Name = "Batteries", MaxCount = max, BlockTypes = Batteries };
        }

        private static BlockLimit GetBarbetteLimit(int max)
        {
            return new BlockLimit() { Name = "Barbettes", MaxCount = max, BlockTypes = Barbettes };
        }

        private static BlockLimit ConnectorLimit = new BlockLimit() { Name = "Connectors", MaxCount = 10, BlockTypes = Connectors };
        private static BlockLimit StationConnectorLimit = new BlockLimit() { Name = "Connectors", MaxCount = 20, BlockTypes = Connectors };
        private static BlockLimit PBLimit = new BlockLimit() { Name = "PBs", MaxCount = 1, BlockTypes = ProgrammableBlocks };
        private static BlockLimit NoPBLimit = new BlockLimit() { Name = "PBs", MaxCount = 0, BlockTypes = ProgrammableBlocks };
        private static BlockLimit GyroLimit = new BlockLimit() { Name = "Gyros", MaxCount = 200, BlockTypes = Gyros };
        private static BlockLimit WelderLimit = new BlockLimit() { Name = "Welders", MaxCount = 10, BlockTypes = Welders };
        private static BlockLimit NoDrillsLimit = new BlockLimit() { Name = "Drills", MaxCount = 0, BlockTypes = Drills };
        private static BlockLimit DrillsLimit = new BlockLimit() { Name = "Drills", MaxCount = 80, BlockTypes = Drills };
        private static BlockLimit NoProductionLimit = new BlockLimit() { Name = "Production", MaxCount = 0, BlockTypes = Utils.ConcatArrays(Refineries, Assemblers) };
        private static BlockLimit NoShieldsLimit = new BlockLimit() { Name = "Shields", MaxCount = 0, BlockTypes = EnergyShieldGenerators };
        private static BlockLimit O2H2GeneratorsLimit = new BlockLimit() { Name = "O2/H2 gens", MaxCount = 5, BlockTypes = O2H2Generators };
        private static BlockLimit NoArtilleryLimit = new BlockLimit() { Name = "Artillery", MaxCount = 0, BlockTypes = Artillery };
        private static BlockLimit NoBuildAndRepairLimit = new BlockLimit() { Name = "B&R", MaxCount = 0, BlockTypes = new SingleBlockType[] { BuildAndRepair } };
        private static BlockLimit BuildAndRepairLimit = new BlockLimit() { Name = "B&R", MaxCount = 1, BlockTypes = new SingleBlockType[] { BuildAndRepair } };
        private static BlockLimit NoStealthLimit = new BlockLimit() { Name = "Stealth", MaxCount = 0, BlockTypes = StealthDrives };
        private static BlockLimit NoMissilesLimit = new BlockLimit() { Name = "Missiles", MaxCount = 0, BlockTypes = Utils.ConcatArrays(TIOMissiles, TIOTorpedo) };
        private static BlockLimit NoBigGunsLimit = new BlockLimit() { Name = "Capital Guns", MaxCount = 0, BlockTypes = Utils.ConcatArrays(TIOLargeMk2Guns, TIOLargeMk3Guns, Coilgun, SuperLaser) };
        private static BlockLimit NoTorpedosLimit = new BlockLimit() { Name = "Torpedos", MaxCount = 0, BlockTypes = Utils.ConcatArrays(TIOTorpedo) };
        private static BlockLimit MechanicalLimit = new BlockLimit() { Name = "Mech. Parts", MaxCount = 2, BlockTypes = new SingleBlockType[] { 
            new SingleBlockType("MotorStator", null),
            new SingleBlockType("MotorAdvancedStator", null),
            new SingleBlockType("ExtendedPistonBase", null),
        } };

        private static BlockLimit SuperLaserStart = new BlockLimit() { 
            Name = "Super laser start", 
            MaxCount = 1, 
            BlockTypes = new SingleBlockType[] {
                new SingleBlockType("ConveyorSorter", "SuperLaserLoader_Block", 1),
            }
        };

        private static BlockLimit SuperLaserEnd = new BlockLimit() {
            Name = "Super laser end",
            MaxCount = 1,
            BlockTypes = new SingleBlockType[] {
                new SingleBlockType("ConveyorSorter", "SuperLaserMuzzle_Block", 1),
            }
        };

        private static BlockLimit NoCapitalWeaponsLimit = new BlockLimit() { Name = "Capital Weapons", MaxCount = 0, BlockTypes = Utils.ConcatArrays(Coilgun, SuperLaser) };
        private static BlockLimit NoSuperLaserLimit = new BlockLimit() { Name = "Super Laser", MaxCount = 0, BlockTypes = SuperLaser };

        private static BlockLimit NoXLCargoLimit = new BlockLimit() { Name = "XL Cargo", MaxCount = 0, BlockTypes = XLCargo };
        private static BlockLimit NoLaserTools = new BlockLimit() { Name = "Laser Tools", MaxCount = 0, BlockTypes = new SingleBlockType[] { LaserToolFixed, LaserToolTurret } };

        //private static BlockLimit NoXLBlocksLimit = new BlockLimit() { Name = "XL blocks", MaxCount = 0, BlockTypes = XLBlocks };

        public static GridModifiers DefaultGridModifiers = new GridModifiers()
        {
            ThrusterForce = 1,
            ThrusterEfficiency = 1,
            GyroForce = 0.5f,
            GyroEfficiency = 0.5f,
            AssemblerSpeed = 0,
            DrillHarvestMutiplier = 0,
            PowerProducersOutput = 1,
            RefineEfficiency = 0,
            RefineSpeed = 0
        };

        public static GridClass DefaultGridClassDefinition = new GridClass()
        {
            Id = 0,
            Name = "Derelict",
            SmallGridMobile = true,
            SmallGridStatic = true,
            LargeGridMobile = true,
            LargeGridStatic = true,
            ForceBroadCast = true,
            ForceBroadCastRange = 20000,
            Modifiers = DefaultGridModifiers
        };

        public static ModConfig DefaultModConfig = new ModConfig()
        {
            BlockGroups = new BlockGroup[] {
                new BlockGroup(){
                    Id = "testgroup",
                    BlockTypes = LargeGridFixedWeapons
                }
            },
            DefaultGridClass = DefaultGridClassDefinition,
            GridClasses = new GridClass[] {
                /*new GridClass() {
                    Id = 1,
                    Name = "Fighter",
                    SmallGridMobile = true,
                    MaxBlocks = 1500,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 1500,
                    MaxPerFaction = 7,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 3f,
                        ThrusterEfficiency = 3f,
                        GyroForce = 2f,
                        GyroEfficiency = 2f,
                        DrillHarvestMutiplier = 0,
                        PowerProducersOutput = 1,
                        DamageModifier = 0.5f,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 8, BlockTypes = Utils.ConcatArrays(SmallGridWeapons, SCSmallLasers, TIOMissiles, TIOSGTorpedo, TIOSmallGuns) },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = Utils.ConcatArrays(TIOMissiles) },
                        new BlockLimit() { Name = "Shields", MaxCount = 6, BlockTypes = EnergyShieldGenerators },
                        PBLimit,
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        WelderLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        NoProductionLimit,
                        NoDrillsLimit,
                        new BlockLimit() { Name = "Laser Tools", MaxCount = 0, BlockTypes = new BlockType[] {
                            new BlockType("ConveyorSorter", "SG_Simple_Laser_Multitool"),
                        } },
                    } },
                new GridClass() {
                    Id = 10,
                    Name = "Miner",
                    SmallGridMobile = true,
                    LargeGridMobile = true,
                    MaxBlocks = 1250,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 500,
                    MaxPerFaction = 12,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1.5f,
                        ThrusterEfficiency = 2,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        AssemblerSpeed = 1,
                        DrillHarvestMutiplier = 1.2f,
                        PowerProducersOutput = 1,
                        RefineEfficiency = 1,
                        RefineSpeed = 1
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = Utils.ConcatArrays(SmallGridWeapons, LargeGridWeapons, TIOSmallGuns, TIOLargeGeneralGuns, SCSmallLasers, SCLargeLasers) },
                        DrillsLimit,
                        new BlockLimit() { Name = "Shields", MaxCount = 1, BlockTypes = EnergyShieldGenerators },
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        MechanicalLimit,
                        WelderLimit,
                        ConnectorLimit,
                        NoPBLimit,
                        NoMissilesLimit,
                        NoBigGunsLimit,
                        NoProductionLimit,
                        NoStealthLimit,
                        NoBuildAndRepairLimit,
                        GetBarbetteLimit(0),
                        NoXLCargoLimit,
                        NoLaserTools,
                    } },
                new GridClass() {
                    Id = 11,
                    Name = "PAM Miner",
                    SmallGridMobile = true,
                    LargeGridMobile = true,
                    MaxBlocks = 1000,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 2500,
                    MaxPerFaction = 2,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1,
                        ThrusterEfficiency = 1,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        AssemblerSpeed = 1,
                        DrillHarvestMutiplier = 1.2f,
                        PowerProducersOutput = 1,
                        RefineEfficiency = 1,
                        RefineSpeed = 1
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = Utils.ConcatArrays(SmallGridWeapons, LargeGridWeapons, TIOSmallGuns, TIOLargeGeneralGuns, SCSmallLasers, SCLargeLasers) },
                        new BlockLimit() { Name = "Drills", MaxCount = 40, BlockTypes = new BlockType[] { LargeDrill, SmallDrill } },
                        PBLimit,
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        MechanicalLimit,
                        WelderLimit,
                        ConnectorLimit,
                        NoShieldsLimit,
                        NoMissilesLimit,
                        NoBigGunsLimit,
                        NoProductionLimit,
                        NoStealthLimit,
                        NoBuildAndRepairLimit,
                        GetBarbetteLimit(0),
                        NoXLCargoLimit,
                        NoLaserTools,
                    } },
                new GridClass() {
                    Id = 20,
                    Name = "Industrial Ship",
                    SmallGridMobile = true,
                    LargeGridMobile = true,
                    MaxBlocks = 4000,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 2500,
                    MaxPerFaction = 12,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1,
                        ThrusterEfficiency = 1,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        AssemblerSpeed = 0,
                        DrillHarvestMutiplier = 0,
                        PowerProducersOutput = 1,
                        RefineEfficiency = 0,
                        RefineSpeed = 0,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = Utils.ConcatArrays(SmallGridWeapons, LargeGridWeapons, TIOSmallGuns, TIOLargeGeneralGuns, SCSmallLasers, SCLargeLasers) },
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        MechanicalLimit,
                        WelderLimit,
                        ConnectorLimit,
                        NoPBLimit,
                        NoShieldsLimit,
                        NoDrillsLimit,
                        NoArtilleryLimit,
                        NoMissilesLimit,
                        NoBigGunsLimit,
                        NoProductionLimit,
                        NoStealthLimit,
                        NoBuildAndRepairLimit,
                        GetBarbetteLimit(0),
                        NoXLCargoLimit,
                        NoLaserTools,
                    } },
                new GridClass() {
                    Id = 21,
                    Name = "Builder Ship",
                    SmallGridMobile = true,
                    LargeGridMobile = true,
                    MaxBlocks = 500,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 500,
                    MaxPerFaction = 1,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1,
                        ThrusterEfficiency = 1,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        AssemblerSpeed = 0,
                        DrillHarvestMutiplier = 0,
                        PowerProducersOutput = 1,
                        RefineEfficiency = 0,
                        RefineSpeed = 0,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = Utils.ConcatArrays(SmallGridWeapons, LargeGridWeapons, TIOSmallGuns, TIOLargeGeneralGuns, SCSmallLasers, SCLargeLasers) },
                        new BlockLimit() { Name = "Laser Tools", MaxCount = 3, BlockTypes = new BlockType[] {
                            new BlockType("ConveyorSorter", "LG_Simple_Laser_Multitool"),
                            new BlockType("ConveyorSorter", "LG_Simple_Laser_Multitool_Turret", 3),
                        } },
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        MechanicalLimit,
                        WelderLimit,
                        ConnectorLimit,
                        NoPBLimit,
                        NoShieldsLimit,
                        NoDrillsLimit,
                        NoArtilleryLimit,
                        NoMissilesLimit,
                        NoBigGunsLimit,
                        NoProductionLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        NoXLCargoLimit,
                        NoBuildAndRepairLimit,
                    } },
                new GridClass() {
                    Id = 110,
                    Name = "Outpost",
                    MaxBlocks = 2000,
                    LargeGridStatic = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 1000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 0,
                        ThrusterEfficiency = 0,
                        GyroForce = 0,
                        GyroEfficiency = 0,
                        RefineEfficiency = 1,
                        RefineSpeed = 1,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 1,
                        AssemblerSpeed = 1,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 20, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, TIOLargeMk2Guns, TIOLargeMk3Guns, SCLargeLasers) },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 1, BlockTypes = Assemblers },
                        new BlockLimit() { Name = "Refineries", MaxCount = 1, BlockTypes = Refineries },
                        new BlockLimit() { Name = "Shields", MaxCount = 4, BlockTypes = EnergyShieldGenerators },
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        BuildAndRepairLimit,
                        PBLimit,
                        MechanicalLimit,
                        StationConnectorLimit,
                        NoCapitalWeaponsLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 1600,
                    Name = "Beacon Shrine",
                    MaxBlocks = 500,
                    LargeGridStatic = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 20000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 0,
                        ThrusterEfficiency = 0,
                        GyroForce = 0,
                        GyroEfficiency = 0,
                        RefineEfficiency = 0,
                        RefineSpeed = 0,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 0,
                        AssemblerSpeed = 0,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 0, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, TIOLargeMk2Guns, TIOLargeMk3Guns, SCLargeLasers, Coilgun, SuperLaser) },
                        new BlockLimit() { Name = "Welders", MaxCount = 0, BlockTypes = Welders },
                        new BlockLimit() { Name = "O2/H2 gens", MaxCount = 0, BlockTypes = O2H2Generators },
                        BuildAndRepairLimit,
                        PBLimit,
                        new BlockLimit() { Name = "Mech. Parts", MaxCount = 0, BlockTypes = new BlockType[] {
                            new BlockType("MotorStator", null),
                            new BlockType("MotorAdvancedStator", null),
                            new BlockType("ExtendedPistonBase", null),
                        } },
                        ConnectorLimit,
                        NoProductionLimit,
                        NoDrillsLimit,
                        NoShieldsLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 120,
                    Name = "Assembler Outpost",
                    MaxBlocks = 2000,
                    LargeGridStatic = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 5000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1,
                        ThrusterEfficiency = 1,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        RefineEfficiency = 3,
                        RefineSpeed = 1,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 1,
                        AssemblerSpeed = 50,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 20, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, TIOLargeMk2Guns, TIOLargeMk3Guns, SCLargeLasers) },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 6, BlockTypes = Assemblers },
                        new BlockLimit() { Name = "Refineries", MaxCount = 1, BlockTypes = Refineries },
                        new BlockLimit() { Name = "Shields", MaxCount = 4, BlockTypes = EnergyShieldGenerators },
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        BuildAndRepairLimit,
                        PBLimit,
                        MechanicalLimit,
                        StationConnectorLimit,
                        NoCapitalWeaponsLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 130,
                    Name = "Refinery Outpost",
                    MaxBlocks = 2000,
                    LargeGridStatic = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 5000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1,
                        ThrusterEfficiency = 1,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        RefineEfficiency = 5,
                        RefineSpeed = 30,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 1,
                        AssemblerSpeed = 2,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 20, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, TIOLargeMk2Guns, TIOLargeMk3Guns, SCLargeLasers) },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 1, BlockTypes = Assemblers },
                        new BlockLimit() { Name = "Refineries", MaxCount = 5, BlockTypes = Refineries },
                        new BlockLimit() { Name = "Shields", MaxCount = 4, BlockTypes = EnergyShieldGenerators },
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        BuildAndRepairLimit,
                        PBLimit,
                        MechanicalLimit,
                        StationConnectorLimit,
                        NoCapitalWeaponsLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 100,
                    Name = "Faction base",
                    MaxBlocks = 30000,
                    MinBlocks = 2000,
                    MaxPerFaction = 1,
                    LargeGridStatic = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 20000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 1,
                        ThrusterEfficiency = 1,
                        GyroForce = 1,
                        GyroEfficiency = 1,
                        RefineEfficiency = 3,
                        RefineSpeed = 10,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 1,
                        AssemblerSpeed = 10,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 40, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, TIOLargeMk2Guns, TIOLargeMk3Guns, SCLargeLasers) },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 5, BlockTypes = Assemblers },
                        new BlockLimit() { Name = "Refineries", MaxCount = 5, BlockTypes = Refineries },
                        new BlockLimit() { Name = "B&R", MaxCount = 1, BlockTypes = new BlockType[]{BuildAndRepair } },
                        new BlockLimit() { Name = "Laser Tool Turrets", MaxCount = 4, BlockTypes = new BlockType[] {
                            new BlockType("ConveyorSorter", "LG_Simple_Laser_Multitool_Turret", 1),
                        } },
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        new BlockLimit() { Name = "PBs", MaxCount = 2, BlockTypes = ProgrammableBlocks },
                        MechanicalLimit,
                        new BlockLimit() { Name = "Connectors", MaxCount = 40, BlockTypes = Connectors },
                        NoCapitalWeaponsLimit,
                        NoDrillsLimit,
                        NoShieldsLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        new BlockLimit() { Name = "Fixed Laser Tools", MaxCount = 0, BlockTypes = new BlockType[] {
                            new BlockType("ConveyorSorter", "LG_Simple_Laser_Multitool"),
                        } },
                    }
                },*/
                new GridClass() {
                    Id = 200,
                    Name = "Corvette",
                    MaxBlocks = 1000,
                    LargeGridMobile = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 1000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 6,
                        ThrusterEfficiency = 6,
                        GyroForce = 2,
                        GyroEfficiency = 2,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 0,
                        DamageModifier = 0.6f,
                    },
                    BlockLimits = new BlockLimit[]{
                        //new BlockLimit() { Name = "Weapons", MaxCount = 8, BlockTypes = Utils.ConcatArrays(LargeGridFixedWeapons, SCLargeLasers, TIOMissiles, TIOTorpedoFixed) },
                        //new BlockLimit() { Name = "Shields", MaxCount = 1, BlockTypes = EnergyShieldGenerators },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = Utils.ConcatArrays(TIOMissiles, new BlockType[] { new BlockTypeGroup() { GroupId = "testgroup" }, new SingleBlockType("ConveyorSorter", "FixedTorpedo_Block", 1)})},
                        /*GetHydrogenTankLimit(20),
                        GetBatteryLimit(10),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        new BlockLimit() { Name = "Turrets", MaxCount = 0, BlockTypes = Utils.ConcatArrays(LargeGridTurretWeapons, TIOLargeGeneralGuns, TIOLargeMk2Guns, TIOLargeMk3Guns, TIOTorpedoTurret) },
                        NoCapitalWeaponsLimit,
                        NoProductionLimit,
                        NoBuildAndRepairLimit,
                        NoDrillsLimit,
                        GetBarbetteLimit(0),
                        NoXLCargoLimit,
                        NoLaserTools,*/
                    }
                },
                /*new GridClass() {
                    Id = 250,
                    Name = "Frigate",
                    MaxBlocks = 2000,
                    MaxPerFaction = 6,
                    LargeGridMobile = true,
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
                        DamageModifier = 0.8f,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 15, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, SCLargeLasers, TIOMissiles) },
                        new BlockLimit() { Name = "Missiles", MaxCount = 1, BlockTypes = TIOMissiles },
                        new BlockLimit() { Name = "Mk1 guns", MaxCount = 6, BlockTypes = new BlockType[]{
                            new BlockType("ConveyorSorter", "MK1BattleshipGun_Block", 2),
                            new BlockType("ConveyorSorter", "MK1Railgun_Block", 2), 
                        } },
                        new BlockLimit() { Name = "Shields", MaxCount = 2, BlockTypes = EnergyShieldGenerators },
                        GetHydrogenTankLimit(45),
                        GetBatteryLimit(20),
                        GetBarbetteLimit(2),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        NoBigGunsLimit,
                        NoTorpedosLimit,
                        NoProductionLimit,
                        NoBuildAndRepairLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        NoXLCargoLimit,
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 300,
                    Name = "Destroyer",
                    MaxBlocks = 4000,
                    MinBlocks = 2000,
                    MaxPerFaction = 3,
                    LargeGridMobile = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 4000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 2f,
                        ThrusterEfficiency = 2f,
                        GyroForce = 2,
                        GyroEfficiency = 2,
                        RefineEfficiency = 1,
                        RefineSpeed = 1,
                        PowerProducersOutput = 1.2f,
                        DrillHarvestMutiplier = 0,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 30, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, SCLargeLasers, TIOMissiles, TIOTorpedo, TIOLargeMk2Guns, TIOLargeMk3Guns, Coilgun) },
                        new BlockLimit() { Name = "Missiles", MaxCount = 8, BlockTypes = TIOMissiles },
                        new BlockLimit() { Name = "Torpedos", MaxCount = 6, BlockTypes = TIOTorpedo },
                        new BlockLimit() { Name = "Shields", MaxCount = 5, BlockTypes = EnergyShieldGenerators },
                        GetHydrogenTankLimit(90),
                        GetBatteryLimit(40),
                        GetBarbetteLimit(8),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        NoSuperLaserLimit,
                        NoProductionLimit,
                        NoBuildAndRepairLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        NoXLCargoLimit,
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 400,
                    Name = "Battleship",
                    MaxBlocks = 8000,
                    MinBlocks = 5000,
                    MaxPerFaction = 2,
                    LargeGridMobile = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 7000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 2.5f,
                        ThrusterEfficiency = 2.5f,
                        GyroForce = 4,
                        GyroEfficiency = 2,
                        RefineEfficiency = 0,
                        RefineSpeed = 0,
                        PowerProducersOutput = 1.5f,
                        DrillHarvestMutiplier = 0,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 45, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, SCLargeLasers, TIOMissiles, TIOLargeMk2Guns, TIOLargeMk3Guns, Coilgun) },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = TIOMissiles },
                        new BlockLimit() { Name = "Shields", MaxCount = 10, BlockTypes = EnergyShieldGenerators },
                        GetHydrogenTankLimit(120),
                        GetBatteryLimit(80),
                        GetBarbetteLimit(10),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        NoBuildAndRepairLimit,
                        NoTorpedosLimit,
                        NoSuperLaserLimit,
                        NoProductionLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        NoXLCargoLimit,
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 569,
                    Name = "[Admin] Heavy Dreadnought",
                    MaxBlocks = 40000,
                    MinBlocks = 15000,
                    MaxPerFaction = 1,
                    LargeGridMobile = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 50000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 15f,
                        ThrusterEfficiency = 15f,
                        GyroForce = 15,
                        GyroEfficiency = 15,
                        RefineEfficiency = 1,
                        RefineSpeed = 1,
                        PowerProducersOutput = 10,
                        DrillHarvestMutiplier = 0,
                        AssemblerSpeed = 1,
                        DamageModifier = 0.5f,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Admin only", MaxCount = 1, BlockTypes = new BlockType[]{
                            new BlockType() { TypeId = "ButtonPanel", SubtypeId = "FactionButton", CountWeight = 1 }
                        } },
                    }
                },
                new GridClass() {
                    Id = 500,
                    Name = "Capital",
                    MaxBlocks = 10000,
                    MinBlocks = 6000,
                    MaxPerFaction = 1,
                    LargeGridMobile = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 10000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 3.5f,
                        ThrusterEfficiency = 7f,
                        GyroForce = 5,
                        GyroEfficiency = 5,
                        RefineEfficiency = 1,
                        RefineSpeed = 1,
                        PowerProducersOutput = 1,
                        DrillHarvestMutiplier = 0,
                        AssemblerSpeed = 1,
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 35, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, SCLargeLasers, TIOMissiles, TIOLargeMk2Guns, Coilgun, SuperLaser) },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = TIOMissiles },
                        new BlockLimit() { Name = "Shields", MaxCount = 15, BlockTypes = EnergyShieldGenerators },
                        GetHydrogenTankLimit(150),
                        GetBatteryLimit(120),
                        GetBarbetteLimit(10),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        new BlockLimit() { Name = "Production", MaxCount = 4, BlockTypes = Utils.ConcatArrays(Refineries, Assemblers) },
                        SuperLaserStart,
                        SuperLaserEnd,
                        new BlockLimit() { Name = "Mk3 guns", MaxCount = 0, BlockTypes = TIOLargeMk3Guns },
                        NoTorpedosLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        NoBuildAndRepairLimit,
                        NoXLCargoLimit,
                        NoLaserTools,
                    }
                },
                new GridClass() {
                    Id = 600,
                    Name = "Capital Dreadnought",
                    MaxBlocks = 18000,
                    MinBlocks = 11000,
                    MaxPerFaction = 1,
                    LargeGridMobile = true,
                    ForceBroadCast = true,
                    ForceBroadCastRange = 12000,
                    Modifiers = new GridModifiers() {
                        ThrusterForce = 4f,
                        ThrusterEfficiency = 4f,
                        GyroForce = 5,
                        GyroEfficiency = 5,
                        RefineEfficiency = 1,
                        RefineSpeed = 0,
                        PowerProducersOutput = 1.5f,
                        DrillHarvestMutiplier = 0,
                        AssemblerSpeed = 0,
                        DamageModifier = 0.9f
                    },
                    BlockLimits = new BlockLimit[]{
                        new BlockLimit() { Name = "Weapons", MaxCount = 90, BlockTypes = Utils.ConcatArrays(LargeGridWeapons, TIOLargeGeneralGuns, SCLargeLasers, TIOMissiles, TIOLargeMk2Guns, TIOLargeMk3Guns, Coilgun, SuperLaser) },
                        new BlockLimit() { Name = "Missiles", MaxCount = 4, BlockTypes = TIOMissiles },
                        new BlockLimit() { Name = "Shields", MaxCount = 26, BlockTypes = EnergyShieldGenerators },
                        GetHydrogenTankLimit(270),
                        GetBatteryLimit(200),
                        GetBarbetteLimit(20),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        SuperLaserStart,
                        SuperLaserEnd,
                        NoProductionLimit,
                        NoTorpedosLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        NoBuildAndRepairLimit,
                        NoXLCargoLimit,
                        NoLaserTools
                    }
                }*/
            }
        };
    }
}
