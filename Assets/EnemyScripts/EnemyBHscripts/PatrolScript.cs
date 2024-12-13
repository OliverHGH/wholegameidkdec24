using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class PatrolScript : BehaviourNode
{
    pathfinder finder;
    Grid grid;
    Transform enemy;
    List<PathfindingNode> pathpatrol;
    public float timer;
    public float dtime;
    int Lpos = 0;
    Vector3 nextpos;
    public PatrolScript(pathfinder find,Grid g, Transform e)
    {
        enemy = e;
        finder = find;
        grid = g;
    }
    public override NodeState Evaluate()
    {
        BehaviourNode p = getroot();
        if (p.DataChecker.TryGetValue("DeltaTime", out object time) && time is float)
        {
            dtime = (float)time;
        }
        if (pathpatrol == null)
        {
   
            Lpos = 0;
            int x = Random.Range(-49, 50);
            int z = Random.Range(-49, 50);
            Vector3 pos = new Vector3(x, 0, z);
            while (grid.nodefromworldpoint(pos).passable == false)
            {
                x = Random.Range(-49, 50);
                z = Random.Range(-49, 50);
                pos = new Vector3(x, 0, z);

            }
           
            pathpatrol = finder.FindPath(enemy.position,pos);
        }
       
        nextpos = new Vector3(pathpatrol[Lpos].worldcoord.x, enemy.position.y, pathpatrol[Lpos].worldcoord.z);
        enemy.LookAt(nextpos);
        enemy.position = Vector3.MoveTowards(enemy.position, nextpos, 6f * dtime);
        float xdif = Mathf.Abs(enemy.position.x - nextpos.x);
        float zdif = Mathf.Abs(enemy.position.z - nextpos.z);
        if (xdif + zdif < 0.05) //checks if close to next point
        {
            Lpos += 1;
        }
        if (xdif + zdif < 0.05 && Lpos == (pathpatrol.Count - 1))
        {
            pathpatrol = null;
        }
        return NodeState.SUCCESS;
    }
}
