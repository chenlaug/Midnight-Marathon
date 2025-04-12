using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SeparateGameManager : MonoBehaviour
{
    public static SeparateGameManager Instance;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private Vector2 inputPosition;
    [SerializeField] private float selectionValue;

    [Header("Game Objects")]
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private List<GameObject> objectsToSpawn;
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject objectSpawnPoint;
    [SerializeField] private GameObject groundToSpawn;
    [SerializeField] private GameObject groundParent;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private Camera cam;
    public HidePhone hidePhoneScript;
    [SerializeField] private GameObject changeMiniGame;

    [Header("Time")]
    [SerializeField] private float time;
    public float defaultTime;
    [SerializeField] private float interval;
    [SerializeField] private float intervalTime;

    [Header("Status")] 
    public bool isGameRunning;
    [SerializeField] private bool isGameOver;
    
    public enum ElementType
    {
        Left,
        Right
    }
    
    private void OnEnable()
    {
        holdAction.action.Enable();
        positionAction.action.Enable();
    }
    
    private void OnDisable()
    {
        holdAction.action.Disable();
        positionAction.action.Disable();
    }
    
    private void Awake()
    {
        Instance = this;
        
        if (hidePhoneScript == null)
        {
            hidePhoneScript = FindObjectOfType<HidePhone>();
        }

        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script not found in the scene!");
        }
        holdAction.action.Enable();
        positionAction.action.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        objectsToSpawn = Resources.LoadAll<GameObject>("Separate/Objects").ToList();
        groundToSpawn = Resources.Load<GameObject>("Separate/Ground/Ground");
        FitBordersToScreen();
        InitializeGrounds();
        time = defaultTime;
        isGameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        GamePaused();
        if (isGameRunning)
        {
            InstantiateObject();
            DecreaseTime();
            CheckObjectDestruction();
            EndOfGame();
            GetInput();
            SelectObject();
            if (selectedObject != null)
            {
                selectedObject.GetComponent<TrailRenderer>().emitting = true;
                selectedObject.transform.position = cam.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, cam.nearClipPlane));
            }
        }
    }

    private void GetInput()
    {
        inputPosition = positionAction.action.ReadValue<Vector2>();
        selectionValue = holdAction.action.ReadValue<float>();
        if (selectionValue == 0f) selectedObject = null;
    }

    private void SelectObject()
    {
        if (selectionValue != 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(inputPosition), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<Object>() != null)
            {
                selectedObject = objects.FirstOrDefault(x => x == hit.collider.gameObject);
            }
        }
    }
    
    // Decrease the time
    private void DecreaseTime()
    {
        if (time <= 0) return;
        time -= Time.deltaTime;
    }

    // Check if the object should be destroyed
    private void CheckObjectDestruction()
    {
        foreach (var spawnedObject in objects.Where(spawnedObject => spawnedObject.GetComponent<Object>().shouldBeDestroyed).ToList())
        {
            objects.Remove(spawnedObject);
            Destroy(spawnedObject);
        }
    }
    
    // End of the game
    private void EndOfGame()
    {
        if (time <= 0f) // if the time is up
        {
            foreach (var unfinishedObject in objects)
            {
                unfinishedObject.GetComponent<Rigidbody2D>().simulated = false; // stop the object from moving
            }
            SaveScore.Instance.IncrementScore(100);
            isGameRunning = false;

            foreach (var objectToKill in objects.ToList())
            {
                objects.Remove(objectToKill);
                Destroy(objectToKill);
            }
            
            changeMiniGame.GetComponent<ChangeMinigame>().OnGameOver();
        }
    }

    // Manage the score
    public void ScoreManager(bool increment)
    {
        if (increment) SaveScore.Instance.IncrementScore(100);
        else SaveScore.Instance.IncrementScore(-100);
    }

    // Fit the borders to the screen
    private void FitBordersToScreen()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // get the camera height
        float cameraWidth = cameraHeight * screenAspect; // get the camera width

        var isWallLeft = true;

        foreach (var wall in walls)
        {
            if (isWallLeft)
            {
                wall.transform.position = new Vector3(-cameraWidth / 2, 0, 0);
                isWallLeft = false;
            }
            else
            {
                wall.transform.position = new Vector3(cameraWidth / 2, 0, 0);
            }
        }
    }
    
    // Instantiate the object
    private void InstantiateObject()
    {
        if (intervalTime >= interval)
        {
            int randomIndex = UnityEngine.Random.Range(0, objectsToSpawn.Count); // get a random index from the objects to spawn list
            GameObject obj = Instantiate(objectsToSpawn[randomIndex], objectSpawnPoint.transform.position, Quaternion.identity, objectParent.transform);
            objects.Add(obj);
            intervalTime = 0; // reset the interval time
        }

        if (isGameRunning) intervalTime += Time.deltaTime; // increment the interval time
    }
    
    // Initialize the ground objects
    private void InitializeGrounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // get the camera height
        float cameraWidth = cameraHeight * screenAspect; // get the camera width

        for (int i = 0; i < 2; i++)
        {
            GameObject ground = Instantiate(groundToSpawn, Vector3.zero, Quaternion.identity, groundParent.transform);
            ground.GetComponent<Ground>().GroundType = (ElementType)i;
            ground.transform.localScale = new Vector3(cameraWidth / 2, 0.5f, 1); // set the scale of the ground object
            ground.transform.localPosition = new Vector3((i == 0 ? -cameraWidth / 4 : cameraWidth / 4), -cameraHeight / 2 + ground.transform.localScale.y / 2, 0); // set the position of the ground object
        }
    }

    public void GamePaused()
    {
        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script is not assigned.");
            return;  
        }

        if (hidePhoneScript.isvisble == false)
        {
            isGameRunning = false;
        }
        else if (hidePhoneScript.isvisble == true)
        {
            isGameRunning = true;
        }
    }
    
    public void ResetGame()
    {
        objects.Clear();
        time = defaultTime;
        isGameRunning = true;
    }
}
