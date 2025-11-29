using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("Text UI")]
    public TMP_Text scoreText;
    public TMP_Text diamondText;
    public TMP_Text goldText;
    public TMP_Text silverText;

    [Header("Icon UI")]
    public RawImage diamondIcon;
    public RawImage goldIcon;
    public RawImage silverIcon;

    [Header("Other UI")]
    public Slider hpBar;
    public GameObject pauseMenu;

    // เปลี่ยนจาก Start เป็น Update (ทำงานตลอดเวลา)
    void Update()
    {
        // 1. เช็คก่อนว่า GameManager มีตัวตนไหม
        if (GameManager.Instance == null) return;

        // 2. ดึงข้อมูลมาใส่ดื้อๆ เลย (ไม่ต้องรอใครสั่ง)
        if (scoreText != null)
            scoreText.text = GameManager.Instance.currentScore.ToString();

        if (diamondText != null)
        {
            diamondText.text = GameManager.Instance.diamondScore.ToString();
            // เปลี่ยนสีถ้าติดลบ
            diamondText.color = (GameManager.Instance.diamondScore < 0) ? Color.red : Color.white;
        }

        if (goldText != null)
        {
            goldText.text = GameManager.Instance.goldScore.ToString();
            goldText.color = (GameManager.Instance.goldScore < 0) ? Color.red : Color.white;
        }

        if (silverText != null)
        {
            silverText.text = GameManager.Instance.silverScore.ToString();
            silverText.color = (GameManager.Instance.silverScore < 0) ? Color.red : Color.white;
        }

        if (hpBar != null)
        {
            int maxHP = GameManager.Instance._maxHealth;
            if (maxHP <= 0) maxHP = 100; // กัน Error หาร 0

            hpBar.maxValue = maxHP;
            hpBar.value = GameManager.Instance._currentHealth;
        }

        // *หมายเหตุ: ส่วน PauseMenu ไม่ต้องใส่ใน Update เพราะมันใช้ปุ่มกดแยกต่างหาก
    }
}