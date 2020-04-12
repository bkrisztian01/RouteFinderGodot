using Godot;
using System;
using System.Collections.Generic;

namespace Program
{
    public class RouteFinder : Node2D
    {
        int size = 50;
        int cellSize = 16;
        int spacer = 2;

        Position2D startPosition;

        PackedScene cell = ResourceLoader.Load<PackedScene>("res://Scenes/Cell.tscn");

        public override void _Ready() 
        {
            startPosition = GetNode<Position2D>("StartPosition");
            Global.Grid = new Cell[size, size];
            OS.WindowSize = new Vector2(size * cellSize + (size - 1) * spacer, size * cellSize + (size - 1) * spacer);
            InitializeGrid();
        }

        public void InitializeGrid() 
        {
            for (int y = 0; y < size; y++) 
            {
                for (int x = 0; x < size; x++)
                {
                    Cell new_cell = (Cell)cell.Instance();
                    new_cell.InitializeTile(x, y);
                    new_cell.Position = new Vector2(startPosition.Position.x + x * (cellSize + spacer), startPosition.Position.y + y *(cellSize + spacer));
                    AddChild(new_cell);
                    Global.Grid[x, y] = new_cell;
                }
            }
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionPressed("place_start"))
            {
                Global.Tool = Constants.Start;
            }
            else if (Input.IsActionPressed("place_end"))
            {
                Global.Tool = Constants.End;
            }
            else if (Input.IsActionPressed("place_block"))
            {
                Global.Tool = Constants.Block;
            }
            else if (Input.IsActionPressed("calculate"))
            {
                Global.Route = Astar.CalculateRoute(size, Global.StartLocation, Global.EndLocation, Global.Blocks);
                foreach (var i in Global.Route)
                {
                    Global.Grid[(int)i.x, (int)i.y].SetStatus(Constants.Route);
                }
            }
        }
    }
}