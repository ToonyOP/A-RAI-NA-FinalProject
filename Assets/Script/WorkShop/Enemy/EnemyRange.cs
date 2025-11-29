using UnityEngine;

public class EnemyRange : Enemy
{
    [Header("AI Settings")]
    public float attackRange = 5f; // ระยะยิง
    public float chaseRange = 10.0f; // ระยะมองเห็น

    void Update()
    {
        if (player == null)
        {
            animator.SetBool("Attack", false);
            return;
        }

        // 1. เช็คระยะห่าง
        float distance = GetDistanPlayer();

        // 2. ถ้าไกลเกินระยะมองเห็น -> เลิกตาม/ยืนเฉยๆ
        if (distance > chaseRange)
        {
            animator.SetBool("Attack", false);
            Move(Vector3.zero); // สั่งหยุดเดิน
            return;
        }

        // --- คำนวณทิศทาง (ตัดแกน Y เพื่อไม่ให้เดินจมดิน/หันหน้าลงพื้น) ---
        Vector3 diff = player.transform.position - transform.position;
        diff.y = 0;
        Vector3 direction = diff.normalized;
        // -----------------------------------------------------------

        // หันหน้าหาผู้เล่นเสมอ (ตราบใดที่อยู่ในระยะมองเห็น)
        Turn(direction);
        timer -= Time.deltaTime;

        // --- ⭐️ Logic การเดินและการโจมตี ⭐️ ---
        if (distance < attackRange)
        {
            // A. ถ้าระยะถึงแล้ว (ยิงถึง)
            Move(Vector3.zero); // **สั่งหยุดเดิน** เพื่อยืนยิงนิ่งๆ
            Attack(player);     // ยิง
        }
        else
        {
            // B. ถ้ายังยิงไม่ถึง (แต่อยู่ในระยะมองเห็น)
            animator.SetBool("Attack", false);
            Move(direction);    // **สั่งเดินเข้าหา** จนกว่าจะเข้าระยะยิง
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}