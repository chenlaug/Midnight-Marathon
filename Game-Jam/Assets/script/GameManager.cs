using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float time;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncrementTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator IncrementTime()
    {
        while (true)
        {
            yield return null;
            time += Time.deltaTime;
        }
    }
}
