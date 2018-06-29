using UnityEngine;
using System.Collections;

public class EnemigoScript : MonoBehaviour {

	public float Life;
	private bool muerto;
    private int moneyLoot;

    // Use this for initialization
	void Start () {
        muerto = false;

	}
	
	// Update is called once per frame
	void Update () {
        
        
        
	}
	public void restarVida(float resto)
    {
		Life-=resto;
    }

    public void setVida(float Life)
    {
        this.Life = Life;
    }
      
    public float getLife()
    {
        return Life;

    }
    public void setMoney(double money)
    {
        moneyLoot =(int) money;
    }
    public int getMoney()
    {

        return moneyLoot;
    }
    public void setMuerto()
    {
        muerto = true;
    }

    public bool getMuerto()
    {
        return muerto;
    }
}
