using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularDamager : MonoBehaviour {

    private int damage;
    private GameObject instigator;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(deathCounter());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharacterMovement>().applyDamage(damage,this.instigator);
        }
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public void setInstigator(GameObject instigator)
    {
        this.instigator = instigator;
    }

    public GameObject getInstigator()
    {
        return this.instigator;
    }

    IEnumerator deathCounter()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
