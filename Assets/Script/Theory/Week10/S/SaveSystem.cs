using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveProgress(PlayerData playerData)
    {
        Debug.Log($"Saving game for {playerData.PlayerName} to local file...");
        // Logic for saving player data to a file
    }
}
