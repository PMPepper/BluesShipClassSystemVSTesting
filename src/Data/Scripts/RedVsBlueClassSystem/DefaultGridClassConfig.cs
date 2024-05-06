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
        private static SingleBlockType[] BuildAndRepair = new SingleBlockType[] {
            new SingleBlockType("ShipWelder", "SELtdLargeNanobotBuildAndRepairSystem")
        };

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

        private static SingleBlockType[] MAC = new SingleBlockType[] {
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

        private static SingleBlockType[] LaserMultitools = new SingleBlockType[] {
            new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool", 1),
            new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool_Turret", 1)
        };

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

        private static BlockLimit GetShieldLimit(int max)
        {
            return new BlockLimit() { Name = "Shields", MaxCount = max, BlockTypes = new BlockType[] { EnergyShieldGeneratorsGroup } };
        }
        private static BlockLimit GetHydrogenTankLimit(int max) {
            return new BlockLimit() { Name = "Hydrogen Tanks", MaxCount = max, BlockTypes = new BlockType[] { HydrogenTanksGroup } };
        }
        private static BlockLimit GetBatteryLimit(int max)
        {
            return new BlockLimit() { Name = "Batteries", MaxCount = max, BlockTypes = new BlockType[] { BatteriesGroup } };
        }

        private static BlockLimit GetBarbetteLimit(int max)
        {
            return new BlockLimit() { Name = "Barbettes", MaxCount = max, BlockTypes = new BlockType[] { BarbettesGroup } };
        }

        private static BlockTypeGroup SmallGridFixedWeaponsGroup = new BlockTypeGroup() { GroupId = "SmallGridFixedWeapons" };
        private static BlockTypeGroup SmallGridTurretWeaponsGroup = new BlockTypeGroup() { GroupId = "SmallGridTurretWeapons" };
        private static BlockTypeGroup LargeGridFixedWeaponsGroup = new BlockTypeGroup() { GroupId = "LargeGridFixedWeapons" };
        private static BlockTypeGroup LargeGridTurretWeaponsGroup = new BlockTypeGroup() { GroupId = "LargeGridTurretWeapons" };
        private static BlockTypeGroup SmallGridWeaponsGroup = new BlockTypeGroup() { GroupId = "SmallGridWeapons" };
        private static BlockTypeGroup LargeGridWeaponsGroup = new BlockTypeGroup() { GroupId = "LargeGridWeapons" };

        private static BlockTypeGroup SCLargeLasersGroup = new BlockTypeGroup() { GroupId = "SCLargeLasers" };
        private static BlockTypeGroup SCSmallLasersGroup = new BlockTypeGroup() { GroupId = "SCSmallLasers" };
        private static BlockTypeGroup TIOSmallGunsGroup = new BlockTypeGroup() { GroupId = "TIOSmallGuns" };
        private static BlockTypeGroup TIOMissilesGroup = new BlockTypeGroup() { GroupId = "TIOMissiles" };
        private static BlockTypeGroup TIOTorpedoGroup = new BlockTypeGroup() { GroupId = "TIOTorpedo" };
        private static BlockTypeGroup TIOTorpedoFixedGroup = new BlockTypeGroup() { GroupId = "TIOTorpedoFixed" };
        private static BlockTypeGroup TIOTorpedoTurretGroup = new BlockTypeGroup() { GroupId = "TIOTorpedoTurret" };
        private static BlockTypeGroup TIOSGTorpedoGroup = new BlockTypeGroup() { GroupId = "TIOSGTorpedo" };
        private static BlockTypeGroup TIOLargeGeneralGunsGroup = new BlockTypeGroup() { GroupId = "TIOLargeGeneralGuns" };
        private static BlockTypeGroup TIOLargeMk2GunsGroup = new BlockTypeGroup() { GroupId = "TIOLargeMk2Guns" };
        private static BlockTypeGroup TIOLargeMk3GunsGroup = new BlockTypeGroup() { GroupId = "TIOLargeMk3Guns" };
        private static BlockTypeGroup MACGroup = new BlockTypeGroup() { GroupId = "MAC" };
        private static BlockTypeGroup SuperLaserGroup = new BlockTypeGroup() { GroupId = "SuperLaser" };

        private static BlockTypeGroup DrillsGroup = new BlockTypeGroup() { GroupId = "Drills" };
        private static BlockTypeGroup WeldersGroup = new BlockTypeGroup() { GroupId = "Welders" };
        private static BlockTypeGroup ProgrammableBlocksGroup = new BlockTypeGroup() { GroupId = "ProgrammableBlocks" };
        private static BlockTypeGroup AssemblersGroup = new BlockTypeGroup() { GroupId = "Assemblers" };
        private static BlockTypeGroup RefineriesGroup = new BlockTypeGroup() { GroupId = "Refineries" };
        private static BlockTypeGroup O2H2GeneratorsGroup = new BlockTypeGroup() { GroupId = "O2H2Generators" };
        private static BlockTypeGroup GyrosGroup = new BlockTypeGroup() { GroupId = "Gyros" };
        private static BlockTypeGroup ConnectorsGroup = new BlockTypeGroup() { GroupId = "Connectors" };
        private static BlockTypeGroup HydrogenTanksGroup = new BlockTypeGroup() { GroupId = "HydrogenTanks" };
        private static BlockTypeGroup BatteriesGroup = new BlockTypeGroup() { GroupId = "Batteries" };
        private static BlockTypeGroup MechanicalPartsGroup = new BlockTypeGroup() { GroupId = "MechanicalParts" };

        private static BlockTypeGroup BuildAndRepairGroup = new BlockTypeGroup() { GroupId = "BuildAndRepair" };
        private static BlockTypeGroup EnergyShieldGeneratorsGroup = new BlockTypeGroup() { GroupId = "EnergyShieldGenerators" };
        private static BlockTypeGroup StealthDrivesGroup = new BlockTypeGroup() { GroupId = "StealthDrives" };
        private static BlockTypeGroup BarbettesGroup = new BlockTypeGroup() { GroupId = "Barbettes" };
        private static BlockTypeGroup XLCargoGroup = new BlockTypeGroup() { GroupId = "XLCargo" };
        private static BlockTypeGroup LaserMultitoolsGroup = new BlockTypeGroup() { GroupId = "LaserMultitools" };

        private static BlockLimit ConnectorLimit = new BlockLimit() { Name = "Connectors", MaxCount = 10, BlockTypes = new BlockType[] { ConnectorsGroup } };
        private static BlockLimit StationConnectorLimit = new BlockLimit() { Name = "Connectors", MaxCount = 20, BlockTypes = new BlockType[] { ConnectorsGroup } };
        private static BlockLimit PBLimit = new BlockLimit() { Name = "PBs", MaxCount = 1, BlockTypes = new BlockType[] { ProgrammableBlocksGroup } };
        private static BlockLimit NoPBLimit = new BlockLimit() { Name = "PBs", MaxCount = 0, BlockTypes = new BlockType[] { ProgrammableBlocksGroup } };
        private static BlockLimit GyroLimit = new BlockLimit() { Name = "Gyros", MaxCount = 200, BlockTypes = new BlockType[] { GyrosGroup } };
        private static BlockLimit WelderLimit = new BlockLimit() { Name = "Welders", MaxCount = 10, BlockTypes = new BlockType[] { WeldersGroup } };
        private static BlockLimit NoDrillsLimit = new BlockLimit() { Name = "Drills", MaxCount = 0, BlockTypes = new BlockType[] { DrillsGroup } };
        private static BlockLimit DrillsLimit = new BlockLimit() { Name = "Drills", MaxCount = 80, BlockTypes = new BlockType[] { DrillsGroup } };
        private static BlockLimit NoProductionLimit = new BlockLimit() { Name = "Production", MaxCount = 0, BlockTypes = new BlockType[] { RefineriesGroup, AssemblersGroup } };
        private static BlockLimit NoShieldsLimit = new BlockLimit() { Name = "Shields", MaxCount = 0, BlockTypes = new BlockType[] { EnergyShieldGeneratorsGroup } };
        private static BlockLimit O2H2GeneratorsLimit = new BlockLimit() { Name = "O2/H2 gens", MaxCount = 5, BlockTypes = new BlockType[] { O2H2GeneratorsGroup } };
        private static BlockLimit NoBuildAndRepairLimit = new BlockLimit() { Name = "B&R", MaxCount = 0, BlockTypes = new BlockType[] { BuildAndRepairGroup } };
        private static BlockLimit BuildAndRepairLimit = new BlockLimit() { Name = "B&R", MaxCount = 1, BlockTypes = new BlockType[] { BuildAndRepairGroup } };
        private static BlockLimit NoStealthLimit = new BlockLimit() { Name = "Stealth", MaxCount = 0, BlockTypes = new BlockType[] { StealthDrivesGroup } };
        private static BlockLimit NoMissilesLimit = new BlockLimit() { Name = "Missiles", MaxCount = 0, BlockTypes = new BlockType[] { TIOMissilesGroup, TIOTorpedoGroup } };
        private static BlockLimit NoBigGunsLimit = new BlockLimit() { Name = "Capital Guns", MaxCount = 0, BlockTypes = new BlockType[] { TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, MACGroup, SuperLaserGroup } };
        private static BlockLimit NoTorpedosLimit = new BlockLimit() { Name = "Torpedos", MaxCount = 0, BlockTypes = new BlockType[] { TIOTorpedoGroup } };
        private static BlockLimit MechanicalLimit = new BlockLimit() { Name = "Mech. Parts", MaxCount = 2, BlockTypes = new BlockType[] { MechanicalPartsGroup } };

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

        private static BlockLimit NoCapitalWeaponsLimit = new BlockLimit() { Name = "Capital Weapons", MaxCount = 0, BlockTypes = new BlockType[] { MACGroup, SuperLaserGroup } };
        private static BlockLimit NoSuperLaserLimit = new BlockLimit() { Name = "Super Laser", MaxCount = 0, BlockTypes = new BlockType[] { SuperLaserGroup } };

        private static BlockLimit NoXLCargoLimit = new BlockLimit() { Name = "XL Cargo", MaxCount = 0, BlockTypes = new BlockType[] { XLCargoGroup } };
        private static BlockLimit NoLaserTools = new BlockLimit() { Name = "Laser Tools", MaxCount = 0, BlockTypes = new BlockType[] { LaserMultitoolsGroup } };

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
                    Id = "SmallGridFixedWeapons",
                    BlockTypes = SmallGridFixedWeapons
                },
                new BlockGroup(){
                    Id = "SmallGridTurretWeapons",
                    BlockTypes = SmallGridTurretWeapons
                },
                new BlockGroup(){
                    Id = "LargeGridFixedWeapons",
                    BlockTypes = LargeGridFixedWeapons
                },
                new BlockGroup(){
                    Id = "LargeGridTurretWeapons",
                    BlockTypes = LargeGridTurretWeapons
                },
                new BlockGroup(){
                    Id = "SmallGridWeapons",
                    BlockTypes = SmallGridFixedWeapons.Concat(SmallGridTurretWeapons).ToArray()
                },
                new BlockGroup(){
                    Id = "LargeGridWeapons",
                    BlockTypes = LargeGridFixedWeapons.Concat(LargeGridTurretWeapons).ToArray()
                },
                new BlockGroup(){
                    Id = "SCLargeLasers",
                    BlockTypes = SCLargeLasers
                },
                new BlockGroup(){
                    Id = "SCSmallLasers",
                    BlockTypes = SCSmallLasers
                },
                new BlockGroup(){
                    Id = "TIOSmallGuns",
                    BlockTypes = TIOSmallGuns
                },
                new BlockGroup(){
                    Id = "TIOMissiles",
                    BlockTypes = TIOMissiles
                },
                new BlockGroup(){
                    Id = "TIOTorpedo",
                    BlockTypes = TIOTorpedo
                },
                new BlockGroup(){
                    Id = "TIOTorpedoFixed",
                    BlockTypes = TIOTorpedoFixed
                },
                new BlockGroup(){
                    Id = "TIOTorpedoTurret",
                    BlockTypes = TIOTorpedoTurret
                },
                new BlockGroup(){
                    Id = "TIOSGTorpedo",
                    BlockTypes = TIOSGTorpedo
                },
                new BlockGroup(){
                    Id = "TIOLargeGeneralGuns",
                    BlockTypes = TIOLargeGeneralGuns
                },
                new BlockGroup(){
                    Id = "TIOLargeMk2Guns",
                    BlockTypes = TIOLargeMk2Guns
                },
                new BlockGroup(){
                    Id = "TIOLargeMk3Guns",
                    BlockTypes = TIOLargeMk3Guns
                },
                new BlockGroup(){
                    Id = "MAC",
                    BlockTypes = MAC
                },
                new BlockGroup(){
                    Id = "SuperLaser",
                    BlockTypes = SuperLaser
                },
                new BlockGroup(){
                    Id = "Drills",
                    BlockTypes = Drills
                },
                new BlockGroup(){
                    Id = "Welders",
                    BlockTypes = Welders
                },
                new BlockGroup(){
                    Id = "ProgrammableBlocks",
                    BlockTypes = ProgrammableBlocks
                },
                new BlockGroup(){
                    Id = "Assemblers",
                    BlockTypes = Assemblers
                },
                new BlockGroup(){
                    Id = "Refineries",
                    BlockTypes = Refineries
                },
                new BlockGroup(){
                    Id = "O2H2Generators",
                    BlockTypes = O2H2Generators
                },
                new BlockGroup(){
                    Id = "Gyros",
                    BlockTypes = Gyros
                },
                new BlockGroup(){
                    Id = "Connectors",
                    BlockTypes = Connectors
                },
                new BlockGroup(){
                    Id = "HydrogenTanks",
                    BlockTypes = HydrogenTanks
                },
                new BlockGroup(){
                    Id = "Batteries",
                    BlockTypes = Batteries
                },
                new BlockGroup(){
                    Id = "MechanicalParts",
                    BlockTypes = new SingleBlockType[] {
                        new SingleBlockType("MotorStator", null),
                        new SingleBlockType("MotorAdvancedStator", null),
                        new SingleBlockType("ExtendedPistonBase", null),
                    }
                },
                new BlockGroup(){
                    Id = "BuildAndRepair",
                    BlockTypes = BuildAndRepair
                },
                new BlockGroup(){
                    Id = "EnergyShieldGenerators",
                    BlockTypes = EnergyShieldGenerators
                },
                new BlockGroup(){
                    Id = "StealthDrives",
                    BlockTypes = StealthDrives
                },
                new BlockGroup(){
                    Id = "Barbettes",
                    BlockTypes = Barbettes
                },
                new BlockGroup(){
                    Id = "XLCargo",
                    BlockTypes = XLCargo
                },
                new BlockGroup(){
                    Id = "LaserMultitools",
                    BlockTypes = LaserMultitools
                },
            },
            DefaultGridClass = DefaultGridClassDefinition,
            GridClasses = new GridClass[] {
                new GridClass() {
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 8, BlockTypes = new BlockType[] { SmallGridWeaponsGroup, SCSmallLasersGroup, TIOMissilesGroup, TIOSGTorpedoGroup, TIOSmallGunsGroup } },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = new BlockType[] { TIOMissilesGroup } },
                        GetShieldLimit(6),
                        PBLimit,
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        WelderLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        NoProductionLimit,
                        NoDrillsLimit,
                        new BlockLimit() { Name = "Laser Tools", MaxCount = 0, BlockTypes = new BlockType[] {
                            new SingleBlockType("ConveyorSorter", "SG_Simple_Laser_Multitool"),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = new BlockType[] { SmallGridWeaponsGroup, LargeGridWeaponsGroup, TIOSmallGunsGroup, TIOLargeGeneralGunsGroup, SCSmallLasersGroup, SCLargeLasersGroup } },
                        DrillsLimit,
                        GetShieldLimit(1),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = new BlockType[] { SmallGridWeaponsGroup, LargeGridWeaponsGroup, TIOSmallGunsGroup, TIOLargeGeneralGunsGroup, SCSmallLasersGroup, SCLargeLasersGroup } },
                        new BlockLimit() { Name = "Drills", MaxCount = 40, BlockTypes = new BlockType[] { DrillsGroup } },
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = new BlockType[] { SmallGridWeaponsGroup, LargeGridWeaponsGroup, TIOSmallGunsGroup, TIOLargeGeneralGunsGroup, SCSmallLasersGroup, SCLargeLasersGroup } },
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        MechanicalLimit,
                        WelderLimit,
                        ConnectorLimit,
                        NoPBLimit,
                        NoShieldsLimit,
                        NoDrillsLimit,
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 6, BlockTypes = new BlockType[]{ SmallGridWeaponsGroup, LargeGridWeaponsGroup, TIOSmallGunsGroup, TIOLargeGeneralGunsGroup, SCSmallLasersGroup, SCLargeLasersGroup } },
                        new BlockLimit() { Name = "Laser Tools", MaxCount = 3, BlockTypes = new BlockType[] {
                            new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool"),
                            new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool_Turret", 3),
                        } },
                        GyroLimit,
                        O2H2GeneratorsLimit,
                        MechanicalLimit,
                        WelderLimit,
                        ConnectorLimit,
                        NoPBLimit,
                        NoShieldsLimit,
                        NoDrillsLimit,
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 20, BlockTypes = new BlockType[] { LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, SCLargeLasersGroup } },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 1, BlockTypes = new BlockType[] { AssemblersGroup } },
                        new BlockLimit() { Name = "Refineries", MaxCount = 1, BlockTypes = new BlockType[] { RefineriesGroup } },
                        GetShieldLimit(4),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 0, BlockTypes = new BlockType[] { LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, SCLargeLasersGroup, MACGroup, SuperLaserGroup } },
                        new BlockLimit() { Name = "Welders", MaxCount = 0, BlockTypes = new BlockType[] { WeldersGroup } },
                        new BlockLimit() { Name = "O2/H2 gens", MaxCount = 0, BlockTypes = new BlockType[] { O2H2GeneratorsGroup } },
                        BuildAndRepairLimit,
                        PBLimit,
                        new BlockLimit() { Name = "Mech. Parts", MaxCount = 0, BlockTypes = new BlockType[] {
                            MechanicalPartsGroup
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 20, BlockTypes = new BlockType[] { LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, SCLargeLasersGroup } },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 5, BlockTypes = new BlockType[] { AssemblersGroup } },
                        new BlockLimit() { Name = "Refineries", MaxCount = 1, BlockTypes = new BlockType[] { RefineriesGroup } },
                        GetShieldLimit(4),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 20, BlockTypes = new BlockType[] { LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, SCLargeLasersGroup } },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 1, BlockTypes = new BlockType[] { AssemblersGroup } },
                        new BlockLimit() { Name = "Refineries", MaxCount = 5, BlockTypes = new BlockType[] { RefineriesGroup } },
                        GetShieldLimit(4),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 40, BlockTypes = new BlockType[]{ LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, SCLargeLasersGroup, TIOMissilesGroup, TIOTorpedoGroup } },
                        new BlockLimit() { Name = "Assemblers", MaxCount = 5, BlockTypes = new BlockType[]{ AssemblersGroup } },
                        new BlockLimit() { Name = "Refineries", MaxCount = 5, BlockTypes = new BlockType[]{ RefineriesGroup } },
                        new BlockLimit() { Name = "B&R", MaxCount = 1, BlockTypes = new BlockType[]{ BuildAndRepairGroup } },
                        new BlockLimit() { Name = "Laser Tool Turrets", MaxCount = 4, BlockTypes = new BlockType[] {
                            new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool_Turret", 1),
                        } },
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        new BlockLimit() { Name = "PBs", MaxCount = 2, BlockTypes = new BlockType[]{ ProgrammableBlocksGroup } },
                        MechanicalLimit,
                        new BlockLimit() { Name = "Connectors", MaxCount = 40, BlockTypes = new BlockType[]{ ConnectorsGroup } },
                        NoCapitalWeaponsLimit,
                        NoDrillsLimit,
                        NoShieldsLimit,
                        NoStealthLimit,
                        GetBarbetteLimit(0),
                        new BlockLimit() { Name = "Fixed Laser Tools", MaxCount = 0, BlockTypes = new BlockType[] {
                            new SingleBlockType("ConveyorSorter", "LG_Simple_Laser_Multitool"),
                        } },
                    }
                },
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 8, BlockTypes = new BlockType[] { LargeGridFixedWeaponsGroup, SCLargeLasersGroup, TIOMissilesGroup, TIOTorpedoFixedGroup } },
                        GetShieldLimit(1),
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = Utils.ConcatArrays(TIOMissiles, new BlockType[] { new SingleBlockType("ConveyorSorter", "FixedTorpedo_Block", 1)})},
                        GetHydrogenTankLimit(20),
                        GetBatteryLimit(10),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        new BlockLimit() { Name = "Turrets", MaxCount = 0, BlockTypes = new BlockType[]{ LargeGridTurretWeaponsGroup, TIOLargeGeneralGunsGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, TIOTorpedoTurretGroup } },
                        NoCapitalWeaponsLimit,
                        NoProductionLimit,
                        NoBuildAndRepairLimit,
                        NoDrillsLimit,
                        GetBarbetteLimit(0),
                        NoXLCargoLimit,
                        NoLaserTools,
                    }
                },
                new GridClass() {
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 15, BlockTypes = new BlockType[]{ LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, SCLargeLasersGroup, TIOMissilesGroup } },
                        new BlockLimit() { Name = "Missiles", MaxCount = 1, BlockTypes = new BlockType[]{ TIOMissilesGroup } },
                        new BlockLimit() { Name = "Mk1 guns", MaxCount = 6, BlockTypes = new BlockType[]{
                            new SingleBlockType("ConveyorSorter", "MK1BattleshipGun_Block", 2),
                            new SingleBlockType("ConveyorSorter", "MK1Railgun_Block", 2), 
                        } },
                        GetShieldLimit(2),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 30, BlockTypes = new BlockType[]{ LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, SCLargeLasersGroup, TIOMissilesGroup, TIOTorpedoGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, MACGroup } },
                        new BlockLimit() { Name = "Missiles", MaxCount = 8, BlockTypes = new BlockType[]{ TIOMissilesGroup } },
                        new BlockLimit() { Name = "Torpedos", MaxCount = 6, BlockTypes = new BlockType[]{ TIOTorpedoGroup } },
                        GetShieldLimit(5),
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 45, BlockTypes = new BlockType[] { LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, SCLargeLasersGroup, TIOMissilesGroup, TIOLargeMk2GunsGroup, TIOLargeMk3GunsGroup, MACGroup } },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = new BlockType[] { TIOMissilesGroup } },
                        GetShieldLimit(10),
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
                        new BlockLimit() { Name = "Admin only", MinCount = 1, BlockTypes = new BlockType[]{
                            new SingleBlockType() { TypeId = "ButtonPanel", SubtypeId = "FactionButton", CountWeight = 1 }
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
                        new BlockLimit() { Name = "Weapons", MaxCount = 35, BlockTypes = new BlockType[]{ LargeGridWeaponsGroup, TIOLargeGeneralGunsGroup, SCLargeLasersGroup, TIOMissilesGroup, TIOLargeMk2GunsGroup, MACGroup, SuperLaserGroup } },
                        new BlockLimit() { Name = "Missiles", MaxCount = 2, BlockTypes = TIOMissiles },
                        GetShieldLimit(15),
                        GetHydrogenTankLimit(150),
                        GetBatteryLimit(120),
                        GetBarbetteLimit(10),
                        WelderLimit,
                        O2H2GeneratorsLimit,
                        GyroLimit,
                        PBLimit,
                        MechanicalLimit,
                        ConnectorLimit,
                        new BlockLimit() { Name = "Production", MaxCount = 4, BlockTypes = new BlockType[]{ RefineriesGroup, AssemblersGroup } },
                        SuperLaserStart,
                        SuperLaserEnd,
                        new BlockLimit() { Name = "Mk3 guns", MaxCount = 0, BlockTypes = new BlockType[]{ TIOLargeMk3GunsGroup } },
                        NoTorpedosLimit,
                        NoDrillsLimit,
                        NoStealthLimit,
                        NoBuildAndRepairLimit,
                        NoXLCargoLimit,
                        NoLaserTools,
                    }
                },
                /*new GridClass() {
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
