using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droppedWeapon : MonoBehaviour {

    [SerializeField] private GameObject weapon;


    public GameObject getWeapon()
    {
        return weapon;
    }
}
