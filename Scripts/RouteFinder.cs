using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Program
{
    public class RouteFinder : Node2D
    {
        Position2D startPosition;

        PackedScene cell = ResourceLoader.Load<PackedScene>("res://Scenes/Cell.tscn");

        public override void _Ready() 
        {
            startPosition = GetNode<Position2D>("StartPosition");
            Global.Grid = new Cell[Global.Map.Size, Global.Map.Size];
            OS.WindowSize = new Vector2(Global.Map.Size * Global.Map.Unit + (Global.Map.Size - 1) * Global.Map.Spacer, Global.Map.Size * Global.Map.Unit + (Global.Map.Size - 1) * Global.Map.Spacer);
            InitializeGrid();
        }

        public void InitializeGrid() 
        {
            for (int y = 0; y < Global.Map.Size; y++) 
            {
                for (int x = 0; x < Global.Map.Size; x++)
                {
                    Cell new_cell = (Cell)cell.Instance();
                    new_cell.InitializeTile(x, y);
                    new_cell.Position = new Vector2(startPosition.Position.x + x * (Global.Map.Unit + Global.Map.Spacer), startPosition.Position.y + y *(Global.Map.Unit + Global.Map.Spacer));
                    AddChild(new_cell);
                    Global.Grid[x, y] = new_cell;
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionPressed("move_up"))
            {
                Global.Player.Up();
            }
            else if (Input.IsActionPressed("move_left"))
            {
                Global.Player.Left();
            }
            else if (Input.IsActionPressed("move_down"))
            {
                Global.Player.Down();
            }
            else if (Input.IsActionPressed("move_right"))
            {
                Global.Player.Right();
            }
            else if (!(Input.IsActionPressed("move_up") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_down") || Input.IsActionPressed("move_right")))
            {
                Global.Player.Stop();
            }

            Vector2 OldPlayerLocation = Global.Player.Move();
            
            Global.Grid[(int)OldPlayerLocation.x, (int)OldPlayerLocation.y].SetStatus(Constants.Clear);
            Global.Grid[(int)Global.Player.Location.x, (int)Global.Player.Location.y].SetStatus(Constants.Player);
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionJustPressed("place_start"))
            {
                Global.Tool = Constants.Start;
            }
            else if (Input.IsActionJustPressed("place_end"))
            {
                Global.Tool = Constants.End;
            }
            else if (Input.IsActionJustPressed("place_block"))
            {
                Global.Tool = Constants.Block;
            }
            else if (Input.IsActionJustPressed("place_player"))
            {
                Global.Tool = Constants.Player;
            }

            else if (Input.IsActionJustPressed("generate_map"))
            {
                Generator generator = new Generator(Global.Map, 10, 5, 10);
                generator.GenerateMap();
                Debug.WriteLine(Global.Map.Blocks.Count);
                for (int i = 0; i < Global.Map.Blocks.Count; i++)
                {
                    int x = (int)Global.Map.Blocks[i].x;
                    int y = (int)Global.Map.Blocks[i].y;
                    Global.Grid[x, y].SetStatus(Constants.Block);
                }
            }
            else if (Input.IsActionJustPressed("calculate"))
            {
                if (!(Global.Map.StartLocation.x < 0 || Global.Map.StartLocation.y < 0) && !(Global.Map.EndLocation.x < 0 || Global.Map.EndLocation.y < 0))
                {
                    Global.Route = Astar.CalculateRoute(Global.Map.Size, Global.Map.StartLocation, Global.Map.EndLocation, Global.Map.Blocks);
                    foreach (var i in Global.Route)
                    {
                        Global.Grid[(int)i.x, (int)i.y].SetStatus(Constants.Route);
                    }
                }
            }
        }
    }
}