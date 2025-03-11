using UnityEngine;
using System.Collections;

public class MobHunger : MonoBehaviour
{
    public float eatingTime = 10f;
    public float hungerCooldown = 10f;
    public float moveSpeed;
    public float maxHungerTime = 10f;   

    [SerializeField] private Animator animator;
    [SerializeField] private MobMovement mobMovement;

    private bool isHungry;
    private bool isEating;
    private float lastEatTime = 0;
    private float hungerStartTime = 0;
    private GameObject chosenGrass;

    private void Start()
    {
        lastEatTime = Time.time - 5;
        moveSpeed = mobMovement.moveSpeed;
        SetHungry();
    }

    private void Update()
    {
        if (Time.time - lastEatTime > hungerCooldown && !isHungry)
        {
            SetHungry();
        }

        if (isEating) return;

        if (isHungry)
        {
            
            if (Time.time - hungerStartTime > maxHungerTime)
            {
                gameObject.GetComponent<MobMovement>().StartDieAnimation();
                return;
            }

            HandleHunger();
        }
    }

    private void HandleHunger()
    {
        if (chosenGrass == null)
        {
            FindGrass();
        }

        if (chosenGrass != null)
        {
            Vector2 grassPosition = chosenGrass.transform.position;
            mobMovement.Move(grassPosition);

            float distanceToGrass = Vector2.Distance(transform.position, grassPosition);
            if (distanceToGrass < 0.1f)
            {
                StartCoroutine(EatGrassSequence());
            }
        }
        else
        {
            mobMovement.isMoving = true;
        }
    }

    private void FindGrass()
    {
        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");

        if (grasses.Length > 0)
        {
            int randomIndex = Random.Range(0, grasses.Length);
            chosenGrass = grasses[randomIndex];
        }
        else
        {
            chosenGrass = null;
        }
    }

    private IEnumerator EatGrassSequence()
    {
        isEating = true;
        mobMovement.isMoving = false;

        if (animator != null)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Eating", true);
        }

        yield return new WaitForSeconds(eatingTime);

        if (chosenGrass != null)
        {
            Destroy(chosenGrass);
        }

        if (animator != null)
        {
            animator.SetBool("Eating", false);
        }

        MobItemSpawn m_itemSpawn = GetComponent<MobItemSpawn>();
        m_itemSpawn.WaitForSpawn();

        lastEatTime = Time.time;
        isHungry = false;
        isEating = false;
        mobMovement.isMoving = true;
        chosenGrass = null;
    }

    public void SetHungry()
    {
        isHungry = true;
        hungerStartTime = Time.time;
        
    }

    public bool IsEating()
    {
        return isEating;
    }

    
}
