using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class VidaRestante : MonoBehaviour {
    public int vidaJugador;
    public Text texto;
	// Use this for initialization
	void Start () {
        texto.text =  "20 / 20";

    }

    // Update is called once per frame
    void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy1")
        {
            vidaJugador--;
            texto.text = vidaJugador.ToString() + " / 20";
            other.GetComponent<scriptNavEnem>().morir();
        }
            


        if (vidaJugador == 0)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
