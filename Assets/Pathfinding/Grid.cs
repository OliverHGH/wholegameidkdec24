using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkablemask;
    public Vector2 worldsize;
    public float noderadius;
    public PathfindingNode[,] grid;
    float nodediameter;
    public int gridx, gridy;
    public Transform player;
    List<PathfindingNode> weightedlist = new List<PathfindingNode>();

    void Start()
    {
        nodediameter = 2 * noderadius;
        gridx = Mathf.RoundToInt(worldsize.x / nodediameter);
        gridy = Mathf.RoundToInt(worldsize.y / nodediameter);
        creategrid();
        StartCoroutine(UpdateObserved());

    }
    void creategrid()
    {
        grid = new PathfindingNode[gridx, gridy];
        Vector3 bottomleft = transform.position - Vector3.right * worldsize.x / 2- Vector3.forward*worldsize.y/2;
        for(int x = 0; x < gridx; x++)
        {
            for(int y = 0;y < gridy; y++)
            {
                Vector3 worldpos = bottomleft + Vector3.right * (x * nodediameter + noderadius) + Vector3.forward * (y * nodediameter + noderadius);
                bool walkable =!(Physics.CheckSphere(worldpos, noderadius,unwalkablemask));
                grid[x, y] = new PathfindingNode(walkable, worldpos,x,y);
            }
        }
        
    }
    public List<PathfindingNode> neighbourFind(PathfindingNode findNof)
    {
        List<PathfindingNode> listofneigbours = new List<PathfindingNode>();
        int ym1 = findNof.ypos - 1;
        int yp1 = findNof.ypos + 1;
        int xm1 = findNof.xpos - 1;
        int xp1 = findNof.xpos + 1;
        if (ym1 >= 0 && ym1 < gridy)
        {
            listofneigbours.Add(grid[findNof.xpos, ym1]);
        }
        if (yp1 >= 0 && yp1 < gridy)
        {
            listofneigbours.Add(grid[findNof.xpos, yp1]);
        }
        if (xm1 >= 0 && xm1 < gridx)
        {
            listofneigbours.Add(grid[xm1, findNof.ypos]);
        }
        if (xp1 >= 0 && xp1 < gridx)
        {
            listofneigbours.Add(grid[xp1, findNof.ypos]);
        }
        if (xm1 >= 0 && xm1 < gridx&& ym1 >= 0 && ym1 < gridy)
        {
            listofneigbours.Add(grid[xm1, ym1]);
        }
        if (xp1 >= 0 && xp1 < gridx && ym1 >= 0 && ym1 < gridy)
        {
            listofneigbours.Add(grid[xp1, ym1]);
        }
        if (xm1 >= 0 && xm1 < gridx && yp1 >= 0 && yp1 < gridy)
        {
            listofneigbours.Add(grid[xm1, yp1]);
        }
        if (xp1 >= 0 && xp1 < gridx && yp1 >= 0 && yp1 < gridy) { 
        listofneigbours.Add(grid[ xp1, yp1]); }
        return listofneigbours;
       
    }

    public PathfindingNode nodefromworldpoint(Vector3 wpos)
    {
        float xpercent = (wpos.x + worldsize.x/2) / worldsize.x;
        float ypercent = (wpos.z + worldsize.y/2) / worldsize.y;
        xpercent = Mathf.Clamp01(xpercent);
        ypercent = Mathf.Clamp01(ypercent);
        int x =Mathf.RoundToInt((gridx - 1)* xpercent);
        int y = Mathf.RoundToInt((gridy - 1) * ypercent);
        return (grid[x, y]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
           
        }
    }
    IEnumerator UpdateObserved()
    {

        while (true)
        {
            PathfindingNode playerpos = nodefromworldpoint(player.position + player.forward * 1);
            PathfindingNode infront = playerpos;       
            int distance = 1;
            foreach(PathfindingNode x in weightedlist)
            {
                x.Weight = 0;
            }
            weightedlist.Clear();
            for (int dist = 0; dist <= 30; dist += 2)
            {
                try
                {
                    infront = nodefromworldpoint(player.position + player.forward * dist);
                    distance = dist;
                    if (infront.passable == false)
                    {
                        distance -= 2;
                        break;
                    }
                } // tries to find the point infront of the player, and cuts off if it is over boundary
                catch
                {
                    distance -= 2;
                    Debug.Log("distance is " + distance);
                    break;
                }
            } // dist represents the point the plater is looking at- it will be exteened until the view is blocked or extends over edge of world
            PathfindingNode centre1 = nodefromworldpoint(player.position + player.forward * (distance/4));
            PathfindingNode centre2 = nodefromworldpoint(player.position + player.forward * (distance /2));
            PathfindingNode centre3 = nodefromworldpoint(player.position + player.forward * (distance *3/4));
            float timetowait = 10f;
            foreach (PathfindingNode x in squaremaker(centre1, 2))
            {
                weightedlist.Add(x);
                x.Weight = 500;
            }
            foreach (PathfindingNode x in squaremaker(centre2, 2))
            {
                weightedlist.Add(x);
                x.Weight = 500;
            }
            foreach (PathfindingNode x in squaremaker(centre3, 2))
            {
                weightedlist.Add(x);
                x.Weight = 500;
            }
            yield return new WaitForSeconds(timetowait);
        }
    }
    List<PathfindingNode> squaremaker(PathfindingNode C, int num)
    {
        List<PathfindingNode> clist = new List<PathfindingNode>();
        List<PathfindingNode> N = new List<PathfindingNode>();
        N = neighbourFind(C);
        foreach(PathfindingNode x in N)
        {
           foreach(PathfindingNode n in neighbourFind(x))
           {
                if (clist.Contains(n) == false)
                {
                    clist.Add(n);
                }
           }
        }
        return clist;
    }
    

}
