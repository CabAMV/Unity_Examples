using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfig : MonoBehaviour {
    [Header("Distance")]
    [SerializeField] private float armLenght;

    [Header("Offsets")]
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private Vector3 offset;

    [Header("Rotation")]
    [SerializeField] private bool useTargetRotation;
    [SerializeField] private Vector3 rotation;

    [Header("Camera speed")]
    [SerializeField] [Range(1,20)]public float speed;
    [SerializeField] [Range(1, 20)] private float rotationSpeed;


    [Header("Enviroment collision")]
    [SerializeField] private bool doCollisionTest;



    private cameraConfig config;

	void Start()
	{
        config = new cameraConfig(this.GetComponent<Transform>(), armLenght, targetOffset, offset, useTargetRotation,rotation, speed,rotationSpeed,doCollisionTest);
	}

    public cameraConfig getConfig()
    {
        return config;
    }
}



