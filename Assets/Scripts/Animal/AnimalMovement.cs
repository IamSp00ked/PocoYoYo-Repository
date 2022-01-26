using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AnimalMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    public LayerMask whatIsGround;

    // Walk 
    public Vector3 walkPos;
    public float walkPosRange;
    public float rangeOfSet;
    [HideInInspector]public float knockbackReset=1;
    public bool knockbackCDActive;
    [HideInInspector] public float walkPosRangeX, walkPosRangeZ;

    private bool walkPosSet;
    private float walkingCooldown;
    private bool beingPulled;

    [HideInInspector] public float originalSpeed;

    // Cage
    public BoxCollider cage;
    [HideInInspector] public bool inCage = false;

    public Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        originalSpeed = agent.speed;
        walkPosRangeX = walkPosRange;
        walkPosRangeZ = walkPosRange;
    }

    private void Update()
    {
        Walking();
        animator.SetFloat("isWalking", agent.velocity.magnitude);
        if (!GetComponent<NavMeshAgent>().enabled && GetComponent<Rigidbody>().velocity == Vector3.zero && !beingPulled)
        {
            if (!knockbackCDActive)
            {
                StartCoroutine(ResetNavmeshAgent());
            }
        }
    }


    public void Walking()
    {
        if (!walkPosSet)
        {
            SearchWalkPos();
        }

        if (walkPosSet && GetComponent<NavMeshAgent>().enabled)
        {
            if (agent.SetDestination(walkPos) == true)
            {
                agent.SetDestination(walkPos);
            }
        }

        Vector3 distanceToWalkPos = transform.position - walkPos;

        // Enemy has reached the randomized position, and gets a velocity of 0
        if (distanceToWalkPos.magnitude < 2f)
        {
            walkPosSet = false;
            // Delays the next SearchWalkPos
            agent.speed = 0;
            StartCoroutine(SetEnemyMoveDelay(SetWalingCooldown()));

        }
    }

    private void SearchWalkPos()
    {
        // Calculate Random point in range
        float randomZ = Random.Range(-walkPosRangeZ + rangeOfSet, walkPosRangeZ - rangeOfSet);
        float randomX = Random.Range(-walkPosRangeX + rangeOfSet, walkPosRangeX - rangeOfSet);
        if (!inCage)
        {
            walkPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        }
        else
        {
            walkPos = new Vector3(cage.bounds.center.x + randomX, transform.position.y, cage.bounds.center.z + randomZ);
        }

        // Checks if the pos are on the ground(not of the map)
        if (Physics.Raycast(walkPos, -transform.up, 2f, whatIsGround))
        {
            walkPosSet = true;
        }
    }

    /// <summary>
    /// Sets a delay for the enemy to get its original speed back
    /// </summary>
    /// <param name="time"></param>the amount of time that the delay will regard
    /// <returns></returns>the delay to wait
    private IEnumerator SetEnemyMoveDelay(float time)
    {
        yield return new WaitForSeconds(time);
        agent.speed = originalSpeed;
    }

    private float SetWalingCooldown()
    {
        return walkingCooldown = Random.Range(2, 16);
        //walkingCooldown = 0f;
    }

    public IEnumerator ResetNavmeshAgent()
    {
        knockbackCDActive = true;
        yield return new WaitForSeconds(knockbackReset);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<NavMeshAgent>().enabled = true;
        knockbackCDActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cage"))
        {
            inCage = true;
            walkPosRangeX = other.bounds.size.x;
            walkPosRangeZ = other.bounds.size.z;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cage"))
        {
            inCage = false;
            walkPosRangeX = walkPosRange;
            walkPosRangeZ = walkPosRange;
        }
    }

    // Draw the walk pos range
    private void OnDrawGizmosSelected()
    {
        if (!inCage)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, walkPosRange);
        }
        else 
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(cage.bounds.center, walkPosRangeX - rangeOfSet);
            Gizmos.DrawWireSphere(cage.bounds.center, walkPosRangeZ - rangeOfSet);
        }
    }
}
