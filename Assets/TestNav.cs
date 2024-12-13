using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNav : MonoBehaviour
{
    public GameObject a, p1, p2;
    NewPathfinding path;
    public List<PathfindingNode> waypoints;
    void Start()
    {
        path = a.GetComponent<NewPathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("hmm");
            waypoints =path.FindPath(p1.transform.position, p2.transform.position);
        }
    }
}
