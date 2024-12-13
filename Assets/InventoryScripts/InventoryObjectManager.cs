using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManage;

public class InventoryObjectManager : MonoBehaviour
{
    public GameObject o1,o2,o3,o4,o5;
    public Transform p;
    public GameObject managerobject;
    public List<InventoryObj> ItemList = new List<InventoryObj>();
    public LayerMask blocked;
    public void ListofAllItems()
    {
        GameManagerScript manager = managerobject.GetComponent<GameManagerScript>();
        List<GameObject> enemies = manager.enemybodylist;
        CubeTestItem cubey = new CubeTestItem(p, o1,"cube1");
        ItemList.Add(cubey);
        CubeTestItem cubey2 = new CubeTestItem(p, o2, "cube2");
        ItemList.Add(cubey2);
        TestItem2 t2 = new TestItem2(p, o3, "item2");
        ItemList.Add(t2);
        TorchItem torch = new TorchItem(p, o4, "torch1");
        ItemList.Add(torch);
        GunItem gun = new GunItem(p, o5, "gun1", enemies, blocked);
        ItemList.Add(gun);

    }

   
}
