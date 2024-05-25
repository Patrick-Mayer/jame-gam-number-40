using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : EnemyScript
{    
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 5;
        enemyMovementSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(nonAggressive){
            // Check if the timer is a whole number
            if (Mathf.Approximately(GameManager.instance.globalTimer % 1f, 0f))
            {
                Walk();
            }else{
                Move();
            }
        }
    }
};