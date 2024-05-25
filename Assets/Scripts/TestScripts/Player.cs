using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool debugMode = false;
    
    public float speed = 5f; // Speed at which the object moves
    public int playerHealth = 3;

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
    }

    //this is all ChatGPT BS, but Unity's input system is stupid so I don't care.
    void Update()
    {
        float oldYRot = playerObj.transform.rotation.eulerAngles.y;

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
        
        //this way we avoid Quaternions, cause I don't know crap about those
        Vector3 newRotation = playerObj.transform.rotation.eulerAngles;
        newRotation.y = newYRot;
        playerObj.transform.rotation = Quaternion.Euler(newRotation);
    }

    public void DamagePlayer(int damage){
        playerHealth -= damage;
        //Debug.Log(playerHealth);

        if(playerHealth <= 0){
            //reloads current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
