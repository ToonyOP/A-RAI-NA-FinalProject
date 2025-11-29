using System;
using UnityEngine;

public class Respawner : Stuff, IInteractable
{
    [Header("Settings")]
    public GameObject playerPrefab;

    [Header("Death Penalty Settings")]
    public int silverCost = 0;
    public int goldCost = 0;
    public int diamondCost = 0;

    // ❌ ลบตัวแปร Static ที่ใช้จำค่าออกให้หมด เพื่อป้องกันบั๊ก
    // private static int s_currentSilverCost; ... (ไม่ต้องใช้แล้ว)

    [Header("Audio Effects")]
    public AudioClip saveSound;     // เสียงตอนเดินชน
    public AudioClip respawnSound;  // เสียงตอนเกิดใหม่

    [Header("Visual Effects")]
    public GameObject saveAnim;
    public GameObject spawnAnim;
    public GameObject saveEffect;

    // เก็บตำแหน่งเสาล่าสุด
    public static Transform lastCheckpoint;

    private static GameObject s_playerPrefab;
    private static bool isEventSubscribed = false;

    public CameraControl cameraControl;
    public bool isInteractable { get => isLock; set => isLock = value; }

    private void Awake()
    {
        if (s_playerPrefab == null && playerPrefab != null)
        {
            s_playerPrefab = playerPrefab;
            isEventSubscribed = false;
        }
    }

    private void Start()
    {
        if (!isEventSubscribed)
        {
            Player p = FindObjectOfType<Player>();
            if (p != null)
            {
                p.OnDestory += HandlePlayerDestroyed;
                isEventSubscribed = true;
                Debug.Log("Respawner: Started tracking Player.");
            }
        }
    }

    // ---------------------------------------------------------
    // ส่วนของการ "เซฟ" (Checkpoint)
    // ---------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // แค่จำว่าเสาไหนคือเสาล่าสุดก็พอ ไม่ต้องจำราคา (เดี๋ยวไปดึงเอาตอนตาย)
            lastCheckpoint = this.transform;

            PlaySaveSound();
            Debug.Log($"Checkpoint Updated: {gameObject.name}");
        }
    }

    public void Interact(Player player)
    {
        Interact(player, gameObject);
    }

    public void Interact(Player player, GameObject gameObject)
    {
        lastCheckpoint = this.transform;

        PlaySaveSound();
        Debug.Log($"Checkpoint Updated via Interact: {gameObject.name}");

        if (saveAnim != null) saveAnim.SetActive(false);
        if (spawnAnim != null) spawnAnim.SetActive(true);

        if (saveEffect != null)
        {
            GameObject g = Instantiate(saveEffect, transform.position, Quaternion.identity, transform);
            Destroy(g, 1);
        }
    }

    private void PlaySaveSound()
    {
        if (saveSound != null)
        {
            AudioSource.PlayClipAtPoint(saveSound, transform.position);
        }
    }

    // ---------------------------------------------------------
    // ส่วนของการ "เกิดใหม่" (Respawn)
    // ---------------------------------------------------------

    public static void HandlePlayerDestroyed(Idestoryable entity)
    {
        isEventSubscribed = false;

        if (s_playerPrefab == null)
        {
            Debug.LogError("FATAL: No Player Prefab!");
            return;
        }

        // ⭐ 1. ดึงข้อมูลจากเสาล่าสุด (lastCheckpoint) โดยตรง
        // วิธีนี้แก้บั๊กเงินไม่ลดได้ชะงัด เพราะดึงจากตัวเสาจริงๆ
        Respawner currentCheckpoint = null;
        if (lastCheckpoint != null)
        {
            currentCheckpoint = lastCheckpoint.GetComponent<Respawner>();
        }

        Vector3 pos = lastCheckpoint != null ? lastCheckpoint.position : Vector3.zero;
        Quaternion rot = lastCheckpoint != null ? lastCheckpoint.rotation : Quaternion.identity;

        // ⭐ 2. ถ้าเจอเสา ให้หักเงินและเล่นเสียงตามการตั้งค่าของเสานั้น
        if (currentCheckpoint != null && GameManager.Instance != null)
        {
            // หักเงิน
            if (currentCheckpoint.silverCost > 0)
                GameManager.Instance.AddSilverScore(-currentCheckpoint.silverCost);

            if (currentCheckpoint.goldCost > 0)
                GameManager.Instance.AddGoldScore(-currentCheckpoint.goldCost);

            if (currentCheckpoint.diamondCost > 0)
                GameManager.Instance.AddDiamondScore(-currentCheckpoint.diamondCost);

            // เล่นเสียงเกิดใหม่
            if (currentCheckpoint.respawnSound != null)
            {
                AudioSource.PlayClipAtPoint(currentCheckpoint.respawnSound, pos);
            }

            Debug.Log($"Respawn Penalty Applied from {currentCheckpoint.name}");
        }

        Debug.Log("Respawning Player...");

        // สร้างตัวผู้เล่นใหม่
        GameObject newPlayer = Instantiate(s_playerPrefab, pos, rot) as GameObject;

        CameraControl camCon = Camera.main.GetComponent<CameraControl>();
        if (camCon != null)
        {
            camCon.target = newPlayer.transform;
        }

        if (newPlayer == null)
        {
            Debug.LogError("Failed to instantiate Player.");
            return;
        }

        Player p = newPlayer.GetComponent<Player>();
        if (p != null)
        {
            p.OnDestory += HandlePlayerDestroyed;
            isEventSubscribed = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdatePlayerReference(p);
            }
        }

        Debug.Log("Player respawned successfully.");
    }
}