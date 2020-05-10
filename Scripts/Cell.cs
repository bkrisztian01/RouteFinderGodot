using Godot;
using System;

namespace Program
{
    public class Cell : Area2D
    {
        int status = Constants.Clear;
        Vector2 Location;
        AnimationPlayer animation;

        public override void _Ready()
        {
            animation = GetNode<AnimationPlayer>("AnimationPlayer");
            animation.Play("Clear");
        }

        public void InitializeTile(int x, int y) 
        {
            Location = new Vector2(x, y);
        }

        public void SetStatus(int status) 
        {
            this.status = status;
            switch (status) 
            {
                case Constants.Clear:
                    if (Global.Map.StartLocation == Location)
                    {
                        Global.Map.StartLocation = new Vector2(-1, -1);
                    }
                    else if (Global.Map.EndLocation == Location)
                    {
                        Global.Map.StartLocation = new Vector2(-1, -1);
                    }
                    else if (Global.Map.Blocks.Contains(Location))
                    {
                        Global.Map.Blocks.Remove(Location);
                    }
                    animation.Play("Clear");
                    break;

                case Constants.Block:
                    if (!Global.Map.Blocks.Contains(Location))
                    {
                        Global.Map.Blocks.Add(Location);
                        if (Location.Equals(Global.Map.StartLocation))
                            Global.Map.StartLocation = new Vector2(-1, -1);
                        else if (Location.Equals(Global.Map.EndLocation))
                            Global.Map.EndLocation = new Vector2(-1, -1);
                    }
                    animation.Play("Block");
                    break;

                case Constants.Start:
                    if (!Global.Map.StartLocation.Equals(new Vector2(-1, -1)))
                        Global.Grid[(int)Global.Map.StartLocation.x, (int)Global.Map.StartLocation.y].animation.Play("Clear");

                    if (Global.Map.Blocks.Contains(Location))
                        Global.Map.Blocks.Remove(Location);

                    Global.Map.StartLocation = Location;

                    if (Global.Map.StartLocation.Equals(Global.Map.EndLocation))
                        Global.Map.EndLocation = new Vector2(-1, -1);

                    animation.Play("Start");
                    break;

                case Constants.End:
                    if (!Global.Map.EndLocation.Equals(new Vector2(-1, -1)))
                        Global.Grid[(int)Global.Map.EndLocation.x, (int)Global.Map.EndLocation.y].animation.Play("Clear");

                    if (Global.Map.Blocks.Contains(Location))
                        Global.Map.Blocks.Remove(Location);

                    Global.Map.EndLocation = Location;

                    if (Global.Map.EndLocation.Equals(Global.Map.StartLocation))
                        Global.Map.StartLocation = new Vector2(-1, -1);

                    animation.Play("End");
                    break;

                case Constants.Route:
                    animation.Play("Route");
                    break;

                case Constants.Player:
                    if (Global.Map.Blocks.Contains(Location))
                        break;
                    else if (Global.Map.StartLocation.Equals(Location))
                        break;
                    else if (Global.Map.EndLocation.Equals(Location))
                        break;

                    if (!Global.Player.Location.Equals(new Vector2(-1, -1)))
                        Global.Grid[(int)Global.Player.Location.x, (int)Global.Player.Location.y].animation.Play("Clear");

                    animation.Play("Player");
                    break;
            }
        }

        public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx) 
        {
            if (@event is InputEventMouseButton btn)
            {
                if (btn.ButtonIndex == (int)ButtonList.Left && @event.IsPressed())
                {
                    SetStatus(Global.Tool);
                    DeletePrevRoute();
                }
                else if (btn.ButtonIndex == (int)ButtonList.Right && @event.IsPressed()) 
                {
                    SetStatus(Constants.Clear);
                    DeletePrevRoute();
                }
            }
        }

        public void DeletePrevRoute()
        {
            if (Global.Route.Count != 0) //Delete previous route
            {
                foreach (var i in Global.Route)
                {
                Global.Grid[(int)i.x, (int)i.y].SetStatus(Constants.Clear);
                }
                Global.Route.Clear();
            }
        }
    }
}