using UnityEngine;

public class Potion : Item
{
    public int AmountHealth = 20;

    // ⭐ เพิ่มตัวแปรกำหนดคะแนน Silver ที่ต้องใช้ในการปลดล็อก
    [Header("Unlock Requirements")]
    public int unlockSilverCost = 50; // กำหนดค่าปลดล็อก (คุณสามารถปรับค่านี้ได้ใน Inspector)

    public override void OnCollect(Player player)
    {
        // ⭐ ตรวจสอบคะแนน Silver
        if (GameManager.Instance.silverScore >= unlockSilverCost)
        {
            // 1. หักคะแนน Silver
            GameManager.Instance.AddSilverScore(-unlockSilverCost); // ใช้ค่าลบเพื่อหักคะแนน

            // 2. ดำเนินการตามปกติ (เรียกฐาน, รักษาผู้เล่น)
            base.OnCollect(player);
            player.Heal(AmountHealth);
            Destroy(gameObject);

            Debug.Log($"Potion purchased and consumed! Health +{AmountHealth}. Silver remaining: {GameManager.Instance.silverScore}");
        }
        else
        {
            // 3. ถ้าคะแนนไม่พอ ให้แสดงข้อความแจ้งเตือน (และไม่ทำลาย Potion)
            Debug.Log($"Need {unlockSilverCost} Silver to purchase the Potion, but only have {GameManager.Instance.silverScore}.");
            // Potion ยังคงอยู่ในโลกของเกม
        }
    }
}