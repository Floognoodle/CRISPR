using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFailedUI : MonoBehaviour
{
    public static LevelFailedUI Instance { get; private set; }

    public GameObject levelFailedPanel;
    public string restartSceneName = "";
    public bool pauseOnShow = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (levelFailedPanel != null) levelFailedPanel.SetActive(false);
    }

    public void Show()
    {
        if (levelFailedPanel == null) return;
        levelFailedPanel.SetActive(true);
        if (pauseOnShow) Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (levelFailedPanel == null) return;
        levelFailedPanel.SetActive(false);
        if (pauseOnShow) Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        if (pauseOnShow) Time.timeScale = 1f;
        string sceneToLoad = string.IsNullOrEmpty(restartSceneName) ? SceneManager.GetActiveScene().name : restartSceneName;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}