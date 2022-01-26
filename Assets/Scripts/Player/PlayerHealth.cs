using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public HealthBarScript healthBar;
    [HideInInspector] public PlayerScript pS;
    void Start()
    {
        // Setting max health
        healthBar.SetMaxHealth(maxHealth);
        pS = FindObjectOfType<PlayerScript>();
    }
    public override void TakeDamage(int damage, float knockbackForce=0f,GameObject damageSender=null)
    {

        health -= damage;

        if (health <= 0)
        {
            //Debug.Log("Death");
            //Destroy(gameObject);
        }
        healthBar.SetHealth(health); // Slider is set to health everytime you take damage

    }
    void Update()
    {
        healthBar.SetHealth(health);
    }
}
