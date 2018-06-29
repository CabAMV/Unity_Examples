using UnityEngine;
using System.Collections;

public class PonerTorretas : MonoBehaviour {
    private RaycastHit hit;
    public GameObject muro;
    public GameObject Torreta;
    public GameObject slowMoArea;
    public Camera camara;
    private int estado;
    // Use this for initialization
    void Start () {
        estado = 1;
    }
	
	// Update is called once per frame
	void Update () {
        cambiarEstado();

        if (Input.GetButtonDown("Fire2"))
        {
            muroTurret();
            SlowMoArea();

            

        }

    }
    public void muroTurret()
    {
        if (Physics.Raycast(camara.transform.position, camara.transform.TransformDirection(Vector3.forward), out hit, 1000) && estado == 1)
        {

            if (hit.collider.tag == "Suelo" && GameManager.Instance.getMoney() >= 25)
            {
                Instantiate(muro, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 0.5f, hit.collider.transform.position.z), Quaternion.identity);
                GameManager.Instance.restarMoney(25);
            }

            if (hit.collider.tag == "muro" && !hit.collider.GetComponent<muroScript>().getOcupado() && GameManager.Instance.getMoney() >= 100 && !Input.GetKey(KeyCode.E))
            {
                hit.collider.GetComponent<muroScript>().setOcupado(true);
                GameObject temp;
                temp = (GameObject)Instantiate(Torreta, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 1.5f, hit.collider.transform.position.z), Quaternion.identity);

                hit.collider.GetComponent<muroScript>().setTurret(temp);
                GameManager.Instance.restarMoney(100);
            }

            if (Input.GetKey(KeyCode.E) && hit.collider.tag == "muro")
            {

                if (hit.collider.GetComponent<muroScript>().getOcupado())
                {
                    Destroy(hit.collider.GetComponent<muroScript>().getTurret().GetComponent<TurretScript>().getTemp().gameObject);
                    Destroy(hit.collider.GetComponent<muroScript>().getTurret().gameObject);

                    GameManager.Instance.addMoney(50);
                }


                GameManager.Instance.addMoney(10);


                Destroy(hit.collider.gameObject);
            }
        }
    }
    public void SlowMoArea()
    {
        if (Physics.Raycast(camara.transform.position, camara.transform.TransformDirection(Vector3.forward), out hit, 1000) && estado == 2)
            if (GameManager.Instance.getMoney() >= 50)
            {
                GameManager.Instance.restarMoney(50);
                Instantiate(slowMoArea, hit.point, Quaternion.identity);

            }

    }

    public void cambiarEstado()
    {
        if (Input.GetKeyDown(KeyCode.Q) && estado < 2)
            estado++;
        else if (Input.GetKeyDown(KeyCode.Q) && estado == 2)
            estado = 1;
    }


}
