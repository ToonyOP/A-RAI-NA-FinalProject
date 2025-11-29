using System;
using UnityEngine;

namespace Theory.Week10
{

    public class GoodDExample : MonoBehaviour
    {
        public abstract class Enemy
        {
            public virtual void Spawn()
            {

            }
            public virtual void PerformAction()
            {

            }
        }

        public class Goblin : Enemy
        {
            public override void Spawn()
            {
                base.Spawn();
                Debug.Log("Goblin Spawn");
            }
            public override void PerformAction()
            { 
                base.PerformAction();
                Debug.Log("Goblin Perform Action");
            }

        }
        public class Orc : Enemy
        {
            public override void Spawn()
            {
                base.Spawn();
                Debug.Log("Orc Spawn");
            }
            public override void PerformAction()
            {
                base.PerformAction();
                Debug.Log("Orc Perform Action");
            }
        }
        private void Start()
        {
            GoblinEnemy goblinEnemy = new GoblinEnemy();
            goblinEnemy.Spawn();
            goblinEnemy.PerformAction();

            Orc orcEnemy = new Orc();
            orcEnemy.Spawn();
            orcEnemy.PerformAction();
        }

        private Enemy enemy;
        public void StartWave()
        {
            enemy.Spawn();
            enemy.PerformAction(); 
        }
    }
}

  
