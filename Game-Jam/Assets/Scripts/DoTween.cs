using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DoTween : MonoBehaviour
{
    [SerializeField] private RectTransform title;
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private RectTransform[] buttons;

    private void Start()
    {
        // Animation du titre
        if (title != null)
        {
            AnimatedTitle(title);
        }

        // Animation de la couleur du texte
        if (textTitle != null)
        {
            AnimateColor();
        }

        // Attacher les événements pour les boutons
        foreach (var button in buttons)
        {
            if (button != null)
            {
                AddButtonEvents(button);
            }
        }
    }

    void AnimatedTitle(RectTransform title)
    {
        title.DOScale(new Vector3(1.2f, 1.2f, 1.0f), 1.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void AnimateColor()
    {
        textTitle.DOColor(Color.black, 1.0f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
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
