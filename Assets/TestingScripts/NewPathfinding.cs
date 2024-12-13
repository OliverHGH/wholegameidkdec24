using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class NewPathfinding : MonoBehaviour
{
    Grid grid;
    public List<PathfindingNode> pointstowalk;
    void Awake()
    {
        grid = GetComponent<Grid>();
    }
    static int FindDif(PathfindingNode a, PathfindingNode b)
    {
        int ydif = a.ypos - b.ypos;
        if (ydif < 0)
        {
            ydif = -ydif;
        }
        int xdif = a.xpos - b.xpos;
        if (xdif < 0)
        {
            xdif = -xdif;
        }
        float Fanswer = Mathf.Sqrt((xdif * xdif) + (ydif * ydif));
        return (Mathf.FloorToInt(10 * Fanswer));
    }
    public List<PathfindingNode> FindPath(Vector3 start, Vector3 end)
    {
        bool alreadyinopen;
        Stopwatch tchck = new Stopwatch();
        tchck.Start();
        newMH Openlist = new newMH();
        HashSet<PathfindingNode> ClosedList = new HashSet<PathfindingNode>();
        PathfindingNode firstnode = grid.nodefromworldpoint(start);
        PathfindingNode endnode = grid.nodefromworldpoint(end);
        if (endnode.passable == false)
        {
            foreach (PathfindingNode alt in grid.neighbourFind(endnode))
            {
                if (alt.passable == true)
                {
                    endnode = alt;
                }
            }
        }
        firstnode.parent = null;
        Openlist.Insert(firstnode);
        while (Openlist.listMax >= 0)
        {

            PathfindingNode currentnode = Openlist.Takemin();
            currentnode.inopen = false;
            ClosedList.Add(currentnode);
            if (currentnode.xpos == endnode.xpos && currentnode.ypos == endnode.ypos)
            {
                Openlist.clear();
                List<PathfindingNode> foundpath = new List<PathfindingNode>();
                PathfindingNode x = currentnode;
                while (x.parent != null)
                {
                    foundpath.Add(x);
                    x = x.parent;
                }
                foundpath.Add(firstnode);
                foundpath.Reverse();
                tchck.Stop();
                List<PathfindingNode> waypoints = simplifypath(foundpath);
                UnityEngine.Debug.Log("found path " + tchck.ElapsedMilliseconds);
                return waypoints;
            }
            else
            {
                List<PathfindingNode> listofneigbours = grid.neighbourFind(currentnode);
                foreach (PathfindingNode posbeingchecked in listofneigbours)
                {
                    alreadyinopen = false;

                    if (posbeingchecked.passable == false || ClosedList.Contains(posbeingchecked)) // skips if neighbour is inpassable
                    {
                        continue;
                    }

                    if (posbeingchecked.inopen == true) //checks if item is already in heap
                    {

                        alreadyinopen = true;
                        int ngcost = currentnode.Gcost + FindDif(posbeingchecked, currentnode);
                        if (posbeingchecked.Gcost > ngcost)
                        {

                            posbeingchecked.Gcost = ngcost;
                            posbeingchecked.parent = currentnode;
                            Openlist.UpShift(posbeingchecked.HeapPos); // update node and mantain heap


                        }
                        break;
                    }
                    if (alreadyinopen == false)
                    {

                        posbeingchecked.parent = currentnode;
                        posbeingchecked.Gcost = currentnode.Gcost + FindDif(currentnode, posbeingchecked);
                        posbeingchecked.Hcost = FindDif(posbeingchecked, endnode);
                        posbeingchecked.inopen = true;
                        Openlist.Insert(posbeingchecked);      // add to open
                    }
                }
                
            }
        }
        ClosedList = null;
        Openlist = null;
        List<PathfindingNode> empty = new List<PathfindingNode>();
        UnityEngine.Debug.Log("failure :( ");
        return empty;

    }
    List<PathfindingNode> simplifypath(List<PathfindingNode> path)
    {
        List<PathfindingNode> waypoints = new List<PathfindingNode>();
        Vector2 lastdirection = new Vector2(0, 0);
        for (int n = 1; n < path.Count; n++)
        {
            Vector2 newdirection = new Vector2(path[n - 1].xpos - path[n].xpos, path[n - 1].ypos - path[n].ypos);
            if (lastdirection != newdirection)
            {
                waypoints.Add(path[n - 1]);
            }
            lastdirection = newdirection;
        }
        waypoints.Add(path[path.Count - 1]);
        return waypoints;
    }


}
