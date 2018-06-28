using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Posses a turret and controls it.
/// </summary>
public class ShootMode : GameMode {
    /// <summary>
    /// Possesed turret.
    /// </summary>
    TurretMiniGun turret;
    /// <summary>
    /// Camera controller.
    /// </summary>
    CameraController camController;
    /// <summary>
    /// Camera.
    /// </summary>
    SpringArm camera;

    private void Start()
    {
        camController = GetComponent<CameraController>();
        camera = camController.getCamera();

    }

    public override void initMode(SelectableObject obj)
    {

        cellSelected = obj.getCell();
        if (cellSelected.getBuilding() != null)
            currentSelection = cellSelected.getBuilding();
        else
            currentSelection = cellSelected;

        turret = currentSelection as TurretMiniGun;
        turret.setIsShooting(false);
        turret.transform.Find("ConicVision").gameObject.SetActive(false);

        camController.saveCameraState();
        camera.changeTarget(turret.GetComponent<CameraConfig>().getConfig());

        turret.activateAimReticle(true);

        Cursor.lockState=CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public override void gameModeEvents()
    {
        turret.rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        turret.playManually(Input.GetButton("Fire1"));

        if (Input.GetKey(KeyCode.Q))
            (gameModeManager as GameModeManager).enableSelectionMode();
    }

    public override void disableMode()
    {
        turret.transform.Find("ConicVision").gameObject.SetActive(true);
        camController.resetCamera();
        turret.activateAimReticle(false);
        Cursor.visible = true;

    }

}
