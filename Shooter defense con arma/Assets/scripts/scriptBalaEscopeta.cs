using UnityEngine;
using System.Collections;

public class scriptBalaEscopeta : MonoBehaviour {
    public float speed;
    public float aceleracion;
    public GameObject bala;
    

	
    // Use this for initialization
	void Start () {
       
        
	}
	
	// Update is called once per frame
	void Update () {
        speed = speed + aceleracion * Time.deltaTime;
        this.transform.position = this.transform.position + (transform.forward * speed);
        if (speed <= 0)
        {
            speed = 0;
            aceleracion = 0;
        }

        if (speed == 0)
        {
            for (int i = 0; i <20; i++)
            {
                Instantiate(bala,this.transform.position,Quaternion.identity);
                Destroy(this.gameObject);
            }
        }  
    }

   
    void OnTriggerEnter()
    {
        speed = 0;
        aceleracion = 0;
    }
}
