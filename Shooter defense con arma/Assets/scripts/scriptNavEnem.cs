using UnityEngine;
using System.Collections;

public class scriptNavEnem : MonoBehaviour {

    private NavMeshAgent agent;
    public GameObject destino1;
	public GameObject destino2;
    Transform destinoActual;
	public float speed = 4f;
    public GameObject esfera;
    private SphereCollider area;
    public GameObject explosion;
    private GameObject temp;
    private bool explotado;
    

	enum State { Idle, Moving }
	State state;

    // Use this for initialization
	void Await () 
	{
		state = State.Idle;
	}


    public void IniciarMovimiento()
	{
		agent = this.GetComponent<NavMeshAgent>();
		agent.SetDestination(destino1.transform.position);
		
		explotado = false;
		esfera.SetActive(false);
		state = State.Moving;
        destinoActual = destino1.transform;
			
	}
	
	// Update is called once per frame
	void Update () {

		if(state == State.Moving)
        	explotar ();
        Debug.Log(agent.speed);

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Fire")
			cambiarDestino ();
	
	}
    public void setSpeed(float vel)
    {
        speed = vel;
        

    }
    public void setAgentSpeed(float vel)
    {
        
        agent.speed = vel;

    }

    public float getAgentSpeed()
    {
        return agent.speed;

    }


	void cambiarDestino()
	{		
			agent.SetDestination(destino2.transform.position);
        destinoActual = destino2.transform;
	}

	void explotar()
	{
		if (!agent.hasPath && !explotado) 
		{
            RaycastHit hit;
            Vector3 vector = new Vector3(transform.position.x,transform.position.y-0.4f,transform.position.z);
            if (Physics.Raycast(vector, transform.TransformDirection(Vector3.forward), out hit, 20))
            { 
                agent.SetDestination(destinoActual.position);
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(destinoActual.position, path);
                if (path.status == NavMeshPathStatus.PathPartial && !agent.hasPath && hit.collider.tag == "muro") 
                {
                     esfera.SetActive(true);

			         
                    morir();

                    explotado = true;
                }
		    }
            agent.SetDestination(destinoActual.position);
        }
	}

    public void morir()
    {
        StartCoroutine(destruir());
    }
    IEnumerator destruir()
    {
        temp = (GameObject)Instantiate(explosion, this.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);      
        Destroy(this.gameObject);
        yield return new  WaitForSeconds(1f);
		Destroy(temp.gameObject);
       
    }
}
