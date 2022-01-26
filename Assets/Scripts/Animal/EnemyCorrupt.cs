using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorrupt : AnimalEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        animalTarget = GameObject.Find("sheep").transform;
        originalSpeed = 5f;
        pR = FindObjectOfType<PlayerHealth>();
        pS = FindObjectOfType<PlayerScript>();
        h = FindObjectOfType<Health>();

    }

    // Update is called once per frame
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
        if (animalInSightRange && !animalInAttackRange)
        {
            ChaseAnimalTarget();
        }
        else if (targetInsightRange && !targetInAttckRange)
        {
            ChaseTarget();
        }
        // If the enemy are in sight of the target and in range of attck, the enemy will attack
        if (animalInSightRange && animalInAttackRange)
        {
            AttackAnimalTarget();
        }
        else if (targetInsightRange && targetInAttckRange)
        {
            AttackTarget();
        }
    }
}
