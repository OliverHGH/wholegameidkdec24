using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManage
{
    public enum Difficulty
    { 
        Easy,
        Normal,
        Difficult
    }


    public class GameManagerScript : MonoBehaviour
    {
        public Difficulty currentdifficulty;
        public List<AngelBT> enemylist = new List<AngelBT>();
        public List<GameObject> enemybodylist = new List<GameObject>();
        public GameObject pausemenu, enemy1obj;
        private bool gamepaused = false;
        float timer;
        void Start()
        {
            enemylist.Add(enemy1obj.GetComponent<AngelBT>());
            enemybodylist.Add(enemy1obj);
            currentdifficulty = Difficulty.Normal;
            timer = 0;
        }
        public void ChangeDifficulty(Difficulty d)
        {
            foreach(AngelBT x in enemylist)
            {
                x.ChangeDifficulty(d);
            }
        }

        void TogglePaused()
        {
            gamepaused =! gamepaused;
            if (gamepaused == true)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
        void Update()
        {
            timer += Time.deltaTime; 
            foreach(AngelBT x in enemylist)
            {
                if (timer / (x.Encounters / 3) < 20 && timer>120)
                {
                    Debug.Log("this angel has been seen a lot");
                    x.MakeSneaky();
                }
                else if (x.Encounters / 3>0 && timer / (x.Encounters / 3)>100 && timer>120)
                {
                    Debug.Log("you are hidey");
                    x.HuntBetter();
                }
            }
        }
    }

}