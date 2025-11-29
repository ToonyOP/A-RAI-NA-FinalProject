using UnityEngine;

// When the Player enters this trigger, mark this transform as the last checkpoint
// so Respawner will use it for future respawns.
public class Spawnpoint : MonoBehaviour
{
    public int checkpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            Respawner.lastCheckpoint = this.transform;

            Debug.Log($"Spawn point set to checkpoint {checkpointIndex}");
        }
    }
}
