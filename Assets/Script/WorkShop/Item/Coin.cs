using UnityEngine;

public class Coin : Item
{
    public int ScoreValue = 10;
    public AudioClip SoundCoin;

    public override void OnCollect(Player player)
    {
        base.OnCollect(player);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(ScoreValue);
        }

        if (SoundCoin != null)
        {
            AudioSource.PlayClipAtPoint(SoundCoin, transform.position);
        }

        Destroy(gameObject);
    }
}