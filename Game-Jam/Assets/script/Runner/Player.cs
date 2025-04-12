using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public bool isAlive;
    
    [Header("Lanes")]
    [SerializeField] private List<GameObject> lanes;
    [SerializeField] private int currentLane;
    
    [Header("Input Actions")]
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference positionAction;
    [SerializeField] private Vector2 inputPosition;
    [SerializeField] private float selectionValue;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 currentPosition;
    [SerializeField] private bool isSwiping;
    private bool moveIsDone;
    private Vector3 targetPosition;
    
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
        holdAction.action.Enable();
        positionAction.action.Enable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
    }
    
    private void GetInput()
    {
        if (holdAction.action.ReadValue<float>() > 0)
        {
            if (!isSwiping)
            {
                startPosition = positionAction.action.ReadValue<Vector2>();
                isSwiping = true;
            }
            currentPosition = positionAction.action.ReadValue<Vector2>();
        }
        else
        {
            isSwiping = false;
        }
    }

    private void Move()
    {
        if (!isAlive || !isSwiping) return;

        float swipeDistance = currentPosition.x - startPosition.x;
        float swipeThreshold = Screen.width * 0.1f; // Adjust the threshold as needed

        if (Mathf.Abs(swipeDistance) > swipeThreshold && !moveIsDone)
        {
            if (swipeDistance > 0 && currentLane < lanes.Count - 1)
            {
                currentLane++;
            }
            else if (swipeDistance < 0 && currentLane > 0)
            {
                currentLane--;
            }
            targetPosition = new Vector3(lanes[currentLane].transform.position.x, lanes[currentLane].transform.position.y, transform.position.z);
            moveIsDone = true;
        }
        else if (Mathf.Abs(swipeDistance) < swipeThreshold)
        {
            moveIsDone = false;
        }

        // Smoothly interpolate to the target position
        transform.DOMove(lanes[currentLane].transform.position, 0.4f).SetEase(Ease.OutBack);
    }
    
    public void SetLanes(List<GameObject> _lanes)
    {
        lanes = new List<GameObject>();
        lanes = _lanes;
        transform.position = new Vector3(lanes[currentLane].transform.position.x, lanes[currentLane].transform.position.y, transform.position.z);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        isAlive = false;
        Debug.Log("Collision detected");
    }
}
