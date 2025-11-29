using UnityEngine;

public class Gold : Item
{
    public int ScoreValue = 10;
    public AudioClip GoldM;
    public override void OnCollect(Player player)
    {
        base.OnCollect(player);
        // ⭐️ เรียกเมธอดเฉพาะ
        GameManager.Instance.AddGoldScore(ScoreValue);
        SoundManager.Instance.PlaySFX(GoldM);
        Destroy(gameObject);
    }
}