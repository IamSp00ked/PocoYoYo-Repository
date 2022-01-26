using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private float speed = 1000f;
    public int damage = 10;
    public float shootStrength;
    [HideInInspector] public AnimalEnemy aE;
    [HideInInspector] public PlayerScript pS;

    void Start()
    {
        pS = FindObjectOfType<PlayerScript>();
        aE = FindObjectOfType<AnimalEnemy>();
        aE.pushStrength = shootStrength;
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Destroy the spike
        Destroy(gameObject);

        // Try to get health component from the player
        PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
        Health health1 = other.gameObject.GetComponent<Health>();

        // Check if there is a health component and will take damage to it
        if (health1)
        {
            health1.TakeDamage(damage);
        }
        // Push the player with a force
        if (other.gameObject.tag == "Player")
        {
            pS.rb.AddForce(transform.forward * aE.pushStrength, ForceMode.VelocityChange);
            if (health)
            {
                health.TakeDamage(damage);
            }
        }
    
    }
}
