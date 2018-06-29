using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	private float delay;
	public GameObject Enemy;
	public GameObject destino1;
	public GameObject destino2;
    public Text TextoRonda;
    private int Ronda;
    private int enPerRound;
    private bool jugando;
    private float vidaEnem;
    private double moneyLoot;
    private  float speed;

    // Use this for initialization
    void Start () {
        Ronda = 1;
        enPerRound = 5;
        jugando = true;
        vidaEnem = 10;
        moneyLoot=10;
        delay = 3;
        speed = 4f;
        StartCoroutine(generarEnemigos());
	}
	
	// Update is called once per frame
	void Update () {
        TextoRonda.text = Ronda.ToString();
	}

	IEnumerator generarEnemigos()
	{
        int i= 0;

        do
        {
            do
            {
                GameObject go = (GameObject)Instantiate(Enemy, this.transform.position, Quaternion.identity);
                scriptNavEnem navegacion = go.GetComponent<scriptNavEnem>();
                EnemigoScript enemigo = go.GetComponent<EnemigoScript>();
                navegacion.destino1 = destino1;
                navegacion.destino2 = destino2;
                
                enemigo.setVida(vidaEnem);
                enemigo.setMoney(moneyLoot);
                navegacion.IniciarMovimiento();
                navegacion.setAgentSpeed(speed);
                navegacion.setSpeed(speed);
                i++;
                yield return new WaitForSeconds(delay);

            } while (i < enPerRound);
            if (enPerRound < 31)
            {
                enPerRound++;
            }

            vidaEnem=vidaEnem+(vidaEnem*0.1f);
            i = 0;
            if (delay>=0.7f )
            {
                delay -= 0.5f;
            }
            if(Ronda<=10)
                moneyLoot = moneyLoot + (moneyLoot * 0.05f);
            if(speed<7)
                speed = speed + (speed * 0.1F);
            yield return new WaitForSeconds(15);
            Ronda++;
            
        } while (jugando);
	}
}
