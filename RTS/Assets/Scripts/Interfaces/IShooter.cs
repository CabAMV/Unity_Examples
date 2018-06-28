using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used by those classes that shoot projectiles and have lists of targets to shoot.
/// </summary>
public interface IShooter{

    void addTarget(Transform target);
    void removeTarget(Transform target);
    float getDamage();
    Player getPlayer();
    
}
