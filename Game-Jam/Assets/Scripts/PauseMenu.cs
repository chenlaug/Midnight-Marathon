using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            Debug.Log("Game Paused");
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            Debug.Log("Game Resumed");
        }
    }
}
