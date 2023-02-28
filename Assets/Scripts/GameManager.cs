using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // player
    public float maxHealth = 100.0f;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void applyDamage(float damage) {
        currentHealth = currentHealth - damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log(currentHealth);

        // DEATH LOGIC 
    }
}
