using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpringArm : MonoBehaviour
{
    private Vector3 desiredPosition;

    [Header("Target and distance")]
    [SerializeField] private Transform target;
    [SerializeField] private float armLenght;
    private float desiredArmLenght;

    [Header("Offsets")]
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private Vector3 offset;

    [Header("Rotation")]
    [SerializeField] private bool useTargetRotation;
    [SerializeField]private Vector3 rotation = new Vector3(0, 0, 0);

    [Header("Camera speed")]
    [SerializeField] [Range(1, 20)] private float speed;
    [SerializeField] [Range(1, 20)] private float rotationSpeed;

    private float finalSpeed;

    [Header("Enviroment collision")]
    [SerializeField] private bool doCollisionTest;

    private List<Vector3> lista = new List<Vector3>();

    Vector3 velocity;


    private void Start()
    {
        desiredArmLenght = armLenght;
        finalSpeed = speed;
    }


    void FixedUpdate()
    {

        updateTransform();
        if (doCollisionTest)
        {
            CollisionTest(desiredPosition);
            return;
        }
        else
        {
            desiredArmLenght = armLenght;
            finalSpeed = speed;
        }

    }


    private void CollisionTest(Vector3 origin)
    {
        RaycastHit hit;
        Debug.DrawRay(origin, -transform.forward * armLenght * Mathf.Abs(offset.z), Color.red);
        //center
        if (Physics.Raycast(origin, -transform.forward, out hit, desiredArmLenght + 0.5f))
        {
            if (hit.collider.gameObject != target.gameObject)
            {

                desiredArmLenght = Vector3.Distance(origin, hit.point)-0.2f;
                finalSpeed = 15;
                return;
            }

        }

        else
        {
            desiredArmLenght = armLenght;
            finalSpeed = speed;
        }
        lista.Clear();

        lista.Add(transform.position + (transform.right*4) + (transform.up*4 ) - transform.forward*1.5f);
        lista.Add(transform.position + (transform.right * 4) + (-transform.up * 4) - transform.forward * 1.5f) ;
        lista.Add(transform.position + (-transform.right * 4) + (transform.up * 4) - transform.forward * 1.5f) ;
        lista.Add(transform.position + (-transform.right * 4) + (-transform.up * 4) - transform.forward * 1.5f);


        foreach (Vector3 element in lista)
        {

            if (Physics.Raycast(origin, element-origin, out hit, desiredArmLenght))
            {
                if (hit.collider.gameObject != target.gameObject)
                {
                    desiredArmLenght = Vector3.Distance(origin, hit.point)-0.2f;
                    finalSpeed = 20;
                    return;
                }
            }

            else
            {
                desiredArmLenght = armLenght;
                finalSpeed = speed;
            }

        }
    }


    private void updateTransform()
    {
        //if collision test is done shouldnt use offset
        print(1/finalSpeed);
        if (useTargetRotation)
        {
            Vector3 rotX = target.right * targetOffset.x;
            Vector3 rotY = target.up * targetOffset.y;
            Vector3 rotZ = target.forward * targetOffset.z;

            Vector3 vectorX = target.right * offset.x;
            Vector3 vectorY = target.up * offset.y;
            Vector3 vectorZ = target.forward * offset.z * desiredArmLenght;


            transform.position = Vector3.SmoothDamp(transform.position, target.position + vectorX + vectorY + vectorZ, ref velocity, 1/finalSpeed);
            this.transform.rotation = Quaternion.Slerp( this.transform.rotation,Quaternion.LookRotation((target.position + rotX + rotY + rotZ) - this.transform.position), rotationSpeed * Time.deltaTime);

            if (doCollisionTest)
            {
                desiredPosition = target.position + rotX + rotY + rotZ;
            }


        }

        else 
        {
            Vector3 rotX = target.right * targetOffset.x;
            Vector3 rotY = target.up * targetOffset.y;
            Vector3 rotZ = target.forward * targetOffset.z;
            Vector3 vectorX = target.right * offset.x;
            Vector3 vectorY = target.up * offset.y;
            Quaternion eulerRot = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

            Vector3 desiredPos = target.position + eulerRot * new Vector3(0, 0, -1 * desiredArmLenght);
            desiredPos = desiredPos + vectorX + vectorY;

            transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, 1 / finalSpeed);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation((target.position + rotX + rotY + rotZ) - this.transform.position), rotationSpeed * Time.deltaTime);
            if (doCollisionTest)
            {
                desiredPosition = target.position + rotX + rotY + rotZ;
            }

        }




    }



    public void changeTarget(cameraConfig newTarget)
    {
        speed = newTarget.speed;
        rotationSpeed = newTarget.rotationSpeed;
        desiredArmLenght = armLenght = newTarget.armLenght;
        targetOffset = newTarget.targetOffset;
        offset = newTarget.offset;
        useTargetRotation = newTarget.useTargetRotation;
        rotation = newTarget.rotation;
        target = newTarget.target;
        doCollisionTest = newTarget.doCollisionTest;

    }

    public void addRotation(float x, float y, float z)
    {
        rotation += new Vector3(x,y,z);

    }

}


public struct cameraConfig
{
    public Transform target;
    public float armLenght;
    public Vector3 targetOffset;
    public Vector3 offset;
    public bool useTargetRotation;
    public Vector3 rotation;
    public float speed;
    public float rotationSpeed;
    public bool doCollisionTest;

    public cameraConfig(Transform target, float armLenght, Vector3 targetOffset, Vector3 offset,bool useTargetRotation,Vector3 rotation, float speed, float rotationSpeed, bool doCollisionTest)
    {
        this.target = target;
        this.armLenght = armLenght;
        this.targetOffset = targetOffset;
        this.offset = offset;
        this.useTargetRotation = useTargetRotation;
        this.rotation = rotation;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.doCollisionTest = doCollisionTest;
    }
}



