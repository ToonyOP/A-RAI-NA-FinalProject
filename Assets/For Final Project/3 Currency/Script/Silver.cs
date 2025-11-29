using UnityEngine;

public class Silver : Item
{
    public int ScoreValue = 10;
    public AudioClip SilverM;
    public override void OnCollect(Player player)
    {
        base.OnCollect(player);
        // ⭐️ เรียกเมธอดเฉพาะ
        GameManager.Instance.AddSilverScore(ScoreValue);
        SoundManager.Instance.PlaySFX(SilverM);
        Destroy(gameObject);
    }
}