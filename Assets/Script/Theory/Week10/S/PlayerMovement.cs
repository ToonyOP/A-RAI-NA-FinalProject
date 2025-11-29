using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public void Move(PlayerData playerData, float deltaX, float deltaY)
    {
        playerData.PositionX += deltaX;
        playerData.PositionY += deltaY;
        Debug.Log($"{playerData.PlayerName} moved to ({playerData.PositionX}, {playerData.PositionY})");
    }
}
