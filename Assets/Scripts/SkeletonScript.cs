using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : EnemyScript
{    
    // Start is called before the first frame update
    void Start()
    {
        enemySFX = GameObject.Find("SkeletonHit_SFX").GetComponent<AudioSource>();
        enemyHealth = 1;
        enemyMovementSpeed = 10f;
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