using UnityEngine;
using System.Collections;

public class particlecollision : MonoBehaviour {

    public GameObject sPart;
	private float daño;

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy1")
        {
			other.GetComponent<EnemigoScript>().restarVida(daño);
            if (other.GetComponent<EnemigoScript>().getLife() <= 0)
            {
                GetComponentInParent<TurretScript>().EliminarPosicion(other);
                Instantiate(sPart, other.transform.position, Quaternion.identity);
                GameManager.Instance.addMoney(other.GetComponent<EnemigoScript>().getMoney());
                Destroy(other.gameObject);

            }
        }

    }
	public void setDaño(float daño){
		this.daño = daño;
	}
}
