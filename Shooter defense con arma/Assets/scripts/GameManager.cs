using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    static GameManager instance;
    public int inicialMoney;
    private int totalMoney;
    public Text textoDinero;
    private int armaActual;
    public GameObject fusil;
    public GameObject escopeta;
    public GameObject textoFusil;




    public static GameManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        //Asigna esta instancia al campo instance
        if (instance == null)
            instance = this;
        else
            Destroy(this);  //Garantiza que sólo haya una instancia de esta clase

    }
    void Start()
    {
        totalMoney = inicialMoney;
        textoDinero.text = totalMoney.ToString() +"  " +"$";
        armaActual = 1;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Alpha1) && armaActual == 2)
        {
            armaActual = 1;
            escopeta.SetActive(false);
            fusil.SetActive(true);
        }
            
        if (Input.GetKey(KeyCode.Alpha2) && armaActual==1)
        {
            armaActual = 2;
            escopeta.SetActive(true);
            fusil.SetActive(false);
        }


    }

    public int  getMoney()
    {
        return totalMoney;

    }

    public void addMoney(int money)
    {
        this.totalMoney += money;
        textoDinero.text = totalMoney.ToString() + "  " + "$";

        
    }
    public void restarMoney(int money)
    {
        totalMoney -= money;
        textoDinero.text = totalMoney.ToString() + "  " + "$";
    }

    public void actualizarTextoFusil()
    {
        textoFusil.GetComponent<Texto>().siguienteSistema();
    }
}
