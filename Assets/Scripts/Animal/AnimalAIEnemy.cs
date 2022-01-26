using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;

public class AnimalAIEnemy : AnimalAIMovement
{
    public LayerMask whatIsTarget;
    public LayerMask whatIsAnimal;


    //Attack
    public float attackRange;

    //Chase
    public float sightRange;
    private Transform animalTarget;
    private Transform target;
    public LayerMask whatIsObstacle;

    public int targetLayerIndex;
   
    private bool targetInSight=false;
    public RaycastHit[] sightHit;


    [HideInInspector] public bool targetInsightRange, targetInAttckRange;
    [HideInInspector] public bool animalInSightRange, animalInAttackRange;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        animalTarget = GameObject.Find("sheep").transform;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        // Checks for sight and attack range
        animalInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsAnimal);
        animalInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsAnimal);
        targetInsightRange = Physics.CheckSphere(transform.position, sightRange, whatIsTarget);
        targetInAttckRange = Physics.CheckSphere(transform.position, attackRange, whatIsTarget);
        // If Enemy are not in sight of Target, the enemy will Walk
        if (!animalInSightRange && !animalInAttackRange || !targetInsightRange && !targetInAttckRange)
        {
            Walk();
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
    }
    public void ChaseTarget()
    {
        RaycastHit hit;
        // If nothing is in between Enemy and target

        if (!Physics.Linecast(transform.position, target.position, out hit, whatIsObstacle))
        {
            Debug.Log("Chase");
        }
        // If something is between the enemy and target
        else
        {
            Walk();
            //Debug.Log("Walk");
        }
    }
    public void ChaseAnimalTarget()
    {

    }

    public void AttackTarget()
    {

    }

    public void AttackAnimalTarget()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
