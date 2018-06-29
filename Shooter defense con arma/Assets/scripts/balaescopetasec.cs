using UnityEngine;
using System.Collections;

public class balaescopetasec : MonoBehaviour {
    private float speed;
    public float aceleracion;
    public int daño;
    public GameObject sPart;
    private GameObject objetivo;
    private bool inUse;


    // Use this for initialization
    void Start()
    {
        this.transform.rotation=Random.rotation;
        speed = Random.Range(0.1f,0.2f);
        inUse = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        speed = speed + aceleracion * Time.deltaTime;
        this.transform.position = this.transform.position + (transform.forward * speed);
        if(speed<=0 && objetivo==null)
        {
            speed = 0;
            aceleracion = 0;
        }
        
            StartCoroutine(destruir());

        if (objetivo != null)
        {
            this.transform.LookAt(objetivo.transform.position);
            inUse = true;
            this.GetComponent<SphereCollider>().radius = 0.05f;
            aceleracion = 0.5f;
        }
            


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy1")
            objetivo = other.gameObject;
        if (other.gameObject.tag == "Enemy1" && inUse)
        {
            if (other.gameObject.tag == "Enemy1")
            {
                other.gameObject.GetComponent<EnemigoScript>().restarVida(daño);
                if (other.gameObject.GetComponent<EnemigoScript>().getLife() <= 0 && !other.gameObject.GetComponent<EnemigoScript>().getMuerto())
                {
                    other.gameObject.GetComponent<EnemigoScript>().setMuerto();
                    Instantiate(sPart, other.transform.position, Quaternion.identity);
                    GameManager.Instance.addMoney(other.gameObject.GetComponent<EnemigoScript>().getMoney());
                    Destroy(other.gameObject);
                }
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);



            }


        }

    }

    IEnumerator destruir()
    {

        yield return new WaitForSeconds(5);
        if (!inUse)
            Destroy(this.gameObject);
    }

    
}
