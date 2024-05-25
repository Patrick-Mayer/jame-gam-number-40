using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //public Rigidbody rigidBody;
    public GameObject playerObj;
    public GameObject enemyObj;
    public AudioSource enemySFX;

    protected int enemyHealth;
    protected float enemyMovementSpeed;

    //the fact that I have to do it this way makes me think Unity's parents are cousins
    protected Player playerScript;

    private const int MAX_JANK_COOLDOWN = 60;
    private int currentCooldown = 90;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerObj.GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject == playerObj){
            Debug.Log("Successful collision");
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //prevents vampire immediately killing player when teleporting
        if (VampireInstaKill()){
            Debug.Log(GetState());
            return;
        }

        GameObject needleObj = GameObject.Find("Needle");

        if (collision.gameObject == needleObj) {
            enemyHealth--;
            enemySFX.Play();
            ScoreScript.AddScore(1);

            if(enemyHealth <= 0){
                EnemyDeath();
            }

        }else if(collision.gameObject == playerObj){
            //playerScript.DamagePlayer(1);
            //Debug.Log("we get here");
            playerObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            playerObj.GetComponent<Player>().DamagePlayer(1);
        }
    }

    //this is jank but it's a game jam
    private void OnTriggerStay(Collider collision)
    {
        //prevents vampire immediately killing player when teleporting
        if (VampireInstaKill()){
            Debug.Log(GetState());
            return;
        }
        
        if (currentCooldown <= 0){
            currentCooldown = MAX_JANK_COOLDOWN;

            //Debug.Log("collision detected");

            GameObject needleObj = GameObject.Find("Needle");
            if (collision.gameObject == needleObj) {
                enemyHealth--;
                enemySFX.Play();
                ScoreScript.AddScore(1);

                if(enemyHealth <= 0){
                    EnemyDeath();
                }

            }else if(collision.gameObject == playerObj){
                //playerScript.DamagePlayer(1);
                //Debug.Log("we get here");
                playerObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                playerObj.GetComponent<Player>().DamagePlayer(1);
            }

        }else{
            currentCooldown--;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void EnemyDeath(){
        Destroy(enemyObj);
    }

    protected void ChangeDirection(){
        Vector3 direction = (playerObj.transform.position - enemyObj.transform.position).normalized;
        direction.y = 0;
        enemyObj.transform.forward = direction;
    }

    protected void Move(){
        ChangeDirection();
        enemyObj.GetComponent<Rigidbody>().velocity = enemyObj.transform.forward * enemyMovementSpeed;
    }


    //this crap is here for inheritance reasons
    
    enum VampireState
        {
            MOVING,
            TELEPORTING,
            FIRST_COOLDOWN,
            ATTACK,
            SECOND_COOLDOWN,
        }
    
    //! ONLY CALL THIS METHOD IF YOU KNOW FOR SURE IT'S A VAMPIRE OBJ
    VampireState GetState(){
        return (VampireState)enemyObj.GetComponent<VampireScript>().state;
    }

    bool VampireInstaKill(){
        return (enemyObj.GetComponent<VampireScript>() != null &&
                (enemyObj.GetComponent<VampireScript>().GetState() == VampireState.TELEPORTING ||
                enemyObj.GetComponent<VampireScript>().GetState() == VampireState.FIRST_COOLDOWN));
    }
};