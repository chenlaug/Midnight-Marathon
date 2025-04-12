using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMinigame : MonoBehaviour
{
    [SerializeField] private List<GameObject> games;
    [SerializeField] private List<GameObject> managers;
    private int currentGameIndex = -1;
    private GameObject currentGame;
    private GameObject currentManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize with a random game
        ChooseRandomGame();
    }

    // Function to choose a random game, avoiding the previously played one
    void ChooseRandomGame()
    {
        int previousIndex = currentGameIndex;
        while (currentGameIndex == previousIndex)
        {
            currentGameIndex = Random.Range(0, games.Count);
        }

        foreach (var game in games)
        {
            game.SetActive(game == games[currentGameIndex]);
        }

        // Reset the new game
        currentGame = games[currentGameIndex];
        currentManager = managers[currentGameIndex];
        if (currentManager.GetComponent<SeparateGameManager>() != null)
        {
            currentManager.GetComponent<SeparateGameManager>().ResetGame();
        }
        else if (currentManager.GetComponent<RunnerGameManager>() != null)
        {
            currentManager.GetComponent<RunnerGameManager>().ResetGame();
        }
        else if (currentManager.GetComponent<CirclePuzzleGameManager>() != null)
        {
            currentManager.GetComponent<CirclePuzzleGameManager>().ResetGame();
        }
    }

    // This function can be called when the current game ends (e.g., in score management)
    public void OnGameOver()
    {
        // Choose another game to play
        ChooseRandomGame();
        FindObjectOfType<VisualTimerMiniGames>().ResetTimer();
    }
}