using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding
{
    public static List<Node> FindPath(Vector2Int start, Vector2Int target)
    {
        Node startNode = Map.RoadMap[start.x, start.y];
        Node targetNode = Map.RoadMap[target.x, target.y];
        Heap<Node> openSet = new Heap<Node>(MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        if (start == target)
        {
            List<Node> l = new List<Node>();
            l.Add(new Node(true, new Vector3(0, 0), start.x, start.y));
            return l;
        }
        
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                var Path = RetracePath(startNode, targetNode);
                return Path;
            }
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    //Debug.Log(neighbour.gridX + "," + neighbour.gridY);
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                    {
                        //openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        return null;
    }

    static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        return path;
    }

    static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    public static int MaxSize
    {
        get
        {
            return Map.MapSideSize * 2;
        }
    }
    public static List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        foreach (var id in GameUtility.NearbyIDs)
        {
            int checkX = node.gridX + id.x;
            int checkY = node.gridY + id.y;
            //Debug.Log(node.gridX + "," + node.gridY + "+" + id);
            //Debug.Log(checkX + "," + checkY);
            if (checkX >= 0 && checkX < Map.MapSideSize && checkY >= 0 && checkY < Map.MapSideSize)
            {
                neighbours.Add(Map.RoadMap[checkX, checkY]);
            }
        }

        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        if (x == 0 && y == 0)
        //            continue;
        //        int checkX = node.gridX + x;
        //        int checkY = node.gridY + y;

        //        if (checkX >= 0 && checkX < MapGenerator.SideSize && checkY >= 0 && checkY < MapGenerator.SideSize)
        //        {
        //            neighbours.Add(Map.RoadMap[checkX, checkY]);
        //        }
        //    }
        //}
        return neighbours;
    }
}
