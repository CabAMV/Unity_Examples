using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float sensibilityX = 20f;
    [SerializeField]
    private float sensibilityY = 20f;

    [SerializeField]
    private bool inverseX = false;
    [SerializeField]
    private bool inverseY = true;

    private float rotationX;
    private float rotationY;

    private Vector3 inputRot;

    private Camera cam;
    private Transform myTransform;

    void Start()
    {
        cam = GetComponent<Camera>();
        myTransform = transform;

        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
       inputRot = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);

        rotationX += inputRot.x * sensibilityX * Time.deltaTime * ((inverseX) ? 1 : -1);
        rotationY += inputRot.y * sensibilityY * Time.deltaTime * ((inverseY) ? 1 : -1);

        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
    }
    
    void LateUpdate()
    {
        myTransform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }
}

