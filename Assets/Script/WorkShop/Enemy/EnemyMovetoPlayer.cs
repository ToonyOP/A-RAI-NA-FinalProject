using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovetoPlayer : Enemy
{
    [Header("AI Settings")]
    public float chaseRange = 8.0f; // ระยะไกลสุดที่จะตาม ถ้าเกินนี้จะเลิกตาม

    private void Update()
    {
        // ถ้าไม่มีผู้เล่น ให้หยุดโจมตีแล้วจบการทำงาน
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        // 1. เช็คระยะห่าง
        float distance = GetDistanPlayer();

        // 2. ถ้าผู้เล่นอยู่ไกลเกินระยะไล่ (chaseRange) ให้เลิกตาม
        if (distance > chaseRange)
        {
            animator.SetBool("Attack", false);
            Move(Vector3.zero); // สั่งหยุดเดิน
            return; // จบการทำงานทันที
        }

        // --- ส่วนคำนวณทิศทาง (แก้บั๊กเดินติดพื้น) ---

        // หาความต่างของตำแหน่ง
        Vector3 diff = player.transform.position - transform.position;

        // ⭐️ ตัดแกน Y ทิ้ง (ให้เป็น 0) เพื่อให้มองและเดินในระนาบราบเท่านั้น
        // จะได้ไม่ก้มหน้าปักดินเวลาอยู่บนเสาหรือที่สูง
        diff.y = 0;

        // แปลงเป็นทิศทาง (Normalized)
        Vector3 direction = diff.normalized;

        // ------------------------------------------

        // หันหน้าไปตามทิศทางที่ตัดแกน Y แล้ว
        Turn(direction);
        timer -= Time.deltaTime;

        // เช็คระยะโจมตี (1.5 เมตร)
        if (distance < 1.5f)
        {
            Attack(player);
        }
        else
        {
            animator.SetBool("Attack", false);
            // เดินไปตามทิศทางที่ตัดแกน Y แล้ว (ตัวจะไม่จมดิน)
            Move(direction);
        }
    }

    // วาดวงกลมสีแดงในหน้า Scene เพื่อให้กะระยะ Chase Range ได้ง่าย
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}