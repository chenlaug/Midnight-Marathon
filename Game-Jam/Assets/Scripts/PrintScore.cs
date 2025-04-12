using UnityEngine;
using TMPro;

public class PrintScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        // Assurez-vous que SaveScore.Instance existe
        if (SaveScore.Instance != null)
        {
            UpdateScoreText();
        }
        else
        {
            Debug.LogError("SaveScore.Instance is missing. Ensure SaveScore is in the scene.");
        }
    }

    private void UpdateScoreText()
    {
        // Assurez-vous que scoreText n'est pas null
        if (scoreText != null)
        {
            scoreText.text = "Score: " + SaveScore.Instance.GetScore().ToString();
        }
        else
        {
            Debug.LogError("scoreText is missing in the PrintScore component.");
        }
    }
}
