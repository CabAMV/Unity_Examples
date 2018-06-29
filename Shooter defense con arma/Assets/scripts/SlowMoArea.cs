using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowMoArea : MonoBehaviour {
    private int tiempodevida;
    private bool desapareciendo;
	// Use this for initialization
	void Start () {
        tiempodevida = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.localScale.x<8 && !desapareciendo)
                 transform.localScale += new Vector3(0.3F, 0.3f, 0.3f);
        tiempodevida++;
        if (tiempodevida > 1000 && !desapareciendo)
        {
           StartCoroutine(destruir());
            desapareciendo = true;
        }
            

    
    }

    
    void OnTriggerEnter(Collider Enemigo)
    {
        if (Enemigo.gameObject.tag == "Enemy1")
        {

            Enemigo.gameObject.GetComponent<scriptNavEnem>().setAgentSpeed(Enemigo.gameObject.GetComponent<scriptNavEnem>().speed - Enemigo.gameObject.GetComponent<scriptNavEnem>().speed * 0.6f);
        }


    }

    void OnTriggerExit(Collider Enemigo)
    {

        if (Enemigo.gameObject.tag == "Enemy1")
        {

            Enemigo.gameObject.GetComponent<scriptNavEnem>().setAgentSpeed(Enemigo.gameObject.GetComponent<scriptNavEnem>().speed);
        }



    }

    IEnumerator destruir()
    {
        do
        {
            transform.localScale -= new Vector3(0.2F, 0.2f, 0.2f);
            yield return new WaitForSeconds(0.001f);
        } while (this.transform.localScale.x > 0.1);
        Destroy(this.gameObject);

    }
}
