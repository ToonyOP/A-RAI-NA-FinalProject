using UnityEngine;

public class Diamond : Item
{
    public int ScoreValue = 10;
    public AudioClip DiamondM;
    public override void OnCollect(Player player)
    {
        base.OnCollect(player);
        // ⭐️ เรียกเมธอดเฉพาะ
        GameManager.Instance.AddDiamondScore(ScoreValue);
        SoundManager.Instance.PlaySFX(DiamondM);
        Destroy(gameObject);
    }
}