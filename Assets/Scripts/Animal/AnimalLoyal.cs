using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalLoyal : AnimalMovement
{

    public LayerMask whatIsPlayer;

    // Follow
    public Transform target;

    // Stop
    private bool isStill = true;

    // Sound
    private bool targetInHearRange;
    public float hearRange;

    private void Start()
    {
        originalSpeed = agent.speed;
        try
        {
            target = GameObject.Find("Player").transform;
        }
        catch
        {

        }
    }

    private void Update()
    {
        // Checks for hearRange
        targetInHearRange = Physics.CheckSphere(transform.position, hearRange, whatIsPlayer);
        animator.SetFloat("isWalking", agent.velocity.magnitude);
        Move();
    }

    public void Move()
    {
        // If the Animal are in the HearRange 
        if (targetInHearRange)
        {

            if (Input.GetButton("Follow") && gameObject.tag == "Loyalty")
            {
                Follow();
            }
            else if (Input.GetButtonDown("Stop") && gameObject.tag == "Loyalty")
            {
                //Debug.Log(isStill);
                if (isStill)
                {
                    Stop();
                    isStill = false;
                    Debug.Log("Stop");
                }
                else if (!isStill)
                {
                    UnStop();
                    isStill = true;
                    Debug.Log("Go");
                }
            }
            else
            {
                Walking();
            }
        }
        else
        {
            Walking();
        }
    }
    private void Follow()
    {
        agent.SetDestination(target.position);
        walkPos = new Vector3(target.position.x, target.position.y, target.position.z);
    }

    private void Stop()
    {
        // Stops the animal
        agent.speed = 0f;

    }
    private void UnStop()
    {
        // Un stop the animal
        agent.speed = originalSpeed;

    }
    private void OnDrawGizmosSelected()
    {
        if (!inCage)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, hearRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, walkPosRange);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, hearRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(cage.bounds.center, walkPosRangeX - rangeOfSet);
            Gizmos.DrawWireSphere(cage.bounds.center, walkPosRangeZ - rangeOfSet);
        }
    }

}