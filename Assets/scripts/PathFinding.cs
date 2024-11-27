using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public List<Node> openSet;
    public List<Node> closeSet;

    Node[,] grids;
    public List<Node> findPath(Node startNode, Node targetNode, Node[,] grids)
    {
        openSet = new List<Node>();
        closeSet = new List<Node>();
        if (grids == null || grids.Length <= 0) return null;
        this.grids = grids;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node curNode = openSet[0];
            foreach (Node n in openSet)
            {
                if (curNode.Fcost > n.Fcost || (curNode.Fcost == n.Fcost && curNode.Hcost > n.Hcost))
                {
                    curNode = n;
                }
            }
            openSet.Remove(curNode); 
            closeSet.Add(curNode);

            if (curNode == targetNode)
            {
                return reTracePath(curNode, startNode);
            }

            foreach (Node n in getNeighbor(curNode))
            {
                if (!n.isWalkable || closeSet.Contains(n)) continue;
                float neighborGcost = curNode.Gcost + Vector2.Distance(curNode.pos, n.pos);
                if (n.Gcost > neighborGcost || !openSet.Contains(n))
                {
                    n.Gcost = neighborGcost;
                    n.Hcost = Vector2.Distance(n.pos, targetNode.pos);
                    n.parent = curNode;

                    if (openSet.Contains(n)) continue;
                    openSet.Add(n);
                }

            }

        }

        return null;
    }

    public int[,] getDir()
    {
        int[,] dirs = new int[,]
        {
            {1,0 },//right
            {0,1 },//up
            {-1,0 },//left
            {0,-1 },//down
            {1,1 },//up right
            {-1,1 },// up left
            {1,-1 },// down right
            {-1,-1 },// down left
        };

        return dirs;
    }
    public List<Node> getNeighbor(Node curNode)
    {
        List<Node> neighbors = new List<Node>();
        int[,] dirs = getDir();
        int width = grids.GetLength(0);
        int height = grids.GetLength(1);
        for (int i = 0; i < dirs.GetLength(0); i++)
        {
            int neighborX = curNode.gridx + dirs[i, 0];
            int neighborY = curNode.gridy + dirs[i, 1];

            // Kiểm tra nếu ô nằm trong giới hạn grid
            if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
            {
                neighbors.Add(grids[neighborX, neighborY]);
            }
        }
        return neighbors;
    }

    public List<Node> reTracePath(Node curNode, Node startNode)
    {
        List<Node> paths = new List<Node>();
        Node node = curNode;
        while (node !=null)
        {
            paths.Add(node);
            node = node.parent;
            if(node == startNode)
            {
                paths.Add(startNode);
                break;
            }
        }
        paths.Reverse();

        return paths; 
    }

}
