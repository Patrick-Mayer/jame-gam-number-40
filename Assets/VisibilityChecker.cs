using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    private MeshRenderer objectRenderer;
    private Camera mainCamera;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (objectRenderer.isVisible)
        {
            Debug.Log("Object is visible to the camera.");
        }
        else
        {
            Debug.Log("Object is NOT visible to the camera.");
        }
    }
}