using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class UIButtonAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform[] stars;
    [SerializeField] private int[] scoreThresholds = { 200, 500, 1000 };
    [SerializeField] private Color unlockedColor = new Color(245 / 255f, 213 / 255f, 112 / 255f);
    [SerializeField] private RectTransform[] buttons;

    [SerializeField] private AudioClip starSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = starSound;
        audioSource.playOnAwake = false;
    }
    void Start()
    {
        AnimateStarsBasedOnScore();

        foreach (var button in buttons)
        {
            if (button != null)
            {
                AddButtonEvents(button);
            }
        }
    }

    void AnimateStarsBasedOnScore()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (SaveScore.Instance.GetScore() >= scoreThresholds[i])
            {
                UnlockStar(stars[i]);
            }
        }
    }

    void UnlockStar(RectTransform star)
    {
        var starImage = star.GetComponent<UnityEngine.UI.Image>();
        if (starImage != null)
        {
            starImage.color = unlockedColor;
        }

        star.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 0.8f)
            .SetEase(Ease.OutElastic)
            .SetLoops(-1, LoopType.Yoyo);

        StartCoroutine(PlaySoundWithAnimation());
    }
    private IEnumerator PlaySoundWithAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            audioSource.Play();
        }
    }

    void AddButtonEvents(RectTransform button)
    {
        var eventTrigger = button.gameObject.AddComponent<EventTrigger>();

        var pointerEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        pointerEnter.callback.AddListener((data) => AnimatedButtonHover(button));
        eventTrigger.triggers.Add(pointerEnter);

        var pointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExit.callback.AddListener((data) => ResetButtonScale(button));
        eventTrigger.triggers.Add(pointerExit);
    }

    void AnimatedButtonHover(RectTransform button)
    {
        button.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 0.3f)
            .SetEase(Ease.OutBounce);
    }

    void ResetButtonScale(RectTransform button)
    {
        button.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutSine);
    }
}