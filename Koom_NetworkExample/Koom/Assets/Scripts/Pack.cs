using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pack : NetworkBehaviour
{
    public enum ResourceType        
    {
        HEALTH,
        ARMOR,
        AMMO
    };

    public ResourceType r_type;

    [SerializeField]
    private int resource;
    
    public int GetResource()
    {
        return resource;
    }

    public void Start()
    {
        switch (r_type)
        {
            case ResourceType.HEALTH:
                {
                    GetComponent<Renderer>().material.color = Color.green;
                }
                break;

            case ResourceType.ARMOR:
                {
                    GetComponent<Renderer>().material.color = Color.blue;
                }
                break;

            case ResourceType.AMMO:
                {
                    GetComponent<Renderer>().material.color = Color.red;
                }
                break;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            switch (r_type)
            {
                case ResourceType.HEALTH:
                {
                    col.gameObject.GetComponent<PlayerHealth>().ChangeHealth(resource);
                    CmdDestroyObj(gameObject);
                }
                break;

                case ResourceType.ARMOR:
                {
                    col.gameObject.GetComponent<PlayerHealth>().ChangeArmor(resource);
                    CmdDestroyObj(gameObject);
                }
                break;

                case ResourceType.AMMO:
                {
                    col.gameObject.GetComponent<PlayerShooting>().CmdChangeAmmo(resource);
                    CmdDestroyObj(gameObject);
                }
                break;
            }
        }
    }

    [Command]
    public void CmdDestroyObj(GameObject obj)
    {   
        NetworkServer.Destroy(obj);        
    }

}
