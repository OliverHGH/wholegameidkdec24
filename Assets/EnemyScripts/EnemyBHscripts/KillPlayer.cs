using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class KillPlayer : BehaviourNode
{

    public override NodeState Evaluate()
    {
        Debug.Log("u ded lol");
        return NodeState.SUCCESS;
    }

}
