using UnityEngine;

public class SaveScore : MonoBehaviour
{
    public static SaveScore Instance { get; private set; }

    [SerializeField] private int playerScore;

    private void Awake()
    {
        // V�rifier si une instance existe d�j�, sinon la cr�er
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Garder cet objet entre les sc�nes
        }
        else
        {
            Destroy(gameObject); // D�truire l'objet s'il existe d�j�
        }
    }

    public void SetScore(int score)
    {
        playerScore = score;
    }
    
    public void IncrementScore(int score)
    {
        playerScore += score;
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void ResetScore()
    {
        playerScore = 0; // Remettre le score � z�ro
    }
}
