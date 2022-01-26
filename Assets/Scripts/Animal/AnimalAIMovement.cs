using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAIMovement : MonoBehaviour
{
    public float walkingSpeed;
    public bool inCage;
    public float walkPosRange;
    public float rangeOfSet;
    public bool walkPosSet;
    public BoxCollider cage;
    public LayerMask whatIsGround;
    public float rotationSpeed;
    public Animator animator;
    public Rigidbody rb;

    private float walkingCooldown;
    private float originalSpeed;
    private Vector3 distanceToTarget;
    public Vector3 walkPos;
    private float walkPosRangeZ, walkPosRangeX;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        walkPosRangeX = walkPosRange;
        walkPosRangeZ = walkPosRange;
        originalSpeed = walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = rb.position - walkPos;
        //Debug.Log(distanceToTarget.magnitude);
    }
    private void FixedUpdate()
    {
        Walk();
        /*
        if(walkPosSet)
        {

            Vector3 direction =walkPos -transform.position;
            Quaternion toRotation = Quaternion.LookRotation(transform.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x,toRotation.y,transform.rotation.z)), rotationSpeed * Time.fixedDeltaTime);
        }
        */
        if(walkPosSet)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(walkPos-transform.position, Vector3.up),rotationSpeed);
    }

    public void Walk()
    {
        if(!walkPosSet)
        {
            SearchWalkPos();
        }
        // THIS DID NOT WORK!!!
        //rb.MovePosition(rb.position);
        rb.MovePosition(rb.position + (transform.forward * walkingSpeed * Time.fixedDeltaTime));
        //rb.rotation= Quaternion.Lerp(rb.rotation, Quaternion.Euler(new Vector3(rb.rotation.x,Mathf.Atan2(rb.rotation.z-walkPos.z, rb.rotation.x - walkPos.x),rb.rotation.z)),rotationSpeed);
        //rb.rotation= Quaternion.Lerp(rb.rotation,Quaternion.Euler)
        if(distanceToTarget.magnitude<2f)
        {
            walkPosSet = false;
            walkingSpeed = 0f;
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
    private IEnumerator SetEnemyMoveDelay(float time)
    {
        yield return new WaitForSeconds(time);
        walkingSpeed = originalSpeed;
    }

    private float SetWalingCooldown()
    {
        return walkingCooldown = Random.Range(2, 16);
        //walkingCooldown = 0f;
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
