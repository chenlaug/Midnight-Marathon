using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Pour manipuler le composant Image
using UnityEngine.SceneManagement;

public class MomScript : MonoBehaviour
{
    [SerializeField] private Transform Spawn1;
    [SerializeField] private Transform Spawn2;
    [SerializeField] private Transform Spawn3;
    [SerializeField] private Transform InitialPos;  // Position initiale de "mom"
    [SerializeField] private HidePhone hidePhoneScript;  // Référence au script HidePhone pour accéder à isVisible et Room

    int RandomSpawn;
    float SpawnInterval; // Intervalle entre chaque spawn
    float StayDuration;  // Durée de séjour après le spawn
    bool Momtrigger;

    Coroutine mycoroutine;

    // Références aux composants Image des spawns
    [SerializeField] private Image spawn1Image;  // Image de Spawn1
    [SerializeField] private Image spawn2Image;  // Image de Spawn2
    [SerializeField] private Image spawn3Image;  // Image de Spawn3

    // Sprites pour chaque état de spawn
    [SerializeField] private Sprite spawn1IdleImage;  // Image de Spawn1 quand inactif
    [SerializeField] private Sprite spawn2IdleImage;  // Image de Spawn2 quand inactif
    [SerializeField] private Sprite spawn3IdleImage;  // Image de Spawn3 quand inactif
    [SerializeField] private Sprite spawn1ComingSprite;  // Image de Spawn1 avant l'apparition de "Mom"
    [SerializeField] private Sprite spawn2ComingSprite;  // Image de Spawn2 avant l'apparition de "Mom"
    [SerializeField] private Sprite spawn3ComingSprite;  // Image de Spawn3 avant l'apparition de "Mom"
    [SerializeField] private Sprite spawn1ActiveSprite;  // Image de Spawn1 après l'apparition de "Mom"
    [SerializeField] private Sprite spawn2ActiveSprite;  // Image de Spawn2 après l'apparition de "Mom"
    [SerializeField] private Sprite spawn3ActiveSprite;  // Image de Spawn3 après l'apparition de "Mom"

    [SerializeField] private AudioClip Sound;
    private AudioSource audioSource;
    [SerializeField] private AudioClip Sound2;
    private AudioSource audioSource2;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Sound;
        audioSource.playOnAwake = false;

        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = Sound;
        audioSource2.playOnAwake = false;
    }

    private void Start()
    {
        // Démarre le processus de spawn continu
        StartCoroutine(MomSpawnCoroutine());
    }

    private void Update()
    {
        MomKiller();
    }

    IEnumerator MomSpawnCoroutine()
    {
        while (true) // La boucle infinie pour faire réapparaître "mom" en continu
        {
            // Attendre un d�lai al�atoire avant de faire appara�tre "mom"
            SpawnInterval = Random.Range(10, 20); // Intervalle entre deux apparitions al�atoires
            yield return new WaitForSeconds(SpawnInterval); // Attente avant de spawner

            // Choisir le point de spawn où "Mom" va apparaître avant de changer l'image
            RandomSpawn = Random.Range(1, 4); // Nombre aléatoire entre 1 et 3 pour choisir le point de spawn

            // Avant de spawn "mom", changer l'image du point de spawn choisi
            yield return StartCoroutine(ChangeSpawnImageBeforeAppear(RandomSpawn));

            // Appeler la fonction de spawn
            MomSpawn();

            // Durée de séjour aléatoire après le spawn
            StayDuration = Random.Range(3f, 6f); // Durée de séjour aléatoire
            yield return new WaitForSeconds(StayDuration); // Attendre le temps que "mom" reste à sa position

            // Retourner à la position initiale après le temps de séjour
            MoveToInitialPosition(); // Retour à la position initiale
        }
    }

    // Coroutine pour changer l'image du point de spawn avant le spawn de "Mom"
    IEnumerator ChangeSpawnImageBeforeAppear(int spawnChoice)
    {
        // Selon le choix de spawn, change l'image du point de spawn
        switch (spawnChoice)
        {
            case 1:
                if (spawn1Image != null)
                {
                    spawn1Image.sprite = spawn1ComingSprite;  // Changer l'image de Spawn1 avant l'apparition de "Mom"
                    audioSource.Play();
                    yield return new WaitForSeconds(2f); // Attendre 1 seconde
                    spawn1Image.sprite = spawn1IdleImage; // Réinitialiser l'image de Spawn1 à son état original
                }
                break;

            case 2:
                if (spawn2Image != null)
                {
                    spawn2Image.sprite = spawn2ComingSprite;  // Changer l'image de Spawn2 avant l'apparition de "Mom"
                    audioSource.Play();
                    yield return new WaitForSeconds(2f); // Attendre 1 seconde
                    spawn2Image.sprite = spawn2IdleImage; // Réinitialiser l'image de Spawn2 à son état original
                }
                break;

            case 3:
                if (spawn3Image != null)
                {
                    spawn3Image.sprite = spawn3ComingSprite;  // Changer l'image de Spawn3 avant l'apparition de "Mom"
                    audioSource.Play();
                    yield return new WaitForSeconds(2f); // Attendre 1 seconde
                    spawn3Image.sprite = spawn3IdleImage; // Réinitialiser l'image de Spawn3 à son état original
                }
                break;
        }
    }

    public void MomSpawn()
    {
        if (RandomSpawn == 1)
        {
            transform.position = Spawn1.position; // Positionner "mom" sur Spawn1
            if (spawn1Image != null) spawn1Image.sprite = spawn1ActiveSprite; // Mettre l'image de Spawn1 à l'état "occupé"
            audioSource2.Play();
            Momtrigger = true;
        }
        else if (RandomSpawn == 2)
        {
            transform.position = Spawn2.position; // Positionner "mom" sur Spawn2
            if (spawn2Image != null) spawn2Image.sprite = spawn2ActiveSprite; // Mettre l'image de Spawn2 à l'état "occupé"
            audioSource2.Play();
            Momtrigger = true;
        }
        else if (RandomSpawn == 3)
        {
            transform.position = Spawn3.position; // Positionner "mom" sur Spawn3
            if (spawn3Image != null) spawn3Image.sprite = spawn3ActiveSprite; // Mettre l'image de Spawn3 à l'état "occupé"
            audioSource2.Play();
            Momtrigger = true;
        }
    }

    // Fonction pour déplacer "mom" vers sa position initiale
    public void MoveToInitialPosition()
    {
        transform.position = InitialPos.position;  // Déplacer "mom" à la position initiale
        // Réinitialiser l'image des spawns
        if (spawn1Image != null) spawn1Image.sprite = spawn1IdleImage;
        if (spawn2Image != null) spawn2Image.sprite = spawn2IdleImage;
        if (spawn3Image != null) spawn3Image.sprite = spawn3IdleImage;

        Momtrigger = false; // Assurez-vous que Momtrigger soit réinitialisé après qu'elle soit retournée à sa position initiale
    }

    // Fonction qui gère la couleur de la Room dans HidePhone en fonction de isVisible et Momtrigger
    public void MomKiller()
    {
        if (Momtrigger && hidePhoneScript != null)
        {

            if (hidePhoneScript.isvisble == true && Momtrigger == true)
            {
                hidePhoneScript.isvisble = false;
                SceneManager.LoadScene("EndMenu");
            }
        }
    }
}
