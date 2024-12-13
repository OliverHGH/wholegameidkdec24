using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SpinToLook : BehaviourNode
{
    private Transform enemy;
    private Vector3 rotate = new Vector3(0, 1, 0);
    public float dtime;
    private BehaviourNode p;
    
    float timer = 0;
    public SpinToLook(Transform e)
    {
        enemy = e;
    }

    public override NodeState Evaluate()
    {
        p = getroot();
        if (p.DataChecker.TryGetValue("DeltaTime", out object time) && time is float)
        {
            dtime = (float)time;
        }
        timer += dtime;
        if (timer > 2)
        {
            p.DataChecker["NeedToSpin"] = false;
            timer = 0;
            return NodeState.SUCCESS;
        }
        enemy.Rotate(rotate * dtime*180);
        return NodeState.SUCCESS;
    }

}
