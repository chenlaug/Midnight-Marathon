using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void GoMainScene()
    {
        // R�initialiser le score avant de charger la sc�ne principale
        if (SaveScore.Instance != null)
        {
            SaveScore.Instance.ResetScore();  // Appeler la m�thode pour r�initialiser le score
        }

        // Charger la sc�ne "MainScene"
        SceneManager.LoadScene("MainScene");
    }

    public void GoMainMenu()
    {
        // Charger la sc�ne "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
