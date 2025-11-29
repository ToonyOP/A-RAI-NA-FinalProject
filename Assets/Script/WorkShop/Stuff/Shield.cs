using UnityEngine;
using TMPro;

public class Shield : Item
{
    public GameObject shieldMesh;
    public int Deffent = 10;
    public AudioClip purchaseSound;

    [Header("Unlock Requirements")]
    public int unlockDiamondCost = 100;

    [Header("UI References")]
    public GameObject priceLabelObject;

    // ⭐️ 1. เพิ่ม Constructor (ส่วนที่ขาดไปเมื่อเทียบกับ Sword)
    // จำเป็นมากถ้าระบบ Inventory มีการ Clone ไอเทม
    public Shield(Shield shield) : base(shield)
    {
        this.Deffent = shield.Deffent;
        this.shieldMesh = shield.shieldMesh;
        this.unlockDiamondCost = shield.unlockDiamondCost;
        this.purchaseSound = shield.purchaseSound;
        this.priceLabelObject = shield.priceLabelObject;
    }

    public override void OnCollect(Player player)
    {
        // ตรวจสอบเงิน (Diamond)
        if (GameManager.Instance.diamondScore >= unlockDiamondCost)
        {
            // 1. หักเงิน
            GameManager.Instance.AddDiamondScore(-unlockDiamondCost);

            // 2. เล่นเสียง
            if (purchaseSound != null)
            {
                AudioSource.PlayClipAtPoint(purchaseSound, transform.position);
            }

            // 3. ทำลายป้ายราคา
            if (priceLabelObject != null)
            {
                Destroy(priceLabelObject);
            }

            // 4. เรียก Base Logic (เก็บเข้ากระเป๋า หรือ Logic พื้นฐานของ Item)
            base.OnCollect(player);

            // ⭐️ 5. Logic การสวมใส่
            // เช็คว่า Player มีการตั้งค่ามือซ้าย (LeftHand) มาหรือไม่
            if (player.LeftHand != null)
            {
                // ปิด Collider ของตัว Item หลัก
                if (itemcollider != null)
                    itemcollider.enabled = false;

                // ปิด Collider ของ Mesh โล่ (ถ้ามี) เพื่อไม่ให้ชนตัวคนถือ
                if (shieldMesh != null)
                {
                    Collider shieldCollider = shieldMesh.GetComponent<Collider>();
                    if (shieldCollider != null)
                        shieldCollider.enabled = false;
                }

                // ย้ายไปติดมือซ้าย
                transform.parent = player.LeftHand;
                transform.localPosition = Vector3.zero;

                // ตั้งค่าการหมุน (ปรับแกนตามโมเดลของคุณ ส่วนใหญ่โล่ใช้ 180 หรือ 90)
                Vector3 ShieldUp = new Vector3(0, 0, 180);
                transform.localRotation = Quaternion.Euler(ShieldUp);

                // เพิ่มค่าพลังป้องกันให้ Player
                player.Deffent += Deffent;

                Debug.Log($"Shield Equipped! Diamond left: {GameManager.Instance.diamondScore}");
            }
            else
            {
                Debug.LogError("Error: Player does not have 'LeftHand' assigned in the Inspector!");
            }
        }
        else
        {
            // เงินไม่พอ
            Debug.Log($"Need {unlockDiamondCost} Diamond to unlock the Shield, but only have {GameManager.Instance.diamondScore}.");
        }
    }
}