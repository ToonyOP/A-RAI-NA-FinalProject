using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    // 1. ส่วน SINGLETON (ของเดิม - เก็บไว้)
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager_AutoCreated");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 2. ข้อมูลเกม (ของเดิม - เก็บไว้ครบถ้วน)
    [Header("Game State")]
    public int currentScore = 0;
    public bool isGamePaused = false;

    [Header("Currency Scores")]
    public int diamondScore = 0;
    public int goldScore = 0;
    public int silverScore = 0;

    [Header("Player Reference")]
    public Player playerInstance;

    [Header("Debug Values")]
    public int _currentHealth;
    public int _maxHealth;

    // 3. ตัวเชื่อม UI (ของใหม่ - มาแทนตัวแปร UI ยิบย่อยอันเดิม)
    // เราไม่ประกาศ public Text/Slider แยกแล้ว แต่เราจะเก็บ "ตัวจัดการ UI" ไว้ตัวเดียวจบ
    private GameUIManager currentUI;

    // ฟังก์ชันนี้มาแทน FindUIElements (ทำงานชัวร์กว่าเดิม)
    public void RegisterUI(GameUIManager ui)
    {
        currentUI = ui; // เก็บ UI เข้ากระเป๋า

        // ปิดเมนู Pause ทันทีที่เชื่อมต่อ (ของเดิมก็ทำแบบนี้)
        if (currentUI.pauseMenu != null)
        {
            currentUI.pauseMenu.SetActive(false);
            isGamePaused = false;
        }

        // สั่งอัปเดตหน้าจอทันที
        RefreshUI();

        Debug.Log("GameManager: เชื่อมต่อ UI สำเร็จแล้ว!");
    }

    // 4. ฟังก์ชันอัปเดตหน้าจอ (ของเดิมผสมของใหม่)
    public void RefreshUI()
    {
        // ถ้ายังไม่มี UI ให้ข้ามไปก่อน (กัน Error)
        if (currentUI == null) return;

        // อัปเดตคะแนน (ดึงจาก currentUI แทนการ Find)
        if (currentUI.scoreText != null) currentUI.scoreText.text = currentScore.ToString();

        // อัปเดตเงินต่างๆ (ใช้ฟังก์ชันเดิม UpdateScoreText)
        UpdateScoreText(currentUI.diamondText, diamondScore);
        UpdateScoreText(currentUI.goldText, goldScore);
        UpdateScoreText(currentUI.silverText, silverScore);

        // อัปเดตหลอดเลือด
        if (currentUI.hpBar != null)
        {
            if (_maxHealth <= 0) _maxHealth = 100;
            currentUI.hpBar.maxValue = _maxHealth;
            currentUI.hpBar.value = _currentHealth;
        }
    }

    // ฟังก์ชันช่วยเปลี่ยนสีตัวเลข (ของเดิม - เก็บไว้)
    private void UpdateScoreText(TMP_Text textUI, int value)
    {
        if (textUI != null)
        {
            textUI.text = value.ToString();
            textUI.color = (value < 0) ? Color.red : Color.white;
        }
    }

    // 5. ระบบ Player และ Score (ของเดิม - เก็บไว้ครบ)
    public void UpdatePlayerReference(Player p)
    {
        playerInstance = p;
        if (playerInstance != null)
        {
            UpdateHealthBar(playerInstance.health, playerInstance.maxHealth);
        }
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        RefreshUI(); // เรียก RefreshUI ตัวเดียวจบ
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        RefreshUI();
    }

    public void AddDiamondScore(int amount)
    {
        diamondScore += amount;
        RefreshUI();
    }

    public void AddGoldScore(int amount)
    {
        goldScore += amount;
        RefreshUI();
    }

    public void AddSilverScore(int amount)
    {
        silverScore += amount;
        RefreshUI();
    }

    // 6. ระบบ Pause (ของเดิม - ปรับให้เรียกผ่าน currentUI)
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;

        // เช็คผ่าน currentUI แทน
        if (currentUI != null && currentUI.pauseMenu != null)
        {
            currentUI.pauseMenu.SetActive(isGamePaused);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}