using UnityEngine;
using System.Collections;

public class ExplosionParticulasScript : MonoBehaviour {

	public ParticleSystem [] sPart ;
    public int numero;
    public float repeticiones;
	// Use this for initialization
	void Start () {
		StartCoroutine (explotar());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator explotar()
	{
		int i = 0;
		do{
			sPart [i].Emit (numero);
			yield return new WaitForSeconds (0.1f);
			i++;

		}while(i<repeticiones);
		yield return new WaitForSeconds (2f);
		Destroy (this.gameObject);
	
	}

}
