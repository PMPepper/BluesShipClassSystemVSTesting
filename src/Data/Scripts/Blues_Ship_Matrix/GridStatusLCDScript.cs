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

        private CubeGridLogic GridLogic { get { return TerminalBlock?.GetGridLogic(); } }

        private Table HeaderTable = new Table() {
            Columns = new List<Column>() {
                new Column() { Name = "Label" },
                new Column() { Name = "Name" },
                new Column() { Name = "Success" }
            }
        };

        private Table GridResultsTable = new Table()
        {
            Columns = new List<Column>() {
                new Column() { Name = "Property" },
                new Column() { Name = "Value", Alignment = TextAlignment.RIGHT },
                new Column() { Name = "Separator" },
                new Column() { Name = "Max" },
                new Column() { Name = "Success" }
            }
        };

        private Table AppliedModifiersTable = new Table()
        {
            Columns = new List<Column>() {
                new Column() { Name = "ModifierName" },
                new Column() { Name = "Value", Alignment = TextAlignment.RIGHT },
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
            Vector2 screenSize = Surface.SurfaceSize;
            Vector2 screenTopLeft = (Surface.TextureSize - screenSize) * 0.5f;
            Vector2 padding = new Vector2(16, 16);
            Vector2 cellGap = new Vector2(5, 5);
            float screenInnerWidth = Surface.SurfaceSize.X - (padding.X * 2);
            var SuccessColor = Color.Green;
            var FailColor = Color.Red;

            /*RectangleF _viewport = new RectangleF(
                (Surface.TextureSize - Surface.SurfaceSize) / 2f,
                Surface.SurfaceSize
            );*/

            var frame = Surface.DrawFrame();

            // https://github.com/malware-dev/MDK-SE/wiki/Text-Panels-and-Drawing-Sprites

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

            Vector2 currentPosition;

            //Render the header
            HeaderTable.Clear();

            HeaderTable.Rows.Add(new Row()
            {
                new Cell() { Value = "Ship class:" },
                new Cell() { Value = shipClass.Name, Color = checkGridResult.Passed ? SuccessColor : FailColor },
                checkGridResult.Passed ? null : new Cell() { Value = "X", Color = FailColor }
            });

            HeaderTable.Render(frame, screenTopLeft + padding, screenInnerWidth, new Vector2(15, 0), out currentPosition, 0.75f);

            //Render the results checklist

            if (checkGridResult.MaxBlocks.Active)
            {
                GridResultsTable.Rows.Add(new Row() {
                    new Cell() {Value = "Blocks"},
                    new Cell() {Value = checkGridResult.MaxBlocks.Value.ToString()},
                    new Cell() {Value = "/"},
                    new Cell() {Value = checkGridResult.MaxBlocks.Max.ToString(), Color = checkGridResult.MaxBlocks.Passed ? SuccessColor : FailColor },
                    checkGridResult.MaxBlocks.Passed ? null : new Cell() {Value = "X", Color = FailColor},
                });
            }

            if (checkGridResult.MaxMass.Active)
            {
                GridResultsTable.Rows.Add(new Row() {
                    new Cell() {Value = "Mass"},
                    new Cell() {Value = checkGridResult.MaxMass.Value.ToString()},
                    new Cell() {Value = "/"},
                    new Cell() {Value = checkGridResult.MaxMass.Max.ToString(), Color = checkGridResult.MaxMass.Passed ? SuccessColor : FailColor },
                    checkGridResult.MaxMass.Passed ? null : new Cell() {Value = "X", Color = FailColor},
                });
            }

            if (checkGridResult.MaxPCU.Active)
            {
                GridResultsTable.Rows.Add(new Row() {
                    new Cell() {Value = "PCU"},
                    new Cell() {Value = checkGridResult.MaxPCU.Value.ToString()},
                    new Cell() {Value = "/"},
                    new Cell() {Value = checkGridResult.MaxPCU.Max.ToString(), Color = checkGridResult.MaxPCU.Passed ? SuccessColor : FailColor },
                    checkGridResult.MaxPCU.Passed ? null : new Cell() {Value = "X", Color = FailColor},
                });
            }

            if(shipClass.BlockLimits != null)
            {
                for (int i = 0; i < shipClass.BlockLimits.Length; i++)
                {
                    var blockLimit = shipClass.BlockLimits[i];
                    var checkResults = checkGridResult.BlockLimits[i];

                    GridResultsTable.Rows.Add(new Row() {
                        new Cell() {Value = blockLimit.Name},
                        new Cell() {Value = checkResults.Score.ToString()},
                        new Cell() {Value = "/"},
                        new Cell() {Value = checkResults.Max.ToString(), Color = checkResults.Passed ? SuccessColor : FailColor },
                        checkResults.Passed ? null : new Cell() {Value = "X", Color = FailColor},
                    });
                }
            }
            
            Vector2 gridResultsTableTopLeft = currentPosition + new Vector2(0, 5);

            GridResultsTable.Render(frame, gridResultsTableTopLeft, screenInnerWidth, cellGap, out currentPosition, 0.5f);

            //Applied modifiers
            frame.Add(CreateLine($"Applied modfiers", currentPosition + new Vector2(0, 5), out currentPosition, 0.75f));

            AppliedModifiersTable.Clear();

            Vector2 appliedModifiersTableTopLeft = currentPosition + new Vector2(0, 5);

            foreach(var modifierValue in GridLogic.Modifiers.GetModifierValues())
            {
                AppliedModifiersTable.Rows.Add(new Row()
                {
                    new Cell() { Value = $"{modifierValue.Name}:" },
                    new Cell() { Value = modifierValue.Value.ToString() },
                });
            }

            AppliedModifiersTable.Render(frame, appliedModifiersTableTopLeft, screenInnerWidth, cellGap, out currentPosition, 0.5f);

            frame.Dispose(); // send sprites to the screen
        }

        MySprite CreateLine(string text, Vector2 position, float scale = 1)
        {
            var ignored = new Vector2();

            return CreateLine(text, position, out ignored, scale);
        }

        MySprite CreateLine(string text, Vector2 position, out Vector2 positionAfter, float scale = 1)
        {
            var sprite = MySprite.CreateText(text, "Monospace", Color.White, scale, TextAlignment.LEFT);
            sprite.Position = position;// screenCorner + padding + new Vector2(0, y); // 16px from topleft corner of the visible surface

            positionAfter = position + new Vector2(0, TextUtils.GetTextHeight(text, scale));

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

        public void Render(MySpriteDrawFrame frame, Vector2 topLeft, float width, Vector2 cellGap, float scale = 1)
        {
            Vector2 ignored;

            Render(frame, topLeft, width, cellGap, out ignored, scale);
        }

        public void Render(MySpriteDrawFrame frame, Vector2 topLeft, float width, Vector2 cellGap, out Vector2 positionAfter, float scale = 1)
        {
            //Calculate column widths & row heights
            float[] columnContentWidths = new float[Columns.Count];
            float[] columnWidths = new float[Columns.Count];
            float[] rowHeights = new float[Rows.Count];
            float totalFreeSpaceWeight = 0;
            float minWidthRequired = 0;

            for(int colNum = 0; colNum < Columns.Count; colNum++)
            {
                totalFreeSpaceWeight += Columns[colNum].FreeSpace;

                //foreach (var row in Rows)
                for (int rowNum = 0; rowNum < Rows.Count; rowNum++)
                {
                    var row = Rows[rowNum];
                    var cell = row[colNum];

                    if(cell != null && !string.IsNullOrEmpty(cell.Value))
                    {
                        columnContentWidths[colNum] = Math.Max(columnContentWidths[colNum], TextUtils.GetTextWidth(cell.Value, scale));
                        rowHeights[rowNum] = Math.Max(rowHeights[rowNum], TextUtils.GetTextHeight(cell.Value, scale));
                    }
                }

                minWidthRequired += columnContentWidths[colNum] + (colNum > 0 ? cellGap.X : 0);
                columnWidths[colNum] = columnContentWidths[colNum];
            }

            //distribute free space
            if(minWidthRequired < width && totalFreeSpaceWeight > 0)
            {
                float freeSpace = width - minWidthRequired;

                for (int i = 0; i < Columns.Count; i++)
                {
                    if(Columns[i].FreeSpace > 0)
                    {
                        columnWidths[i] += freeSpace * (Columns[i].FreeSpace / totalFreeSpaceWeight);
                    }
                }
            }

            var rowTopLeft = topLeft;
            var tableHeight = 0f;

            //render rows
            for (int rowNum = 0; rowNum < Rows.Count; rowNum++)
            {
                var row = Rows[rowNum];
                
                float rowX = 0;

                for (int colNum = 0; colNum < Columns.Count; colNum++)
                {
                    var cell = row[colNum];
                    var column = Columns[colNum];

                    if (cell != null)
                    {
                        var sprite = MySprite.CreateText(cell.Value, "Monospace", cell.Color, scale, column.Alignment);

                        switch(column.Alignment)
                        {
                            case TextAlignment.LEFT:
                                sprite.Position = rowTopLeft + new Vector2(rowX, 0);
                                break;
                            case TextAlignment.RIGHT:
                                sprite.Position = rowTopLeft + new Vector2(rowX + columnWidths[colNum], 0);
                                break;
                            case TextAlignment.CENTER:
                                sprite.Position = rowTopLeft + new Vector2(rowX + (columnWidths[colNum] / 2), 0);
                                
                                break;
                        }

                        frame.Add(sprite);
                    }

                    rowX += columnWidths[colNum] + cellGap.X;
                }

                float rowTotalHeight = rowHeights[rowNum] + (rowNum > 0 ? cellGap.Y : 0);
                rowTopLeft += new Vector2(0, rowTotalHeight);
                tableHeight += rowTotalHeight;
            }

            positionAfter = topLeft + new Vector2(0, tableHeight);
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
        
    }

    class Cell
    {
        public string Value;
        public Color Color = Color.White;
    }

    public static class TextUtils
    {
        public static readonly float CharWidth = 19f;
        public static readonly float BaseLineHeight = 30f;

        public static float GetLineHeight(float scale = 1f)
        {
            return BaseLineHeight * scale;
        }
        
        public static float GetTextWidth(string text, float scale = 1f)
        {
            //It might be more complex than this..?
            return text.Length * CharWidth * scale;
        }

        public static float GetTextHeight(string text, float scale = 1f)
        {
            return NumLines(text) * GetLineHeight(scale);
        }

        public static int NumLines(string text)
        {
            var charDiff = text.Length - text.Replace("\n", string.Empty).Length;

            return charDiff + 1;
        }
    }

    public static class VectorUtils
    {
        public static Vector2 Round(this Vector2 vector)
        {
            return new Vector2((float)Math.Round(vector.X), (float)Math.Round(vector.Y));
        }

        public static Vector2 Floor(this Vector2 vector)
        {
            return new Vector2((float)Math.Floor(vector.X), (float)Math.Round(vector.Y));
        }

        public static Vector2 Ceiling(this Vector2 vector)
        {
            return new Vector2((float)Math.Ceiling(vector.X), (float)Math.Round(vector.Y));
        }
    }
}
