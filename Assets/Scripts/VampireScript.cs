using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VampireScript : EnemyScript
{    
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
    
    private const float MAX_MOVEMENT_TIMER = 3f;
    private const float MAX_FIRST_COOLDOWN_TIMER = 1.5f, MAX_SECOND_COOLDOWN_TIMER = 1.0f;

    private float movementTimer, firstCooldownTimer, secondCooldownTimer;

    private const float SLOW_SPEED = 3f, FAST_SPEED = 20f;

    private bool needsToChangeDirection = true;
    
    // Start is called before the first frame update
    void Start()
    {
        movementTimer = MAX_MOVEMENT_TIMER;
        firstCooldownTimer = MAX_FIRST_COOLDOWN_TIMER;
        secondCooldownTimer = MAX_SECOND_COOLDOWN_TIMER;
        
        enemyHealth = 3;
        enemyMovementSpeed = SLOW_SPEED;

        state = VampireState.MOVING;
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
            float xOffset = Random.Range(-1.0f, 1.0f), zOffset = Random.Range(-1.0f, 1.0f);
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
                LookTowardsPlayer();
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