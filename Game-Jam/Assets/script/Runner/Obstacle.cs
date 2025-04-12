using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject trigger;
    public bool shouldBeDestroyed;
    
    [SerializeField] private List<Sprite> sprites;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Count)];
        gameObject.AddComponent<PolygonCollider2D>();
        float random = UnityEngine.Random.Range(-360f, 360f);
        transform.DORotate(new Vector3(0, 0, random), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart);
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == trigger)
        {
            shouldBeDestroyed = true;
        }
    }
}
