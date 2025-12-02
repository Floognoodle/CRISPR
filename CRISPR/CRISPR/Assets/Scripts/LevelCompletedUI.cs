using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletedUI : MonoBehaviour
{
    public static LevelCompletedUI Instance { get; private set; }

    public GameObject levelCompletedPanel;
    public string nextSceneName = "";
    public bool pauseOnShow = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (levelCompletedPanel != null) levelCompletedPanel.SetActive(false);
    }

    public void Show()
    {
        if (levelCompletedPanel == null) return;
        levelCompletedPanel.SetActive(true);
        if (pauseOnShow) Time.timeScale = 0f;
    }

    public void Hide()
    {
        if (levelCompletedPanel == null) return;
        levelCompletedPanel.SetActive(false);
        if (pauseOnShow) Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        if (pauseOnShow) Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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