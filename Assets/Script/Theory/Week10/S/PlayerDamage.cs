using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public void TakeDamage(PlayerData playerData,int amount)
    {
        playerData.Health -= amount;
        Debug.Log($"{playerData.PlayerName} took {amount} damage. Health: {playerData.Health}");
        if (playerData.Health <= 0)
        {
            Debug.Log($"{playerData.PlayerName} " + " has been defeated!");
        }
    }
}
