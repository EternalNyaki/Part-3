using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public int maxHealth = 100;

    [DoNotSerialize] public int currentHealth { get; protected set; }
    protected bool dead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        if(!dead)
        {
            currentHealth -= damage;
            Debug.Log("Health: " + currentHealth.ToString());
            if (currentHealth <= 0)
            {
                Die();
                dead = true;
            }
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Bleh");
        Destroy(gameObject);
    }
}
