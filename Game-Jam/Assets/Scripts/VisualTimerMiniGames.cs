using UnityEngine;
using UnityEngine.UI;

public class VisualTimerMiniGames : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float totalGameTime = 15f; // Temps total en secondes
    private float timeRemaining;

    [Header("UI Elements")]
    [SerializeField] private Image[] timerIndicators; // Les 5 indicateurs (100%, 75%, ... 0%)
    private int currentIndicatorIndex = 0; // L'indicateur actuellement affiché

    [SerializeField] private AudioClip Sound;
    private AudioSource audioSource;

    [Header("Game Manager")]
    [SerializeField] private GameObject game1;
    [SerializeField] private GameObject game2;
    [SerializeField] private GameObject game3;

    [Header("Scripts")]
    [SerializeField] private SeparateGameManager sepScript;
    [SerializeField] private RunnerGameManager runScript;
    [SerializeField] private CirclePuzzleGameManager circleScript;
    private int activeScriptIndex;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Sound;
        audioSource.playOnAwake = false;
    }
    void Start()
    {
        sepScript = game1.GetComponent<SeparateGameManager>();
        runScript = game2.GetComponent<RunnerGameManager>();
        circleScript = game3.GetComponent<CirclePuzzleGameManager>();
        ResetTimer();
    }

    void Update()
    {
        if (timeRemaining > 0 && ((sepScript.isGameRunning && activeScriptIndex == 0) || (runScript.isGameRunning && activeScriptIndex == 1) || (circleScript.isGameRunning && activeScriptIndex == 0)))
        {
            timeRemaining -= Time.deltaTime;

            // Calcul du pourcentage restant
            float percentage = timeRemaining / totalGameTime;

            // Passer au prochain indicateur si nécessaire
            if (currentIndicatorIndex < timerIndicators.Length - 1)
            {
                float nextThreshold = 1f - (currentIndicatorIndex + 1) / (float)(timerIndicators.Length - 1);
                if (percentage <= nextThreshold)
                {
                    SwitchToNextIndicator();
                }
            }
        }
    }

    private void UpdateTimerIndicators()
    {
        // Activer uniquement l'indicateur courant
        for (int i = 0; i < timerIndicators.Length; i++)
        {
            timerIndicators[i].enabled = (i == currentIndicatorIndex);
        }
    }

    private void SwitchToNextIndicator()
    {
        // Désactiver l'indicateur actuel et activer le suivant
        timerIndicators[currentIndicatorIndex].enabled = false;
        currentIndicatorIndex++;
        timerIndicators[currentIndicatorIndex].enabled = true;
        audioSource.Play();
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void ResetTimer()
    {
        if (game1.activeSelf == true)
        {
            timeRemaining = sepScript.defaultTime;
            activeScriptIndex = 0;
        }
        else if (game2.activeSelf == true)
        {
            timeRemaining = runScript.defaultTime;
            activeScriptIndex = 1;
        }
        else if (game3.activeSelf == true)
        {
            timeRemaining = circleScript.defaultTime;
            activeScriptIndex = 2;
        }
        timeRemaining = totalGameTime;
        currentIndicatorIndex = 0;
        UpdateTimerIndicators();
    }
}
