using UnityEngine;
using System.Collections;

public class muroScript : MonoBehaviour {

	public bool ocupado;
    private GameObject Turret;
	// Use this for initialization
	void Start () {
		ocupado = false;
        
	}
   
	public bool getOcupado(){
		return ocupado;
	}
	public void setOcupado(bool j){
		ocupado = j;
	}
    public void setTurret(GameObject torreta)
    {
        Turret = torreta;
        setMuro();
    }
    public GameObject getTurret()
    {
        return this.Turret;
    }

    void setMuro() {
        Turret.GetComponent<TurretScript>().setMuro(this.gameObject);

    }
    

}
