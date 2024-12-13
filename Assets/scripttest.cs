using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scripttest : MonoBehaviour
{
    public Transform player;
    playermovement setasct;

    void Start()
    {
        setasct = player.GetComponent<playermovement>();
    }
    void Update()

    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            setasct.loading = true;
            player.position = new Vector3(-35,1.6f,-45);
            Debug.Log(player.position.x+","+ player.position.y + ","+ player.position.z);
            Debug.Log("hi");

        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            setasct.loading = false;
        }
    }
}
