using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RunnerGameManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float time;
    public float defaultTime;
    [SerializeField] private float interval;
    [SerializeField] private float intervalTime;

    [Header("Obstacle Game Objects")]
    [SerializeField] private GameObject obstaclesParent;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<GameObject> obstaclesList;
    [SerializeField] private GameObject obstacleTriggerResource;
    [SerializeField] private GameObject obstacleTrigger;
    public HidePhone hidePhoneScript;

    [Header("Obstacle Spawn Points")]
    [SerializeField] private GameObject obstaclesParentSpawnPoint;
    [SerializeField] private List<GameObject> obstaclesSpawnPoints;
    
    [Header("Lanes")]
    [SerializeField] private GameObject laneParent;
    [SerializeField] private List<GameObject> lanes;
    
    [Header("Player")]
    [SerializeField] private GameObject player;
    
    [Header("Gameplay")]
    [SerializeField] private float obstacleSpeed;
    [SerializeField] private Camera cam;
    
    [Header("Status")] 
    [SerializeField] private GameObject changeMiniGame;
    public bool isGameRunning;
    
    // Start is called before the first frame update
    void Start()
    {

        if (hidePhoneScript == null)
        {
            hidePhoneScript = FindObjectOfType<HidePhone>();
        }

        if (hidePhoneScript == null)
        {
            Debug.LogError("HidePhone script not found in the scene!");
        }

        obstacle = Resources.Load<GameObject>("Runner/Obstacle");
        obstacleTriggerResource = Resources.Load<GameObject>("Runner/Trigger");
        SetObstacleTrigger();
        GetObstaclesSpawnPoints();
        GetLanes();
        SetObstaclesSpawnPoints();
        SetLanes();
        player.GetComponent<Player>().SetLanes(lanes);
        time = defaultTime;
        isGameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        GamePaused();
        if (isGameRunning)
        {
            SpawnObstacle();
            DecreaseTime();
            EndOfGame();
        }
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            MoveObstacles();
        }
        else if (!isGameRunning)
        {
            foreach (var obs in obstaclesList)
            {
                obs.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
    }

    // Get the obstacles spawn points
    private void GetObstaclesSpawnPoints()
    {
        foreach (Transform child in obstaclesParentSpawnPoint.transform)
        {
            obstaclesSpawnPoints.Add(child.gameObject);
        }
    }
    
    // Get the lanes
    private void GetLanes()
    {
        foreach (Transform child in laneParent.transform)
        {
            lanes.Add(child.gameObject);
        }
    }
    
    // Set the obstacles spawn points position
    private void SetObstaclesSpawnPoints()
    {
        float screenWidth = Screen.width;
        float spacing = screenWidth / obstaclesSpawnPoints.Count;

        for (int i = 0; i < obstaclesSpawnPoints.Count; i++)
        {
            float xPos = (i + 1) * spacing - spacing / 2;
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(xPos, 0, cam.nearClipPlane));
            obstaclesSpawnPoints[i].transform.position = new Vector3(worldPosition.x, obstaclesSpawnPoints[i].transform.position.y, obstaclesSpawnPoints[i].transform.position.z);
        }
    }

    // Set the lanes position
    private void SetLanes()
    {
        float screenWidth = Screen.width;
        float spacing = screenWidth / lanes.Count;

        for (int i = 0; i < lanes.Count; i++)
        {
            float xPos = (i + 1) * spacing - spacing / 2;
            Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(xPos, 0, cam.nearClipPlane));
            lanes[i].transform.position = new Vector3(worldPosition.x, lanes[i].transform.position.y, lanes[i].transform.position.z);
        }
    }

    private void SetObstacleTrigger()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height; // get the screen aspect ratio
        float cameraHeight = cam.orthographicSize * 2; // get the camera height
        float cameraWidth = cameraHeight * screenAspect; // get the camera width
        
        obstacleTrigger = Instantiate(obstacleTriggerResource, new Vector3(0, 0 - cameraHeight / 2 - obstacleTriggerResource.transform.localScale.y, 0), Quaternion.identity, transform);
        obstacleTrigger.transform.localScale = new Vector3(cameraWidth, obstacleTriggerResource.transform.localScale.y, obstacleTriggerResource.transform.localScale.z);
    }
    
    // Spawn the obstacles
    private void SpawnObstacle()
    {
        if (intervalTime <= 0)
        {
            int numberOfObstacles = obstaclesSpawnPoints.Count - 1;
            List<int> usedSpawnPoints = new List<int>();

            for (int i = 0; i < numberOfObstacles; i++)
            {
                int randomObstacleSpawnPoint;
                do
                {
                    randomObstacleSpawnPoint = Random.Range(0, obstaclesSpawnPoints.Count);
                } while (usedSpawnPoints.Contains(randomObstacleSpawnPoint));

                usedSpawnPoints.Add(randomObstacleSpawnPoint);

                Vector3 spawnPosition = new Vector3(obstaclesSpawnPoints[randomObstacleSpawnPoint].transform.position.x, obstaclesSpawnPoints[randomObstacleSpawnPoint].transform.position.y, obstaclesSpawnPoints[randomObstacleSpawnPoint].transform.position.z);
                GameObject obstacleObject = Instantiate(obstacle, spawnPosition, Quaternion.identity, obstaclesParent.transform);
                obstacleObject.GetComponent<Obstacle>().trigger = obstacleTrigger;
                obstaclesList.Add(obstacleObject);
            }

            intervalTime = interval;
        }
        else
        {
            intervalTime -= Time.deltaTime;
        }
    }
    
    private void MoveObstacles()
    {
        for (int i = 0; i < obstaclesList.Count; i++)
        {
            if (obstaclesList[i].GetComponent<Obstacle>().shouldBeDestroyed)
            {
                Destroy(obstaclesList[i]);
                obstaclesList.RemoveAt(i);
                SaveScore.Instance.IncrementScore(50);
                continue;
            }
            Rigidbody2D obsRb = obstaclesList[i].GetComponent<Rigidbody2D>();
            obsRb.velocity = new Vector3(0, -obstacleSpeed, 0);
        }
    }
    
    // Decrease the time
    private void DecreaseTime()
    {
        if (time <= 0)
        {
            isGameRunning = false;
            return;
        }
        time -= Time.deltaTime;
    }
    
    // End of the game
    private void EndOfGame()
    {
        if (time <= 0f) // if the time is up
        {
            isGameRunning = false;
            SaveScore.Instance.IncrementScore(100);
            changeMiniGame.GetComponent<ChangeMinigame>().OnGameOver();
        }
        else if (!player.GetComponent<Player>().isAlive)
        {
            isGameRunning = false;
            SaveScore.Instance.IncrementScore(-100);
            changeMiniGame.GetComponent<ChangeMinigame>().OnGameOver();
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
        foreach (var obstacleToDestroy in obstaclesList.ToList())
        {
            obstaclesList.Remove(obstacleToDestroy);
            Destroy(obstacleToDestroy);
        }
        player.GetComponent<Player>().isAlive = true;
        time = defaultTime;
        isGameRunning = true;
    }
}
