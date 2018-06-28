using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class that manages where the worker should go. And its state of live.
/// </summary>
public class Worker : MonoBehaviour,IDamageable
{
    /// <summary>
    /// Possition where the worker is located at the beggining of the game.
    /// </summary>
    Vector3 orPos;
    /// <summary>
    /// Agent that allows to use the unity pathfinding methods on this object.
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// Manager that manages this worker.
    /// </summary>
    WorkersManager manager;
    /// <summary>
    /// GameModeManager that acts as representation of the player.
    /// </summary>
    [SerializeField] private Player player;

    [SerializeField] private float life;
    /// <summary>
    /// House that spawns this worker.
    /// </summary>
    House home;
    /// <summary>
    /// Destination of the worker if it is active.
    /// </summary>
    GameObject building;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        orPos = transform.position;
    }

    private void Update()
    {
        if (building == null)
        {
            agent.SetDestination(orPos);
            manager.makeWorkerIdle(this);
        }
    }
    /// <summary>
    /// Sets the building this worker is tasked to go.
    /// </summary>
    /// <param name="building"></param>
    public void setBuilding(GameObject building)
    {
        this.building = building;
        agent.SetDestination(building.transform.position);
    }
    /// <summary>
    /// Sets the tasked building to null.
    /// </summary>
    public void setBuilding()
    {
        this.building = null;
    }
    /// <summary>
    /// Returns the building the worker is tasked to go to.
    /// </summary>
    /// <returns></returns>
    public GameObject getBuilding()
    {
        return building;
    }
    /// <summary>
    /// Sets the destination of this worker.
    /// </summary>
    /// <param name="destiny"></param>
    public void setDestination(Vector3 destiny)
    {
        agent.SetDestination(destiny);
    }
	/// <summary>
    /// Sets the manager of this worker.
    /// </summary>
    /// <param name="manager"></param>
    public void setWorkerManager(WorkersManager manager)
    {
        this.manager = manager;
    }
    /// <summary>
    /// Sets the Game Mode Manager that acts as player.
    /// </summary>
    /// <param name="gameMode"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }
    /// <summary>
    /// Returns the Game Mode Manager that acts as player.
    /// </summary>
    /// <returns></returns>
    public Player getPlayer()
    {
        return player;
    }
	/// <summary>
    /// Sets the variable home.
    /// </summary>
    /// <param name="home"></param>
    public void setHome(House home)
    {
        this.home = home;
    }
    /// <summary>
    /// Removes this worker from the lists of the Worker manager and clears the space in the house attached for a new worker to spawn.
    /// </summary>
    public void removeSelf()
    {
        if(home != null)
            home.removeWorker(this.gameObject);
        if (manager.isWorkerActive(this))
        {
            manager.taskWorkers(building, 1);
        }
        manager.RemoveWorker(this);
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Applies damage to himself.
    /// </summary>
    /// <param name="instigator"></param>
    public void ApplyDamage(IShooter instigator)
    {
        //instigator.removeTarget(this.transform);
        life-=instigator.getDamage();
        if (life <= 0)
        {
            removeSelf();
        }
    }


}
