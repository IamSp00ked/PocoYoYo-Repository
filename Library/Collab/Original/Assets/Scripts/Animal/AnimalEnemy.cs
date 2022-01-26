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

    // Attacking
    public float attackCd;
    public static bool alreadyAttacked;
    public float attackRange;
    private int damage = 10;

    // Chase
    public Transform target;

    public LayerMask whatIsObstacle;

    public static float sightRange = 20;
    [HideInInspector] public bool targetInsightRange, targetInAttckRange;
    void Start()
    {
        target = GameObject.Find("Player").transform;
        originalSpeed = 5f;
    }

    void Update()
    {
        // Checks for sight and attack range
        targetInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        targetInAttckRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        // If Enemy are not in sight of Target, the enemy will Walk
        if (!targetInsightRange && !targetInAttckRange)
        {
            Walking();
            Debug.Log("Walk");
        }
        // If Enemy are in sight of target but not in range of attack, the enemy will chase the target
        if (targetInsightRange && !targetInAttckRange)
        {
            ChaseTarget();
        }
        // If the enemy are in sight of the target and in range of attck, the enemy will attack
        if (targetInsightRange && targetInAttckRange)
        {
            AttackTarget();
        }
        animator.SetFloat("isWalking", agent.velocity.magnitude);
    }
    // Walks towards the target
    private void ChaseTarget()
    {

        RaycastHit hit;
        // If nothing is in between Enemy and target
        if (!Physics.Linecast(transform.position, target.position, out hit, whatIsObstacle))
        {
            Debug.Log("Chase");
            agent.SetDestination(target.position);
            //agent.speed = originalSpeed;
        }
        // If something is between the enemy and target
        else
        {
            Walking();
            Debug.Log("Walk");
        }
    }
    private void AttackTarget()
    {
        // Make sure the enemy doesn't move
        agent.SetDestination(transform.position);

        // Make so that the enemy will look at the target
        transform.LookAt(target);



        if (!alreadyAttacked)
        {
            Debug.Log("Attack");
            //PlayerHealth.TakeDamage(damage);
            alreadyAttacked = true;
            // Delays the attack
            Invoke(nameof(ResetAttack), attackCd);
        }
    }

    private void ResetAttack()
    {
        // Makes so we know that its already attackt
        alreadyAttacked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
