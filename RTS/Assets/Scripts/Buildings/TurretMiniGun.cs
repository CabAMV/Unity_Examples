using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatic turret with a conic vision that shoots the enemy units.
/// </summary>
public class TurretMiniGun : Building,IShooter {
    /// <summary>
    /// Layer mask for only collide with the default layer.
    /// </summary>
    int layerMask = 1 << 0;
    /// <summary>
    /// Current target to shoot.
    /// </summary>
    Transform target;
    /// <summary>
    /// Spring arm for the canon to look at. Points to the target.
    /// </summary>
    SpringArm targetArm;
    /// <summary>
    /// Transform of the mobile part of the turret.
    /// </summary>
    Transform gun;
    /// <summary>
    /// Transform where the projectiles are spawn.
    /// </summary>
    Transform canon;
    /// <summary>
    /// List of future targets of the turret.
    /// </summary>
    List<Transform> enemyTarget;
    /// <summary>
    /// Prefab of a reticle to be spawn on construction.
    /// </summary>
    [SerializeField] GameObject aimReticleObj;
    /// <summary>
    /// Reticle that points where the turret is aiming. Only used in ShootMode being active.
    /// </summary>
    GameObject aimReticle;
    /// <summary>
    /// Projectile to be spawn when the turret shoots.
    /// </summary>
    [SerializeField] GameObject projectile;
    /// <summary>
    /// Determines if the turret can shoot.
    /// </summary>
    private bool canShoot;
    /// <summary>
    /// Determines if the turret is shooting.
    /// </summary>
    private bool isShooting;
    /// <summary>
    /// Button that activates the ShootMode.
    /// </summary>
    private Button posses;

    private float damage = 1;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        target = transform.Find("Target");
        targetArm = target.gameObject.GetComponent<SpringArm>();

        gun = transform.Find("Gun");

        canon = gun.Find("Canon");

        enemyTarget = new List<Transform>();

        canShoot = true;

        targetArm.setUseRotationLimits(false,false,false);

        //switch that determines the initial orientation of the turret
        switch (_orientation)
        {
            case orientation.North:
                targetArm.addRotation(0, 180, 0);
                targetArm.setMinLimits(-30, 150, 0);
                targetArm.setMaxLimits(30, 210, 0);
                targetArm.setUseRotationLimits(true, true, true);
                break;
            case orientation.West:
                targetArm.addRotation(0, 90, 0);
                targetArm.setMinLimits(-30, 60, 0);
                targetArm.setMaxLimits(30, 120, 0);
                targetArm.setUseRotationLimits(true, true, true);
                break;
            case orientation.South:
                targetArm.setMinLimits(-30, -30, 0);
                targetArm.setMaxLimits(30, 30, 0);
                targetArm.setUseRotationLimits(true, true, true);
                break;
            case orientation.East:
                targetArm.addRotation(0, 270, 0);
                targetArm.setMinLimits(-30, 240, 0);
                targetArm.setMaxLimits(30, 300, 0);
                targetArm.setUseRotationLimits(true, true, true);
                break;
        }


        posses = canvas.transform.GetChild(0).Find("Posses").GetComponent<Button>();
        if(gameMode is GameModeManager)
            posses.onClick.AddListener((gameMode as GameModeManager).enableShootMode);

        aimReticle = Instantiate(aimReticleObj, GameObject.Find("LocalUI").transform);
        aimReticle = aimReticle.transform.GetChild(0).gameObject;
        aimReticle.SetActive(false);

    }

    protected override void Update()
    {
        base.Update();
        //Remove all the targets inside a building or dead
        enemyTarget.RemoveAll(enemy => enemy == null);
        enemyTarget.RemoveAll(enemy => enemy.gameObject.activeSelf==false);

        if (!(gameMode.getCurrentMode() is ShootMode))
            play();
       
    }


    //automatic gameplay
    #region

    /// <summary>
    /// Called once per update controls the activity of the turret. 
    /// </summary>
    private void play()
    {
        if (isShooting)
        {
            if (enemyTarget.Count > 0)
            {
                Vector3 aux = enemyTarget[0].position - gun.position;
                float dirY = AngleDir(-target.forward, aux, target.up);
                float dirX = AngleDir(target.forward, aux, -target.right);
                targetArm.addRotation(dirX, dirY, 0);
                if (canShoot)
                    StartCoroutine(pressTrigger());
            }
            else
                isShooting = false;
        }

    }
    /// <summary>
    /// Spawns a projectile and sends it forward by a given force.
    /// </summary>
    public void shoot()
    {
        GameObject temp=Instantiate(projectile, canon.position, canon.rotation);
        temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward*2000);

        Projectile tempProj = temp.GetComponent<Projectile>();
        tempProj.ignore(GetComponent<Collider>());
        tempProj.Instigator=this;

    }
    /// <summary>
    /// Apllies the delay in the shoots.
    /// </summary>
    /// <returns></returns>
    public IEnumerator pressTrigger()
    {
        shoot();
        canShoot = false;
        yield return new WaitForSeconds(0.1f);
        canShoot = true;

    }

    /// <summary>
    /// Adds a new target to the enemyTarget list.
    /// </summary>
    /// <param name="target"></param>
    public void addTarget(Transform target)
    {
        enemyTarget.Add(target);
        isShooting = true;
    }
    /// <summary>
    /// Removes a target from the enemyTarget list. 
    /// </summary>
    /// <param name="target"></param>
    public void removeTarget(Transform target)
    {
        enemyTarget.Remove(target);
        if (enemyTarget.Count > 0)
            isShooting = true;
        else
            isShooting = false;
    }
    /// <summary>
    /// Return the amount of damage to make the objetive.
    /// </summary>
    /// <returns></returns>
    public float getDamage()
    {
        return damage;
    }
    /// <summary>
    /// Setter for isShooting variable.
    /// </summary>
    /// <param name="flag"></param>
    public void setIsShooting(bool flag)
    {
        isShooting = flag;
    }

    /// <summary>
    /// Checks in which direction the target is relative to the gun.
    /// </summary>
    /// <param name="fwd"></param>
    /// <param name="targetDir"></param>
    /// <param name="up"></param>
    /// <returns></returns>
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    #endregion


    //manual gameplay
    #region

    /// <summary>
    /// Rotates the mobile part of the turret by a given amount in each axis.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void rotate(float x, float y)
    {
        targetArm.addRotation(x,y,0);
    }
    /// <summary>
    /// Called from the gameModeManager in ShootMode. checks the inputs to shoot.
    /// </summary>
    /// <param name="pressed"></param>
    public void playManually(bool pressed)
    {
        if (pressed)
        {
            if (canShoot)
                StartCoroutine(pressTrigger());
        }

        RaycastHit hit;
        Ray ray = new Ray(canon.position,canon.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        //if(Physics.Raycast(ray, out hit))
        {
            aimReticle.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(hit.point);       

        }


    }
    /// <summary>
    /// Activates or deactivates the aimReticle.
    /// </summary>
    /// <param name="activate"></param>
    public void activateAimReticle(bool activate)
    {
        aimReticle.SetActive(activate);
    }


    #endregion
}
