using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class followpls : MonoBehaviour
{
    int n;
    public List<PathfindingNode> waypoints;
    public GameObject Astar;
    public Transform seeker, prey;
    pathfinder pathy;
    Vector3 nextpoint = new Vector3(0f, 0f, 0f);
    bool observed = false;
    checkifobserved checksee;
    bool checkifchanged;
    int howmanyencounters;
    void Start()
    {
        checksee = GetComponent<checkifobserved>();
        pathy = Astar.GetComponent<pathfinder>();
        n = 0;
        StartCoroutine(pathupdator());
        StartCoroutine(checkobserved());
    }

    IEnumerator pathupdator()
    {
        while (true)
        {
            waypoints = pathy.FindPath(seeker.position, prey.position);
            n = 0;
            yield return new WaitForSeconds(1.5f);


        }
    }
    IEnumerator checkobserved()
    {

        while (true)
        {
            checkifchanged = observed;
            observed = checksee.observecheck();
            float timetowait = 0.05f;

            yield return new WaitForSeconds(timetowait);
        }
    }
   
  
    void Update()
    {
        if (observed == false)
        {
            if (waypoints != null)
            {
                if (waypoints.Count > 0 && n < waypoints.Count)
                {

                    transform.LookAt(prey.position);
                    nextpoint = new Vector3(waypoints[n].worldcoord.x, transform.position.y, waypoints[n].worldcoord.z); //vector 3 of next waypoint
                    transform.position = Vector3.MoveTowards(transform.position, nextpoint, 15f * Time.deltaTime);
                    float xdif = Mathf.Abs(transform.position.x - nextpoint.x);
                    float zdif = Mathf.Abs(transform.position.z - nextpoint.z);
                    if (xdif + zdif < 0.05) //checks if close to next point
                    {
                        n += 1;
                    }
                    if (xdif + zdif < 0.05 && n == (waypoints.Count - 1))
                    {
                        waypoints.Clear();
                    }
                }
            }
        }
    }
}

