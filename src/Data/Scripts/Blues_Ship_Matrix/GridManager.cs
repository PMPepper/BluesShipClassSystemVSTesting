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
                gridsData.Add(grid.EntityId, new GridData(grid, 0));
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
        public long ShipClassId;

        public GridData(IMyCubeGrid grid, int shipClassId)
        {
            Grid = grid;
            ShipClassId = shipClassId;

            grid.OnBlockAdded += Grid_OnBlockAdded;
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
}
