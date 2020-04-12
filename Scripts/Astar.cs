using System;
using System.Collections.Generic;
using Priority_Queue;
using Godot;

namespace Program
{
    public class PQNode : FastPriorityQueueNode
    {
        public Vector2 Node;

        public PQNode(Vector2 Node)
        {
            this.Node = Node;
        }

        public PQNode(int x, int y)
        {
            this.Node = new Vector2(x, y);
        }
    }

    public static class Astar
    {
        static int mapSize;
        static Vector2 startLocation;
        static Vector2 endLocation;
        static FastPriorityQueue<PQNode> openSet;
        static Vector2[,] cameFrom;
        static float[,] gScore;
        static float[,] fScore;
        static float d = 1;
        static float dDiagonal = 1.414f;
        static List<Vector2> blocks;

        public static List<Vector2> CalculateRoute(int MapSize, Vector2 StartLocation, Vector2 EndLocation, List<Vector2> Blocks)
        {
            mapSize = MapSize;
            startLocation = StartLocation;
            endLocation = EndLocation;
            blocks = Blocks;

            openSet = new FastPriorityQueue<PQNode>(mapSize * mapSize);

            Vector2 current;
            cameFrom = new Vector2[mapSize, mapSize];
            gScore = new float[mapSize, mapSize];
            fScore = new float[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    fScore[i, j] = float.MaxValue;
                    gScore[i, j] = float.MaxValue;
                    cameFrom[i, j] = new Vector2(-1, -1);
                }
            }
            gScore[(int)startLocation.x, (int)startLocation.y] = 0;
            fScore[(int)startLocation.x, (int)startLocation.y] = h(startLocation);
            openSet.Clear();
            openSet.Enqueue(new PQNode(startLocation), 0);

            while (openSet.Count != 0)
            {
                current = openSet.Dequeue().Node;
                if (current == endLocation)
                {
                    return ReconstructPath();
                }

                Neighbour((int)current.x - 1, (int)current.y - 1, dDiagonal, current);
                Neighbour((int)current.x - 1, (int)current.y, d, current);
                Neighbour((int)current.x - 1, (int)current.y + 1, dDiagonal, current);

                Neighbour((int)current.x, (int)current.y - 1, d, current);
                Neighbour((int)current.x, (int)current.y + 1, d, current);

                Neighbour((int)current.x + 1, (int)current.y - 1, dDiagonal, current);
                Neighbour((int)current.x + 1, (int)current.y, d, current);
                Neighbour((int)current.x + 1, (int)current.y + 1, dDiagonal, current);
            }

            return new List<Vector2>(); //empty route, if goal was never reached
        }

        public static float h(Vector2 node)
        {
            float dx = Math.Abs(node.x - endLocation.x);
            float dy = Math.Abs(node.y - endLocation.y);
            return 1 * (dx + dy) + (1.414f - 2 * 1) * Math.Min(dx, dy); //octile distance
        }

        public static void Neighbour(int x, int y, float alt, Vector2 root)
        {
            if ((x < 0 || x > mapSize - 1 || y < 0 || y > mapSize - 1))
                return;
            if (blocks.Contains(new Vector2(x, y)))
                return;

            float tentative_gScore = gScore[(int)root.x, (int)root.y] + alt;
            if (tentative_gScore < gScore[x, y])
            {
                cameFrom[x, y] = root;
                gScore[x, y] = tentative_gScore;
                fScore[x, y] = gScore[x, y] + h(new Vector2(x, y));
                if (!openSet.Contains(new PQNode(x, y)))
                {
                    openSet.Enqueue(new PQNode(x, y), fScore[x, y]);
                }
            }
        }

        static List<Vector2> ReconstructPath()
        {
            List<Vector2> route = new List<Vector2>();
            Vector2 nextLocation = cameFrom[(int)endLocation.x, (int)endLocation.y];
            while (nextLocation != startLocation)
            {
                route.Add(nextLocation);

                nextLocation = cameFrom[(int)nextLocation.x, (int)nextLocation.y];
            }
            return route;
        }
    }
}

