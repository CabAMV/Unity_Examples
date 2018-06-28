using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    [SerializeField] private int destructionRadius;
    [SerializeField] private int damage;
    [SerializeField] private float force;
    [SerializeField] private GameObject prefab;
    private GameObject instigator;




    private int healthDamage;

    public void Init(Transform origin,Vector3 vector,GameObject instigator)
    {
       
        this.transform.position = origin.position + (vector * 0.75f);
        this.transform.rotation = Quaternion.LookRotation(this.transform.position - origin.position);
        this.transform.Rotate(0, -90, 0); 
        this.GetComponent<Rigidbody2D>().AddForce(this.transform.right * force);
        this.setInstigator(instigator);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.GetComponent<PolygonCollider2D>());
        detonate(collision);
        Destroy(this.gameObject);
    }

    public void detonate(Collision2D collision)
    {
        Vector2 hitPoint = collision.contacts[0].point;
        GameObject collider = Instantiate(prefab, new Vector3(hitPoint.x, hitPoint.y, 0.0f),Quaternion.identity);
        collider.GetComponent<circularDamager>().setDamage(damage);
        collider.GetComponent<circularDamager>().setInstigator(this.instigator);
        collider.GetComponent<CircleCollider2D>().radius = (float)destructionRadius /(float)100;
        TerrainManager.Instance.destroyTerrains(hitPoint.x, hitPoint.y, destructionRadius);          
    }

    public void setInstigator(GameObject instigator)
    {
        this.instigator = instigator;
    }

}
