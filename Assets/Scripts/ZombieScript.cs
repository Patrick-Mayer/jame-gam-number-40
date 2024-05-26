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
        playerObj = GameObject.Find("Player");

        enemySFX = GameObject.Find("ZombVampHit_SFX").GetComponent<AudioSource>();

        //FlashRed();
    }

    // Update is called once per frame
    void Update()
    {
        //FlashRed();

        if (isSpawning)
        {
            summonTimer += Time.deltaTime;

            Vector3 raisedPosition = transform.position;

            float yDiff = (-1.5f - (-8f));

            // inner expression after y diff is 1 when accumulated time equals summonLength
            float yIncrement = (yDiff * (Time.deltaTime / NecromancerScript.summonLength));
            raisedPosition.y += yIncrement;

            transform.position = raisedPosition;
        }
        else
        {
            Move();
        }
        
        if (summonTimer >= NecromancerScript.summonLength)
        {
            //Debug.Log("enemy can move");
            summonTimer = 0f;
            isSpawning = false;
        }
    }
}
