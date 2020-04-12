using Godot;
using System;
using System.Collections.Generic;

namespace Program 
{
    public static class Global 
    {
        public static Vector2 StartLocation;
        public static Vector2 EndLocation;
        public static List<Vector2> Blocks = new List<Vector2>();
        public static List<Vector2> Route = new List<Vector2>();
        public static Cell[,] Grid;
        public static int Tool = Constants.Start;
    }
}