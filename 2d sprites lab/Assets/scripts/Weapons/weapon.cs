using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {

    private Vector3 recoilVector;
    private Transform shootPoint;
    private Transform grabPoint;
    private GameObject rightArm;
    private GameObject leftArm;
    private GameObject currentArm;
    private GameObject deadArm;

    private bool isShooting;
    private bool coroutineRunning;
    [SerializeField] private int ammoCapacity;
    [SerializeField] private float fireRate;
    [SerializeField] private float triggerRate;
    [SerializeField] private int spread;
    [SerializeField] private float recoilForce;



    [SerializeField] private bool semiautomatic;
    [SerializeField] private bool raycastedShoot;

    private int currentAmmo;
    private bool canShoot;

    [SerializeField] private GameObject bulletTrailSprite;
    private GameObject bulletTrail;

    [SerializeField] private GameObject Projectile;

    // Use this for initialization
    void Start() {
        recoilVector = new Vector3(0, 0, 0);
        isShooting = false;
        coroutineRunning = false;

        currentAmmo = ammoCapacity;
        canShoot = true;
        if (raycastedShoot)
        {
            bulletTrail = Instantiate(bulletTrailSprite);
            bulletTrail.SetActive(false);
        }

        rightArm = transform.Find("ArmR").gameObject;
        leftArm = transform.Find("ArmL").gameObject;
        currentArm = rightArm;
        deadArm = this.transform.parent.Find("body/deadArm").gameObject;
        shootPoint = currentArm.transform.Find("ShootPoint");
        grabPoint=currentArm.transform.Find("GrabPoint");

        if (this.transform.parent.GetComponent<CharacterMovement>().IsFlipped())
            this.flip();
    }

    // Update is called once per frame
    void Update() {
        if (this.isShooting)
            pullTrigger();
        Vector3 dir = grabPoint.position - deadArm.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        deadArm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void weaponImputs(string shotButton)
    {
        if (Input.GetButtonDown(shotButton))
        {
            this.isShooting = true;
        }
        if (Input.GetButtonUp(shotButton))
        {
            this.isShooting = false;
            //this.canShoot = true;
            if (!coroutineRunning)
            {
                StartCoroutine(applyTriggerRate(triggerRate));
            }
                
        }

    }

    public void pullTrigger()
    {
        if (currentAmmo > 0)
        {
            if (canShoot)
            {
                shoot();
            }

        }
    }

    public void shoot()
    {
        canShoot = false;
        applyRecoil();
        float spreadAngle = Random.Range(-(spread / 2), (spread / 2));
        Vector3 spreadVector = Quaternion.AngleAxis(spreadAngle, shootPoint.forward) * shootPoint.right;
        if (raycastedShoot)
            raycastShoot(spreadVector);
        else
            projectileShoot(spreadVector);
        currentAmmo--;
        if(semiautomatic)
            StartCoroutine(applyRate(fireRate));
    }

    private void raycastShoot(Vector3 spreadVector)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(shootPoint.position, spreadVector);
        StartCoroutine(showBullet(spreadVector));
        if (hitInfo)
        {
            if (hitInfo.collider.CompareTag("Terrain"))
            {
                hitInfo.collider.GetComponent<TerrainScript>().destroyTerrain(hitInfo.point.x, hitInfo.point.y, 20);

            }
        }
    }

    private void projectileShoot(Vector3 spreadVector)
    {
        Vector3 rotation = shootPoint.transform.right+ spreadVector;
        GameObject projectile=Instantiate(Projectile,shootPoint.position,Quaternion.Euler(rotation));
        projectile.GetComponent<Projectile>().Init(shootPoint,spreadVector,this.transform.parent.gameObject);


    }
    private void applyRecoil()
    {
        recoilVector = Quaternion.AngleAxis(-30, shootPoint.forward) * (-shootPoint.right);
        currentArm.GetComponent<Rigidbody2D>().AddForceAtPosition(recoilVector * recoilForce, shootPoint.position);

    }

    public void flip()
    {
        if (rightArm.activeSelf)
        {
            rightArm.SetActive(false);
            leftArm.SetActive(true);
            currentArm = leftArm;
            shootPoint = currentArm.transform.Find("Rotator/ShootPoint");
            deadArm = this.transform.parent.Find("body/deadArm").gameObject;
            grabPoint = currentArm.transform.Find("Rotator/GrabPoint");
            return;
        }

        else
        {
            rightArm.SetActive(true);
            leftArm.SetActive(false);
            currentArm = rightArm;
            shootPoint = currentArm.transform.Find("ShootPoint");
            deadArm = this.transform.parent.Find("body/deadArm").gameObject;
            grabPoint = currentArm.transform.Find("GrabPoint");
            return;
        }
    }

    private IEnumerator showBullet(Vector3 spreadVector)
    {

        bulletTrail.SetActive(true);
        bulletTrail.transform.position = shootPoint.position + (spreadVector*0.75f);
        bulletTrail.transform.rotation = Quaternion.LookRotation(bulletTrail.transform.position - shootPoint.position);
        bulletTrail.transform.Rotate(0,90,0);
        yield return new WaitForSeconds(0.1f);
        bulletTrail.SetActive(false);
    }

    private IEnumerator applyRate(float s)
    {
        yield return new WaitForSeconds(s);
        canShoot = true;
    }

    private IEnumerator applyTriggerRate(float s)
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(s);
        canShoot = true;
        coroutineRunning = false;
    }
}
