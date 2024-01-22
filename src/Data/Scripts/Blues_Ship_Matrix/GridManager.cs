using System;
using System.Collections.Generic;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    public class GridManager
    {
        private readonly Dictionary<long, GridData> gridsData = new Dictionary<long, GridData>();

        private const ushort SHIP_CLASS_MESSAGE_ID = 53642;
        internal Comms<ShipClassMessage> ShipClassComms = new Comms<ShipClassMessage>(SHIP_CLASS_MESSAGE_ID);

        public GridManager()
        {
            ShipClassComms.OnMessage = OnShipClassMessage;
        }

        internal void OnShipClassMessage(ShipClassMessage message, ulong from)
        {
            if(from == 0 && Constants.IsServer)//from the server
            {
                string msg = $"Recieved ShipClassMessage message from server, but this is the server";
                Utils.ClientDebug(msg);
                Utils.Log(msg, 2);

                return;
            }

            if(from != 0 && !Constants.IsServer)
            {
                string msg = $"Recieved ShipClassMessage message from user, but non-server should not get message from players";
                Utils.ClientDebug(msg);
                Utils.Log(msg, 2);

                return;
            }

            GridData gridData = gridsData[message.EntityId];

            //TODO check ShipClassId is valid value

            if (gridData == null) {
                Utils.Log($"Recieved ShipClassMessage regarding unknown grid {message.EntityId}", 1);
            } else
            {
                gridData._SetShipClass(message.ShipClassId);
            }
        }
 

        public GridData GetGridData(IMyCubeGrid grid) {
            if(grid == null)
            {
                Utils.Log($"GetGridData: grid is null", 1);
                return null;
            }

            if(!gridsData.ContainsKey(grid.EntityId))
            {
                Utils.Log($"GetGridData: unknown grid: {grid.EntityId}", 1);
                return null;
            }

            return gridsData[grid.EntityId];
        }

        public GridData GetGridData(IMyCubeBlock block) {
            Utils.ClientDebug($"GetGridData: {block.CubeGrid.EntityId}");
            if (block == null)
            {
                Utils.Log($"GetGridData: block is null", 1);
                return null;
            }

            return GetGridData(block.CubeGrid);
        }

        public void LoadData()
        {
            MyAPIGateway.Entities.OnEntityAdd += EntityAdded;
        }

        public void UnloadData()
        {
            MyAPIGateway.Entities.OnEntityAdd -= EntityAdded;

            gridsData.Clear();
        }

        private void EntityAdded(IMyEntity ent)
        {
            var grid = ent as IMyCubeGrid;

            if (grid != null && !grid.MarkedForClose)
            {
                Utils.ClientDebug($"Add Grid: {grid.EntityId}");
                gridsData.Add(grid.EntityId, new GridData(grid, this));
                grid.OnMarkForClose += GridMarkedForClose;
            }
        }

        private void GridMarkedForClose(IMyEntity ent)
        {
            Utils.ClientDebug($"Remove Grid: {ent.EntityId}");
            Utils.Log($"Remove Grid: {ent.EntityId}", 0);
            var gridData = GetGridData(ent as IMyCubeGrid);

            if(gridData != null)
            {
                gridData.MarkedForClose();

                gridsData.Remove(ent.EntityId);

            }
            
        }
    }

    public class GridData {
        public IMyCubeGrid Grid;
        private long _ShipClassId = 0;

        private ISet<IMyBeacon> Beacons = new HashSet<IMyBeacon>();

        private Comms<ShipClassMessage> Comms;

        public long ShipClassId { get { return _ShipClassId; } }

        internal GridData(IMyCubeGrid grid, GridManager gridManager)
        {
            Grid = grid;
            Comms = gridManager.ShipClassComms;

            grid.OnBlockAdded += Grid_OnBlockAdded;
            grid.OnBlockRemoved += Grid_OnBlockRemoved;

            long shipClassId = 0;

            foreach (var Beacon in grid.GetFatBlocks<IMyBeacon>())
            {
                Beacons.Add(Beacon);

                if(shipClassId == 0 && Beacon.CustomData != null)
                {
                    //attempt to parse ship class id from beacon custom data
                    shipClassId = UnserialiseBeaconData(Beacon.CustomData);

                    Utils.ClientDebug($"Unserialised ship class from Beacon as {_ShipClassId}");
                }
            }

            _SetShipClass(shipClassId);
        }

        internal void MarkedForClose()
        {
            //Tidy up/clear references
            Grid = null;
            Beacons.Clear();
            Beacons = null;
            Comms = null;
        }

        public void SetShipClass(long newShipClass) {
            if (Constants.IsServer)
            {
                _SetShipClass(newShipClass);

                if (Constants.IsDedicated)
                {
                    // Send message to clients to inform them this value has changed
                    Comms.SendMessage(new ShipClassMessage(Grid.EntityId, newShipClass), false);
                }
            }
            else
            {
                // Send a request to the server to update this value
                Comms.SendMessage(new ShipClassMessage(Grid.EntityId, newShipClass), true);
            }
        }

        internal void _SetShipClass(long newShipClass)
        {
            _ShipClassId = newShipClass;

            foreach(var Beacon in Beacons)
            {
                Beacon.CustomData = SerialiseShipClass();
            }
        }

        private void AddBeacon(IMyBeacon Beacon)
        {
            Beacons.Add(Beacon);
            Beacon.CustomData = SerialiseShipClass();
        }

        private void Grid_OnBlockAdded(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;

            if (fatBlock is IMyBeacon) {
                AddBeacon(fatBlock as IMyBeacon);
            }
        }

        private void Grid_OnBlockRemoved(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;

            if (fatBlock is IMyBeacon)
            {
                Beacons.Remove(fatBlock as IMyBeacon);
                Utils.ClientDebug("Beacon Removed");
            }
        }

        private string SerialiseShipClass()
        {
            //format = [data format version int]:[Ship Class Id]
            return $"1:{ShipClassId}";
        }

        public static long UnserialiseBeaconData(string beaconCustomData)
        {
            if (!String.IsNullOrWhiteSpace(beaconCustomData))
            {
                Utils.Log($"Unserialising beacon data = {beaconCustomData}", 0);
                var beaconDataSplit = beaconCustomData.Split(':');
                int serialisationVersion = 0;

                try
                {
                    serialisationVersion = int.Parse(beaconDataSplit[0]);
                }
                catch (Exception e)
                {
                    Utils.Log($"Failed to unserialise beacon data, error = {e.Message}", 1);

                    return 0;
                }

                switch (serialisationVersion)
                {
                    case 1:
                        return Convert.ToInt64(beaconDataSplit[1]);
                    default:
                        Utils.Log($"Failed to unserialise beacon data, unknown format = \"{serialisationVersion}\", CustomData = {beaconCustomData}", 1);
                        return 0;
                }

            }

            return 0;
        }
    }

    internal struct ShipClassMessage
    {
        public long EntityId;
        public long ShipClassId;

        internal ShipClassMessage(long entityId, long shipClassId) {
            EntityId = entityId;
            ShipClassId = shipClassId;
        }

    }
}
