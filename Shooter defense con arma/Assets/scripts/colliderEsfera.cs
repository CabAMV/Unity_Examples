using UnityEngine;
using System.Collections;

public class colliderEsfera : MonoBehaviour {

	

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "muro" || other.tag == "Torreta" )
        {
			if (other.tag == "Torreta") {
				other.GetComponent<TurretScript> ().setMorir ();
			}
				
            Destroy(other.gameObject);
        }
    }
}
