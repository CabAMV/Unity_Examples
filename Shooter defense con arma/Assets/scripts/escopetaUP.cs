using UnityEngine;
using System.Collections;

public class escopetaUP : MonoBehaviour {

    public GameObject bala;
    public float delay;
    private bool canShoot;
    public Camera camara;
    private RaycastHit hit;
    public GameObject muro;
    public GameObject Torreta;


    // Use this for initialization
    void Start () {
        canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
        apuntar();
        
        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(Delay());
            Instantiate(bala,this.transform.position,this.transform.rotation);


        }
	}

    IEnumerator Delay()
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    void apuntar()
    {
        

        if (Physics.Raycast(camara.transform.position, camara.transform.TransformDirection(Vector3.forward), out hit, 1000))
        {
            this.transform.rotation = Quaternion.LookRotation((hit.point - this.transform.position), Vector3.up);

        }


    }
   
    
}
