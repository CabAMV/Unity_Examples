using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that controls the camera movement.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Camera.
    /// </summary>
    private SpringArm camera;
    /// <summary>
    /// Determines if camera can rotate.
    /// </summary>
    private bool canRotate;
    /// <summary>
    /// GameManager of the player.
    /// </summary>
    private GameModeManager gameMode;
    /// <summary>
    /// Initial configuration of the camera.
    /// </summary>
    private cameraConfig initialConfig;

    /// <summary>
    /// Bounds of the camera movement area.
    /// </summary>
	[SerializeField]
	private Transform boundTRmin,boundTRmax;
    /// <summary>
    /// Bounds of the camera movement area.
    /// </summary>
    private Vector3 min,max;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main.gameObject.GetComponent<SpringArm>();
        canRotate = true;
        gameMode = GetComponent<GameModeManager>();
        initialConfig = camera.getConfig();

		min = boundTRmin.position;
		max = boundTRmax.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!(gameMode.getCurrentMode() is ShootMode))
        {
            cameraMovement();
            if (canRotate)
                cameraRotation();
            cameraDistance();
        }

    }
    
    /// <summary>
    /// Sets if the camera can rotate manually.
    /// </summary>
    /// <param name="rotate"></param>
    public void setCanRotate(bool rotate)
    {
        canRotate = rotate;
    }
    /// <summary>
    /// Moves the camera by the desired inputs.
    /// </summary>
    private void cameraMovement()
    {
        Vector3 cameraProyection = new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z);
		Vector3 verticalInput = (transform.position - cameraProyection).normalized * Input.GetAxis("Vertical");
		Vector3 horizontalInput = Camera.main.transform.right * Input.GetAxis("Horizontal");
		Vector3 newPos = transform.position + verticalInput + horizontalInput;
		if (newPos.z < max.z && newPos.x < max.x) 
		{
			if (newPos.z > min.z && newPos.x > min.x) 				
				this.transform.position = newPos;
		}
    }
    /// <summary>
    /// Rotates the camera by the desired inputs.
    /// </summary>
    private void cameraRotation()
    {
        if (Input.GetButton("Fire2"))
        {
            camera.addRotation(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }

    }
    /// <summary>
    /// Zooms the camera in and out by the desired inputs.
    /// </summary>
    private void cameraDistance()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if(camera.getArmLenght() - Input.GetAxis("Mouse ScrollWheel") * 20 > 10 && camera.getArmLenght() - Input.GetAxis("Mouse ScrollWheel") * 20 < 200)
            	camera.setArmLenght(camera.getArmLenght() - Input.GetAxis("Mouse ScrollWheel") * 20);
        }
    }
    /// <summary>
    /// Returns the camera.
    /// </summary>
    /// <returns></returns>
    public SpringArm getCamera()
    {
        return camera;
    }
    /// <summary>
    /// Saves the camera configuration in the initialConfig variable.
    /// </summary>
    public void saveCameraState()
    {
        initialConfig = camera.getConfig();
    }
    /// <summary>
    /// Sets the current configuration to the initial configuration.
    /// </summary>
    public void resetCamera()
    {
        camera.changeTarget(initialConfig);
    }
}
