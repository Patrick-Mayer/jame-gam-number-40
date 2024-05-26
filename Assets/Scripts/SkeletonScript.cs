using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : EnemyScript
{
    [SerializeField] private GameObject skeletonObj;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 1;
        enemySFX = GameObject.Find("SkeleHit_SFX").GetComponent<AudioSource>();
        enemyMovementSpeed = 10f;
        playerObj = GameObject.Find("Player");

        sightRange = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            summonTimer += Time.deltaTime;

            Vector3 raisedPosition = transform.position;

            float yDiff = (-0.3f - (-8f));

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
            isSpawning = false;
            summonTimer = 0f;
        }
    }
};
