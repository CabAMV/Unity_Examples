using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleComponentsEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{
    [SerializeField]
    ToggleComponentsEvent onToggleShared;
    [SerializeField]
    ToggleComponentsEvent onToggleLocal;
    [SerializeField]
    ToggleComponentsEvent onToggleRemote;

    [SerializeField]
    float respawnTime = 3f;

    void Start()
    {
        EnablePlayer();
    }

    void DisablePlayer()
    {
        if(isLocalPlayer)
        {
            // canvas
        }

        onToggleShared.Invoke(false);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(false);
        }
        else
        {
            onToggleRemote.Invoke(false);
        }
    }

    void EnablePlayer()
    {
        if (isLocalPlayer)
        {
            // canvas
        }

        onToggleShared.Invoke(true);

        if(isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
        }
        else
        {
            onToggleRemote.Invoke(true);
        }
    }

    public void Die()
    {
        if(isLocalPlayer)
        {
            // canvas
        }

        DisablePlayer();
        
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        if(isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
        }

        EnablePlayer();
    }
}
