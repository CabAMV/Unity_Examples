using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float movSpeed = 5;
        
    private Vector3 input;

    [SerializeField]
    private Camera cam;
    private Transform camTr;

    private Transform myTransform;


    // CHECK GROUND 
    [SerializeField]
    private float sphereCollisionSize;
    [SerializeField]
    private float offset;
    [SerializeField]
    private LayerMask groundMask;

    // FISICA
    [SerializeField]
    private float gravity = -10f;

    private Vector3 velocity;

    [SerializeField]
    private float jumpForce = 25f;
    [SerializeField]
    private float jumpHeight = 2f;
    private bool onGround;

    private bool jumpInput;

    private CharacterController cc;

    // velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);

    void Start()
    {
        myTransform = transform;
        camTr = cam.transform;
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            
		myTransform.localEulerAngles = new Vector3(0, camTr.eulerAngles.y, 0);

        jumpInput = Input.GetKey(KeyCode.Space);

        // JUMP          
        if (onGround)
        {
            velocity.y = 0;
        }
             
        if (onGround && jumpInput)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void FixedUpdate()
    {
        // CHECK GROUND
        onGround = OnGroundCheck ();
            
        // MOVEMENT
        velocity.x = input.x * movSpeed;
        velocity.z = input.z * movSpeed;

        velocity.y += gravity * Time.fixedDeltaTime;
            
        cc.Move(myTransform.rotation * velocity * Time.fixedDeltaTime);
    }

    bool OnGroundCheck()
    {
        RaycastHit hit;
        if (Physics.SphereCast(cc.transform.localPosition, sphereCollisionSize, Vector3.down, out hit, offset, groundMask))
        {
            cc.Move(new Vector3(0, -0.4f, 0));
            return true;
        }
            
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

		if(cc != null)
			Gizmos.DrawSphere(cc.transform.localPosition + Vector3.down * offset, sphereCollisionSize);
		else
			Gizmos.DrawSphere(transform.localPosition + Vector3.down * offset, sphereCollisionSize);
    }
}
