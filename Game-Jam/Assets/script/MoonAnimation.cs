using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Screen = UnityEngine.Device.Screen;

public class MoonAnimation : MonoBehaviour
{
    [Header("Moon Animation")]
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private float animationDuration; // time of the animation in seconds
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 screenStartPoint = new Vector3(0, Screen.height / 2f, 0);
        startPoint = Camera.main.ScreenToWorldPoint(screenStartPoint);
        transform.position = startPoint; // Set the moon's position to the start point
        Vector3 screenEndPoint = new Vector3(Screen.width, Screen.height / 2f, 0);
        endPoint = Camera.main.ScreenToWorldPoint(screenEndPoint);
        AnimateMoon();
    }
    
    // Animate the moon along a path
    private void AnimateMoon()
    {
        // Create a path with an arc
        Vector3[] path = new Vector3[]
        {
            startPoint,
            new Vector3((startPoint.x + endPoint.x) / 3, startPoint.y + 2, (startPoint.z + endPoint.z) / 3),
            endPoint
        };

        // Animate the moon along the path
        transform.DOPath(path, animationDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLoops(0, LoopType.Restart);
    }
}
