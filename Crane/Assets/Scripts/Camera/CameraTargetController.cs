using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [SerializeField] private List<CameraConfig> targetList;
    private cameraConfig currentTarget;
    private SpringArm camera;

    private List<cameraConfig> cameras;

    // Use this for initialization
    void Start()
    {
        camera = this.GetComponent<SpringArm>();
        cameras = new List<cameraConfig>();
        cameras.Add(targetList[0].getConfig());
        cameras.Add(targetList[1].getConfig());
        cameras.Add(new cameraConfig(targetList[1].getConfig().target, 30, new Vector3(0, 7, 0), new Vector3(0, 15, -1.5f), true,Vector3.zero, 0.5f,10,false));
        cameras.Add(new cameraConfig(targetList[1].getConfig().target, 1, new Vector3(0, 1.5f, 0), new Vector3(0, 1.5f, -1), true, Vector3.zero, 20,10,false));
        cameras.Add(new cameraConfig(targetList[1].getConfig().target, 20, Vector3.zero, new Vector3(0, 15, -1), false, new Vector3 (0,0,0), 20,10,false));
        cameras.Add(new cameraConfig(targetList[1].getConfig().target, 20, Vector3.zero, new Vector3(0, 0, -1), false, new Vector3(30, 0, 0), 2,10,false));
        cameras.Add(new cameraConfig(targetList[1].getConfig().target, 20, Vector3.zero, new Vector3(0, 0, -1), false, new Vector3(30 ,0, 0), 2,10, true));


        currentTarget = cameras[0];
        camera.changeTarget(currentTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int index = cameras.IndexOf(currentTarget) + 1;
            if (index >= targetList.Count)
                index = 0;
            currentTarget = cameras[index];
            camera.changeTarget(currentTarget);
        }

        if (cameras.IndexOf(currentTarget) > 0)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                camera.changeTarget(cameras[1]);
                currentTarget = cameras[1];
            }
                
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                camera.changeTarget(cameras[2]);
                currentTarget = cameras[2];
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                camera.changeTarget(cameras[3]);
                currentTarget = cameras[3];
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                camera.changeTarget(cameras[4]);
                currentTarget = cameras[4];
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                camera.changeTarget(cameras[5]);
                currentTarget = cameras[5];
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                camera.changeTarget(cameras[6]);
                currentTarget = cameras[6];
            }

            if (cameras.IndexOf(currentTarget) > cameras.Count-3)
            {
                if (Input.GetKey(KeyCode.W))
                    camera.addRotation(1,0,0);
                if (Input.GetKey(KeyCode.S))
                    camera.addRotation(-1, 0, 0);
                if (Input.GetKey(KeyCode.D))
                    camera.addRotation(0, -1, 0);
                if (Input.GetKey(KeyCode.A))
                    camera.addRotation(0, 1, 0);
            }


        }


    }


}
