using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRageMath;
using IngameTextSurface = Sandbox.ModAPI.Ingame.IMyTextSurface;
using IngameCubeBlock = VRage.Game.ModAPI.Ingame.IMyCubeBlock;
using IngameIMyEntity = VRage.Game.ModAPI.Ingame.IMyEntity;
using VRage.Utils;
using VRage.Game.GUI.TextPanel;
using VRage.Game;
using Sandbox.Game.Entities;

namespace YourName.ModName.src.Data.Scripts.Blues_Ship_Matrix
{
    [MyTextSurfaceScript("GridStatusLCDScript", "Ship class status")]
    class GridStatusLCDScript : MyTSSCommon
    {
        public override ScriptUpdate NeedsUpdate => ScriptUpdate.Update10; // frequency that Run() is called.
        private readonly IMyTerminalBlock TerminalBlock;

        private CubeGridLogic CubeGridLogic { get { return TerminalBlock?.GetGridLogic(); } }

        private Table GridResultsTable = new Table()
        {
            Columns = new List<Column>() {
                new Column() { Name = "Property" },
                new Column() { Name = "Value", Alignment = TextAlignment.RIGHT, FreeSpace = 1 },
                new Column() { Name = "Max" },
                new Column() { Name = "Success" }
            }
        };

        public GridStatusLCDScript(IMyTextSurface surface, IngameCubeBlock block, Vector2 size) : base(surface, block, size)
        {
            TerminalBlock = (IMyTerminalBlock)block; // internal stored m_block is the ingame interface which has no events, so can't unhook later on, therefore this field is required.
            TerminalBlock.OnMarkForClose += BlockMarkedForClose; // required if you're gonna make use of Dispose() as it won't get called when block is removed or grid is cut/unloaded.

            // Called when script is created.
            // This class is instanced per LCD that uses it, which means the same block can have multiple instances of this script aswell (e.g. a cockpit with all its screens set to use this script).
        }

        public override void Dispose()
        {
            base.Dispose(); // do not remove
            TerminalBlock.OnMarkForClose -= BlockMarkedForClose;

            // Called when script is removed for any reason, so that you can clean up stuff if you need to.
        }

        void BlockMarkedForClose(IngameIMyEntity ent)
        {
            Dispose();
        }

        // gets called at the rate specified by NeedsUpdate
        // it can't run every tick because the LCD is capped at 6fps anyway.
        public override void Run()
        {
            try
            {
                base.Run(); // do not remove

                // hold L key to see how the error is shown, remove this after you've played around with it =)
                if (MyAPIGateway.Input.IsKeyPress(VRage.Input.MyKeys.L))
                    throw new Exception("Oh noes an error :}");

                Draw();
            }
            catch (Exception e) // no reason to crash the entire game just for an LCD script, but do NOT ignore them either, nag user so they report it :}
            {
                DrawError(e);
            }
        }

        void Draw() // this is a custom method which is called in Run().
        {
            /*Vector2 screenSize = Surface.SurfaceSize;
            Vector2 screenCorner = (Surface.TextureSize - screenSize) * 0.5f;*/
            Vector2 padding = new Vector2(16, 16);
            var SuccessColor = Color.Green;
            var FailColor = Color.Red;

            /*RectangleF _viewport = new RectangleF(
                (Surface.TextureSize - Surface.SurfaceSize) / 2f,
                Surface.SurfaceSize
            );*/

            var frame = Surface.DrawFrame();


            // Drawing sprites works exactly like in PB API.
            // Therefore this guide applies: https://github.com/malware-dev/MDK-SE/wiki/Text-Panels-and-Drawing-Sprites

            AddBackground(frame, Color.White.Alpha(0.05f));
            
            // the colors in the terminal are Surface.ScriptBackgroundColor and Surface.ScriptForegroundColor, the other ones without Script in name are for text/image mode.
            var shipClass = TerminalBlock.GetGridLogic().ShipClass;

            if(shipClass == null)
            {
                return;
            }

            //TODO cache this and only recalculate when things change?
            var checkGridResult = shipClass.CheckGrid(TerminalBlock.CubeGrid);
            GridResultsTable.Clear();

            //var cubeGrid = TerminalBlock.CubeGrid as MyCubeGrid;

            //int lineNumber = 0;
            
            frame.Add(CreateLine($"Ship class: {shipClass.Name}", 0, padding));

            if(checkGridResult.MaxBlocks.Active)
            {
                GridResultsTable.Rows.Add(new Row() {
                    new Cell() {Value = "Blocks"},
                    new Cell() {Value = checkGridResult.MaxBlocks.Value.ToString()},
                    new Cell() {Value = checkGridResult.MaxBlocks.Max.ToString(), Color = checkGridResult.MaxBlocks.Passed ? SuccessColor : FailColor },
                    new Cell() {Value = checkGridResult.MaxBlocks.Max.ToString()},
                });
            }

            if (checkGridResult.MaxMass.Active)
            {
                GridResultsTable.Rows.Add(new Row() {
                    new Cell() {Value = "Mass"},
                    new Cell() {Value = checkGridResult.MaxMass.Value.ToString()},
                    new Cell() {Value = checkGridResult.MaxMass.Max.ToString(), Color = checkGridResult.MaxMass.Passed ? SuccessColor : FailColor },
                    new Cell() {Value = checkGridResult.MaxMass.Max.ToString()},
                });
            }

            if (checkGridResult.MaxPCU.Active)
            {
                GridResultsTable.Rows.Add(new Row() {
                    new Cell() {Value = "PCU"},
                    new Cell() {Value = checkGridResult.MaxPCU.Value.ToString()},
                    new Cell() {Value = checkGridResult.MaxPCU.Max.ToString(), Color = checkGridResult.MaxPCU.Passed ? SuccessColor : FailColor },
                    new Cell() {Value = checkGridResult.MaxPCU.Max.ToString()},
                });
            }

            GridResultsTable.Render(frame, ((Surface.TextureSize - Surface.SurfaceSize) * 0.5f ) + padding, Surface.SurfaceSize.X - (padding.X * 2));

            // add more sprites and stuff

            frame.Dispose(); // send sprites to the screen
        }

        MySprite CreateLine(string text, int rowNumber, Vector2 padding)
        {
            Vector2 screenSize = Surface.SurfaceSize;
            Vector2 screenCorner = (Surface.TextureSize - screenSize) * 0.5f;

            var sprite = MySprite.CreateText(text, "Monospace", Color.White, 1f, TextAlignment.LEFT);
            sprite.Position = screenCorner + padding + new Vector2(0, rowNumber * 30); // 16px from topleft corner of the visible surface

            return sprite;
        }

        void DrawError(Exception e)
        {
            MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");

            try // first try printing the error on the LCD
            {
                Vector2 screenSize = Surface.SurfaceSize;
                Vector2 screenCorner = (Surface.TextureSize - screenSize) * 0.5f;

                var frame = Surface.DrawFrame();

                var bg = new MySprite(SpriteType.TEXTURE, "SquareSimple", null, null, Color.Black);
                frame.Add(bg);

                var text = MySprite.CreateText($"ERROR: {e.Message}\n{e.StackTrace}\n\nPlease send screenshot of this to mod author.\n{MyAPIGateway.Utilities.GamePaths.ModScopeName}", "White", Color.Red, 0.7f, TextAlignment.LEFT);
                text.Position = screenCorner + new Vector2(16, 16);
                frame.Add(text);

                frame.Dispose();
            }
            catch (Exception e2)
            {
                MyLog.Default.WriteLineAndConsole($"Also failed to draw error on screen: {e2.Message}\n{e2.StackTrace}");

                if (MyAPIGateway.Session?.Player != null)
                    MyAPIGateway.Utilities.ShowNotification($"[ ERROR: {GetType().FullName}: {e.Message} | Send SpaceEngineers.Log to mod author ]", 10000, MyFontEnum.Red);
            }
        }
    }

    class Table {
        public List<Column> Columns = new List<Column>();

        public List<Row> Rows = new List<Row>();

        public void Clear()
        {
            Rows.Clear();
        }

        public void Render(MySpriteDrawFrame frame, Vector2 topLeft, float width)
        {
            //TODO Going to need to figure out how big a character is to calculate stuff. Just guess to start with I suppose?
            float charWidth = 5f;
        }
    }

    class Column
    {
        public string Name;
        public float FreeSpace = 0;
        public TextAlignment Alignment = TextAlignment.LEFT;
    }

    class Row : List<Cell>
    {
        public float Height = 30f;
    }

    class Cell
    {
        public string Value;
        public Color Color = Color.White;
    }
}
