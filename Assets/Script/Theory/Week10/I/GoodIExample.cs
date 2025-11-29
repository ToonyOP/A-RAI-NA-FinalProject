using UnityEngine;

public class GoodIExample : MonoBehaviour
{
    interface IMeleeSkill
    {
        void PhysicalAttack();
        void UseShield();
    }

    interface IMagicSkill
    {
        void CastSpell(string spellName);
        void CheckMana();
    }

    interface IBuffSkill
    {
        void Heal(int amount);
        void ApplyBuff();
    }

    public class Kinght: IMeleeSkill
    {
        public void PhysicalAttack()
        {
            Debug.Log("Melee Attack");
        }

        public void UseShield()
        {
            Debug.Log("Shield Up");
        }
    }

    public class Elf : IMeleeSkill, IMagicSkill
    {
        public void CastSpell(string spellName)
        {
            Debug.Log("Spell Casting");
        }
        public void CheckMana()
        {
            Debug.Log("Mana Checking");
        }
        public void PhysicalAttack()
        {
            Debug.Log("Melee Attack");
        }
        public void UseShield()
        {
            Debug.Log("Shield Up");
        }
    }
}
