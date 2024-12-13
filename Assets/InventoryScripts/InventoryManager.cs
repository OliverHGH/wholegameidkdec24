﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private InventoryObj[,] InventoryList;
    public InventoryObj[,] loadlist
    {
        get { return InventoryList; }
        set { InventoryList = value; }
    }
    private List<InventoryObj> allitems;
    public List<InventoryObj> itemstoload
    {
        get { return allitems; }
    }
    bool showinventory =true;
    bool lastframe = true;
    private int chosenslot=-1;
    private int eqipedslot;

    public Button b1,b2,b3,b4,b5;
    public Text t1,t2,t3,t4,t5;
    private InventorySlot s1, s2, s3, s4, s5;
    public GameObject InventoryUI;
    public Transform infrontplayer, playerC;
   
    public GameObject Objectcreator;
    private List<InventorySlot> slotlist = new List<InventorySlot>();
    MathsMethods mathsy = new MathsMethods();
    private InventoryObj equippedobject = null;
    bool deselectedslot;



    void Start()
    {
        InventoryList = new InventoryObj[5, 10];
        s1 = new InventorySlot(b1, t1, 0, InventoryList);
        slotlist.Add(s1);
        s2 = new InventorySlot(b2, t2, 1, InventoryList);
        slotlist.Add(s2);
        s3 = new InventorySlot(b3, t3, 2, InventoryList);
        slotlist.Add(s3);
        s4 = new InventorySlot(b4, t4, 3, InventoryList);
        slotlist.Add(s4);
        s5 = new InventorySlot(b5, t5, 4, InventoryList);
        slotlist.Add(s5);


        InventoryObjectManager objcreat = Objectcreator.GetComponent<InventoryObjectManager>();
        objcreat.ListofAllItems();
        allitems = objcreat.ItemList;
    }

    // Update is called once per frame
    void Update()
    {
        lastframe = showinventory;
        if (Input.GetKeyDown(KeyCode.I))
        {
            showinventory =! showinventory;
        }
        if (showinventory == false && lastframe== true) 
        {
            InventoryUI.SetActive(false);
        }
        else if(showinventory==true)
        {
            if (lastframe == false)
            {
                InventoryUI.SetActive(true);
            }
            for(int c = 0; c < slotlist.Count; c++)
            {
                slotlist[c].display();
                if (Input.GetKeyDown(slotlist[c].keychoice))
                {
                    chosenslot = c;
                    slotlist[c].highlight();
                    for (int np = 0; np < slotlist.Count; np++)
                    {
                        if (np != c)
                        {
                            slotlist[np].unhighlight();
                        }
                    }
                } //hilights only one slot

            }//displays all the items in inventory
            if (chosenslot != -1)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (equippedobject != null)
                    {
                        equippedobject.UnEquip();
                    }
                    EquipSlot(chosenslot);
                    eqipedslot = chosenslot;
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    equippedobject = null;
                    RemoveFromInventory(chosenslot);
                }
            }
        }
        foreach (InventoryObj x in allitems)
        {
            if (mathsy.findDif(transform.position, x.Object.transform.position) < 1.5 && x.ininv == false)
            {
                AddtoInventory(x);
            }
        }
        if (equippedobject != null)
        {
            equippedobject.Object.transform.position = infrontplayer.transform.position;
            equippedobject.Object.transform.rotation = playerC.rotation;

        }
        if (Input.GetMouseButtonDown(0))
        {
            equippedobject.Use();
            if(equippedobject.UseOnce == true)
            {
                RemoveDisposable(eqipedslot);
                equippedobject.UnEquip();
            }
        }
    }


   
    private void EquipSlot(int n)
    {
        deselectedslot = false;
        if (InventoryList[n, 0] != null) //checks is column is empty
        {
            if (equippedobject!= null)
            {

                if (equippedobject.objectname == InventoryList[n, 0].objectname)
                {
                    equippedobject.UnEquip();
                    equippedobject = null;
                    deselectedslot = true;

                }
            }
            if (deselectedslot == false)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (InventoryList[n, y] == null)
                    {
                        equippedobject = InventoryList[n, y - 1];
                        equippedobject.Equip();
                        break;
                    }

                }
                if (InventoryList[n, 9] != null)
                {
                    equippedobject = InventoryList[n, 9];
                    equippedobject.Equip();
                }
            }

        }


    }
   
    

    
    
    
    private void AddtoInventory(InventoryObj thingtoadd)
    {
        for(int x = 0; x < 5; x++)
        {
            if (InventoryList[x, 0] == null)
            {
                thingtoadd.Pickup();
                InventoryList[x, 0] = thingtoadd;
                break;
            }
            else if (InventoryList[x, 0].objectname == thingtoadd.objectname)
            {
                for(int y = 0; y< thingtoadd.maxstack; y++)
                {
                    if (InventoryList[x, y] == null)
                    {
                        thingtoadd.Pickup();
                        InventoryList[x, y] = thingtoadd;
                        break;
                    }
                }
                break;
            }
        }
    }
    private void RemoveFromInventory(int n)
    {
        if (InventoryList[n, 0] != null) //checks is column is empty
        {
            for (int y = 0; y < 10; y++)
            {
                if (InventoryList[n, y] == null)
                {
                    InventoryList[n, y - 1].Drop();
                    InventoryList[n, y - 1].ininv = false;
                    InventoryList[n, y - 1] = null;
                    break;
                }

            }
            if (InventoryList[n, 9] != null)
            {
                InventoryList[n, 9].Drop();
                InventoryList[n, 9].ininv = false;
                InventoryList[n, 9] = null; //finds last item in column and removes it
            }

        }
    }

    private void RemoveDisposable(int n)
    {
        if (InventoryList[n, 0] != null) //checks is column is empty
        {
            for (int y = 0; y < 10; y++)
            {
                if (InventoryList[n, y] == null)
                {
                    InventoryList[n, y - 1]= null;
                }

            }
            if (InventoryList[n, 9] != null)
            {
                InventoryList[n, 9] = null;//finds last item in column and removes it
            }

        }
    }
}
