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
    public bool nonAggressive = true;

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

    private void OnCollisionStay(Collision collision){
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

    /*
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
    */

    // Update is called once per frame
    void Update()
    {

    }

    void EnemyDeath(){
        Destroy(enemyObj);
    }

    protected void Walk(){
        //really just picking between 3 values cause it's normalized, but this is most efficient way to do it
        float newX = Random.Range(-0.1f, 0.1f);
        float newZ = Random.Range(-0.1f, 0.1f);

        Vector3 direction = new Vector3(newX, 0f, newZ).normalized;
        enemyObj.transform.forward = direction;
        Move();
    }

    protected void LookTowardsPlayer(){
        Vector3 direction = (playerObj.transform.position - enemyObj.transform.position).normalized;
        direction.y = 0;
        enemyObj.transform.forward = direction;
    }

    protected void MoveTowardsPlayer(){
        LookTowardsPlayer();
        Move();
    }

    protected void Move(){
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