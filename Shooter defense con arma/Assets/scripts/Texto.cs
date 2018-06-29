using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Texto : MonoBehaviour {
    //public GameObject sistema;
    public List<GameObject> sistema;
    private int i;
	// Use this for initialization
	void Start () {
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
        this.GetComponent<TextMesh>().text = sistema[i].GetComponent<scriptDisparo>().getCalor();
        

    }
    public void siguienteSistema()
    {
            i++;
    }
}
