using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody rigidBody;

    public GameObject playerObj;
    public GameObject enemyObj;

    public AudioSource enemySFX;
    public bool nonAggressive = true;

    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private Image healthFill;

    protected int enemyHealth;
    protected float enemyMovementSpeed;

    protected float sightRange;

    //the fact that I have to do it this way makes me think Unity's parents are cousins
    protected Player playerScript;

    private const int MAX_JANK_COOLDOWN = 60;
    private int currentCooldown = 90;

    // temporarily stops enemy while being summoned by necromancer
    public bool isSpawning = false;
    protected float summonTimer;

    // Start is called before the first frame update
    void Start()
    {
        //playerObj = GameObject.Find("Player");
        playerScript = playerObj.GetComponent<Player>();
        keyPrefab = GameObject.Find("Key");
        
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {        
        //healthFill.fillAmount = enemyHealth / maxHealth;
    }

    private void onCollisionEnter(Collision collision)
    {
        GameObject needleObj = GameObject.Find("Needle");
        GameObject collidedObj = collision.gameObject;

        if (collidedObj == needleObj)
        {
            enemyHealth--;
            enemySFX.Play();

            Debug.Log("needle hit");

            //Debug.Log("vampire took damage");

            if (enemyHealth <= 0)
            {
                EnemyDeath();
            }
        }
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
            //Debug.Log(GetState());
            return;
        }

        GameObject needleObj = GameObject.Find("Needle");
        GameObject collidedObj = collision.gameObject;

        if (collidedObj == needleObj) {
            enemyHealth--;
            enemySFX.Play();

            Debug.Log("needle hit");

            //Debug.Log("vampire took damage");

            if(enemyHealth <= 0){
                EnemyDeath();
            }

        }else if(collidedObj == playerObj){
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
            //Debug.Log(GetState());
            return;
        }

        if (currentCooldown <= 0){
            currentCooldown = MAX_JANK_COOLDOWN;

            //Debug.Log("collision detected");

            GameObject needleObj = GameObject.Find("Needle");
            if (collision.gameObject == needleObj) {
                enemyHealth--;
                enemySFX.Play();

                Debug.Log("needle hit");

                if (enemyHealth <= 0){
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

    private bool IsLastEnemy()
    {
        GameObject[] sceneObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> enemies = new List<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            bool isSkeleton = obj.name.Contains("Skeleton");
            bool isZombie = obj.name.Contains("Zombie");
            bool isVampire = obj.name.Contains("Vampire");
            bool isNecromancer = obj.name.Contains("Necromancer");

            if (isSkeleton || isZombie || isVampire || isNecromancer)
            {
                enemies.Add(obj);
            }
        }

        if(enemies.Count == 1)
        {
            return true;
        }
        return false;
    }

    void EnemyDeath(){

        if(IsLastEnemy())
        {
            Vector3 offsetTransform = transform.position;
            offsetTransform.y += 3;
            Instantiate(keyPrefab, offsetTransform, Quaternion.identity);
        }
        Destroy(enemyObj);
    }

    /*protected void FlashRed()
    {
        //MeshRenderer renderer = GetComponent<MeshRenderer>();
        //renderer.material.SetColor("_Color", Color.red);
        Debug.Log("flashing red");

        MeshRenderer renderer = GetComponent<MeshRenderer>();

        //Debug.Log("materials: " + renderer.materials);

        Material solidColorMaterial = new Material(Shader.Find("Unlit/Color"));
        solidColorMaterial.color = Color.red;

        if (solidColorMaterial.shader == null)
        {
            Debug.LogError("Unlit/Color shader not found.");
            return;
        }

        Material[] newMaterials = new Material[renderer.materials.Length];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = solidColorMaterial;
            Debug.Log($"Assigned new material with color {Color.red} to material slot {i}");
        }
        renderer.materials = newMaterials;
    }*/

    protected void FacePlayer(){
    Vector3 direction = (playerObj.transform.position - enemyObj.transform.position).normalized;
    direction.y = 0;
    enemyObj.transform.forward = direction;
    }

    protected void MoveTowardsPlayer(){
        LookTowardsPlayer();
        Move();
    }

    protected void Move(){
        float distance = Vector3.Distance(playerObj.transform.position, transform.position);

        if (distance < sightRange)
        {
            FacePlayer();
            enemyObj.GetComponent<Rigidbody>().velocity = enemyObj.transform.forward * enemyMovementSpeed;
        }
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
