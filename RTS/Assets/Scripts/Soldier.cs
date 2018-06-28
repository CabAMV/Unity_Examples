using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Component in charge of controlling the movement and attack of the soldier.
/// </summary>
public class Soldier : MonoBehaviour, IShooter, IDamageable {

    private NavMeshAgent agent;
    int layerMask = 1 << 0;
    private RaycastHit hit;

    [SerializeField] private Player player;
    [SerializeField] private GameObject projectile;
    private Transform canon;

    private List<Transform> targetList;
    private Transform currentTarget;

    private bool canAttack;

    private float damage=1;

    [SerializeField] private float life;


    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        canon = transform.Find("Canon");

        targetList = new List<Transform>();
        canAttack = true;

    }
	
	// Update is called once per frame
	void Update () {

        targetList.RemoveAll(enemy => enemy == null);
        targetList.RemoveAll(enemy => enemy.gameObject.activeSelf == false);

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    //raycast that ignores the buildings

        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        //    {
        //        agent.SetDestination(hit.point);   
        //    }
        //}

        Play();
    }
    /// <summary>
    /// Checks if the soldiers has something to attack and if true attacks.
    /// </summary>
    private void Play()
    {
        if (targetList.Count > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetList[0].transform.position-this.transform.position),0.5f);
            if (canAttack)
                StartCoroutine(attack());
        }
    }
    /// <summary>
    /// Shoots facing the target to attack.
    /// </summary>
    /// <returns></returns>
    private IEnumerator attack()
    {
        canAttack = false;

        yield return new WaitForSeconds(0.5f);
        if (currentTarget != null)
        {
            if (currentTarget.gameObject.GetComponent<IDamageable>() != null && currentTarget.gameObject.GetComponent<IDamageable>().getPlayer() != player)
                shoot();
                    //currentTarget.gameObject.GetComponent<IDamageable>().ApplyDamage(this);
        }

        canAttack = true;
    }
    /// <summary>
    /// Spawns a projectile facing the canon.
    /// </summary>
    public void shoot()
    {
        GameObject temp = Instantiate(projectile, canon.position, canon.rotation);
        temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * 2000);

        Projectile tempProj = temp.GetComponent<Projectile>();
        tempProj.ignore(GetComponent<Collider>());
        tempProj.Instigator = this;

    }
    /// <summary>
    /// Adds a new target to the targetList.
    /// </summary>
    /// <param name="target"></param>
    public void addTarget(Transform target)
    {
        if (!targetList.Contains(target.transform))
        {
            targetList.Add(target.transform);
            currentTarget = targetList[0];
        }
    }
    /// <summary>
    /// Removes the given target from the target list.
    /// </summary>
    /// <param name="target"></param>
    public void removeTarget(Transform target)
    {
        if (targetList.Contains(target))
        {
            targetList.Remove(target);
            if (targetList.Count > 0)
                currentTarget = targetList[0];
            else
                currentTarget = null;
        }
    }

    /// <summary>
    /// Returns the damage this soldiers applies.
    /// </summary>
    /// <returns></returns>
    public float getDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ForestTree>() != null || other.gameObject.GetComponent<Forest>() !=null)
            return;
        if (other.gameObject.GetComponent<IDamageable>() != null )
        {
            if(other.gameObject.GetComponent<IDamageable>().getPlayer() != player)
            addTarget(other.transform);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (targetList.Contains(other.transform))
        {
            targetList.Remove(other.transform);
        }
    }

    public void ApplyDamage(IShooter instigator)
    {
        life -= instigator.getDamage();
        if (life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// Returns this soldiers player.
    /// </summary>
    /// <returns></returns>
    public Player getPlayer()
    {
        return player;
    }
    /// <summary>
    /// Sets this soldiers player.
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }
    /// <summary>
    /// Sets the damage to apply.
    /// </summary>
    /// <param name="damage"></param>
    public void setDamage(float damage)
    {
        this.damage = damage;
    }
    /// <summary>
    /// Sets the life of this player.
    /// </summary>
    /// <param name="life"></param>
    public void setLife(float life)
    {
        this.life = life;
    }
    /// <summary>
    /// Sets the the destination of this player.
    /// </summary>
    /// <param name="destiny"></param>
    public void setDestination(Vector3 destiny)
    {
        GetComponent<NavMeshAgent>().SetDestination(destiny);
    }
}
