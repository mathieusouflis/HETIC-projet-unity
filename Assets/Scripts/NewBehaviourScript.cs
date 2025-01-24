using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
    }
}

public class ExplodingBarrel : MonoBehaviour
{
    public float explosionRadius = 5.0f;
    public int damageCloseRange = 100;
    public int damageFarRange = 50;
    public int maxHealth = 50;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ApplyDamage(10);
        }
    }

    private void ApplyDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in affectedObjects)
        {
            PlayerHealth player = nearbyObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= explosionRadius / 2)
                {
                    player.ReceiveDamage(damageCloseRange);
                }
                else
                {
                    player.ReceiveDamage(damageFarRange);
                }
            }
        }

        Destroy(gameObject);
    }
}

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100;

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        Debug.Log("The player has died.");
    }
}
