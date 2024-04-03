using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Slider healthBar;

    private static EnemyCombat enemyInstance;

    // Update is called once per frame
    void Update()
    {
        if(enemyPrefab != null)
        {
            if(enemyInstance == null)
            {
                enemyInstance = Instantiate(enemyPrefab).GetComponent<EnemyCombat>();
            }

            healthBar.value = (float)enemyInstance.currentHealth / enemyInstance.maxHealth;
        }
    }
}
