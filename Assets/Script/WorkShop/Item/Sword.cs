using UnityEngine;

public class Sword : Item
{
    public int Damage = 25;
    public AudioClip purchaseSound;

    [Header("Unlock Requirements")]
    public int unlockGoldCost = 100; // กำหนดค่าปลดล็อก เช่น 100 Gold

    // ⭐ 1. เพิ่มตัวแปรสำหรับลาก Text ราคามาใส่ (ลากใส่ใน Inspector)
    [Header("UI References")]
    public GameObject priceLabelObject;

    public Sword(Sword sword) : base(sword)
    {
        Damage = sword.Damage;
    }

    public override void OnCollect(Player player)
    {
        // ⭐ ตรวจสอบคะแนน Gold
        if (GameManager.Instance.goldScore >= unlockGoldCost)
        {
            // 1. หักคะแนน Gold
            GameManager.Instance.AddGoldScore(-unlockGoldCost);

            if (purchaseSound != null)
            {
                // PlayClipAtPoint จะสร้าง object ชั่วคราวมาเล่นเสียง ทำให้เสียงไม่ขาดแม้ object นี้จะถูกย้าย
                AudioSource.PlayClipAtPoint(purchaseSound, transform.position);
            }

            // ⭐ 2. สั่งทำลาย Text ราคา (ให้หายไปจากโลก) ทันทีที่ซื้อสำเร็จ
            if (priceLabelObject != null)
            {
                Destroy(priceLabelObject);
            }

            // 3. ดำเนินการเก็บไอเทม (ติดดาบเข้ามือ, เพิ่มดาเมจ)
            base.OnCollect(player);

            Vector3 swordUp = new Vector3(90, 0, 0);

            if (itemcollider != null)
            {
                itemcollider.enabled = false;
            }

            transform.parent = player.RightHand;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(swordUp);
            player.Damage += Damage;

            Debug.Log($"Sword unlocked and equipped! Gold remaining: {GameManager.Instance.goldScore}");
        }
        else
        {
            // 4. ถ้าคะแนนไม่พอ ให้แสดงข้อความแจ้งเตือน
            Debug.Log($"Need {unlockGoldCost} Gold to unlock the Sword, but only have {GameManager.Instance.goldScore}.");
            // Text จะยังคงอยู่ เพราะยังไม่ได้ซื้อ
        }
    }
}