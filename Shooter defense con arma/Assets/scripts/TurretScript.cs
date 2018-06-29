using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretScript : MonoBehaviour {

    public ParticleSystem Cañon;
    private List<Collider> ListaEnemigos;
    Transform temp;
    public Transform torreta;
	private bool canShoot;
	public float cadencia;
	public float daño;
	
    private GameObject muro;


    // Use this for initialization
    void Start () {
        ListaEnemigos = new List<Collider>();
        temp = new GameObject().transform;
		canShoot = true;
		Cañon.GetComponent<particlecollision> ().setDaño (daño);
		

    }

    // Update is called once per frame
    void Update()
    {
		if(ListaEnemigos.Count>0 && ListaEnemigos[0]==null)
			ListaEnemigos.Remove(ListaEnemigos[0]);

		if (ListaEnemigos.Count>0 && ListaEnemigos[0]!=null)
         {
            

            temp.position = torreta.transform.position;
            temp.LookAt(ListaEnemigos[0].transform);

            torreta.transform.rotation=Quaternion.Slerp(torreta.transform.rotation,temp.transform.rotation , Time.deltaTime * 30);
            
        }

    }

    public void EliminarPosicion(GameObject other)
    {
        if(ListaEnemigos.Count>=1)
               ListaEnemigos.Remove(ListaEnemigos[0]);
    }

    public void setMuro(GameObject other)
    {
        
        muro = other;
        
    }

    public void setMorir()
	{
        muro.GetComponent<muroScript>().setOcupado(false);
		Destroy (temp.gameObject);

	}
    public GameObject getTemp()
    {
        return temp.gameObject;

    }
	void OnTriggerEnter(Collider Enemigo)
    { 
        if (Enemigo.gameObject.tag =="Enemy1")
        {
            
            ListaEnemigos.Add(Enemigo);
            
        }
		

    }
		
    void OnTriggerExit(Collider other)
    {
        
        if (ListaEnemigos.Count>0 )
			ListaEnemigos.Remove(other);

        

    }

    void OnTriggerStay(Collider other)
    {

		if (ListaEnemigos.Count > 0 && other.Equals(ListaEnemigos[0]) && canShoot )
        {
			StartCoroutine (disparar());
            

            
        }
    }

	IEnumerator disparar(){
		canShoot = false;
		Cañon.Emit(1);

		yield return new WaitForSeconds (cadencia);

		canShoot = true;

	}

}
