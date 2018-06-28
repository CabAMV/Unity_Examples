using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the trigger events of a turret.
/// </summary>
public class ConicVision : MonoBehaviour {
    /// <summary>
    /// Turret that owns this trigger.
    /// </summary>
    TurretMiniGun parent;

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<TurretMiniGun>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.GetComponent<Tree>() != null)
        {
            return;
        }
        if (other.GetComponent<Collider>().gameObject.GetComponent<IDamageable>() != null)
        {
			if(other.GetComponent<Collider>().gameObject.GetComponent<IDamageable>().getPlayer() != parent.getPlayer())
			{
				parent.addTarget(other.transform);
			}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.GetComponent<IDamageable>()!=null)
        {
			if(other.GetComponent<Collider>().gameObject.GetComponent<IDamageable>().getPlayer() != parent.getPlayer())
			{
				parent.removeTarget(other.transform);
			}
        }
    }
}
