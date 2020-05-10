using Godot;
using System;

namespace Program
{
    public class Player
    {
        public Vector2 Location { get; set;}
        public Vector2 Velocity { get; set;}
        static int DEFAULT_MOVE_COOLDOWN = 2;
        int MoveCooldown = DEFAULT_MOVE_COOLDOWN;

        public void Up()
        {
            Velocity = new Vector2(0, -1);
        }
        public void Left()
        {
            Velocity = new Vector2(-1, 0);
        }
        public void Down()
        {
            Velocity = new Vector2(0, 1);
        }
        public void Right()
        {
            Velocity = new Vector2(1, 0);
        }
        public void Stop()
        {
            Velocity = new Vector2(0, 0);
        }

        public Vector2 Move()
        {
            Vector2 OldLocation = new Vector2(Location);
            int mapSize = Global.Map.Size;
            if (Location.x < 0 || Location.y < 0 || mapSize <= Location.x || mapSize <= Location.y)
                return OldLocation;
            
            if(MoveCooldown <= 0)
            {
                Location += Velocity;
                MoveCooldown = DEFAULT_MOVE_COOLDOWN;
            }
            MoveCooldown -= 1;
            return OldLocation;
        }
    }
}