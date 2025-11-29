using UnityEngine;

public class Fullpotion : Item
{
    // กำหนดค่า Heal ที่สูงมากเพื่อจำลอง "Full" Heal
    public int AmountHealth = 9999;

    [Header("Unlock Requirements")]
    public int unlockSilverCost = 250;

    [Header("Audio Effects")]
    public AudioClip purchaseSound; // ลากไฟล์เสียงตอนซื้อสำเร็จมาใส่ตรงนี้
    public AudioClip failSound;     // ลากไฟล์เสียงตอนเงินไม่พอมาใส่ตรงนี้
    [Range(0f, 1f)]
    public float volume = 1.0f;     // ปรับความดังเสียง

    // ตัวแปรกันเสียงรัว (Cooldown) สำหรับเสียง Fail
    private float lastFailSoundTime;
    private float failSoundCooldown = 1.0f;

    public override void OnCollect(Player player)
    {
        // ตรวจสอบคะแนน Silver
        if (GameManager.Instance.silverScore >= unlockSilverCost)
        {
            // --- กรณีสำเร็จ (Success) ---

            // 1. เล่นเสียงซื้อสำเร็จ
            // ต้องใช้ PlayClipAtPoint เพราะ gameObject นี้กำลังจะถูก Destroy
            if (purchaseSound != null)
            {
                AudioSource.PlayClipAtPoint(purchaseSound, transform.position, volume);
            }

            // 2. หักคะแนน Silver
            GameManager.Instance.AddSilverScore(-unlockSilverCost);

            // 3. ดำเนินการ Heal และทำลายขวด
            base.OnCollect(player);
            player.Heal(AmountHealth);
            Destroy(gameObject);

            Debug.Log($"Fullpotion purchased! Silver remaining: {GameManager.Instance.silverScore}");
        }
        else
        {
            // --- กรณีล้มเหลว/เงินไม่พอ (Fail) ---

            // ตรวจสอบเวลาเพื่อไม่ให้เสียงเล่นรัวเกินไปถ้ายืนแช่
            if (Time.time > lastFailSoundTime + failSoundCooldown)
            {
                if (failSound != null)
                {
                    AudioSource.PlayClipAtPoint(failSound, transform.position, volume);
                }

                // อัปเดตเวลาล่าสุดที่เล่นเสียงเตือน
                lastFailSoundTime = Time.time;

                Debug.Log($"Need {unlockSilverCost} Silver but have {GameManager.Instance.silverScore}.");
            }
        }
    }
}