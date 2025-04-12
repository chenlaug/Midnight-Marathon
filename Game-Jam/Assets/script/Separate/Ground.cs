using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    
    [SerializeField] private SeparateGameManager.ElementType groundType;
    public SeparateGameManager.ElementType GroundType
    {
        get => groundType;
        set => groundType = value;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Set the color of the object based on its type
    private void SetColor()
    {
        if (GroundType == SeparateGameManager.ElementType.Left) // if the object is of type Left
        {
            GetComponent<SpriteRenderer>().sprite = sprites[0]; // set the sprite to the first sprite in the list
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1]; // set the sprite to the first sprite in the list
        }
    }
}
