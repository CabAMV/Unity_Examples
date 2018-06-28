
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    /// <summary>
    ///  cantidad maxima de vida
    /// </summary>
    [SerializeField]
    int maxHealth = 100;
    /// <summary>
    /// cantidad maxima de escudo
    /// </summary>
    [SerializeField]
    int maxArmor = 100;

    /// <summary>
    /// vida actual del jugador
    /// </summary>
    [SyncVar(hook = "OnHealthChange")] // esta variable solo puede ser modificada por el servidor que sincronizará con los jugadores locales
    int currentHealth;

    /// <summary>
    /// escudo actual del jugador
    /// </summary>
    [SyncVar(hook = "OnShieldChange")]
    int currentArmor;

    /// <summary>
    /// referencia al jugador para decirle cuando ha muerto
    /// </summary>
    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    [ServerCallback] // esto indica que este codigo solo lo va a poder usar el servidor
    void OnEnable()
    {
        //if (isLocalPlayer)

            currentHealth = maxHealth;
            currentArmor = maxArmor;
                //UiController.instance.ReScaleBar("health", maxHealth);
        //UiController.instance.ReScaleBar("armor", maxArmor);
    }

    //[ServerCallback]
    //void Start()
    //{
    //    if(isLocalPlayer)
    //    {
    //        currentHealth = maxHealth;
    //        currentArmor = maxArmor;
    //    }        
    //    UiController.instance.ReScaleBar("health", maxHealth);
    //    UiController.instance.ReScaleBar("armor", maxArmor);
    //}

    [Server]
    public void ChangeHealth(int value)
    {
        if(currentHealth + value < maxHealth)
        {
            currentHealth += value;
        }
        else
        {
            currentHealth = maxHealth;
        }
    }

    [Server]
    public void ChangeArmor(int value)
    {
        if (currentArmor + value < maxArmor)
        {
            currentArmor += value;
        }
        else
        {
            currentArmor = maxArmor;
        }
    }

    /// <summary>
    /// solo queremos que el servidor (al conocer los "hits" de enemigo) haga daño al jugador
    /// </summary>
    /// <returns></returns>
    [Server]
    public bool TakeDamage(int dmg)
    {
        // comprobamos si hemos muerto
        bool died = false;

        // si aun tenemos escudo
        if (currentArmor > 0)
        {
            currentArmor -= (int)(dmg * 0.8f);
        }
        else
        {
            // si no tenemos vida
            if (currentHealth <= 0)
            {
                // entonces es que hemos muerto y no debería aplicarse el daño
                return died;
            }

            // quitamos vida
            currentHealth -= dmg;
            // si la vida es menor o igual que cero, entonces estamos muertos
            died = (currentHealth <= 0);
        }

        // replicamos la muerte en el cliente para que maneje el Despawn y el Respawn
        RpcTakeDamage(died);

        // devolvemos si hemos muerto o no
        return died;
    }

    /// <summary>
    /// Replicamos en el Cliente el daño y si hemos muerto
    /// </summary>
    /// <param name="died"></param>
    [ClientRpc]
    void RpcTakeDamage(bool died)
    {
        if (isLocalPlayer)
        {
            //canvas
            //si quieremos meterle un flash effect ( un pantallazo rojo )
            //tambien podemos meterle un flash para si le quitan escudo
        }

        if (died)
        {
            player.Die();
        }
    }

    // el problema que hay con las variables "sync" es que solo el servidor sabe su estado y hasta que lo sincroniza puede pasar un tiempo
    // con lo que en la pantalla del jugador puede no salir la informacion real en ese momento
    // para ello vamos atener un metodo que actualize la vida independientemente de cuanto tiempo le lleve al servidor actualizarla
    // para ello añadimos la la "syncvar" un metodo que tiene que llamar cuando se actualiza
    void OnHealthChange(int value)
    {
        currentHealth = value;
        RpcShowData();
        if (isLocalPlayer)
        {
            //canvas
            UiController.instance.ReScaleBar("health", currentHealth);
        }
    }

    void OnShieldChange(int value)
    {
        currentArmor = value;
        RpcShowData();
        if (isLocalPlayer)
        {
            UiController.instance.ReScaleBar("armor", currentArmor);
        }
    }

    [ClientRpc]
    void RpcShowData()
    { print(netId+"  "+currentHealth+"  "+currentArmor); }
}
