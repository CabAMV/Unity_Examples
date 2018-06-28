using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used by those objects susceptible to be damage.
/// </summary>
public interface IDamageable
{
    void ApplyDamage(IShooter instigator);
    Player getPlayer();
}
