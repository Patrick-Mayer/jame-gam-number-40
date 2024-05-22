using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = UnityEngine.Vector3;

public class NeedleFollowTest : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody rb;

    private const float yPos = 0.0f, yRot = 0.0f;


    void Update()
    {
        // Raycast checking for mouse position hitting a floor or something with a collider
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Move the needle towards the hit point of the raycase
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            Vector3 lookAtCursor = hit.point - transform.position;
            lookAtCursor.y = yRot;
            
            //transform.position = Vector3.MoveTowards(transform.position, hit.point, speed * Time.deltaTime);
            transform.forward = lookAtCursor;

            //changes velocity on LMB click
            if (Input.GetMouseButtonDown(0)){
                //Debug.Log("Left Mouse Button Clicked");
                Vector3 direction = (hit.point - transform.position).normalized;
                direction.y = 0;
                rb.velocity = direction * speed;                
            }
        }        

        //forces yPos and yRot to always be zero
        //transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, yRot, transform.rotation.z);
    }
}