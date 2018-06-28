using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDecal : MonoBehaviour {

    private float standarSize;
	// Use this for initialization
	void Start () {
        standarSize = 50;
	}

    public float getDecalScale(float blastRadius)
    {
        return blastRadius / 50;
    }
}
