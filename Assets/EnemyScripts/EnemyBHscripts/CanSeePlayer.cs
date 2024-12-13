using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CanSeePlayer : BehaviourNode
{
    MathsMethods mathsh = new MathsMethods();
    Transform enemy;
    Transform forward;
    Transform player;
    private bool xo, wb;
    private bool couldseelastframe = false;
    BehaviourNode ptoedit;
    private LayerMask walls;
    public  CanSeePlayer(Transform e, Transform f, Transform p, LayerMask w)
    {
        enemy = e;
        forward = f;
        player = p;
        walls = w;
    }
    public override NodeState Evaluate()
    {
        ptoedit = getroot();
    
        bool ObservedX()
        {
            Vector2 pp = new Vector2(enemy.position.x, enemy.position.z);
            Vector2 ep = new Vector2(player.position.x, player.position.z);
            Vector2 dahead = new Vector2(forward.position.x, forward.position.z);

            dahead = mathsh.normaliser(dahead - pp);
            pp = mathsh.normaliser(ep - pp);
            float dotProduct = (dahead.x * pp.x + dahead.y * pp.y);
            float angle = Mathf.Rad2Deg * Mathf.Acos(dotProduct);
            float dif = (mathsh.findDif(player.position, enemy.position));
            if (angle < 80 + (10/dif))
            {
        
                return true; 
            }
            else
            {
                return false;
            }
            
        }

        bool wallbetween()
        {
            return Physics.Linecast(enemy.position, player.position- enemy.transform.forward*0.7f, walls);
          
        }

        xo = ObservedX();
        if (xo == true)
        {
            wb = wallbetween();
        }
        if(xo == true && wb== false)
        {
            couldseelastframe = true;
            ptoedit.DataChecker["NeedToSpin"] = false;
            ptoedit.DataChecker["lastseenpos"] = player.position;
            return NodeState.SUCCESS;
        }
        if (couldseelastframe == true)
        {
            ptoedit.DataChecker["NeedToSpin"] = true;
            
        }
       

        couldseelastframe = false;
        return NodeState.FAILURE;

    }
}

 
