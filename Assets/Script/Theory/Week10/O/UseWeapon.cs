using Theory.Week10;
using UnityEngine;


public class UseWeapon : MonoBehaviour
{
    
    void Start()
    {
        Debug.Log("---- Bad Calculate Damage ----");
        Weapon knife = new Weapon(WeaponType.Knife, 10);
        Debug.Log($"Sword Damage: {GoodCalculateDamage((IBonusWeapon)knife)}"); // Output: Sword Damage: 15

    }
    public int GoodCalculateDamage(IBonusWeapon weapon)
    {
        return weapon.GetDamage();
    }

    public int GoodCalculateDamage(Weapon weapon)
    {
        int totalDamage = weapon.BaseDamage;
        if (weapon.Type == WeaponType.Sword)
        {
            totalDamage += 5; 
        }
        else if (weapon.Type == WeaponType.Bow)
        {
            totalDamage += 3;
        }
        else if (weapon.Type == WeaponType.Gun)
        {
            totalDamage += 10; 
        }
        return totalDamage;
    }
    
}
