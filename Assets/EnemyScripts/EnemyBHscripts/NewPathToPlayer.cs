using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class NewPathToPlayer : BehaviourNode
{
    pathfinder finder;
    Transform player, enemy;
    List<PathfindingNode> pathplayer= null;
    public float timer;
    public float dtime;
    int Lpos =0;
    Vector3 nextpos;
    
  public NewPathToPlayer(pathfinder find,Transform e, Transform p)
    {
        enemy = e;
        player = p;
        finder = find;

        
    }

    public override NodeState Evaluate()
    {
        BehaviourNode p = getroot();
        if(p.DataChecker.TryGetValue("DeltaTime",out object time)&&time is float)
        {
            dtime = (float)time;
            timer += dtime;
        }
        if (timer > 1.5 || pathplayer == null)
        {
            timer = 0;
            pathplayer = finder.FindPath(enemy.position, player.position);
            Lpos = 0;
        }
        enemy.LookAt(player.position);
        nextpos = new Vector3(pathplayer[Lpos].worldcoord.x, enemy.position.y, pathplayer[Lpos].worldcoord.z);
        enemy.position = Vector3.MoveTowards(enemy.position, nextpos, 15f * dtime);
        float xdif = Mathf.Abs(enemy.position.x - nextpos.x);
        float zdif = Mathf.Abs(enemy.position.z - nextpos.z);
        if (xdif + zdif < 0.05) //checks if close to next point
        {
            Lpos += 1;
        }
        if (xdif + zdif < 0.05 && Lpos == (pathplayer.Count - 1))
        {
            pathplayer = null;
        }
        p.DataChecker["lastposvisited"] = false;
        return NodeState.SUCCESS;
    }
}
