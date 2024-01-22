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
        private Comms<ShipClassMessage> ShipClassComms = new Comms<ShipClassMessage>(SHIP_CLASS_MESSAGE_ID);

        public GridManager()
        {
            ShipClassComms.OnMessage = OnShipClassMessage;
        }

        internal void OnShipClassMessage(ShipClassMessage message, ulong from)
        {
            if(from == 0 && ModSessionManager.IsServer)//from the server
            {
                string msg = $"Recieved ShipClassMessage message from server, but this is the server";
                ModSessionManager.ClientDebug(msg);
                ModSessionManager.Log(msg, 2);

                return;
            }

            if(from != 0 && !ModSessionManager.IsServer)
            {
                string msg = $"Recieved ShipClassMessage message from user, but non-server should not get message from players";
                ModSessionManager.ClientDebug(msg);
                ModSessionManager.Log(msg, 2);

                return;
            }

            GridData gridData = gridsData[message.EntityId];

            //TODO check ShipClassId is valid value

            if (gridData == null) {
                ModSessionManager.Log($"Recieved ShipClassMessage regarding unknown grid {message.EntityId}", 1);
            } else
            {
                gridData.SetShipClass(message.ShipClassId);
            }
        }
 

        public GridData GetGridData(IMyCubeGrid grid) {
            return gridsData[grid.EntityId];
        }

        public GridData GetGridData(IMyCubeBlock block) {
            ModSessionManager.ClientDebug($"GetGridData: {block.CubeGrid.EntityId}");
            return gridsData[block.CubeGrid.EntityId];
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
                ModSessionManager.ClientDebug($"Add Grid: {grid.EntityId}");
                gridsData.Add(grid.EntityId, new GridData(grid, 0, ShipClassComms));
                grid.OnMarkForClose += GridMarkedForClose;
            }
        }

        private void GridMarkedForClose(IMyEntity ent)
        {
            ModSessionManager.ClientDebug($"Remove Grid: {ent.EntityId}");
            gridsData.Remove(ent.EntityId);
        }

        //If we want to run logic on grids, do it here
        //public override void UpdateBeforeSimulation()
        //{
        //    try
        //    {
        //        foreach (var grid in gridsData.Values)
        //        {
        //            if (grid.MarkedForClose)
        //                continue;

        //            // do your thing
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

        //        if (MyAPIGateway.Session?.Player != null)
        //            MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} | Send SpaceEngineers.Log to mod author ]", 10000, MyFontEnum.Red);
        //    }
        //}
    }

    public class GridData {
        public IMyCubeGrid Grid;
        private long _ShipClassId;

        private Comms<ShipClassMessage> Comms;

        public long ShipClassId { get { return _ShipClassId; } }

        internal GridData(IMyCubeGrid grid, int shipClassId, Comms<ShipClassMessage> comms)
        {
            Grid = grid;
            _ShipClassId = shipClassId;
            Comms = comms;

            grid.OnBlockAdded += Grid_OnBlockAdded;
        }

        public void SetShipClass(long newShipClass) {
            if (ModSessionManager.IsServer)
            {
                _ShipClassId = newShipClass;

                if (ModSessionManager.IsDedicated)
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

        private void Grid_OnBlockAdded(IMySlimBlock obj)
        {
            IMyCubeBlock fatBlock = obj.FatBlock;

            if (fatBlock is IMyBeacon) {
                ModSessionManager.ClientDebug("Beacon Added");
            }
        }

        public bool MarkedForClose { get { return Grid.MarkedForClose; } }
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
