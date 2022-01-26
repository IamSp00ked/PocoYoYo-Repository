using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AnimalShooterEnemy : AnimalEnemy
{
    public Rigidbody bulletPrefab;
    public Transform[] spawnPos;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        animalTarget = GameObject.Find("sheep").transform;
        originalSpeed = 2f;
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
    }
    public override void AttackTarget()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

        if (!alreadyAttacked)
        {
            //Debug.Log("Shoot");
        

            Shoot();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCd);
        }
    }
    public override void AttackAnimalTarget()
    {
        // Make sure the enemy doesn't move
        agent.SetDestination(transform.position);

        // Make so that the enemy will look at the animal target
        transform.LookAt(new Vector3(animalTarget.position.x, transform.position.y, animalTarget.position.z));



        if (!alreadyAttacked)
        {
            //Debug.Log("Animal Shoot");
            Shoot();
            //pS.rb.AddForce(transform.forward * pushStrength, ForceMode.VelocityChange);
            alreadyAttacked = true;
            // Delays the attack
            Invoke(nameof(ResetAttack), attackCd);
        }
    }

    public void Shoot()
    {
        // Loops through each point to  spawn spike from
        foreach (Transform t in spawnPos)
        {
            // Spawn Spike
            Rigidbody bullet = Instantiate(bulletPrefab, t.position, transform.rotation) as Rigidbody;
        }
    }
}
