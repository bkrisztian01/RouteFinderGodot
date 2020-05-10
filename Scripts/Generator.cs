using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Godot;

namespace Program
{
    class Generator
    {
        int MaxRooms;
        int MinRoomXY;
        int MaxRoomXY;
        List<Room> RoomList = new List<Room>();
        List<List<(int,int)>> CorridorList = new List<List<(int, int)>>();
        Random rnd = new Random();
        Map map;

        public Generator(Map map, int MaxRooms, int MinRoomXY, int MaxRoomXY)
        {
            this.MaxRooms = MaxRooms;
            this.MinRoomXY = MinRoomXY;
            this.MaxRoomXY = MaxRoomXY;
            this.map = map;
        }

        Room GenerateRoom()
        {
            int x, y, w, h;
            w = rnd.Next(MinRoomXY, MaxRoomXY);
            h = rnd.Next(MinRoomXY, MaxRoomXY);
            x = rnd.Next(0, map.Size - w);
            y = rnd.Next(0, map.Size - h);
            return new Room(x, y, w, h);
        }

        bool RoomOverlapping(Room room)
        {
            foreach (var currentRoom in RoomList)
            {
                if (room.Intersect(currentRoom))
                {
                    return true;
                }
            }
            return false;
        }

        void CorridorBetweenPoints(int x1, int y1, int x2, int y2)
          {
            if (x1 == x2 || y1 == y2)
            {
                CorridorList.Add(new List<(int, int)> { (x1, y1), (x2, y2) });
            }
            else
            {
                int rndNumber = rnd.Next(0, 2);
                if (rndNumber == 0)
                {
                    CorridorList.Add(new List<(int, int)> { (x1, y1), (x1, y2) });
                    CorridorList.Add(new List<(int, int)> { (x1, y2), (x2, y2) });
                }
                else
                {
                    CorridorList.Add(new List<(int, int)> { (x1, y1), (x2, y1) });
                    CorridorList.Add(new List<(int, int)> { (x2, y1), (x2, y2) });
                }
            }
        }

        void JoinRooms(Room room1, Room room2)
        {
            int x1 = room1.x;
            int y1 = room1.y;
            int w1 = room1.w;
            int h1 = room1.h;

            int x2 = room2.x;
            int y2 = room2.y;
            int w2 = room2.w;
            int h2 = room2.h;

            int p1X = rnd.Next(x1, x1 + w1);
            int p1Y = rnd.Next(y1, y1 + h1);

            int p2X = rnd.Next(x2, x2 + w2);
            int p2Y = rnd.Next(y2, y2 + h2);

            CorridorBetweenPoints(p1X, p1Y, p2X, p2Y);
        }

        public void GenerateMap()
        {
            map.Blocks.Clear();
            map.StartLocation = new Vector2(-1, -1);
            map.EndLocation = new Vector2(-1, -1);
            RoomList.Clear();
            CorridorList.Clear();

            for (int x = 0; x < map.Size; x++)
            {
                for (int y = 0; y < map.Size; y++) 
                {
                    Global.Grid[x, y].SetStatus(Constants.Clear);
                }
            }


            for (int i = 0; i < MaxRooms; i++)
            {
                Room tempRoom = GenerateRoom();
                while (RoomOverlapping(tempRoom))
                    tempRoom = GenerateRoom();
                RoomList.Add(tempRoom);
            }

            for (int i = 0; i < MaxRooms - 1; i++)
            {
                JoinRooms(RoomList[i], RoomList[i + 1]);
            }

            for (int i = 0; i < map.Size; i++)
            {
                for (int j = 0; j < map.Size; j++)
                {
                    map.Blocks.Add(new Vector2(i, j));
                }
            }

            foreach (var room in RoomList)
            {
                int x = room.x;
                int y = room.y;
                int w = room.w;
                int h = room.h;
                for (int i = x; i < x + w; i++)
                {
                    for (int j = y; j < y + h; j++)
                    {
                        map.Blocks.Remove(new Vector2(i, j));
                    }
                }
            }
            foreach (var corridor in CorridorList)
            {
                int x1 = corridor[0].Item1;
                int y1 = corridor[0].Item2;
                int x2 = corridor[1].Item1;
                int y2 = corridor[1].Item2;
                for (int i = 0; i < Math.Abs(x1 - x2) + 1; i++)
                {
                    for (int j = 0; j < Math.Abs(y1 - y2) + 1; j++)
                    {
                        map.Blocks.Remove(new Vector2(Math.Min(x1, x2) + i, Math.Min(y1, y2) + j));
                    }
                }
            }
        }
    }
}
