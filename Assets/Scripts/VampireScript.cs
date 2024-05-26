using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VampireScript : EnemyScript
{
    private int maxHealth = 3;
    //state machine goes BRRRRRRRRRRRRRRRRRRRR
    //Moves, then TPs, then cooldown, then dashes-attacks towards player, then cooldown, then repeat
    public enum VampireState
    {
        MOVING,
        TELEPORTING,
        FIRST_COOLDOWN,
        ATTACK,
        SECOND_COOLDOWN,
    }

    public VampireState state;

    private const float MAX_MOVEMENT_TIMER = 1.5f;
    private const float MAX_FIRST_COOLDOWN_TIMER = 1f, MAX_SECOND_COOLDOWN_TIMER = 1f;

    private float movementTimer, firstCooldownTimer, secondCooldownTimer;

    private const float SLOW_SPEED = 5f, FAST_SPEED = 18f;

    private bool needsToChangeDirection = true;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = 3;

        movementTimer = MAX_MOVEMENT_TIMER;
        firstCooldownTimer = MAX_FIRST_COOLDOWN_TIMER;
        secondCooldownTimer = MAX_SECOND_COOLDOWN_TIMER;

        enemyMovementSpeed = SLOW_SPEED;

        state = VampireState.MOVING;

        playerObj = GameObject.Find("Player");

        enemySFX = GameObject.Find("ZombVampHit_SFX").GetComponent<AudioSource>();

        sightRange = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawning)
        {
            summonTimer += Time.deltaTime;
            if (summonTimer >= NecromancerScript.summonLength)
            {
                summonTimer = 0f;
                isSpawning = false;
            }

            Vector3 raisedPosition = transform.position;

            float yDiff = (-0.3f - (-8f));

            // inner expression after y diff is 1 when accumulated time equals summonLength
            float yIncrement = (yDiff * (Time.deltaTime / NecromancerScript.summonLength));
            raisedPosition.y += yIncrement;

            transform.position = raisedPosition;

            return;
        }

        float distance = Vector3.Distance(playerObj.transform.position, transform.position);

        if (distance > sightRange)
            return;

            VampireState initialState = state;

        //Don't put return statements cause it screws with debugging
        if (state == VampireState.MOVING){
            if (movementTimer > 0)
            {
                movementTimer -= Time.deltaTime;
                enemyMovementSpeed = SLOW_SPEED;
                MoveTowardsPlayer();
            }else{
                //offsets it slightly so that the attacking movement doesn't last too long.
                movementTimer = MAX_MOVEMENT_TIMER - 1.0f;
                state = VampireState.TELEPORTING;
                return;
            }
        }

        //teleporting is only for a single frame
        if (state == VampireState.TELEPORTING){
            //have logic here to tp vampire to player
            float xOffset = Random.Range(-5.0f, 5.0f), zOffset = Random.Range(-5.0f, 5.0f);
            xOffset += playerObj.transform.position.x;
            zOffset += playerObj.transform.position.z;

            enemyObj.transform.position = new Vector3(xOffset, playerObj.transform.position.y, zOffset);

            state = VampireState.FIRST_COOLDOWN;
        }

        if (state == VampireState.FIRST_COOLDOWN){
            if (firstCooldownTimer > 0)
            {
                firstCooldownTimer -= Time.deltaTime;
            }else{
                firstCooldownTimer = MAX_FIRST_COOLDOWN_TIMER;
                state = VampireState.ATTACK;
            }
        }

        if (state == VampireState.ATTACK){
            if (needsToChangeDirection){
                FacePlayer();
                needsToChangeDirection = false;
            }

            if (movementTimer > 0)
            {
                movementTimer -= Time.deltaTime;
                enemyMovementSpeed = FAST_SPEED;
                Dash();
            }else{
                movementTimer = MAX_MOVEMENT_TIMER;
                needsToChangeDirection = true;
                state = VampireState.SECOND_COOLDOWN;
            }
        }

        if (state == VampireState.SECOND_COOLDOWN){
            if (secondCooldownTimer > 0)
            {
                secondCooldownTimer -= Time.deltaTime;
            }else{
                secondCooldownTimer = MAX_SECOND_COOLDOWN_TIMER;
                state = VampireState.MOVING;
            }
        }

        //logs the changing of state
        if (initialState != state){
            //Debug.Log("State has changed to " + state);
        }
    }

    void Dash(){
        enemyObj.GetComponent<Rigidbody>().velocity = enemyObj.transform.forward * enemyMovementSpeed;
    }

    /*
    VampireState GetState(){
        return state;
    }
    */
};
