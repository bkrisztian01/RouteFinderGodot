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
                    if (Global.StartLocation == Location)
                    {
                        Global.StartLocation = new Vector2(-1, -1);
                    }
                    else if (Global.EndLocation == Location)
                    {
                        Global.StartLocation = new Vector2(-1, -1);
                    }
                    else if (Global.Blocks.Contains(Location))
                    {
                        Global.Blocks.Remove(Location);
                    }
                    animation.Play("Clear");
                    break;

                case Constants.Block:
                    if (!Global.Blocks.Contains(Location))
                    {
                        animation.Play("Block");
                        Global.Blocks.Add(Location);
                        if (Location.Equals(Global.StartLocation))
                            Global.StartLocation = new Vector2(-1, -1);
                        else if (Location.Equals(Global.EndLocation))
                            Global.EndLocation = new Vector2(-1, -1);
                    }
                    break;

                case Constants.Start:
                    if (!Global.StartLocation.Equals(new Vector2(-1, -1)))
                        Global.Grid[(int)Global.StartLocation.x, (int)Global.StartLocation.y].animation.Play("Clear");

                    if (Global.Blocks.Contains(Location))
                        Global.Blocks.Remove(Location);

                    Global.StartLocation = Location;

                    if (Global.StartLocation.Equals(Global.EndLocation))
                        Global.EndLocation = new Vector2(-1, -1);

                    animation.Play("Start");
                    break;

                case Constants.End:
                    if (!Global.EndLocation.Equals(new Vector2(-1, -1)))
                        Global.Grid[(int)Global.EndLocation.x, (int)Global.EndLocation.y].animation.Play("Clear");

                    if (Global.Blocks.Contains(Location))
                        Global.Blocks.Remove(Location);

                    Global.EndLocation = Location;

                    if (Global.EndLocation.Equals(Global.StartLocation))
                        Global.StartLocation = new Vector2(-1, -1);

                    animation.Play("End");
                    break;

                case Constants.Route:
                    animation.Play("Route");
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