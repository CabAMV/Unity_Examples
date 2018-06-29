using UnityEngine;
using System.Collections;

public class scriptDisparo : MonoBehaviour {
    
	public GameObject weapon;
    public bool actualizable;
	private bool canShoot=true;
    public Camera camara;
    public float cadencia=1;
    private bool recalentado = false;
    private int recalentamiento;
    public int recalentamientoMAX=10;
    private RaycastHit hit;
    
    public GameObject sPart;
	private Transform retrocesoMAX;
	public float daño;
    public int nBalas;
    public int maxExp;
    private int exp;
    public GameObject actualizacion;
    
    
    
    // Use this for initialization
	void Start () {
        recalentamiento = recalentamientoMAX;
        

		retrocesoMAX = weapon.transform;
		retrocesoMAX.Rotate (0.1f,0,0); 
    }
	
	// Update is called once per frame
	void Update () {
		apuntar ();
        Shoot();
        actualizar();
		

        if (recalentamiento == 0)
        {
            recalentado = true;
            
        }
            

        
    }

    void actualizar()
    {
        if (exp == maxExp && actualizable)
        {
            actualizacion.SetActive(true);
            GameManager.Instance.actualizarTextoFusil();
            this.gameObject.SetActive(false);
            
        }

    }

    void Shoot()
    {
        if (recalentamiento < 0)
            recalentamiento = 0;
        if (Input.GetMouseButton(0) && canShoot && !recalentado)
        {
            StartCoroutine(disparar());
            StartCoroutine(retroceder());
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (recalentamiento < recalentamientoMAX && recalentamiento > 0 && !Input.GetMouseButton(0))
            {
                StartCoroutine(enfriar1());
            }
            else if (recalentamiento == 0)
                StartCoroutine(enfriar());
        }

        

    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy1")
        {
			other.GetComponent<EnemigoScript>().restarVida(daño);
            exp++;
            if (other.GetComponent<EnemigoScript>().getLife()<=0)
            {
                Instantiate(sPart, other.transform.position, Quaternion.identity);
                GameManager.Instance.addMoney(other.GetComponent<EnemigoScript>().getMoney());              
                Destroy(other.gameObject);


            }
        }

    }

    IEnumerator retroceder()
	{
        
        int contador = 0;
        do
        {
            weapon.transform.Rotate(-0.5f, 0, 0);
            yield return new WaitForSeconds(0.01f);
            contador++;
        } while (contador < 5);
        do
        {
            weapon.transform.Rotate(0.5f, 0, 0);
            yield return new WaitForSeconds(0.005f);
            contador--;
        } while (contador > 0);



    }

	IEnumerator enfriar1()
    {
        do 
         {
            recalentamiento += 1;
			yield return new WaitForSeconds(cadencia);

        }
        while (recalentamiento <recalentamientoMAX && !Input.GetMouseButton(0));
        
    }

    IEnumerator enfriar()
    {    
        yield return new WaitForSeconds(1);
        recalentado = false;
        recalentamiento = recalentamientoMAX;
    }



    IEnumerator disparar(){
		canShoot = false;
        this.GetComponent<ParticleSystem>().Emit(nBalas);
        recalentamiento--;
        
		
		yield return new WaitForSeconds (cadencia);
        
        canShoot = true;
	
	}

	void apuntar(){

        if (Physics.Raycast(camara.transform.position, camara.transform.TransformDirection(Vector3.forward), out hit, 1000))
        {
            this.transform.rotation = Quaternion.LookRotation((hit.point - this.transform.position), Vector3.up);

        }


	}
    public string getCalor()
    {
        return recalentamiento.ToString();

    }
	 
}
