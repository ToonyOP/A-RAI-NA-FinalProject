using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Optional: ลาก Canvas หน้าเมนูมาใส่ที่นี่ (ถ้ามี)")]
    public GameObject menuCanvas;

    public void LoadSceneByName(string sceneName)
    {
        if (menuCanvas != null)
        {
            Destroy(menuCanvas);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        if (menuCanvas != null)
        {
            Destroy(menuCanvas);
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReturnToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;

        GameObject musicObject = GameObject.FindGameObjectWithTag("Music");
        if (musicObject != null)
        {
            Destroy(musicObject);
        }

        SceneManager.LoadScene(menuSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}