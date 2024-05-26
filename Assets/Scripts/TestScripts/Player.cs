using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Rigidbody rb;

    private GameObject playerObj;

    [SerializeField] private AudioSource keyCollect_SFX;
    [SerializeField] private AudioSource doorOpen_SFX;

    private bool hasKey = false;
    private bool doorOpened = false;

    private Image heart1;
    private Image heart2;
    private Image heart3;

    [SerializeField] private AudioSource damage_SFX;

    private Image[] heartArr = {null, null, null};

    private int playerHealth = 3;
    private int maxPlayerHP = 3;

    private float keyDoorTextRange = 10;
    [SerializeField] private GameObject doorText;

    // Define isometric movement vectors
    private Vector3 isometricRight = new Vector3(1, 0, 1).normalized;
    private Vector3 isometricLeft = new Vector3(-1, 0, -1).normalized;
    private Vector3 isometricUp = new Vector3(-1, 0, 1).normalized;
    private Vector3 isometricDown = new Vector3(1, 0, -1).normalized;

    //hardcoded rotation vals
    private float turnRight;
    private float turnDownRight;

    private float turnDown;

    private float turnDownLeft;

    private float turnLeft;

    private float turnUpLeft;

    private float turnUp;
    private float turnUpRight;


    void Start(){
        if(debugMode){
            playerHealth = int.MaxValue;
        }

        AssignHearts();

        heartArr[0] = heart1;
        heartArr[1] = heart2;
        heartArr[2] = heart3;

        playerObj = GameObject.Find("Player");

        doorText = GameObject.Find("DoorText");
        doorText.SetActive(false);

        damage_SFX = GameObject.Find("takeDamage_SFX").GetComponent<AudioSource>();
        keyCollect_SFX = GameObject.Find("keyCollect_SFX").GetComponent<AudioSource>();
        doorOpen_SFX = GameObject.Find("doorOpen_SFX").GetComponent<AudioSource>();
    }

    void AssignHearts()
    {
        heart1 = GameObject.Find("Heart1").GetComponent<Image>();
        heart2 = GameObject.Find("Heart2").GetComponent<Image>();
        heart3 = GameObject.Find("Heart3").GetComponent<Image>();
    }

    //this is all ChatGPT BS, but Unity's input system is stupid so I don't care.
    void Update()
    {
        CheckKeyText();
        CheckKeyCollision();
        // Initialize a new velocity vector
        Vector3 newVelocity = Vector3.zero;
        float newYRot = oldYRot;

        // Check if any of the WASD keys are held down and update the velocity accordingly
        if (Input.GetKey(KeyCode.W))
        {
            newVelocity += isometricUp * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newVelocity += isometricLeft * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newVelocity += isometricDown * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newVelocity += isometricRight * speed;
        }

        rb.velocity = newVelocity;
    }

    private void CheckKeyText()
    {
        if (hasKey)
            return;

        GameObject doorObj = GameObject.Find("KeyDoor");
        if (doorObj == null)
            return;

        float distance = Vector3.Distance(playerObj.transform.position, doorObj.transform.position);

        if (distance < keyDoorTextRange)
        {
            doorText.SetActive(true);
        }
        else
        {
            doorText.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject portal = GameObject.Find("Portal");
        GameObject keyDoor = GameObject.Find("KeyDoor");

        //Debug.Log("key(clone): " + key);
        if (collision.gameObject == portal)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            playerHealth = maxPlayerHP;
        }
        else if (collision.gameObject == keyDoor && hasKey)
        {
            collision.gameObject.SetActive(false);
            hasKey = false;
            doorOpen_SFX.Play();
        }
    }

    // check distance to key instead of using collider to prevent jitter since key is solid
    private void CheckKeyCollision()
    {
        GameObject key = GameObject.Find("KeyMesh");
        if(key == null)
            return;

        float distance = Vector3.Distance(playerObj.transform.position, key.transform.position);
        Debug.Log("distance: " + distance);

        if (distance < 3f)
        {
            Destroy(key);
            hasKey = true;
            keyCollect_SFX.Play();
        }
    }

        public void DamagePlayer(int damage){
        playerHealth -= damage;
        //Debug.Log(playerHealth);

        damage_SFX.Play();

        if(playerHealth <= 0){
            //reloads current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            playerHealth = maxPlayerHP;
        }

        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int heartIndex = 0; heartIndex < maxPlayerHP; heartIndex++)
        {
            if (heartIndex >= playerHealth)
            {
                heartArr[heartIndex].enabled = false;
            }
            else
            {
                heartArr[heartIndex].enabled = true;
            }
        }

    }
}
