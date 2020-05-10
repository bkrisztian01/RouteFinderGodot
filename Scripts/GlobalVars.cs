using Godot;
using System;
using System.Collections.Generic;

namespace Program 
{
    public static class Global 
    {
        public static Map Map = new Map(50, 16);
        public static List<Vector2> Route = new List<Vector2>();
        public static Cell[,] Grid;
        public static Player Player = new Player();
        public static int Tool = Constants.Start;
    }
}