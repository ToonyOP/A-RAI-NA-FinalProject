using System;
using UnityEngine;

public class GoodLExample : MonoBehaviour
{
    public class Animal // คลาสพื้นฐานสำหรับสัตว์
    {
        public virtual void Eat()
        {
            Console.WriteLine("Animal is eating.");
        }
    }

    public class Bird : Animal
    {
        public virtual void LayEgg()
        {
            Debug.Log("Lay egg action");
        }
    }

    interface IFlyable
    {
        void Fly();
    }

    public class Penguin : Bird
    {
        
    }

    public class Seagull : Bird, IFlyable
    {
        public void Fly()
        {
              Debug.Log("Fly action");
        }

    }


    private void Start()
    {
        Penguin penguin = new Penguin();
        penguin.Eat();
        penguin.LayEgg();

        Seagull seagull = new Seagull();
        seagull.Eat();
        seagull.LayEgg();
        seagull.Fly();
    }
}
