using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace GameManage
{
    public class AngelBT : Btree
    {
        public Transform player, enemy, forward;
        public GameObject Astar;
        public LayerMask walllayer;

        pathfinder pfinder;
        Grid grid;
        checkifobserved isobserved;

        public NewPathToPlayer playerpath;
        public PatrolScript patrol;
        public SpinToLook spinning;
        public GoLastPos golast;
        int encounters=0;
        public int Encounters
        {
            get { return encounters; }
        }
        bool observed, observedlastiteration;

        SelectorNode easyroot, hardroot;
            
        void Start()
        {

            grid = Astar.GetComponent<Grid>();
            pfinder = Astar.GetComponent<pathfinder>();
            isobserved = enemy.GetComponent<checkifobserved>();
            StartCoroutine(checkobserved());
            base.Start();
        }

        public override BehaviourNode CreateTree()
        {

            playerpath = new NewPathToPlayer(pfinder, enemy, player);
            KillPlayer kill = new KillPlayer();
            CanSeePlayer playervisible = new CanSeePlayer(enemy, forward, player, walllayer);
            AtPlayerNode atplayer = new AtPlayerNode(enemy, player);
            patrol = new PatrolScript(pfinder, grid, enemy);
            spinning = new SpinToLook(enemy);
            SpinCondition needspin = new SpinCondition();
            VisitedLastPosCond vlastpos = new VisitedLastPosCond();
            golast = new GoLastPos(enemy, pfinder);

            List<BehaviourNode> KillList = new List<BehaviourNode>();
            KillList.Add(atplayer);
            KillList.Add(kill);
            List<BehaviourNode> FollowList = new List<BehaviourNode>();
            FollowList.Add(playervisible);
            FollowList.Add(playerpath);
            SequenceNode KillSequence = new SequenceNode(KillList);
            SequenceNode FollowSequence = new SequenceNode(FollowList);
            List<BehaviourNode> ChaseList = new List<BehaviourNode>();
            ChaseList.Add(KillSequence);
            ChaseList.Add(FollowSequence);
            SelectorNode HuntSelector = new SelectorNode(ChaseList);

            List<BehaviourNode> spinlist = new List<BehaviourNode>();
            spinlist.Add(needspin);
            spinlist.Add(spinning);
            SequenceNode SpinSelector = new SequenceNode(spinlist);

            List<BehaviourNode> lastposlist = new List<BehaviourNode>();
            lastposlist.Add(vlastpos);
            lastposlist.Add(golast);
            SequenceNode LPosSelector = new SequenceNode(lastposlist);

            List<BehaviourNode> FinalList = new List<BehaviourNode>();
            FinalList.Add(HuntSelector);
            FinalList.Add(SpinSelector);
            FinalList.Add(LPosSelector);
            FinalList.Add(patrol);
            List<BehaviourNode> EasyList = new List<BehaviourNode>();
            EasyList.Add(HuntSelector);
            EasyList.Add(patrol);
            SelectorNode root = new SelectorNode(FinalList);
            easyroot = new SelectorNode(EasyList);
            return root; //sets up the tree as lists of nodes that each have a list of nodes
        }
        IEnumerator checkobserved()
        {

            while (true)
            {
                observedlastiteration = observed;
                observed = isobserved.observecheck();
                float timetowait = 0.05f;

                yield return new WaitForSeconds(timetowait);
            }
        }
        void Update()
        {
            if (observed == false)
            {

                if (observedlastiteration == true)
                {
                    encounters++;
                    Debug.Log("was just observed");
                    root.DataChecker["NeedToSpin"] = false; //if you freeze the angel, it will look around for you when unfrozen
                }
                if (encounters / 3 > 15)
                {
                    Debug.Log("u are looky");
                }
                root.DataChecker["DeltaTime"] = Time.deltaTime;
                playerpath.dtime = Time.deltaTime;
                playerpath.timer += Time.deltaTime;
                patrol.dtime = Time.deltaTime;
                spinning.dtime = Time.deltaTime;
                golast.dtime = Time.deltaTime;
                base.Update();
            }
        }

        public void ChangeDifficulty(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy:
                    root = easyroot;
                    break;
                case Difficulty.Normal:
                    break;
                case Difficulty.Difficult:
                    break;

            }

        }
        public void MakeSneaky()
        {
            pfinder.sneakymode = true;
        }
        public void HuntBetter()
        {
            pfinder.sneakymode = false;
        }
    }

}
