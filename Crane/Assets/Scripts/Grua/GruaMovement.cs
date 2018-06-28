using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruaMovement : MonoBehaviour {


	private Rigidbody rb;                                      //transform and rigidbodie of the crane
	private Transform tr;
    private Rigidbody rotator;                                 //transform and rigidbodie of the superior part of the crane
    private Transform rotatorTr;

    //bool that determines if it moves on the X axis
    private bool axisX;
    [SerializeField]private float movementSpeed;              //base movement speed
	private float aplicableSpeed;                             //real movement speed to apply

    [SerializeField] private float rotationSpeed;             //base rotation speed
    private float aplicableRotation;                          //real rotation speed to apply


    // Use this for initialization
    void Start () {
		rb = this.GetComponent<Rigidbody> ();
		tr = this.GetComponent<Transform> ();
        rotator = gameObject.GetComponentsInChildren<Rigidbody>()[1];
        rotatorTr = gameObject.GetComponentsInChildren<Transform>()[1];

    }

    // Update is called once per frame
    private void FixedUpdate() {
		movefront();
        rotate();

    }

    //Moves the base of the crane based on the speed
    private void movefront()
    {
		if (aplicableSpeed != 0)
        {
            if (axisX)
                rb.MovePosition(rb.position +(rotatorTr.right* aplicableSpeed));
            else
                rb.MovePosition(rb.position + (rotatorTr.forward * aplicableSpeed));



        }

    }

    //rotates the superior part of the crane
    private void rotate()
    {
        if (aplicableRotation != 0)
        {
            rotator.MoveRotation(rotator.rotation *= Quaternion.AngleAxis(aplicableRotation, Vector3.up));
            rotatorTr.position = rotator.position;
            rotatorTr.rotation = rotator.rotation;
        }

    }

	public void setMovementSpeed(int direction)
	{
		aplicableSpeed = direction*movementSpeed;
	}

	public void setaxis(bool isAxis)
	{
		axisX = isAxis;
	}

    public void setRotationSpeed(int direction)
    {
        aplicableRotation = direction * rotationSpeed;

    }

}
