using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 pos;
    public int gridx, gridy;

    public bool isWalkable;

    public Node parent;

    public int Gcost = 0;
    public int Hcost = 0;
    public float Fcost { get { return Gcost + Hcost; } }

    public Node( Vector3 pos, (int,int) gridPos , bool isWalkable = true)
    {
        this.pos = pos;
        gridx = gridPos.Item1;
        gridy = gridPos.Item2;
        this.isWalkable = isWalkable;
    }
}
public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public int height;
    public int width;
    public float cellsize;

    public Transform gridorigin;
    public static Node[,] grids;
    public Transform player;
    Node nodePlayer;
    int curW, curH;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        curW = width;
        curH = height;
        init();
    }

    private void Update()
    {
        if(width !=curW || height != curH)
        {
            init();
        }
        
        
    }
    public void init()
    {
        grids = new Node[width,height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 wordPos = gridorigin.position + new Vector3(i * cellsize, j * cellsize, 0);
                grids[i, j] = new Node(wordPos,(i,j));//(i%2!=0 || j%2!=0)
            }
        }
    }
   
    public Node posToNode(Vector3 worldPosition)
    {
        if (grids == null) return null;
        // Chuyển từ world position sang grid position
        Vector3 localPosition = worldPosition - gridorigin.position;

        // Chuyển từ local position sang tọa độ lưới
        int x = Mathf.RoundToInt(localPosition.x / cellsize);
        int y = Mathf.RoundToInt(localPosition.y / cellsize);
        // Kiểm tra nếu các giá trị x, y không vượt quá kích thước của grid
        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);

        // Trả về node tương ứng với vị trí trong grid
        return grids[x, y];
    }

    public bool isGizmos = false;
    private void OnDrawGizmos()
    {
        if (!isGizmos) return;
       
        if (grids == null || grids.Length <= 0) return;

        for (int i = 0; i < width; i++) // Lặp qua từng cột
        {
            for (int j = 0; j < height; j++) // Lặp qua từng hàng
            {
                Node node = grids[i, j]; // Lấy node tại vị trí (i, j)
                Gizmos.color = node.isWalkable ? Color.white : Color.red; // Nếu node có thể di chuyển, vẽ màu trắng, ngược lại vẽ màu đỏ
                Gizmos.DrawWireCube(node.pos, Vector3.one * (cellsize - 0.1f)); // Vẽ hình vuông cho mỗi node
            }
        }

    }
}




