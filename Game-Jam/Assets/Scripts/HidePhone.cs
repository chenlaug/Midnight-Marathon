using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidePhone : MonoBehaviour
{
    public GameObject Room;
    [SerializeField] RawImage dupliCamera;
    public bool isvisble = true;

    void Start()
    {
        // Mettre � jour isvisble en fonction de l'�tat actuel de Room
        isvisble = Room.activeInHierarchy;
    }

    void Update()
    {
        // Vous pouvez ajouter d'autres logiques ici si n�cessaire
    }

    public void changeVisibilityOnClick()
    {
        if (Room.activeInHierarchy == true)
        {
            dupliCamera.gameObject.SetActive(false);
            Room.SetActive(false);
            isvisble = false;
        }
        else
        {
            dupliCamera.gameObject.SetActive(true);
            Room.SetActive(true);
            isvisble = true;
        }
    }
}
