using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = UnityEngine.Vector3;

public class DynamicNeedleFollow : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody rigidBody;

    //public GameObject needleObj;
    private const float yPos = 0.0f, yRot = 0.0f;


    void Update()
    {
        // Raycast checking for mouse position hitting a floor or something with a collider
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Move the needle towards the hit point of the raycase
        if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity, (1 << LayerMask.NameToLayer("floor"))))
        {
            Vector3 lookAtCursor = hit.point - transform.position;
            lookAtCursor.y = yRot;

            //transform.position = Vector3.MoveTowards(transform.position, hit.point, speed * Time.deltaTime);
            transform.forward = lookAtCursor;
            
            Vector3 direction = (hit.point - transform.position).normalized;
            direction.y = 0;
            rigidBody.velocity = direction * speed;
            Debug.Log(hit.collider.gameObject.name);
            //Debug.Log(LayerMask.NameToLayer("floor"));
        }

        //forces yPos and yRot to always be zero
        //transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, yRot, transform.rotation.z);
    }
}