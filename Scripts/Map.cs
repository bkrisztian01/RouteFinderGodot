using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Program
{
    public class Map
    {
        public int Size;
        public int Unit;
        public int Spacer;
        public Vector2 StartLocation = new Vector2(-1, -1);
        public Vector2 EndLocation = new Vector2(-1, -1);
        public List<Vector2> Blocks = new List<Vector2>();

        public Map(int Size = 100, int Unit = 16, int Spacer = 1)
        {
            this.Size = Size;
            this.Unit = Unit;
            this.Spacer = Spacer;
        }
    }
}
