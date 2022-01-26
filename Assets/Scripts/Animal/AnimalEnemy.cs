using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AnimalEnemy : AnimalMovement
{
    public LayerMask whatIsPlayer;
    public LayerMask whatIsAnimal;

    // Attacking
    public float attackCd;
    public bool alreadyAttacked;
    public float attackRange;
    [HideInInspector] public int damage = 10;
    public float pushStrength = 10f;
    [HideInInspector] public PlayerHealth pR;
    [HideInInspector] public PlayerScript pS;
    [HideInInspector] public Health h;

    // Chase
    public Transform target;
    public Transform animalTarget;
    public LayerMask whatIsObstacle;

    public float sightRange = 20;
    [HideInInspector] public bool targetInsightRange, targetInAttckRange;
    [HideInInspector] public bool animalInSightRange, animalInAttackRange;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        animalTarget = GameObject.Find("sheep").transform;
        originalSpeed = 5f;
        pR = FindObjectOfType<PlayerHealth>();
        pS = FindObjectOfType<PlayerScript>();
        h = FindObjectOfType<Health>();
    }

    void Update()
    {
        // Checks for sight and attack range
        animalInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsAnimal);
        animalInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsAnimal);
        targetInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        targetInAttckRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        // If Enemy are not in sight of Target, the enemy will Walk
        if (!animalInSightRange && !animalInAttackRange || !targetInsightRange && !targetInAttckRange)
        {
            Walking();
        }
        // If Enemy are in sight of target but not in range of attack, the enemy will chase the target
        if (targetInsightRange && !targetInAttckRange)
        {
            ChaseTarget();
        }
        else if (animalInSightRange && !animalInAttackRange)
        {
            ChaseAnimalTarget();
        }
        // If the enemy are in sight of the target and in range of attck, the enemy will attack
        if (targetInsightRange && targetInAttckRange)
        {
            AttackTarget();
        }
        else if (animalInSightRange && animalInAttackRange)
        {
            AttackAnimalTarget();
        }
        animator.SetFloat("isWalking", agent.velocity.magnitude);
        // Check if the navmeshAgent has been disabled and then enable it
        if (!GetComponent<NavMeshAgent>().enabled && GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            if(!knockbackCDActive)
            {
                StartCoroutine(ResetNavmeshAgent());
            }
        }
    }
    // Walks towards the target
    public void ChaseTarget()
    {
        RaycastHit hit;
        // If nothing is in between Enemy and target

        if (!Physics.Linecast(transform.position, target.position, out hit, whatIsObstacle))
        {
            //Debug.Log("Chase");
            if (GetComponent<NavMeshAgent>().enabled)
                agent.SetDestination(target.position);
            agent.speed = originalSpeed;
        }
        // If something is between the enemy and target
        else
        {
            Walking();
            //Debug.Log("Walk");
        }

    }
    public void ChaseAnimalTarget()
    {

        RaycastHit hit;
        // If nothing is in between Enemy and animal target
        if (!Physics.Linecast(transform.position, animalTarget.position, out hit, whatIsObstacle))
        {
            if (GetComponent<NavMeshAgent>().enabled)
                agent.SetDestination(animalTarget.position);
            agent.speed = originalSpeed;
        }
        // If something is between the enemy and target
        else
        {
            Walking();
            //Debug.Log("Walk");
        }

    }
    public virtual void AttackTarget()
    {
        // Make sure the enemy doesn't move
        if (GetComponent<NavMeshAgent>().enabled)
            agent.SetDestination(transform.position);

        // Make so that the enemy will look at the target
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));



        if (!alreadyAttacked)
        {
            Debug.Log("Attack");
            pR.TakeDamage(damage);
            pS.rb.AddForce(transform.forward * pushStrength, ForceMode.VelocityChange);
            alreadyAttacked = true;
            // Delays the attack
            Invoke(nameof(ResetAttack), attackCd);
        }
    }
    public virtual void AttackAnimalTarget()
    {
        // Make sure the enemy doesn't move
        if (GetComponent<NavMeshAgent>().enabled)
            agent.SetDestination(transform.position);

        // Make so that the enemy will look at the animal target
        transform.LookAt(new Vector3(animalTarget.position.x, transform.position.y, animalTarget.position.z));



        if (!alreadyAttacked)
        {
            //Debug.Log("Animal Attack");
            h.TakeDamage(damage);
            //pS.rb.AddForce(transform.forward * pushStrength, ForceMode.VelocityChange);
            alreadyAttacked = true;
            // Delays the attack
            Invoke(nameof(ResetAttack), attackCd);
        }
    }

    public void ResetAttack()
    {
        // Makes so we know that its already attackt
        alreadyAttacked = false;
    }
    /*
    private IEnumerator ResetNavmeshAgent()
    {
        knockbackCDActive = true;
        yield return new WaitForSeconds(knockbackReset);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<NavMeshAgent>().enabled = true;
        knockbackCDActive = false;
    }
    */


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
