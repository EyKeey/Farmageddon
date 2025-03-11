using System.Collections;
using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxDistance = 10f;

    [SerializeField] public PolygonCollider2D boundaryArea;
    [SerializeField] private Animator animator;

    private Vector2 targetPosition;
    private SpriteRenderer spriteRenderer;
    private Bounds bounds;
    public bool isMoving;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        // Ýlk konumu sýnýr içinde belirle
        transform.position = GetRandomPointInsideBoundary();
        ChooseNewTarget();
    }

    private void OnEnable()
    {
        animator.enabled = true;
        if (!isMoving)
        {
            ChooseNewTarget();
        }
    }

    private void OnDisable()
    {
        animator.enabled = false;
    }

    private void InitializeComponents()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (boundaryArea == null)
        {
            boundaryArea = GameObject.Find("Area").GetComponent<PolygonCollider2D>();
        }
        if (boundaryArea != null)
        {
            bounds = boundaryArea.bounds;
        }
        else
        {
            Debug.LogWarning("No boundary area found!");
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            Move(targetPosition);
            CheckTargetReached();
        }
    }

    public void Move(Vector2 targetPos)
    {
        targetPosition = targetPos;

        Vector2 currentPosition = transform.position;
        transform.position = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // Flip sprite based on movement direction
        spriteRenderer.flipX = (currentPosition.x - targetPosition.x) < 0;

        // Set movement animation
        if (animator != null)
        {
            animator.SetBool("Walking", true);
        }
    }

    private void CheckTargetReached()
    {
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (animator != null)
            {
                animator.SetBool("Walking", false);
            }
            ChooseNewTarget();
        }
    }

    public void StartDieAnimation()
    {
        Stopped();
        animator.SetTrigger("Die");
    }

    public void Stopped()
    {
        isMoving = false;
        animator.SetBool("Walking", false);
    }

    private void ChooseNewTarget()
    {
        targetPosition = GetRandomPointInsideBoundary();
        isMoving = true;
    }

    private Vector2 GetRandomPointInsideBoundary()
    {
        Vector2 randomPoint;
        int maxTries = 10; // Sonsuz döngüye girmemesi için maksimum deneme sayýsý
        int attempts = 0;

        do
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            randomPoint = new Vector2(randomX, randomY);
            attempts++;
        }
        while (!boundaryArea.OverlapPoint(randomPoint) && attempts < maxTries);

        return randomPoint;
    }

    private void LateUpdate()
    {
        if (boundaryArea != null)
        {
            float newX = Mathf.Clamp(transform.position.x, bounds.min.x, bounds.max.x);
            float newY = Mathf.Clamp(transform.position.y, bounds.min.y, bounds.max.y);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }
}
