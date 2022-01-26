using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    public int health = 100;
    public bool healthCheck;
    public int maxHealth = 100;
    void Start()
    {
        
    }
    public virtual void TakeDamage(int damage, float knockbackForce=0f, GameObject damageSender=null)
    {
        health -= damage;

        if (health <= 0)
        {
            // When enemy health = 0 enemy die
            // Entity dies
            Debug.Log(gameObject.name + " died");
        }

        ApplyKnockback(knockbackForce, damageSender);
    }

    void Update()
    {
        if (health >= maxHealth)
        {
            HealthUpdater(); // Updates health when taken damage
        }
        else if (health <= maxHealth)
        {
            healthCheck = false;
        }
    }
    public virtual void HealthUpdater()
    {
        health = maxHealth;
    }
    public void ApplyKnockback(float force, GameObject damageSender)
    {
        Debug.Log("Sends them flying");
        if(GetComponent<AnimalEnemy>()==null)
        {
            //GetComponent<NavMeshAgent>().enabled;
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(/*damageSender.transform.forward * force*/new Vector3(0f,force,0f), ForceMode.VelocityChange);
        }
    }
}
