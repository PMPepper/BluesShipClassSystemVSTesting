using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Extensions;
using VRageMath;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace YourName.ModName.thya
{
    class Program : MyGridProgram
    {

/* 
R e a d m e 
----------- 
Updates by jTurp on July 10, 2023: 
 
[*] Updated the Defense Shield API to newest version 
 
How to use for multi-surface blocks: 
 
[1] Place the LCD Prefix somewhere in the NAME of the block -> Example: Industrial Cockpit [Shield LCD] 
[2] The script will search the CUSTOM DATA of the block for the beginning of its configuration information 
[2.1] If not found, the script will insert this section in the configuration and write it back to the block's custom data 
[2.2] If found, the script will retrieve the given configuration and use it to display stuff on the block's surfaces 
 
NOTES: 
[1] This script now allows for a preface to be placed in the CUSTOM DATA - anything that is written BEFORE the script's config will stay there 
[2] The script will also append three dashes (---) AFTER the end of its configuration information. DO NOT insert any extra data BEFORE the dashes or it may break the script! 
[3] The CUSTOM DATA of the block will contain instructions on how to configure the settings once it has successfully parsed the configuration 
*/

/* 
V1.7.6 
 
Welcome to THYA's Shield HUD Script. 
------------------------------------------------------------------------------------------------------------------------------------- 
Setup Instructions: 
 
1. Install one or both of the following energy shield mods: 
  Energy Shields by Cython: http://steamcommunity.com/workshop/filedetails/?id=484504816 
    AND / OR 
  Defense Shield Mod Pack by DarkStar: http://steamcommunity.com/sharedfiles/filedetails/?id=1365616918 
 
2. Install the THYA Shield HUD Graphics Pack if you wish to use LCD Images: 
    (THYA) Shield HUD Graphics Pack 1%: http://steamcommunity.com/sharedfiles/filedetails/?id=540003236 
 
3. An LCD's name is what decides its display type. If you desire to change the default name change the text in "Quotes". 
 
Edit the first half your LCD's name to the text of lcdNamePrefix. */
const string lcdNamePrefix = "[Shield LCD"; /* 
 
    Edit the second half of your LCD's name to a desired lcdMode string. */
const string lcdModeTDS = ":TDS]"; // Text Display Only - Small Font 
const string lcdModeTDL = ":TDL]"; // Text Display Only - Large Font 
const string lcdModeBTS = ":BTS]"; // Bars and Text Shield Display - Small Font 
const string lcdModeBTL = ":BTL]"; // Bars and Text Shield Display - Large Font 
const string lcdModeCTD = ":CTD]"; // Corner LCD Text Display Only 
const string lcdModeCRB = ":CRB]"; // Corner LCD Rainbow Bar 
const string lcdModeCCB = ":CCB]"; // Corner LCD Colored Bar 
const string lcdModeGfxH = ":THYA-H]"; // Horizontal Shield Bar Images 
const string lcdModeGfxV = ":THYA-V]"; // Vertical Shield Bar Images 
const string lcdModeGfxC = ":THYA-C]"; /* Curved Shield Bar Images 
 
    Example LCD Names: 
    [Shield LCD:THYA-H] 
    [Shield LCD:BTL] 
    [Shield LCD:CCB] 
 
    -Note on the CRB + CCB Modes text overlays are changed in the "Custom Data" of the LCD block. 
        • Blank, or any random text not covered below - shows horizontal "Shield" overlay. 
        • SR gets rotated "Shield" overlay (for vertical screen installation). 
        • ES gets "Energy Shield" overlay (ESR gets rotated "E Shield") - only displays energy shield charge. 
        • DS gets "Defense Shield" overlay (DSR gets rotated "D Shield") - only displays defense shield charge. 
        • SGDUAL gets both displayed on small grids (SGDUALR gets rotated) - not enough room for this on a large grid. 
            *SGDUAL does not mix display types - all rainbow or all colored, not one of each. 
 
    ------------------------------------------------------------------------------------------------------------------------------------- 
    Advanced Settings: 
 
    Text Bar Settings. */
const string lBarSurr = " {"; // Left bar surround 
const string rBarSurr = "}"; // Right bar surround 
const string BarEmpty = ".."; // Blank Filler for bar 
const string BarFill = "[]"; /* Used to fill the bars with text 
 
    Text Bar Color Setting. */
Color txtColHigh = Color.Green;
Color txtColMed = Color.Yellow;
Color txtColLow = Color.Orange;
Color txtColCrit = Color.Red; /* 
 
    Text Font Size Settings. */
const float lFontSize = 3.0f; // Large 
const float sFontSize = 2.5f; // Small 
const float cFontSize = 8.0f; // Corner 
const float barFontSize = 0.375f; // CCB and CRB modes 
const float lFontSizeCP = 1.5f; // Large Cockpit 
const float sFontSizeCP = 1.0f; /* Small Cockpit 
 
    Text Based Shield Color Change Percentages. */
const int shieldHigh = 74; // High shield value 
const int shieldLow = 49; // Low shield value 
const int shieldCrit = 24; /* Critical shield value 
 
    Sound Block Settings. */
const string shieldAudioName = "[Shield Alarm]";
const int shieldAudioWarning = 24; /* Sound Block Activation Percentage 
 
    Critical Shield Light Settings. */
const string shieldLightName = "[Critical Shield Light]";
const int shieldVisualWarning = 24; /* Light activation Percentage 
 
    ------------------------------------------------------------------------------------------------------------------------------------- 
    Troubleshooting: 
 
    -Make sure your LCD is named in the correct format. Examples [Shield LCD:THYA-C] or [Shield LCD:BTS] 
    -Click the edit button on the program block then click "remember and exit" then re-run the script. 
    -Shield generators bug out sometimes so you may just need to reload the game. 
    -It is possible you accidentally bugged the code so try deleting the code and reloading it off the workshop. 
    -Load a new world with only the shield and graphics mods. Some other mods may conflict with the script. 
    -If you are a server operator DO NOT subscribe the server to this script. It will cause numerous issues. Each user must subscribe to this script individually. 
	    If you notice your server has subscribed to this script please inform your server moderator. 
    -If you are still having issues please contact us. 
 
 
    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 
    +++++++++++++++++++++++ Modify below this point at your own risk +++++++++++++++++++++++ 
    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ */

public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10 | UpdateFrequency.Update100;
    _updateLCDs = UpdateLCDs();
}

HashSet<string> shieldTypesCython = new HashSet<string>()
{
  "SmallShipSmallShieldGeneratorBase",
  "SmallShipMicroShieldGeneratorBase",
  "LargeShipSmallShieldGeneratorBase",
  "LargeShipLargeShieldGeneratorBase"
};

HashSet<string> shieldTypesDStar = new HashSet<string>()
{
  "DSControlLarge",
  "DSControlSmall",
  "DSControlTable"
};

char[] COLON = new char[1] { ':' };
char[] SPACE_PARENS = new char[3] { ' ', '(', ')' };
char[] FWD_SLASH = new char[1] { '/' };

int[] shieldOnlyG = { 56, 57, 58, 59, 63, 64, 65, 68, 69, 70, 72, 73, 74, 75, 76, 77, 78, 80, 81, 82, 83, 84, 85, 86, 87, 89, 90, 91, 98, 99, 100, 101, 102, 103, 216, 221, 224, 226, 229, 231, 233, 239, 241, 248, 250, 252, 259, 265, 376, 379, 380, 383, 385, 387, 390, 392, 394, 395, 396, 398, 399, 400, 402, 404, 405, 406, 407, 408, 409, 411, 413, 420, 422, 423, 424, 427, 537, 539, 542, 543, 544, 546, 548, 551, 553, 557, 559, 563, 565, 572, 574, 581, 583, 586, 588, 698, 701, 702, 703, 707, 709, 710, 711, 712, 714, 718, 720, 724, 726, 727, 728, 729, 730, 733, 735, 742, 744, 747, 749, 860, 865, 868, 875, 879, 881, 885, 891, 894, 896, 903, 905, 908, 910, 1022, 1023, 1024, 1027, 1029, 1031, 1032, 1033, 1034, 1036, 1040, 1042, 1046, 1048, 1049, 1050, 1051, 1052, 1055, 1057, 1064, 1066, 1069, 1071, 1186, 1188, 1190, 1192, 1195, 1197, 1201, 1203, 1207, 1209, 1216, 1218, 1225, 1227, 1230, 1232, 1342, 1343, 1344, 1347, 1349, 1351, 1353, 1356, 1358, 1362, 1364, 1368, 1370, 1377, 1379, 1386, 1388, 1391, 1393, 1503, 1506, 1507, 1510, 1512, 1514, 1517, 1519, 1521, 1522, 1523, 1525, 1526, 1527, 1529, 1531, 1532, 1533, 1534, 1535, 1536, 1538, 1540, 1541, 1542, 1543, 1544, 1545, 1547, 1549, 1550, 1551, 1554, 1665, 1670, 1673, 1675, 1678, 1680, 1682, 1688, 1690, 1697, 1699, 1706, 1708, 1714, 1827, 1828, 1829, 1830, 1834, 1835, 1836, 1839, 1840, 1841, 1843, 1844, 1845, 1846, 1847, 1848, 1849, 1851, 1852, 1853, 1854, 1855, 1856, 1857, 1858, 1860, 1861, 1862, 1863, 1864, 1865, 1866, 1867, 1869, 1870, 1871, 1872, 1873, 1874 };

int[] shieldOnlyW = { 56, 57, 58, 59, 64, 69, 73, 74, 75, 76, 77, 81, 82, 83, 84, 85, 86, 90, 99, 100, 101, 102, 103, 216, 217, 220, 221, 225, 230, 236, 242, 251, 260, 264, 265, 377, 386, 391, 397, 403, 412, 421, 426, 538, 539, 547, 552, 558, 564, 573, 582, 587, 700, 701, 702, 703, 708, 709, 710, 711, 712, 713, 719, 725, 726, 727, 728, 729, 734, 743, 748, 864, 865, 869, 874, 880, 886, 895, 904, 909, 1026, 1030, 1035, 1041, 1047, 1056, 1065, 1070, 1187, 1191, 1196, 1202, 1208, 1217, 1226, 1231, 1343, 1344, 1347, 1348, 1352, 1357, 1363, 1369, 1378, 1387, 1391, 1392, 1505, 1506, 1507, 1508, 1513, 1518, 1522, 1523, 1524, 1525, 1526, 1530, 1531, 1532, 1533, 1534, 1535, 1539, 1540, 1541, 1542, 1543, 1544, 1548, 1549, 1550, 1551, 1552 };

int[] shieldOnlyRotG = { 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 81, 82, 83, 90, 91, 92, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 109, 110, 114, 115, 116, 203, 214, 216, 227, 229, 240, 242, 244, 251, 253, 255, 266, 269, 271, 274, 278, 364, 366, 367, 368, 369, 370, 371, 372, 373, 375, 377, 379, 380, 381, 382, 383, 384, 385, 386, 387, 388, 390, 392, 393, 394, 395, 397, 398, 399, 401, 403, 405, 406, 407, 408, 409, 410, 411, 412, 414, 416, 417, 418, 419, 420, 421, 423, 424, 425, 426, 427, 429, 432, 434, 437, 440, 525, 527, 534, 536, 538, 540, 551, 553, 556, 558, 560, 562, 564, 575, 582, 584, 590, 592, 595, 597, 599, 601, 686, 688, 695, 697, 699, 701, 712, 714, 717, 719, 721, 723, 725, 727, 728, 729, 730, 731, 732, 733, 734, 736, 743, 745, 751, 753, 756, 758, 760, 762, 847, 850, 851, 852, 853, 854, 855, 858, 860, 862, 873, 875, 878, 880, 882, 884, 886, 888, 895, 897, 899, 900, 901, 902, 903, 904, 906, 907, 908, 909, 910, 912, 915, 916, 919, 920, 923, 1009, 1018, 1021, 1023, 1034, 1036, 1039, 1040, 1041, 1043, 1045, 1047, 1048, 1049, 1056, 1057, 1058, 1060, 1071, 1074, 1079, 1081, 1083, 1171, 1172, 1173, 1174, 1175, 1176, 1177, 1178, 1182, 1183, 1184, 1195, 1196, 1197, 1204, 1205, 1206, 1221, 1222, 1223, 1224, 1225, 1226, 1227, 1228, 1229, 1230, 1231, 1232, 1236, 1237, 1238, 1239, 1242, 1243 };

int[] shieldOnlyRotW = { 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 82, 91, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 109, 114, 115, 116, 204, 213, 217, 230, 235, 239, 243, 252, 261, 269, 270, 274, 275, 277, 278, 365, 374, 378, 391, 396, 400, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 422, 430, 435, 439, 526, 535, 539, 552, 557, 561, 565, 574, 583, 591, 596, 600, 687, 688, 695, 696, 700, 713, 718, 722, 726, 735, 744, 752, 753, 756, 757, 760, 761, 849, 850, 851, 852, 853, 854, 855, 856, 861, 874, 883, 900, 901, 902, 903, 904, 905, 906, 907, 908, 909, 914, 915, 916, 917, 921 };

int[] energyShieldG = { 24, 25, 26, 27, 28, 29, 30, 31, 33, 34, 35, 38, 39, 40, 42, 43, 44, 45, 46, 47, 48, 49, 51, 52, 53, 54, 55, 56, 62, 63, 64, 65, 69, 70, 71, 73, 74, 75, 86, 87, 88, 89, 93, 94, 95, 98, 99, 100, 102, 103, 104, 105, 106, 107, 108, 110, 111, 112, 113, 114, 115, 116, 117, 119, 120, 121, 128, 129, 130, 131, 132, 133, 185, 192, 194, 196, 199, 201, 203, 210, 212, 218, 222, 227, 230, 232, 234, 236, 246, 251, 254, 256, 259, 261, 263, 269, 271, 278, 280, 282, 289, 295, 346, 348, 349, 350, 351, 352, 353, 355, 358, 360, 362, 364, 366, 367, 368, 369, 370, 371, 373, 375, 376, 377, 380, 382, 385, 386, 389, 391, 393, 395, 397, 406, 409, 410, 413, 415, 417, 420, 422, 424, 425, 426, 428, 429, 430, 432, 434, 435, 436, 437, 438, 439, 441, 443, 450, 452, 453, 454, 457, 507, 509, 516, 519, 521, 523, 525, 527, 534, 536, 539, 541, 543, 545, 548, 549, 550, 552, 555, 558, 567, 569, 572, 573, 574, 576, 578, 581, 583, 587, 589, 593, 595, 602, 604, 611, 613, 616, 618, 668, 670, 671, 672, 673, 674, 677, 679, 681, 682, 684, 686, 688, 689, 690, 691, 692, 695, 697, 698, 699, 702, 704, 706, 714, 716, 718, 728, 731, 732, 733, 737, 739, 740, 741, 742, 744, 748, 750, 754, 756, 757, 758, 759, 760, 763, 765, 772, 774, 777, 779, 829, 835, 838, 840, 842, 843, 845, 847, 853, 856, 862, 865, 867, 869, 870, 871, 872, 875, 879, 890, 895, 898, 905, 909, 911, 915, 921, 924, 926, 933, 935, 938, 940, 990, 992, 993, 994, 995, 996, 999, 1001, 1002, 1004, 1006, 1008, 1010, 1011, 1012, 1013, 1014, 1017, 1019, 1020, 1022, 1026, 1028, 1030, 1033, 1036, 1037, 1039, 1040, 1052, 1053, 1054, 1057, 1059, 1061, 1062, 1063, 1064, 1066, 1070, 1072, 1076, 1078, 1079, 1080, 1081, 1082, 1085, 1087, 1094, 1096, 1099, 1101, 1151, 1153, 1160, 1162, 1163, 1165, 1167, 1169, 1171, 1178, 1180, 1182, 1184, 1187, 1189, 1191, 1192, 1194, 1198, 1200, 1216, 1218, 1220, 1222, 1225, 1227, 1231, 1233, 1237, 1239, 1246, 1248, 1255, 1257, 1260, 1262, 1312, 1314, 1321, 1323, 1325, 1328, 1330, 1332, 1339, 1341, 1343, 1345, 1348, 1350, 1353, 1355, 1359, 1361, 1372, 1373, 1374, 1377, 1379, 1381, 1383, 1386, 1388, 1392, 1394, 1398, 1400, 1407, 1409, 1416, 1418, 1421, 1423, 1473, 1475, 1476, 1477, 1478, 1479, 1480, 1482, 1484, 1486, 1489, 1491, 1493, 1494, 1495, 1496, 1497, 1498, 1500, 1502, 1505, 1507, 1509, 1512, 1513, 1516, 1520, 1522, 1533, 1536, 1537, 1540, 1542, 1544, 1547, 1549, 1551, 1552, 1553, 1555, 1556, 1557, 1559, 1561, 1562, 1563, 1564, 1565, 1566, 1568, 1570, 1571, 1572, 1573, 1574, 1575, 1577, 1579, 1580, 1581, 1584, 1634, 1641, 1643, 1645, 1648, 1650, 1652, 1659, 1661, 1663, 1666, 1668, 1671, 1676, 1681, 1683, 1695, 1700, 1703, 1705, 1708, 1710, 1712, 1718, 1720, 1727, 1729, 1736, 1738, 1744, 1795, 1796, 1797, 1798, 1799, 1800, 1801, 1802, 1804, 1805, 1806, 1809, 1810, 1811, 1813, 1814, 1815, 1816, 1817, 1818, 1819, 1820, 1822, 1823, 1824, 1827, 1828, 1829, 1833, 1834, 1835, 1836, 1842, 1843, 1844, 1857, 1858, 1859, 1860, 1864, 1865, 1866, 1869, 1870, 1871, 1873, 1874, 1875, 1876, 1877, 1878, 1879, 1881, 1882, 1883, 1884, 1885, 1886, 1887, 1888, 1890, 1891, 1892, 1893, 1894, 1895, 1896, 1897, 1899, 1900, 1901, 1902, 1903, 1904 };

int[] energyShieldW = { 25, 26, 27, 28, 29, 30, 34, 39, 43, 44, 45, 46, 47, 48, 52, 53, 54, 55, 56, 62, 63, 64, 65, 70, 74, 86, 87, 88, 89, 94, 99, 103, 104, 105, 106, 107, 111, 112, 113, 114, 115, 116, 120, 129, 130, 131, 132, 133, 186, 195, 196, 200, 204, 213, 217, 218, 222, 223, 226, 227, 231, 235, 246, 247, 250, 251, 255, 260, 266, 272, 281, 290, 294, 295, 347, 356, 357, 361, 365, 374, 379, 383, 392, 393, 395, 396, 407, 416, 421, 427, 433, 442, 451, 456, 508, 517, 519, 522, 526, 535, 539, 540, 544, 554, 556, 568, 569, 577, 582, 588, 594, 603, 612, 617, 669, 670, 671, 672, 673, 678, 680, 683, 687, 688, 689, 690, 691, 696, 697, 698, 699, 700, 705, 715, 716, 717, 730, 731, 732, 733, 738, 739, 740, 741, 742, 743, 749, 755, 756, 757, 758, 759, 764, 773, 778, 830, 839, 842, 844, 848, 857, 860, 866, 870, 871, 877, 894, 895, 899, 904, 910, 916, 925, 934, 939, 991, 1000, 1003, 1005, 1009, 1018, 1022, 1027, 1032, 1038, 1056, 1060, 1065, 1071, 1077, 1086, 1095, 1100, 1152, 1161, 1165, 1166, 1170, 1179, 1183, 1188, 1193, 1199, 1217, 1221, 1226, 1232, 1238, 1247, 1256, 1261, 1313, 1322, 1326, 1327, 1331, 1340, 1345, 1349, 1350, 1353, 1354, 1360, 1373, 1374, 1377, 1378, 1382, 1387, 1393, 1399, 1408, 1417, 1421, 1422, 1474, 1475, 1476, 1477, 1478, 1479, 1483, 1488, 1492, 1493, 1494, 1495, 1496, 1497, 1501, 1506, 1511, 1512, 1513, 1514, 1521, 1535, 1536, 1537, 1538, 1543, 1548, 1552, 1553, 1554, 1555, 1556, 1560, 1561, 1562, 1563, 1564, 1565, 1569, 1570, 1571, 1572, 1573, 1574, 1578, 1579, 1580, 1581, 1582 };

int[] energyShieldRotG = { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 69, 70, 71, 78, 79, 80, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 97, 98, 102, 103, 104, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 191, 202, 204, 215, 217, 228, 230, 232, 239, 241, 243, 254, 257, 259, 262, 266, 280, 291, 352, 354, 355, 356, 357, 358, 359, 360, 361, 363, 365, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 378, 380, 381, 382, 383, 385, 386, 387, 389, 391, 393, 394, 395, 396, 397, 398, 399, 400, 402, 404, 405, 406, 407, 408, 409, 411, 412, 413, 414, 415, 417, 420, 422, 425, 428, 441, 443, 444, 445, 446, 448, 449, 450, 452, 513, 515, 522, 524, 526, 528, 539, 541, 544, 546, 548, 550, 552, 563, 570, 572, 578, 580, 583, 585, 587, 589, 602, 604, 607, 609, 611, 613, 674, 676, 683, 685, 687, 689, 700, 702, 705, 707, 709, 711, 713, 715, 716, 717, 718, 719, 720, 721, 722, 724, 731, 733, 739, 741, 744, 746, 748, 750, 763, 765, 768, 770, 772, 774, 835, 838, 839, 840, 841, 842, 843, 846, 848, 850, 861, 863, 866, 868, 870, 872, 874, 876, 883, 885, 887, 888, 889, 890, 891, 892, 894, 895, 896, 897, 898, 900, 903, 904, 907, 908, 911, 924, 926, 929, 931, 933, 935, 997, 1006, 1009, 1011, 1022, 1024, 1027, 1028, 1029, 1031, 1033, 1035, 1036, 1037, 1044, 1045, 1046, 1048, 1059, 1062, 1067, 1069, 1071, 1085, 1087, 1090, 1091, 1092, 1094, 1096, 1159, 1160, 1161, 1162, 1163, 1164, 1165, 1166, 1170, 1171, 1172, 1183, 1184, 1185, 1192, 1193, 1194, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1224, 1225, 1226, 1227, 1230, 1231, 1246, 1247, 1248, 1255, 1256, 1257 };

int[] energyShieldRotW = { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 70, 79, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 97, 102, 103, 104, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 192, 201, 205, 218, 223, 227, 231, 240, 249, 257, 258, 262, 263, 265, 266, 281, 286, 290, 353, 362, 366, 379, 384, 388, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 410, 418, 423, 427, 442, 447, 451, 514, 523, 527, 540, 545, 549, 553, 562, 571, 579, 584, 588, 603, 608, 612, 675, 676, 683, 684, 688, 701, 706, 710, 714, 723, 732, 740, 741, 744, 745, 748, 749, 764, 769, 773, 837, 838, 839, 840, 841, 842, 843, 844, 849, 862, 871, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 902, 903, 904, 905, 909, 925, 934 };

int[] defenseShieldG = { 19, 20, 21, 22, 23, 24, 28, 29, 30, 31, 32, 33, 34, 35, 37, 38, 39, 40, 41, 42, 43, 44, 46, 47, 48, 49, 50, 51, 52, 53, 55, 56, 57, 60, 61, 62, 66, 67, 68, 69, 73, 74, 75, 76, 77, 78, 79, 80, 91, 92, 93, 94, 98, 99, 100, 103, 104, 105, 107, 108, 109, 110, 111, 112, 113, 115, 116, 117, 118, 119, 120, 121, 122, 124, 125, 126, 133, 134, 135, 136, 137, 138, 180, 186, 189, 196, 198, 205, 207, 214, 216, 218, 221, 223, 226, 231, 234, 241, 251, 256, 259, 261, 264, 266, 268, 274, 276, 283, 285, 287, 294, 300, 341, 343, 344, 345, 348, 350, 352, 353, 354, 355, 356, 357, 359, 361, 362, 363, 364, 365, 366, 368, 370, 371, 372, 373, 374, 375, 377, 380, 382, 384, 386, 389, 390, 393, 395, 397, 398, 399, 400, 401, 402, 411, 414, 415, 418, 420, 422, 425, 427, 429, 430, 431, 433, 434, 435, 437, 439, 440, 441, 442, 443, 444, 446, 448, 455, 457, 458, 459, 462, 502, 504, 507, 509, 511, 513, 520, 522, 529, 531, 538, 541, 543, 545, 547, 549, 552, 553, 554, 556, 558, 572, 574, 577, 578, 579, 581, 583, 586, 588, 592, 594, 598, 600, 607, 609, 616, 618, 621, 623, 663, 665, 668, 670, 672, 674, 675, 676, 677, 678, 681, 683, 684, 685, 686, 687, 690, 692, 693, 694, 695, 696, 699, 701, 703, 704, 706, 708, 711, 712, 713, 717, 719, 720, 721, 722, 723, 733, 736, 737, 738, 742, 744, 745, 746, 747, 749, 753, 755, 759, 761, 762, 763, 764, 765, 768, 770, 777, 779, 782, 784, 824, 826, 829, 831, 833, 839, 842, 848, 851, 857, 860, 862, 864, 865, 867, 870, 875, 878, 884, 895, 900, 903, 910, 914, 916, 920, 926, 929, 931, 938, 940, 943, 945, 985, 987, 990, 992, 994, 996, 997, 998, 999, 1000, 1003, 1005, 1006, 1007, 1008, 1009, 1012, 1014, 1015, 1016, 1017, 1018, 1021, 1023, 1024, 1026, 1028, 1032, 1033, 1034, 1037, 1039, 1041, 1042, 1043, 1044, 1045, 1057, 1058, 1059, 1062, 1064, 1066, 1067, 1068, 1069, 1071, 1075, 1077, 1081, 1083, 1084, 1085, 1086, 1087, 1090, 1092, 1099, 1101, 1104, 1106, 1146, 1148, 1151, 1153, 1155, 1157, 1164, 1166, 1173, 1175, 1182, 1184, 1185, 1187, 1189, 1196, 1198, 1200, 1202, 1221, 1223, 1225, 1227, 1230, 1232, 1236, 1238, 1242, 1244, 1251, 1253, 1260, 1262, 1265, 1267, 1307, 1309, 1312, 1314, 1316, 1318, 1325, 1327, 1334, 1336, 1343, 1345, 1347, 1350, 1352, 1353, 1354, 1357, 1359, 1361, 1363, 1377, 1378, 1379, 1382, 1384, 1386, 1388, 1391, 1393, 1397, 1399, 1403, 1405, 1412, 1414, 1421, 1423, 1426, 1428, 1468, 1470, 1471, 1472, 1475, 1477, 1479, 1480, 1481, 1482, 1483, 1484, 1486, 1488, 1495, 1497, 1498, 1499, 1500, 1501, 1502, 1504, 1506, 1508, 1511, 1513, 1516, 1517, 1520, 1522, 1524, 1525, 1526, 1527, 1528, 1529, 1538, 1541, 1542, 1545, 1547, 1549, 1552, 1554, 1556, 1557, 1558, 1560, 1561, 1562, 1564, 1566, 1567, 1568, 1569, 1570, 1571, 1573, 1575, 1576, 1577, 1578, 1579, 1580, 1582, 1584, 1585, 1586, 1589, 1629, 1635, 1638, 1645, 1647, 1649, 1656, 1663, 1665, 1667, 1670, 1672, 1675, 1680, 1683, 1690, 1700, 1705, 1708, 1710, 1713, 1715, 1717, 1723, 1725, 1732, 1734, 1741, 1743, 1749, 1790, 1791, 1792, 1793, 1794, 1795, 1799, 1800, 1801, 1802, 1803, 1804, 1805, 1806, 1808, 1809, 1810, 1817, 1818, 1819, 1820, 1821, 1822, 1823, 1824, 1826, 1827, 1828, 1831, 1832, 1833, 1837, 1838, 1839, 1840, 1844, 1845, 1846, 1847, 1848, 1849, 1850, 1851, 1862, 1863, 1864, 1865, 1869, 1870, 1871, 1874, 1875, 1876, 1878, 1879, 1880, 1881, 1882, 1883, 1884, 1886, 1887, 1888, 1889, 1890, 1891, 1892, 1893, 1895, 1896, 1897, 1898, 1899, 1900, 1901, 1902, 1904, 1905, 1906, 1907, 1908, 1909 };

int[] defenseShieldW = { 20, 21, 22, 23, 24, 29, 30, 31, 32, 33, 34, 38, 39, 40, 41, 42, 43, 47, 48, 49, 50, 51, 52, 56, 61, 66, 67, 68, 69, 74, 75, 76, 77, 78, 79, 91, 92, 93, 94, 99, 104, 108, 109, 110, 111, 112, 116, 117, 118, 119, 120, 121, 125, 134, 135, 136, 137, 138, 181, 185, 186, 190, 199, 208, 217, 218, 222, 226, 227, 230, 231, 235, 251, 252, 255, 256, 260, 265, 271, 277, 286, 295, 299, 300, 342, 347, 351, 360, 369, 378, 379, 383, 387, 396, 412, 421, 426, 432, 438, 447, 456, 461, 503, 508, 512, 521, 530, 539, 541, 544, 548, 549, 557, 573, 574, 582, 587, 593, 599, 608, 617, 622, 664, 669, 673, 674, 675, 676, 677, 682, 683, 684, 685, 686, 691, 692, 693, 694, 695, 700, 702, 705, 710, 711, 712, 713, 718, 719, 720, 721, 722, 735, 736, 737, 738, 743, 744, 745, 746, 747, 748, 754, 760, 761, 762, 763, 764, 769, 778, 783, 825, 830, 834, 843, 852, 861, 864, 866, 874, 875, 879, 899, 900, 904, 909, 915, 921, 930, 939, 944, 986, 991, 995, 1004, 1013, 1022, 1025, 1027, 1036, 1040, 1061, 1065, 1070, 1076, 1082, 1091, 1100, 1105, 1147, 1152, 1156, 1165, 1174, 1183, 1187, 1188, 1197, 1201, 1222, 1226, 1231, 1237, 1243, 1252, 1261, 1266, 1308, 1312, 1313, 1317, 1326, 1335, 1344, 1348, 1349, 1353, 1354, 1357, 1358, 1362, 1378, 1379, 1382, 1383, 1387, 1392, 1398, 1404, 1413, 1422, 1426, 1427, 1469, 1470, 1471, 1472, 1473, 1478, 1479, 1480, 1481, 1482, 1483, 1487, 1496, 1497, 1498, 1499, 1500, 1501, 1505, 1510, 1515, 1516, 1517, 1518, 1523, 1524, 1525, 1526, 1527, 1528, 1540, 1541, 1542, 1543, 1548, 1553, 1557, 1558, 1559, 1560, 1561, 1565, 1566, 1567, 1568, 1569, 1570, 1574, 1575, 1576, 1577, 1578, 1579, 1583, 1584, 1585, 1586, 1587 };

int[] defenseShieldRotG = { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 69, 70, 71, 78, 79, 80, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 97, 98, 102, 103, 104, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 191, 202, 204, 215, 217, 228, 230, 232, 239, 241, 243, 254, 257, 259, 262, 266, 280, 291, 352, 354, 355, 356, 357, 358, 359, 360, 361, 363, 365, 367, 368, 369, 370, 371, 372, 373, 374, 375, 376, 378, 380, 381, 382, 383, 385, 386, 387, 389, 391, 393, 394, 395, 396, 397, 398, 399, 400, 402, 404, 405, 406, 407, 408, 409, 411, 412, 413, 414, 415, 417, 420, 422, 425, 428, 441, 443, 444, 445, 446, 447, 448, 449, 450, 452, 513, 515, 522, 524, 526, 528, 539, 541, 544, 546, 548, 550, 552, 563, 570, 572, 578, 580, 583, 585, 587, 589, 602, 604, 611, 613, 674, 676, 683, 685, 687, 689, 700, 702, 705, 707, 709, 711, 713, 715, 716, 717, 718, 719, 720, 721, 722, 724, 731, 733, 739, 741, 744, 746, 748, 750, 763, 765, 772, 774, 835, 838, 839, 840, 841, 842, 843, 846, 848, 850, 861, 863, 866, 868, 870, 872, 874, 876, 883, 885, 887, 888, 889, 890, 891, 892, 894, 895, 896, 897, 898, 900, 903, 904, 907, 908, 911, 924, 927, 928, 929, 930, 931, 932, 935, 997, 1006, 1009, 1011, 1022, 1024, 1027, 1028, 1029, 1031, 1033, 1035, 1036, 1037, 1044, 1045, 1046, 1048, 1059, 1062, 1067, 1069, 1071, 1086, 1095, 1159, 1160, 1161, 1162, 1163, 1164, 1165, 1166, 1170, 1171, 1172, 1183, 1184, 1185, 1192, 1193, 1194, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1224, 1225, 1226, 1227, 1230, 1231, 1248, 1249, 1250, 1251, 1252, 1253, 1254, 1255 };

int[] defenseShieldRotW = { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 70, 79, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 97, 102, 103, 104, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 192, 201, 205, 218, 223, 227, 231, 240, 249, 257, 258, 262, 263, 265, 266, 281, 290, 353, 362, 366, 379, 384, 388, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 410, 418, 423, 427, 442, 451, 514, 523, 527, 540, 545, 549, 553, 562, 571, 579, 584, 588, 603, 612, 675, 676, 683, 684, 688, 701, 706, 710, 714, 723, 732, 740, 741, 744, 745, 748, 749, 764, 765, 772, 773, 837, 838, 839, 840, 841, 842, 843, 844, 849, 862, 871, 888, 889, 890, 891, 892, 893, 894, 895, 896, 897, 902, 903, 904, 905, 909, 926, 927, 928, 929, 930, 931, 932, 933 };

IEnumerator<bool> _updateLCDs;
const string _FIGHTER_SUBTYPE = "DBSmallBlockFighterCockpit";
const string _LARGE_CORNER_LCD_FLAT_1 = "LargeBlockCorner_LCD_Flat_1";
const string _LARGE_CORNER_LCD_FLAT_2 = "LargeBlockCorner_LCD_Flat_2";
const string _MONOSPACE = "Monospace";
const string _DEBUG = "Debug";

const string BTL_MODE = lcdNamePrefix + lcdModeBTL;
const string BTS_MODE = lcdNamePrefix + lcdModeBTS;
const string TDL_MODE = lcdNamePrefix + lcdModeTDL;
const string TDS_MODE = lcdNamePrefix + lcdModeTDS;
const string CTD_MODE = lcdNamePrefix + lcdModeCTD;
const string CRB_MODE = lcdNamePrefix + lcdModeCRB;
const string CCB_MODE = lcdNamePrefix + lcdModeCCB;
const string GFXC_MODE = lcdNamePrefix + lcdModeGfxC;
const string GFXH_MODE = lcdNamePrefix + lcdModeGfxH;
const string GFXV_MODE = lcdNamePrefix + lcdModeGfxV;

Dictionary<string, ShieldLCD> lcdDict = new Dictionary<string, ShieldLCD>()
{
  { BTL_MODE, new ShieldLCD() },
  { BTS_MODE, new ShieldLCD() },
  { TDL_MODE, new ShieldLCD() },
  { TDS_MODE, new ShieldLCD() },
  { CTD_MODE, new ShieldLCD() },
  { CRB_MODE, new ShieldLCD() },
  { CCB_MODE, new ShieldLCD() },
  { GFXC_MODE, new ShieldLCD() },
  { GFXH_MODE, new ShieldLCD() },
  { GFXV_MODE, new ShieldLCD() },
};

List<IMySoundBlock> soundBlocks = new List<IMySoundBlock>();
List<IMyLightingBlock> warningLights = new List<IMyLightingBlock>();
List<IMyTerminalBlock> shieldGenList = new List<IMyTerminalBlock>();
List<IMyTerminalBlock> defShieldList = new List<IMyTerminalBlock>();
List<BlockSurface> _surfaceList = new List<BlockSurface>();

IMyTerminalBlock shieldGen;
DefenseShields defShield;

StringBuilder _saveSB = new StringBuilder();
StringBuilder rainbowStripeE = new StringBuilder();
StringBuilder rainbowStripeD = new StringBuilder();
StringBuilder rainbowStripe = new StringBuilder();
StringBuilder rainbowSOBlock = new StringBuilder();
StringBuilder rainbowSORBlock = new StringBuilder();
StringBuilder rainbowESBlock = new StringBuilder();
StringBuilder rainbowESRBlock = new StringBuilder();
StringBuilder rainbowDSBlock = new StringBuilder();
StringBuilder rainbowDSRBlock = new StringBuilder();
StringBuilder roycStripeE = new StringBuilder();
StringBuilder roycStripeD = new StringBuilder();
StringBuilder roycStripe = new StringBuilder();
StringBuilder roycSOBlock = new StringBuilder();
StringBuilder roycSORBlock = new StringBuilder();
StringBuilder roycESBlock = new StringBuilder();
StringBuilder roycESRBlock = new StringBuilder();
StringBuilder roycDSBlock = new StringBuilder();
StringBuilder roycDSRBlock = new StringBuilder();

int curShieldE, curShieldD, curTotal = 0;
int maxShieldE, maxShieldD, maxTotal = 0;
int percent, percentE, percentD = 0;
int barCounter, barCounterE, barCounterD = 0;
int spsE, spsD, spsTotal = 0;
int lastShield = 0;
int lastSPS = 0;
int refreshCounter = 0;
int lcdCount = 0;

const string monoBorderA = "\n";
const string monoBorderB = "\n";
const string monoBorderL = "";
const string monoBorderR = "\n";
string[] clcdBGW = new string[] { "", "", "" };
string[] clcdROYC = new string[] { "", "", "", "" };
string[] rainbowBarChars = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

bool isSetup = false;
bool useCRB = false;
bool useCCB = false;
bool needsUpdate = false;

const double _tickSignificance = 0.005;
double _prevAverage = 0;
double _expAverage = 0;
int _runtimeCtr = 0;
void CalculateAverageRuntime()
{
    if (++_runtimeCtr < 21)
        return;

    _expAverage = (1 - _tickSignificance) * _prevAverage + _tickSignificance * Runtime.LastRunTimeMs;
    _prevAverage = _expAverage;
}

bool Setup()
{
    if (_updateLCDs != null)
    {
        _updateLCDs.Dispose();
        _updateLCDs = UpdateLCDs();
    }

    foreach (var shieldLCD in lcdDict.Values)
        shieldLCD.DrawLCDs.Clear();

    soundBlocks.Clear();
    warningLights.Clear();
    shieldGenList.Clear();
    defShieldList.Clear();
    lcdCount = 0;

    GridTerminalSystem.GetBlocksOfType<IMyTerminalBlock>(null, block =>
    {
        if (!block.IsSameConstructAs(Me))
            return false;

        var panel = block as IMyTextPanel;
        if (panel != null)
        {
            if (block.CustomName.Contains(lcdNamePrefix) || block.CustomData.Contains(lcdNamePrefix))
                AddPanel(panel);
            return false;
        }

        var sound = block as IMySoundBlock;
        if (sound != null)
        {
            if (block.CustomName.Contains(shieldAudioName) || block.CustomData.Contains(shieldAudioName))
                soundBlocks.Add(sound);
            return false;
        }

        var light = block as IMyLightingBlock;
        if (light != null)
        {
            if (block.CustomName.Contains(shieldLightName) || block.CustomData.Contains(shieldLightName))
                warningLights.Add(light);
            return false;
        }

        var provider = block as IMyTextSurfaceProvider;
        if (provider != null && provider.SurfaceCount > 0 && block.CustomName.Contains(lcdNamePrefix))
        {
            BlockSurface surfaceBlock;
            if (!CheckListForBlock(block, out surfaceBlock))
            {
                surfaceBlock = new BlockSurface(block, this);
                _surfaceList.Add(surfaceBlock);
            }

            AddSurface(surfaceBlock);
        }

        var def = block.BlockDefinition.SubtypeName;

        if (shieldTypesCython.Contains(def))
            shieldGenList.Add(block);
        else if (shieldTypesDStar.Contains(def))
            defShieldList.Add(block);

        return false;
    });

    if (shieldGenList.Count == 0 && defShieldList.Count == 0)
    {
        Echo("NO\nSHIELD\nGENERATORS\nFOUND");
        return false;
    }

    if (shieldGenList.Count > 0)
    {
        shieldGen = shieldGenList[0];

        if (!shieldGen.CustomName.EndsWith(":"))
            shieldGen.CustomName += ":";

        if (!(shieldGen.CustomName.Contains("(") || shieldGen.CustomName.Contains(")")))
        {
            Echo("BLOCK NAMED\nIS NOT AN ENERGY\nSHIELD GENERATOR!");
            return false;
        }
    }

    if (defShieldList.Count > 0)
        defShield = new DefenseShields(Me);

    useCCB = lcdDict[CCB_MODE].DrawLCDs.Count > 0;
    useCRB = lcdDict[CRB_MODE].DrawLCDs.Count > 0;

    return true;
}

bool CheckListForBlock(IMyTerminalBlock block, out BlockSurface surfaceBlock)
{
    surfaceBlock = null;

    foreach (var surface in _surfaceList)
    {
        if (surface.EntityId == block.EntityId)
        {
            surfaceBlock = surface;
            return true;
        }
    }

    return false;
}

void AddPanel(IMyTextPanel panel)
{
    var name = panel.CustomName;
    var data = panel.CustomData;

    foreach (var kvp in lcdDict)
    {
        if (name.Contains(kvp.Key) || data.Contains(kvp.Key))
        {
            kvp.Value.AddPanel(panel);
            lcdCount++;
            break;
        }
    }
}

void AddSurface(BlockSurface surface)
{
    foreach (var num in surface.THYA_BTL_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[BTL_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_BTS_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[BTS_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_TDL_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[TDL_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_TDS_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[TDS_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_CTD_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[CTD_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_CRB_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[CRB_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_CCB_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[CCB_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_C_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[GFXC_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_H_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[GFXH_MODE].AddDrawLCD(screen);
    }

    foreach (var num in surface.THYA_V_Screens)
    {
        var screen = surface.GetDrawLCD(num);
        lcdDict[GFXV_MODE].AddDrawLCD(screen);
    }

    lcdCount++;
}

void SubMain(string argument, UpdateType updateSource)
{
    if (!isSetup)
    {
        isSetup = Setup();
        return;
    }
    else if ((updateSource & UpdateType.Update100) > 0 && ++refreshCounter > 3)
    {
        refreshCounter = 0;
        isSetup = false;

        foreach (var surfaceBlock in _surfaceList)
        {
            if (surfaceBlock.NeedsUpdate())
                surfaceBlock.UpdateConfig();
        }
    }

    CalculateAverageRuntime();

    if ((updateSource & UpdateType.Update10) == 0)
    {
        if (_updateLCDs != null && needsUpdate)
        {
            if (!_updateLCDs.MoveNext())
            {
                _updateLCDs.Dispose();
                _updateLCDs = UpdateLCDs();
            }

            needsUpdate = !_updateLCDs.Current;
        }

        return;
    }

    Echo($"Average Runtime = {Math.Round(_expAverage, 4).ToString()} ms\n");

    curTotal = maxTotal = spsTotal = 0;

    if (lcdCount == 0)
        Echo($"Warning!\nNo LCD Panels found. Continuing...\n");

    if (shieldGen != null)
    {
        ParseShieldInfo_E(shieldGen.CustomName, out curShieldE, out maxShieldE);
        spsE = CalcSPS(curShieldE, maxShieldE);

        if (maxShieldE != 0)
        {
            percentE = (int)(curShieldE / (maxShieldE * 0.01));
            barCounterE = (int)((curShieldE * 1.54) / (maxShieldE * 0.01));
        }

        spsTotal += spsE;
    }

    if (defShield != null)
    {
        GetShieldInfo_D(defShield, out curShieldD, out maxShieldD, out spsD);

        if (maxShieldD != 0)
        {
            percentD = (int)(curShieldD / (maxShieldD * 0.01));
            barCounterD = (int)((curShieldD * 1.54) / (maxShieldD * 0.01));
        }

        spsTotal += spsD;
    }

    if (maxTotal != 0)
    {
        percent = CalcPercent(curTotal, maxTotal);
        barCounter = (int)((curTotal * 1.54) / (maxTotal * 0.01));
    }

    if (useCCB || useCRB)
    {
        if (useCRB)
        {
            RainbowStrip(barCounterE, rainbowStripeE);
            RainbowStrip(barCounterD, rainbowStripeD);
            RainbowStrip(barCounter, rainbowStripe);

            rainbowSOBlock.Clear();
            rainbowSORBlock.Clear();
        }

        if (useCCB)
        {
            RoycStrip(barCounterE, roycStripeE);
            RoycStrip(barCounterD, roycStripeD);
            RoycStrip(barCounter, roycStripe);

            roycSOBlock.Clear();
            roycSORBlock.Clear();
        }

        for (int i = 0; i < 12; i++)
        {
            if (useCRB)
                rainbowSOBlock.Append(rainbowStripe);

            if (useCCB)
                roycSOBlock.Append(roycStripe);
        }

        if (useCRB)
            rainbowSORBlock.Append(rainbowSOBlock);

        if (useCCB)
            roycSORBlock.Append(roycSOBlock);

        foreach (int j in shieldOnlyG)
        {
            if (useCRB)
                rainbowSOBlock[j] = '';

            if (useCCB)
                roycSOBlock[j] = '';
        }

        foreach (int j in shieldOnlyW)
        {
            var num = j + 161;

            if (useCRB)
                rainbowSOBlock[num] = '';

            if (useCCB)
                roycSOBlock[num] = '';
        }

        foreach (int j in shieldOnlyRotG)
        {
            var num = j + 322;

            if (useCRB)
                rainbowSORBlock[num] = '';

            if (useCCB)
                roycSORBlock[num] = '';
        }

        foreach (int j in shieldOnlyRotW)
        {
            var num = j + 483;

            if (useCRB)
                rainbowSORBlock[num] = '';

            if (useCCB)
                roycSORBlock[num] = '';
        }

        if (useCRB)
        {
            rainbowESBlock.Clear();
            rainbowESRBlock.Clear();
        }

        if (useCCB)
        {
            roycESBlock.Clear();
            roycESRBlock.Clear();
        }

        for (int i = 0; i < 12; i++)
        {
            if (useCRB)
                rainbowESBlock.Append(rainbowStripeE);

            if (useCCB)
                roycESBlock.Append(roycStripeE);
        }

        if (useCRB)
            rainbowESRBlock.Append(rainbowESBlock);

        if (useCCB)
            roycESRBlock.Append(roycESBlock);

        foreach (int j in energyShieldG)
        {
            if (useCRB)
                rainbowESBlock[j] = '';

            if (useCCB)
                roycESBlock[j] = '';
        }

        foreach (int j in energyShieldW)
        {
            var num = j + 161;

            if (useCRB)
                rainbowESBlock[num] = '';

            if (useCCB)
                roycESBlock[num] = '';
        }

        foreach (int j in energyShieldRotG)
        {
            var num = j + 322;

            if (useCRB)
                rainbowESRBlock[num] = '';

            if (useCCB)
                roycESRBlock[num] = '';
        }

        foreach (int j in energyShieldRotW)
        {
            var num = j + 483;

            if (useCRB)
                rainbowESRBlock[num] = '';

            if (useCCB)
                roycESRBlock[num] = '';
        }

        if (useCRB)
        {
            rainbowDSBlock.Clear();
            rainbowDSRBlock.Clear();
        }

        if (useCCB)
        {
            roycDSBlock.Clear();
            roycDSRBlock.Clear();
        }

        for (int i = 0; i < 12; i++)
        {
            if (useCRB)
                rainbowDSBlock.Append(rainbowStripeD);

            if (useCCB)
                roycDSBlock.Append(roycStripeD);
        }

        if (useCRB)
            rainbowDSRBlock.Append(rainbowDSBlock);

        if (useCCB)
            roycDSRBlock.Append(roycDSBlock);

        foreach (int j in defenseShieldG)
        {
            if (useCRB)
                rainbowDSBlock[j] = '';

            if (useCCB)
                roycDSBlock[j] = '';
        }

        foreach (int j in defenseShieldW)
        {
            var num = j + 161;

            if (useCRB)
                rainbowDSBlock[num] = '';

            if (useCCB)
                roycDSBlock[num] = '';
        }

        foreach (int j in defenseShieldRotG)
        {
            var num = j + 322;

            if (useCRB)
                rainbowDSRBlock[num] = '';

            if (useCCB)
                roycDSRBlock[num] = '';
        }

        foreach (int j in defenseShieldRotW)
        {
            var num = j + 483;

            if (useCRB)
                rainbowDSRBlock[num] = '';

            if (useCCB)
                roycDSRBlock[num] = '';
        }
    }

    if (_updateLCDs != null)
    {
        if (!_updateLCDs.MoveNext())
        {
            _updateLCDs.Dispose();
            _updateLCDs = UpdateLCDs();
        }

        needsUpdate = !_updateLCDs.Current;
    }

    foreach (IMySoundBlock soundBlock in soundBlocks)
    {
        if (percent > shieldAudioWarning)
        {
            if (soundBlock.Enabled)
            {
                soundBlock.LoopPeriod = 0f;
                soundBlock.Enabled = false;
            }
        }
        else
        {
            soundBlock.LoopPeriod = float.MaxValue;
            if (!soundBlock.Enabled)
            {
                soundBlock.Enabled = true;
                soundBlock.Play();
            }
        }
    }

    foreach (IMyLightingBlock warningLight in warningLights)
    {
        if (percent > shieldVisualWarning)
        {
            if (warningLight.Enabled)
                warningLight.Enabled = false;
        }
        else if (!warningLight.Enabled)
        {
            warningLight.Enabled = true;
        }
    }
}

IEnumerator<bool> UpdateLCDs()
{
    while (true)
    {
        foreach (var kvp in lcdDict)
        {
            if (kvp.Value.DrawLCDs.Count == 0)
                continue;

            switch (kvp.Key)
            {
                case BTS_MODE:

                    SmBarFactory(curTotal, maxTotal, percent, spsTotal, kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        var fontSize = lcd.IsCockpitLCD ? sFontSizeCP : sFontSize;

                        PrepLCD(lcd, percent, fontSize);
                        lcd.Surface.WriteText(kvp.Value.TextBuilder);

                        yield return false;
                    }
                    break;

                case BTL_MODE:

                    LgBarFactory(curTotal, maxTotal, percent, spsTotal, kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        var fontSize = lcd.IsCockpitLCD ? lFontSizeCP : lFontSize;

                        PrepLCD(lcd, percent, fontSize);
                        lcd.Surface.WriteText(kvp.Value.TextBuilder);

                        yield return false;
                    }
                    break;

                case TDS_MODE:
                case TDL_MODE:

                    TextFactory(curTotal, maxTotal, percent, spsTotal, kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        var fontSize = lcd.IsCockpitLCD ? kvp.Key == TDL_MODE ? lFontSizeCP : sFontSizeCP : (kvp.Key == TDL_MODE ? lFontSize : sFontSize);

                        PrepLCD(lcd, percent, fontSize);
                        lcd.Surface.WriteText(kvp.Value.TextBuilder);

                        yield return false;
                    }
                    break;

                case CTD_MODE:

                    CornerFactory(percent, spsTotal, MyCubeSize.Small, kvp.Value.TextBuilderSm);
                    CornerFactory(percent, spsTotal, MyCubeSize.Large, kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        PrepLCD(lcd, percent, cFontSize);

                        if (lcd.Block.CubeGrid.GridSizeEnum == MyCubeSize.Small)
                            lcd.Surface.WriteText(kvp.Value.TextBuilderSm);
                        else
                            lcd.Surface.WriteText(kvp.Value.TextBuilder);

                        yield return false;
                    }
                    break;

                case GFXC_MODE:

                    var cString = ImgNameFactory(percent, "THYA-ShieldC", kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        SetImageLCD(cString, lcd);

                        yield return false;
                    }
                    break;

                case GFXH_MODE:

                    var hString = ImgNameFactory(percent, "THYA-ShieldH", kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        SetImageLCD(hString, lcd);

                        yield return false;
                    }
                    break;

                case GFXV_MODE:

                    var vString = ImgNameFactory(percent, "THYA-ShieldV", kvp.Value.TextBuilder);

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        SetImageLCD(vString, lcd);

                        yield return false;
                    }
                    break;

                case CRB_MODE:

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        lcd.Surface.Font = _MONOSPACE;
                        lcd.Surface.FontSize = barFontSize;
                        lcd.Surface.FontColor = Color.Gray;
                        lcd.Surface.Alignment = TextAlignment.CENTER;

                        var subtype = lcd.Block.BlockDefinition.SubtypeName;

                        if (lcd.Block.CubeGrid.GridSizeEnum == MyCubeSize.Large)
                        {
                            int yLimit = (subtype == _LARGE_CORNER_LCD_FLAT_1 || subtype == _LARGE_CORNER_LCD_FLAT_2) ? 27 : 24;
                            RainbowBarFactory(lcd, yLimit, kvp.Value.TextBuilder, lcd.Block.CustomData);
                        }
                        else
                        {
                            int yLimit = (subtype == _LARGE_CORNER_LCD_FLAT_1 || subtype == _LARGE_CORNER_LCD_FLAT_2) ? 50 : 42;
                            RainbowBarFactory(lcd, yLimit, kvp.Value.TextBuilder, lcd.Block.CustomData);
                        }

                        lcd.Surface.WriteText(kvp.Value.TextBuilder);
                        lcd.Surface.ContentType = ContentType.TEXT_AND_IMAGE;
                        lcd.Surface.TextPadding = 0;

                        yield return false;
                    }
                    break;

                case CCB_MODE:

                    foreach (var lcd in kvp.Value.DrawLCDs)
                    {
                        lcd.Surface.Font = _MONOSPACE;
                        lcd.Surface.FontSize = barFontSize;
                        lcd.Surface.FontColor = Color.Gray;
                        lcd.Surface.Alignment = TextAlignment.CENTER;

                        var subtype = lcd.Block.BlockDefinition.SubtypeName;

                        if (lcd.Block.CubeGrid.GridSizeEnum == MyCubeSize.Large)
                        {
                            int yLimit = (subtype == _LARGE_CORNER_LCD_FLAT_1 || subtype == _LARGE_CORNER_LCD_FLAT_2) ? 27 : 24;
                            RoycBarFactory(lcd, yLimit, kvp.Value.TextBuilder, lcd.Block.CustomData);
                        }
                        else
                        {
                            int yLimit = (subtype == _LARGE_CORNER_LCD_FLAT_1 || subtype == _LARGE_CORNER_LCD_FLAT_2) ? 50 : 42;
                            RoycBarFactory(lcd, yLimit, kvp.Value.TextBuilder, lcd.Block.CustomData);
                        }

                        lcd.Surface.WriteText(kvp.Value.TextBuilder);
                        lcd.Surface.ContentType = ContentType.TEXT_AND_IMAGE;
                        lcd.Surface.TextPadding = 0;

                        yield return false;
                    }
                    break;
            }
        }

        yield return true;
    }
}

int CalcPercent(int curShields, int maxShields)
{
    Echo($"-------Total Shields-------\n{curShields} / {maxShields}");

    if (maxShields == 0)
        return 0;

    double percent = (double)curShields / maxShields;
    return (int)(Math.Round(percent, 2) * 100);
}

void TextFactory(int curShields, int maxShields, int percent, int sps, StringBuilder text)
{
    text.Clear()
  .Append("Shields: ")
  .Append(percent)
  .Append("%\n\n")
  .Append("Shields: ")
  .Append(FormatNum(curShields))
  .Append("\nFull: ")
  .Append(FormatNum(maxShields))
  .Append("\nS\\s: ")
  .Append(sps);
}

void CornerFactory(int percent, int sps, MyCubeSize gridS, StringBuilder text)
{
    text.Clear()
  .Append($"Shield Status: ")
  .Append(percent)
  .Append("%");

    if (gridS == MyCubeSize.Small)
        text.Append("\nCharging: ")
          .Append(sps)
          .Append("S\\s");
}

void RainbowBarFactory(DrawLCD lcd, int yLimit, StringBuilder text, string mask)
{
    mask = mask.ToUpper();
    int yPos;
    text.Clear();
    text.Append(monoBorderA + monoBorderA + monoBorderB);
    if (mask == "SGDUAL" || mask == "SGDUALR")
    {
        lcd.Surface.FontSize *= 0.8f;

        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(rainbowStripeD); }
        if (mask == "SGDUALR")
        { text.Append(rainbowDSRBlock); }
        else
        { text.Append(rainbowDSBlock); }
        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(rainbowStripeD); }
        text.Append(monoBorderB + monoBorderA + monoBorderA + monoBorderB);
        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(rainbowStripeE); }
        if (mask == "SGDUALR")
        { text.Append(rainbowESRBlock); }
        else
        { text.Append(rainbowESBlock); }
        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(rainbowStripeE); }
    }
    else
    {
        if (mask == "ES" || mask == "ESR")
        {
            for (yPos = 3; yPos < ((int)(yLimit / 2) - 6); yPos++)
            { text.Append(rainbowStripeE); }
            if (mask == "ESR")
            { text.Append(rainbowESRBlock); }
            else
            { text.Append(rainbowESBlock); }
            for (yPos = ((int)(yLimit / 2) + 6); yPos < (yLimit - 3); yPos++)
            { text.Append(rainbowStripeE); }
        }
        else if (mask == "DS" || mask == "DSR")
        {
            for (yPos = 3; yPos < ((int)(yLimit / 2) - 6); yPos++)
            { text.Append(rainbowStripeD); }
            if (mask == "DSR")
            { text.Append(rainbowDSRBlock); }
            else
            { text.Append(rainbowDSBlock); }
            for (yPos = ((int)(yLimit / 2) + 6); yPos < (yLimit - 3); yPos++)
            { text.Append(rainbowStripeD); }
        }
        else
        {
            for (yPos = 3; yPos < ((int)(yLimit / 2) - 6); yPos++)
            { text.Append(rainbowStripe); }
            if (mask == "SR")
            { text.Append(rainbowSORBlock); }
            else
            { text.Append(rainbowSOBlock); }
            for (yPos = ((int)(yLimit / 2) + 6); yPos < (yLimit - 3); yPos++)
            { text.Append(rainbowStripe); }
        }
    }
    text.Append(monoBorderB)
  .Append(monoBorderA)
  .Append(monoBorderA);
}

void RoycBarFactory(DrawLCD lcd, int yLimit, StringBuilder text, string mask)
{
    mask = mask.ToUpper();
    int yPos;
    text.Clear();
    text.Append(monoBorderA + monoBorderA + monoBorderB);
    if (mask == "SGDUAL" || mask == "SGDUALR")
    {
        lcd.Surface.FontSize *= 0.8f;

        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(roycStripeD); }
        if (mask == "SGDUALR")
        { text.Append(roycDSRBlock); }
        else
        { text.Append(roycDSBlock); }
        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(roycStripeD); }
        text.Append(monoBorderB + monoBorderA + monoBorderA + monoBorderB);
        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(roycStripeE); }
        if (mask == "SGDUALR")
        { text.Append(roycESRBlock); }
        else
        { text.Append(roycESBlock); }
        for (yPos = 0; yPos < ((int)(yLimit - 34) / 4); yPos++)
        { text.Append(roycStripeE); }
    }
    else
    {
        if (mask == "ES" || mask == "ESR")
        {
            for (yPos = 3; yPos < ((int)(yLimit / 2) - 6); yPos++)
            { text.Append(roycStripeE); }
            if (mask == "ESR")
            { text.Append(roycESRBlock); }
            else
            { text.Append(roycESBlock); }
            for (yPos = ((int)(yLimit / 2) + 6); yPos < (yLimit - 3); yPos++)
            { text.Append(roycStripeE); }
        }
        else if (mask == "DS" || mask == "DSR")
        {
            for (yPos = 3; yPos < ((int)(yLimit / 2) - 6); yPos++)
            { text.Append(roycStripeD); }
            if (mask == "DSR")
            { text.Append(roycDSRBlock); }
            else
            { text.Append(roycDSBlock); }
            for (yPos = ((int)(yLimit / 2) + 6); yPos < (yLimit - 3); yPos++)
            { text.Append(roycStripeD); }
        }
        else
        {
            for (yPos = 3; yPos < ((int)(yLimit / 2) - 6); yPos++)
            { text.Append(roycStripe); }
            if (mask == "SR")
            { text.Append(roycSORBlock); }
            else
            { text.Append(roycSOBlock); }
            for (yPos = ((int)(yLimit / 2) + 6); yPos < (yLimit - 3); yPos++)
            { text.Append(roycStripe); }
        }
    }
    text.Append(monoBorderB + monoBorderA + monoBorderA);
}

void RainbowStrip(int barCounter, StringBuilder text)
{
    text.Clear()
    .Append(monoBorderL);

    for (int barPos = 0; barPos < 154; barPos++)
    {
        if (barCounter < barPos || barCounter == 0)
        {
            text.Append("");
        }
        else
        {
            if (barPos < 22)
                text.Append(rainbowBarChars[0]);
            else
                text.Append(rainbowBarChars[(barPos - 22) / 6]);
        }
    }
    text.Append(monoBorderR);
}

void RoycStrip(int barCounter, StringBuilder text)
{
    int barPos;
    text.Clear().Append(monoBorderL);
    for (barPos = 0; barPos < 154; barPos++)
    {
        if (barCounter < barPos || barCounter == 0)
        { text.Append(""); }
        else
        {
            if ((int)(barCounter / 1.54) > shieldHigh)
            {
                text.Append(clcdROYC[3]);
            }
            else if ((int)(barCounter / 1.54) > shieldLow)
            {
                text.Append(clcdROYC[2]);
            }
            else if ((int)(barCounter / 1.54) > shieldCrit)
            {
                text.Append(clcdROYC[1]);
            }
            else
            {
                text.Append(clcdROYC[0]);
            }
        }
    }
    text.Append(monoBorderR);
}

void ParseShieldInfo_E(string shieldName, out int curShields, out int maxShields)
{
    string[] tempStringArray = shieldName.Split(COLON);
    string tempString = tempStringArray[1].Trim(SPACE_PARENS);
    string[] splitString = tempString.Split(FWD_SLASH);

    int.TryParse(splitString[0], out curShields);
    int.TryParse(splitString[1], out maxShields);

    Echo($"------Energy Shields------\n{curShields} / {maxShields}\n");

    curTotal += curShields;
    maxTotal += maxShields;
}

void GetShieldInfo_D(DefenseShields shieldBlock, out int curDefShields, out int maxDefShields, out int shieldSPS)
{
    if (!shieldBlock.IsShieldBlock())
    {
        curDefShields = 0;
        maxDefShields = 0;
        shieldSPS = 0;
    }
    else
    {
        curDefShields = (int)Math.Round(shieldBlock.GetCharge() * shieldBlock.HpToChargeRatio());
        maxDefShields = (int)Math.Round(shieldBlock.GetMaxCharge() * shieldBlock.HpToChargeRatio());
        shieldSPS = (int)Math.Round(shieldBlock.GetChargeRate() * 10000);
    }

    curTotal += curDefShields;
    maxTotal += maxDefShields;

    Echo($"-----Defense Shields-----\n{curDefShields} / {maxDefShields}\n");
}

void PrepLCD(DrawLCD drawLCD, int percent, float font)
{
    var lcdPanel = drawLCD.Surface;
    lcdPanel.Font = _DEBUG;
    lcdPanel.FontSize = font;
    lcdPanel.ContentType = ContentType.TEXT_AND_IMAGE;
    lcdPanel.Alignment = TextAlignment.CENTER;
    lcdPanel.TextPadding = 0;

    if (lcdPanel.CurrentlyShownImage != null)
        lcdPanel.ClearImagesFromSelection();

    if (percent > shieldHigh)
        lcdPanel.FontColor = txtColHigh;
    else if (percent > shieldLow)
        lcdPanel.FontColor = txtColMed;
    else if (percent > shieldCrit)
        lcdPanel.FontColor = txtColLow;
    else
        lcdPanel.FontColor = txtColCrit;
}

void SmBarFactory(int curShields, int maxShields, int percent, int sps, StringBuilder outSB)
{
    outSB.Clear()
  .Append("Shields: ")
  .Append(percent)
  .Append("%\n")
  .Append(lBarSurr);

    percent /= 5;

    int i = 0;
    for (; i < 10; i++)
    {
        if (i < percent)
        {
            outSB.Append(BarFill);
        }
        else
        {
            outSB.Append(BarEmpty);
        }
    }

    outSB
  .Append(rBarSurr)
  .Append("\n")
  .Append(lBarSurr);

    for (; i < 20; i++)
    {
        if (i < percent)
            outSB.Append(BarFill);
        else
            outSB.Append(BarEmpty);
    }

    outSB
  .Append(rBarSurr)
  .Append("\n\nShields:")
  .Append(FormatNum(curShields))
  .Append("\nFull:")
  .Append(FormatNum(maxShields))
  .Append("\nS\\s:")
  .Append(sps);
}

void LgBarFactory(int curShields, int maxShields, int percent, int sps, StringBuilder outSB)
{
    outSB.Clear()
  .Append("Shields: ")
  .Append(percent)
  .Append("%\n")
  .Append(lBarSurr);

    percent /= 5;

    for (int i = 0; i < 20; i++)
    {
        if (i < percent)
            outSB.Append(BarFill);
        else
            outSB.Append(BarEmpty);
    }

    outSB
  .Append(rBarSurr)
  .Append("\nShields: ")
  .Append(FormatNum(curShields))
  .Append("\nFull: ")
  .Append(FormatNum(maxShields))
  .Append("\nS\\s: ")
  .Append(sps);
}

string ImgNameFactory(int percent, string prefix, StringBuilder imgName)
{
    imgName.Clear()
  .Append(prefix);

    if (percent == 0)
        imgName.Append("000");
    else if (percent < 10)
        imgName.Append("00").Append(percent);
    else if (percent < 100)
        imgName.Append("0").Append(percent);
    else if (percent == 100)
        imgName.Append(percent);

    return imgName.ToString();
}

void SetImageLCD(string imgName, DrawLCD drawLCD)
{
    var lcdPanel = drawLCD.Surface;
    lcdPanel.ContentType = ContentType.TEXT_AND_IMAGE;
    lcdPanel.TextPadding = 0;
    var curImage = lcdPanel.CurrentlyShownImage;

    if (curImage == null || imgName != curImage)
    {
        lcdPanel.ClearImagesFromSelection();
        lcdPanel.AddImageToSelection(imgName);
    }

    if (lcdPanel.GetText().Length > 0)
        lcdPanel.WriteText("");
}

int CalcSPS(int curShields, int maxShields)
{
    if (curShields == maxShields)
    {
        StoreData(curShields, 0);
        return 0;
    }

    int sps = curShields - lastShield;

    if (sps == 0)
    {
        StoreData(curShields, lastSPS);
        return lastSPS;
    }

    StoreData(curShields, sps);
    return sps;
}

void StoreData(int curShield, int sps)
{
    lastShield = curShield;
    lastSPS = sps;
}

string FormatNum(int num)
{
    if (num >= 1000000000)
        return (num / 1000000000D).ToString("0.##b");

    if (num >= 1000000)
        return (num / 1000000D).ToString("0.##m");

    if (num >= 100000)
        return (num / 1000D).ToString("0.#k");

    if (num >= 1000)
        return (num / 1000D).ToString("0.##k");

    return num.ToString("N0");
}

void Main(string argument, UpdateType updateSource)
{
    try
    {
        SubMain(argument, updateSource);
    }
    catch (Exception e)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Exception Message:");
        sb.AppendLine($"   {e.Message}");
        sb.AppendLine();
        sb.AppendLine("Stack trace:");
        sb.AppendLine(e.StackTrace);
        sb.AppendLine();
        var exceptionDump = sb.ToString();
        Echo(exceptionDump);
        Me.CustomData = exceptionDump;
        throw;
    }
}

class BlockSurface
{
    public IMyTerminalBlock Block;
    public Dictionary<string, List<int>> Screens;
    public List<int> THYA_H_Screens, THYA_V_Screens, THYA_C_Screens;
    public List<int> THYA_BTL_Screens, THYA_BTS_Screens, THYA_TDL_Screens, THYA_TDS_Screens;
    public List<int> THYA_CTD_Screens, THYA_CRB_Screens, THYA_CCB_Screens;

    public long EntityId => Block.EntityId;

    List<DrawLCD> _drawLCDs;
    List<string> _iniSections;
    List<string> _iniLines;
    HashSet<int> _surfaceNumHash;
    IMyTextSurfaceProvider _provider;
    MyIni _ini = new MyIni();
    StringBuilder _lastConfig = new StringBuilder();
    bool _isFighter;
    bool _isCockpit;

    // Debug 
    //Program _p; 

    public BlockSurface(IMyTerminalBlock block, Program prog)
    {
        //_p = prog; 
        Block = block;
        _provider = block as IMyTextSurfaceProvider;

        int count = _provider.SurfaceCount;

        Screens = new Dictionary<string, List<int>>
{
  { "THYA-H", new List<int>(count) },
  { "THYA-V", new List<int>(count) },
  { "THYA-C", new List<int>(count) },
  { "THYA-BTL", new List<int>(count) },
  { "THYA-BTS", new List<int>(count) },
  { "THYA-TDL", new List<int>(count) },
  { "THYA-TDS", new List<int>(count) },
  { "THYA-CTD", new List<int>(count) },
  { "THYA-CRB", new List<int>(count) },
  { "THYA-CCB", new List<int>(count) }
};

        _drawLCDs = new List<DrawLCD>(count);
        _iniLines = new List<string>();
        _iniSections = new List<string>();
        _surfaceNumHash = new HashSet<int>();
        THYA_H_Screens = new List<int>(count);
        THYA_V_Screens = new List<int>(count);
        THYA_C_Screens = new List<int>(count);
        THYA_BTL_Screens = new List<int>(count);
        THYA_BTS_Screens = new List<int>(count);
        THYA_TDL_Screens = new List<int>(count);
        THYA_TDS_Screens = new List<int>(count);
        THYA_CTD_Screens = new List<int>(count);
        THYA_CRB_Screens = new List<int>(count);
        THYA_CCB_Screens = new List<int>(count);

        _isCockpit = block is IMyCockpit;
        _isFighter = _isCockpit && block.BlockDefinition.SubtypeName == _FIGHTER_SUBTYPE;
        var isProjector = Block is IMyProjector;

        for (int i = 0; i < _provider.SurfaceCount; i++)
        {
            bool isConsole = isProjector && i == 0;
            _drawLCDs.Add(new DrawLCD(_provider.GetSurface(i), block, _isCockpit, _isFighter, isConsole));
        }

        NeedsUpdate();
        UpdateConfig();
    }

    void ClearLists()
    {
        foreach (var list in Screens.Values)
            list.Clear();

        THYA_H_Screens.Clear();
        THYA_V_Screens.Clear();
        THYA_C_Screens.Clear();
        THYA_BTL_Screens.Clear();
        THYA_BTS_Screens.Clear();
        THYA_TDL_Screens.Clear();
        THYA_TDS_Screens.Clear();
        THYA_CTD_Screens.Clear();
        THYA_CRB_Screens.Clear();
        THYA_CCB_Screens.Clear();
        _surfaceNumHash.Clear();
        _iniSections.Clear();
    }

    public DrawLCD GetDrawLCD(int num)
    {
        return _drawLCDs[num];
    }

    public bool NeedsUpdate()
    {
        string data = Block.CustomData;

        if (_lastConfig.FastEquals(data))
            return false;

        _lastConfig.Clear().Append(data);
        return true;
    }

    public bool UpdateConfig(string input = null)
    {
        _ini.Clear();
        ClearLists();

        if (string.IsNullOrEmpty(input))
            input = _lastConfig.ToString();

        string preface, config;
        if (!IsValidConfig(input, out preface, out config) || !_ini.TryParse(config))
            return false;

        if (!_ini.ContainsSection("Shield Script"))
        {
            string newConfig = "";
            for (int i = 0; i < _provider.SurfaceCount; i++)
                newConfig += $"{i}=None\n";

            _ini.Set("Shield Script", "Config", newConfig);
            SetSectionComment();
            Block.CustomData = _ini.ToString();

            return false;
        }

        SetSectionComment();

        if (string.IsNullOrEmpty(_ini.EndContent))
            _ini.EndContent = " ";

        _lastConfig.Clear()
          .Append(preface)
          .Append(_ini.ToString());
        Block.CustomData = _lastConfig.ToString();

        var value = _ini.Get("Shield Script", "Config");
        if (value.IsEmpty)
            return false;

        value.GetLines(_iniLines);

        for (int i = 0; i < _iniLines.Count; i++)
        {
            var split = _iniLines[i].Split('=');
            int num;
            if (split.Length != 2 || !int.TryParse(split[0], out num) || num < 0 || num >= _provider.SurfaceCount || !_surfaceNumHash.Add(num))
                continue;

            var displayType = split[1];
            if (displayType.Equals("None", StringComparison.OrdinalIgnoreCase))
                continue;
            else if (displayType.Equals("THYA-H", StringComparison.OrdinalIgnoreCase))
                THYA_H_Screens.Add(num);
            else if (displayType.Equals("THYA-V", StringComparison.OrdinalIgnoreCase))
                THYA_V_Screens.Add(num);
            else if (displayType.Equals("THYA-C", StringComparison.OrdinalIgnoreCase))
                THYA_C_Screens.Add(num);
            else if (displayType.Equals("TDS", StringComparison.OrdinalIgnoreCase))
                THYA_TDS_Screens.Add(num);
            else if (displayType.Equals("TDL", StringComparison.OrdinalIgnoreCase))
                THYA_TDL_Screens.Add(num);
            else if (displayType.Equals("BTS", StringComparison.OrdinalIgnoreCase))
                THYA_BTS_Screens.Add(num);
            else if (displayType.Equals("BTL", StringComparison.OrdinalIgnoreCase))
                THYA_BTL_Screens.Add(num);
            else if (displayType.Equals("CTD", StringComparison.OrdinalIgnoreCase))
                THYA_CTD_Screens.Add(num);
            else if (displayType.Equals("CRB", StringComparison.OrdinalIgnoreCase))
                THYA_CRB_Screens.Add(num);
            else if (displayType.Equals("CCB", StringComparison.OrdinalIgnoreCase))
                THYA_CCB_Screens.Add(num);
        }

        return true;
    }

    bool IsValidConfig(string input, out string preface, out string config)
    {
        var startIdx = input.IndexOf("; Shield Script Config");
        if (startIdx < 0)
        {
            preface = input;
            config = string.Empty;
            return true;
        }

        preface = input.Substring(0, startIdx);
        config = input.Substring(startIdx);

        //var segment = new StringSegment(config); 
        //segment.GetLines(_iniLines); 
        //bool foundSection = false; 
        //bool inMultiLine = false; 

        return _ini.TryParse(config);

        //for (int i = 0; i < _iniLines.Count; i++) 
        //{ 
        //  var line = new TextPtr(_iniLines[i].Trim()); 
        //  if (line == "---") 
        //    break; 

        //  if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) 
        //  { 
        //    inMultiLine = false; 
        //    continue; 
        //  } 

        //  if (line.StartsWith("[") && line.FindInLine(']').Index > 0) 
        //  { 
        //    inMultiLine = false; 
        //    foundSection = true; 
        //    continue; 
        //  } 

        //  if (!foundSection) 
        //    return false; 

        //  var idx = line.Content.IndexOf('='); 
        //  if (idx + 2 > line.Content.Length) 
        //  { 
        //    inMultiLine = true; 
        //    continue; 
        //  } 

        //  if (idx < 0 && (!inMultiLine || !line.StartsWith("|"))) 
        //    return false; 
        //} 

        //return true; 
    }

    void SetSectionComment() => _ini.SetSectionComment("Shield Script", SECTION_COMMENT);

    const string SECTION_COMMENT = " Shield Script Config \n Below you'll find the configuration for the THYA Shield HUD Script.\n To set up your screens, fill in the VALUE portions below with the type\n of display you'd like. The numbers represent which display to use,\n and are in the same order as found in the terminal window.\n Valid types: THYA-H, THYA-C, THYA-V, TDS, TDL, BTS, BTL, CTD, CRB, CCB.\n EXAMPLE: 0=THYA-H -> will display the Horizontal graphics option\n ";
}

public class DefenseShields
{
    private IMyTerminalBlock _block;

    private readonly Func<IMyTerminalBlock, RayD, Vector3D?> _rayIntersectShield;
    private readonly Func<IMyTerminalBlock, LineD, Vector3D?> _lineIntersectShield;
    private readonly Func<IMyTerminalBlock, Vector3D, bool> _pointInShield;
    private readonly Func<IMyTerminalBlock, float> _getShieldPercent;
    private readonly Func<IMyTerminalBlock, int> _getShieldHeat;
    private readonly Func<IMyTerminalBlock, float> _getChargeRate;
    private readonly Func<IMyTerminalBlock, int> _hpToChargeRatio;
    private readonly Func<IMyTerminalBlock, float> _getMaxCharge;
    private readonly Func<IMyTerminalBlock, float> _getCharge;
    private readonly Func<IMyTerminalBlock, float> _getPowerUsed;
    private readonly Func<IMyTerminalBlock, float> _getPowerCap;
    private readonly Func<IMyTerminalBlock, float> _getMaxHpCap;
    private readonly Func<IMyTerminalBlock, bool> _isShieldUp;
    private readonly Func<IMyTerminalBlock, string> _shieldStatus;
    private readonly Func<IMyTerminalBlock, IMyEntity, bool, bool> _entityBypass;
    // Fields below do not require SetActiveShield to be defined first. 
    private readonly Func<IMyCubeGrid, bool> _gridHasShield;
    private readonly Func<IMyCubeGrid, bool> _gridShieldOnline;
    private readonly Func<IMyEntity, bool> _protectedByShield;
    private readonly Func<IMyEntity, IMyTerminalBlock> _getShieldBlock;
    private readonly Func<IMyTerminalBlock, bool> _isShieldBlock;
    private readonly Func<Vector3D, IMyTerminalBlock> _getClosestShield;
    private readonly Func<IMyTerminalBlock, Vector3D, double> _getDistanceToShield;
    private readonly Func<IMyTerminalBlock, Vector3D, Vector3D?> _getClosestShieldPoint;

    public void SetActiveShield(IMyTerminalBlock block) => _block = block; // AutoSet to TapiFrontend(block) if shield exists on grid. 

    public DefenseShields(IMyTerminalBlock block)
    {
        _block = block;
        var delegates = _block.GetProperty("DefenseSystemsPbAPI")?.As<IReadOnlyDictionary<string, Delegate>>().GetValue(_block);
        if (delegates == null) return;

        _rayIntersectShield = (Func<IMyTerminalBlock, RayD, Vector3D?>)delegates["RayIntersectShield"];
        _lineIntersectShield = (Func<IMyTerminalBlock, LineD, Vector3D?>)delegates["LineIntersectShield"];
        _pointInShield = (Func<IMyTerminalBlock, Vector3D, bool>)delegates["PointInShield"];
        _getShieldPercent = (Func<IMyTerminalBlock, float>)delegates["GetShieldPercent"];
        _getShieldHeat = (Func<IMyTerminalBlock, int>)delegates["GetShieldHeat"];
        _getChargeRate = (Func<IMyTerminalBlock, float>)delegates["GetChargeRate"];
        _hpToChargeRatio = (Func<IMyTerminalBlock, int>)delegates["HpToChargeRatio"];
        _getMaxCharge = (Func<IMyTerminalBlock, float>)delegates["GetMaxCharge"];
        _getCharge = (Func<IMyTerminalBlock, float>)delegates["GetCharge"];
        _getPowerUsed = (Func<IMyTerminalBlock, float>)delegates["GetPowerUsed"];
        _getPowerCap = (Func<IMyTerminalBlock, float>)delegates["GetPowerCap"];
        _getMaxHpCap = (Func<IMyTerminalBlock, float>)delegates["GetMaxHpCap"];
        _isShieldUp = (Func<IMyTerminalBlock, bool>)delegates["IsShieldUp"];
        _shieldStatus = (Func<IMyTerminalBlock, string>)delegates["ShieldStatus"];
        _entityBypass = (Func<IMyTerminalBlock, IMyEntity, bool, bool>)delegates["EntityBypass"];
        _gridHasShield = (Func<IMyCubeGrid, bool>)delegates["GridHasShield"];
        _gridShieldOnline = (Func<IMyCubeGrid, bool>)delegates["GridShieldOnline"];
        _protectedByShield = (Func<IMyEntity, bool>)delegates["ProtectedByShield"];
        _getShieldBlock = (Func<IMyEntity, IMyTerminalBlock>)delegates["GetShieldBlock"];
        _isShieldBlock = (Func<IMyTerminalBlock, bool>)delegates["IsShieldBlock"];
        _getClosestShield = (Func<Vector3D, IMyTerminalBlock>)delegates["GetClosestShield"];
        _getDistanceToShield = (Func<IMyTerminalBlock, Vector3D, double>)delegates["GetDistanceToShield"];
        _getClosestShieldPoint = (Func<IMyTerminalBlock, Vector3D, Vector3D?>)delegates["GetClosestShieldPoint"];

        if (!IsShieldBlock()) _block = GetShieldBlock(_block.CubeGrid) ?? _block;
    }
    public Vector3D? RayIntersectShield(RayD ray) => _rayIntersectShield?.Invoke(_block, ray) ?? null;
    public Vector3D? LineIntersectShield(LineD line) => _lineIntersectShield?.Invoke(_block, line) ?? null;
    public bool PointInShield(Vector3D pos) => _pointInShield?.Invoke(_block, pos) ?? false;
    public float GetShieldPercent() => _getShieldPercent?.Invoke(_block) ?? -1;
    public int GetShieldHeat() => _getShieldHeat?.Invoke(_block) ?? -1;
    public float GetChargeRate() => _getChargeRate?.Invoke(_block) ?? -1;
    public float HpToChargeRatio() => _hpToChargeRatio?.Invoke(_block) ?? -1;
    public float GetMaxCharge() => _getMaxCharge?.Invoke(_block) ?? -1;
    public float GetCharge() => _getCharge?.Invoke(_block) ?? -1;
    public float GetPowerUsed() => _getPowerUsed?.Invoke(_block) ?? -1;
    public float GetPowerCap() => _getPowerCap?.Invoke(_block) ?? -1;
    public float GetMaxHpCap() => _getMaxHpCap?.Invoke(_block) ?? -1;
    public bool IsShieldUp() => _isShieldUp?.Invoke(_block) ?? false;
    public string ShieldStatus() => _shieldStatus?.Invoke(_block) ?? string.Empty;
    public bool EntityBypass(IMyEntity entity, bool remove = false) => _entityBypass?.Invoke(_block, entity, remove) ?? false;
    public bool GridHasShield(IMyCubeGrid grid) => _gridHasShield?.Invoke(grid) ?? false;
    public bool GridShieldOnline(IMyCubeGrid grid) => _gridShieldOnline?.Invoke(grid) ?? false;
    public bool ProtectedByShield(IMyEntity entity) => _protectedByShield?.Invoke(entity) ?? false;
    public IMyTerminalBlock GetShieldBlock(IMyEntity entity) => _getShieldBlock?.Invoke(entity) ?? null;
    public bool IsShieldBlock() => _isShieldBlock?.Invoke(_block) ?? false;
    public IMyTerminalBlock GetClosestShield(Vector3D pos) => _getClosestShield?.Invoke(pos) ?? null;
    public double GetDistanceToShield(Vector3D pos) => _getDistanceToShield?.Invoke(_block, pos) ?? -1;
    public Vector3D? GetClosestShieldPoint(Vector3D pos) => _getClosestShieldPoint?.Invoke(_block, pos) ?? null;
}

class DrawLCD
{
    public readonly IMyTerminalBlock Block;
    public readonly IMyTextSurface Surface;
    public readonly bool IsCockpitLCD, IsFighterLCD, IsConsoleBlock;

    public DrawLCD(IMyTextSurface surface, IMyTerminalBlock block, bool isCockpit = false, bool isFighter = false, bool isConsole = false)
    {
        Block = block;
        Surface = surface;
        IsCockpitLCD = isCockpit;
        IsFighterLCD = isFighter;
        IsConsoleBlock = isConsole;
    }
}

class ShieldLCD
{
    string _lastText = null;
    public StringBuilder TextBuilder = new StringBuilder();
    public StringBuilder TextBuilderSm = new StringBuilder();
    public List<DrawLCD> DrawLCDs = new List<DrawLCD>();

    public void AddPanel(IMyTextPanel lcd)
    {
        var drawLCD = new DrawLCD(lcd, lcd);
        AddDrawLCD(drawLCD);
    }

    public void AddDrawLCD(DrawLCD drawLCD)
    {
        DrawLCDs.Add(drawLCD);
    }

    public bool TextChanged()
    {
        var text = TextBuilder.ToString();

        if (_lastText == text)
            return false;

        _lastText = text;
        return true;
    }

    public void ResetText()
    {
        _lastText = "";
    }
} 
 
} 
 
public static class Extensions
{
    public static bool FastEquals(this StringBuilder sb, string str)
    {
        if (sb.Length != str.Length)
            return false;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != sb[i])
                return false;
        }

        return true;
    }



}

    //END TEMPLATE
}