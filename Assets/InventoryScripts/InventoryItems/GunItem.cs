using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : InventoryObj
{


    int bullets;
    List<GameObject> locs;
    MathsMethods mathsforangle = new MathsMethods();
    Transform cam;
    LayerMask walls;
    public GunItem(Transform play, GameObject obs,string id, List<GameObject> enemies, LayerMask blocked)
    {
        player = play;
        Name = "Gun";
        Object = obs;
        UseableOnce = false;
        stacknum = 1;
        objectparts.Add(obs);
        ID = id;
        locs = enemies;
        walls = blocked;
    }
    public override void Use()
    {
        Debug.Log("pew pew");
        foreach(GameObject x in locs)
        {
            if (mathsforangle.islookingat(Object.transform, x.transform, 3, 3, walls) == true)
            {
                Debug.Log(" shot lol");
            }

            
        }
        bullets -= 1;
    }

}
