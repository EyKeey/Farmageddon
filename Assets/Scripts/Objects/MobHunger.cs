using UnityEngine;
using System.Collections;

public class MobHunger : MonoBehaviour
{
    public float eatingTime = 10f;
    public float hungerCooldown = 10f;
    public float moveSpeed;

    [SerializeField] private Animator animator;
    [SerializeField] private MobMovement mobMovement;

    private bool isHungry;
    private bool isEating;
    private float lastEatTime = 0;
    private GameObject chosenGrass;

    private void Start()
    {
        lastEatTime += Time.time - 5;
        moveSpeed = mobMovement.moveSpeed;
    }

    private void Update()
    {
        if (Time.time - lastEatTime > hungerCooldown)
        {
            SetHungry();
        }

        if (isEating) return;

        if (isHungry)
        {
            if (Time.time > lastEatTime + eatingTime)
            {
                HandleHunger();
            }
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
            isHungry = false;
            mobMovement.isMoving = true;
        }
    }

    private void FindGrass()
    {
        GameObject[] grasses = GameObject.FindGameObjectsWithTag("Grass");

        if (grasses.Length > 0)
        {
            // Rastgele bir çim seç
            int randomIndex = Random.Range(0, grasses.Length);
            chosenGrass = grasses[randomIndex];
        }
        else
        {
            Debug.LogWarning("No grass found in the scene!");
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
    }

    public bool IsEating()
    {
        return isEating;
    }
}