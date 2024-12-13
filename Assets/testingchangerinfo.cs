using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class testingchangerinfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KillPlayer b = new KillPlayer();
        List<BehaviourNode> l = new List<BehaviourNode>();
        l.Add(b);
        SequenceNode s = new SequenceNode(l);
        BehaviourNode r = b.getroot();
        r.DataChecker.Add("helpme", 124);
        Debug.Log(s.DataChecker["helpme"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
