using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace GameManage 
{
    public class Datatosave
    {
        public int numbersave;
        public string stringsave;
        public float[] Ppos = new float[3];
        public float[] Epos = new float[3];
        public Difficulty dlevel;
        public string[] inventoryslot1 = new string[10];
        public string[] inventoryslot2 = new string[10];
        public string[] inventoryslot3 = new string[10];
        public string[] inventoryslot4 = new string[10];
        public string[] inventoryslot5 = new string[10]; // saves each slot as array as JOSN has no 2d arrays

    }
    public class JSONSAVELOAD : MonoBehaviour
    {
        Datatosave TestData;
        string FilePath;
        public Transform player, enemy;
        public GameObject gmanagerobj;
        GameManagerScript gamemanager;
        InventoryManager inventorymanage;
        playermovement setasct;
        void Start()
        {
            setasct = player.GetComponent<playermovement>();
            gamemanager = gmanagerobj.GetComponent<GameManagerScript>();
            inventorymanage = player.GetComponent<InventoryManager>();
            TestData = new Datatosave();
            TestData.stringsave = "eormgot";
            TestData.numbersave = 69;
            FilePath = Application.persistentDataPath + "/PlayerData.json";
            Debug.Log("File path: " + FilePath);
        }


        public void Save()
        {
            Debug.Log("saving");
            TestData.Ppos[0] = player.position.x;
            TestData.Ppos[1] = player.position.y;
            TestData.Ppos[2] = player.position.z;
            TestData.Epos[0] = enemy.position.x;
            TestData.Epos[1] = enemy.position.y;
            TestData.Epos[2] = enemy.position.z;
            TestData.dlevel = gamemanager.currentdifficulty;
            WriteSlot(TestData.inventoryslot1, 0);
            WriteSlot(TestData.inventoryslot2, 1);
            WriteSlot(TestData.inventoryslot3, 2);
            WriteSlot(TestData.inventoryslot4, 3);
            WriteSlot(TestData.inventoryslot5, 4);

            Debug.Log(TestData.inventoryslot1[0] + " to be saved");
            string TestDatastring = JsonUtility.ToJson(TestData);
            File.WriteAllText(FilePath, TestDatastring); // loads data into the file
        }
        public void LoadGame()
        {
            Debug.Log("loading");
            if (File.Exists(FilePath))
            {
                string LoadData = File.ReadAllText(FilePath);
                TestData = JsonUtility.FromJson<Datatosave>(LoadData);
                setasct.loading = true;
                player.transform.position = new Vector3(TestData.Ppos[0], TestData.Ppos[1], TestData.Ppos[2]);
                enemy.transform.position = new Vector3(TestData.Epos[0], TestData.Epos[1], TestData.Epos[2]);
                gamemanager.currentdifficulty = TestData.dlevel;
                ReadSlot(TestData.inventoryslot1, 0);
                ReadSlot(TestData.inventoryslot2, 1);
                ReadSlot(TestData.inventoryslot3, 2);
                ReadSlot(TestData.inventoryslot4, 3);
                ReadSlot(TestData.inventoryslot5, 4);

            }


        }
        public void Delete()
        {
            Debug.Log("deleting");
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
        public void WriteSlot(string[] slot, int number)
        {
            for(int y = 0; y < 9; y++)
            {
                if (inventorymanage.loadlist[number, y] !=null)
                {
                    slot[y] = inventorymanage.loadlist[number, y].ID;
                }
            }
        }
        public void ReadSlot(string[] slot, int number)
        {
            for (int y = 0; y < 9; y++)
            {
                if (slot[y] != null)
                {
                    foreach (InventoryObj o in inventorymanage.itemstoload)
                    {
                        if (o.ID == slot[y])
                        {
                            inventorymanage.loadlist[number, y] = o;
                            inventorymanage.loadlist[number, y].Pickup();
                        }
                    }
                }
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                LoadGame();
            }
            if (Input.GetKeyUp(KeyCode.N))
            {
                setasct.loading = false;
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                Delete();
            }

        }
    }

}
