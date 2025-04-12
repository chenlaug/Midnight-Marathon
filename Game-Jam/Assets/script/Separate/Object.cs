using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Object : MonoBehaviour
{

    [SerializeField] private SeparateGameManager.ElementType objectType;
    public SeparateGameManager.ElementType ObjectType
    {
        get => objectType;
        set => objectType = value;
    }
    public bool isOnCorrectGround;
    public bool isInTheAir;
    public bool shouldBeDestroyed;
    
    // Start is called before the first frame update
    void Start()
    {
        isOnCorrectGround = false;
        isInTheAir = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ground>() == null) return; // if the object collides with a ground
        if (collision.gameObject.GetComponent<Ground>().GroundType == objectType)
        {
            isOnCorrectGround = true; // if the object is on the correct ground
            SeparateGameManager.Instance.ScoreManager(true); // increment the score
        }
        else
        {
            SeparateGameManager.Instance.ScoreManager(false); // decrement the score
        }
        isInTheAir = false;
        GetComponent<Rigidbody2D>().simulated = false; // stop the object from moving
        transform.DOShakePosition(0.2f, new Vector3(0.1f, 0.1f, 0f), 5, 90, false, true);
        FadeOut(); // shake the object // fade out the object
    }
    
    // Fade out the object then destroy it
    private void FadeOut()
    {
        GetComponent<SpriteRenderer>().DOFade(0, 0.5f).OnComplete(() => shouldBeDestroyed = true); // fade out the object and destroy it
    }
}
