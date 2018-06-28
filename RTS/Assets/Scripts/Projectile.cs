using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funtionality for a projectile object type.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Object that spawns the projectile.
    /// </summary>
    private IShooter instigator;
    public IShooter Instigator
    {
        get
        {
            return instigator;
        }

        set
        {
            instigator = value;
        }
    }
    /// <summary>
    /// Life time of the projectile
    /// </summary>
    private float lifeTime = 5;

    private void Start()
    {
        Invoke("Die",lifeTime);

        Soldier[] soldiers = FindObjectsOfType(typeof(Soldier)) as Soldier[];
        foreach (Soldier soldier in soldiers)
        {
            if (soldier is IDamageable)
                if ((soldier as IDamageable).getPlayer() == instigator.getPlayer())
                    Physics.IgnoreCollision(soldier.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Ignores the collision with the given object.
    /// </summary>
    /// <param name="turret"></param>
    public void ignore(Collider turret)
    {
        Physics.IgnoreCollision(turret, GetComponent<Collider>(), true);
    }

    /// <summary>
    /// Checks if the collided object is damageable and then applies damage.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            if (collision.gameObject.GetComponent<IDamageable>().getPlayer() != instigator.getPlayer())
            {
                collision.gameObject.GetComponent<IDamageable>().ApplyDamage(instigator);
            }
        }
        //collision.gameObject.GetComponent<Worker>().removeSelf();
        Die();
    }
}
