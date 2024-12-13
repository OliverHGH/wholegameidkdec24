using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchItem : InventoryObj
{
    GameObject light;
    int batteries = 3;
    public override void Use()
    {
        light.SetActive(!light.activeSelf);
    }

    public TorchItem(Transform play, GameObject obs, string id)
    {
        player = play;
        Name = "Torch";
        Object = obs;
        UseableOnce = false;
        stacknum = 1;
        objectparts.Add(obs);
        Transform lightt = obs.transform.GetChild(0);
        light = lightt.gameObject;
        ID = id;
        
    }

}
