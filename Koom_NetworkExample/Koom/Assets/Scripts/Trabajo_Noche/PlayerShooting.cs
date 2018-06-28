using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField]
    float shotCooldown = 0.3f;
    [SerializeField]
    Transform muzzle;
    [SerializeField]
    ShotEffectsManager shotEffects;


    [SyncVar (hook = "OnScoreChange")]
    int score;

    float ellapsedTime;
    bool canShoot;

    [SerializeField]
    int maxAmmo = 200;

    [SyncVar (hook = "OnAmmunitionChange")]
    int currentAmmo = 200;

    void Start()
    {
        shotEffects.Initialize();
        if (isLocalPlayer)
        {
            canShoot = true;
            currentAmmo = maxAmmo;
            UiController.instance.SetAmmoText(currentAmmo);
        }        
    }

    [Server]
    void OnEnable()
    {
        score = 0;
        if (isLocalPlayer)
        {
            currentAmmo = maxAmmo;
            UiController.instance.SetAmmoText(currentAmmo);
        }
    }

    void Update()
    {
        if (!canShoot) return;

        ellapsedTime += Time.deltaTime;

        if(Input.GetButton("Fire1") && ellapsedTime > shotCooldown && currentAmmo > 0)
        {
            ellapsedTime = 0f;
            CmdFireShot(muzzle.position, muzzle.forward);
        }
    }

    [Command]
    public void CmdChangeAmmo(int value)
    {
        if(currentAmmo + value < maxAmmo)
        {
            currentAmmo += value;
        }
        else
        {
            currentAmmo = maxAmmo;
        }
    }
    
    [Command] // Command sirve para enviar información al servidor
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        // reducimos la municion
        currentAmmo--;

        RaycastHit hit;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 1f);

        bool hasHit = Physics.Raycast(ray, out hit, 50f);

        if (hasHit)
        {
            PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

            if(enemy != null)
            {
                if (isLocalPlayer)
                {
                    //UiController.instance.Hitmarker();
                }
                // el sevidor desde el cliente [Command] le dice al servidor (TakeDamage) [Server] que aplique daño
                bool wasKillShot = enemy.TakeDamage(10);

                if (wasKillShot)
                    score++;
            }
        }

        RpcProcessShotEffect(hasHit, hit.point);
    }

    [ClientRpc] // esto sirve para que el servidor le diga a todos los clientes algo
    void RpcProcessShotEffect(bool playImpact, Vector3 point)
    {
        shotEffects.PlayShotEffects();

        if (playImpact)
            shotEffects.PlayImpactEffect(point);
    }

    void OnScoreChange(int value)
    {
        score = value;
        if(isLocalPlayer)
        {
            //canvas
        }
    }

    void OnAmmunitionChange(int value)
    {
        currentAmmo = value;
        if(isLocalPlayer)
        {
            UiController.instance.SetAmmoText(currentAmmo);
        }
    }
}
