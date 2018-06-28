using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class carScript : MonoBehaviour {

    public GameObject[] physicWheels;
    public GameObject[] visualWheels;
    private float maxSpeed=1750;
    private float maxSteer = 25;
    private float speed;
    private float acceleration;


    void Start () {
        speed = 175;
        acceleration = 1500;
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(-transform.up * GetComponent<Rigidbody>().velocity.magnitude);
        pisar();
        dirigir();
        posicionarRuedas();
        //kph();
    }

    void posicionarRuedas()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        

        for (int i = 0; i < physicWheels.GetLength(0); i++)
        {
            physicWheels[i].GetComponent<WheelCollider>().GetWorldPose(out pos, out rot);
            visualWheels[i].transform.position = pos;
            visualWheels[i].transform.rotation = rot;
        }

    }

    void pisar()
    {
        //aceleracion
        if (Input.GetKeyDown(KeyCode.UpArrow) && GetComponent<Rigidbody>().velocity.magnitude<=2)
        {

            GetComponent<Rigidbody>().velocity = transform.forward * 6.5f;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            acelaracion();
            physicWheels[0].GetComponent<WheelCollider>().motorTorque = speed;
            physicWheels[1].GetComponent<WheelCollider>().motorTorque = speed;
            //physicWheels[2].GetComponent<WheelCollider>().motorTorque = speed;
            //physicWheels[3].GetComponent<WheelCollider>().motorTorque = speed;
        }


        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            physicWheels[0].GetComponent<WheelCollider>().motorTorque = 0;
            physicWheels[1].GetComponent<WheelCollider>().motorTorque = 0;
            physicWheels[2].GetComponent<WheelCollider>().motorTorque = 0;
            physicWheels[3].GetComponent<WheelCollider>().motorTorque = 0;
            speed = 175;

            /*
            physicWheels[2].GetComponent<WheelCollider>().brakeTorque=maxSpeed/2;
            physicWheels[3].GetComponent<WheelCollider>().brakeTorque = maxSpeed / 2;

            physicWheels[2].GetComponent<WheelCollider>().brakeTorque = 0;
            physicWheels[3].GetComponent<WheelCollider>().brakeTorque = 0;*/
        }

        //frenada
        if (Input.GetKey(KeyCode.DownArrow))
        {
            for (int i = 0; i < physicWheels.GetLength(0); i++)
                physicWheels[i].GetComponent<WheelCollider>().brakeTorque = maxSpeed;
                
            
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            for (int i = 0; i < physicWheels.GetLength(0); i++)
                physicWheels[i].GetComponent<WheelCollider>().brakeTorque = 0;
        }
    }

    void dirigir()
    {
        if (Input.GetKey(KeyCode.RightArrow) && physicWheels[0].GetComponent<WheelCollider>().steerAngle <maxSteer)
        {
            physicWheels[0].GetComponent<WheelCollider>().steerAngle += 2;
            physicWheels[1].GetComponent<WheelCollider>().steerAngle += 2;
                 
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            speed = speed * 0.8f;
            
        }

        if (Input.GetKey(KeyCode.LeftArrow) && physicWheels[0].GetComponent<WheelCollider>().steerAngle > -maxSteer)
        {
            physicWheels[0].GetComponent<WheelCollider>().steerAngle -= 2;
            physicWheels[1].GetComponent<WheelCollider>().steerAngle -= 2;
            
        }

        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && physicWheels[0].GetComponent<WheelCollider>().steerAngle > 0)
        {
            
            physicWheels[0].GetComponent<WheelCollider>().steerAngle -= 2;
            physicWheels[1].GetComponent<WheelCollider>().steerAngle -= 2;
        }

        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && physicWheels[0].GetComponent<WheelCollider>().steerAngle < 0)
        {
            
            physicWheels[0].GetComponent<WheelCollider>().steerAngle += 2;
            physicWheels[1].GetComponent<WheelCollider>().steerAngle += 2;
        }

    }

    void acelaracion()
    {
        
        if (speed < maxSpeed)
            speed = speed + acceleration * Time.deltaTime;


        else
            if (speed<maxSpeed)
                speed = speed + acceleration * Time.deltaTime;
        
    }
    void kph()
    {
       Debug.Log(GetComponent<Rigidbody>().velocity.magnitude*3.6); 
    }

}
