using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeClock : MonoBehaviour
{
    [SerializeField] 
    private Transform hourBar;
    [SerializeField]
    private Transform minuteBar;

    private const float secondsPerMinute = 60.0f;
    private const float secondsPerHour = 720.0f;

    private Quaternion initialHourRotation;   // Rotation de base de la barre des heures
    private Quaternion initialMinuteRotation; // Rotation de base de la barre des minutes

    void Start()
    {
        // Enregistrer la rotation initiale
        if (hourBar != null)
            initialHourRotation = hourBar.localRotation;
        if (minuteBar != null)
            initialMinuteRotation = minuteBar.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;

        float minuteAngle = (time % secondsPerMinute) / secondsPerMinute * 360;
        float hourAngle = (time % secondsPerHour) / secondsPerHour * 360;

        if (minuteBar != null)
            minuteBar.localRotation = initialMinuteRotation * Quaternion.Euler(0f, 0f, -minuteAngle);

        if (hourBar != null)
            hourBar.localRotation = initialHourRotation * Quaternion.Euler(0f, 0f, -hourAngle);
    }
}
