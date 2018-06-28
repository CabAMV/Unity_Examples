using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages which of the workers is active and gives them orders. Also recives which orders are still valid and which not.
/// </summary>
public class WorkersManager : MonoBehaviour
{
    /// <summary>
    /// Workers doing a task at the moment.
    /// </summary>
    private List<Worker> activeWorkers;
    /// <summary>
    /// Workers being idle at the moment.
    /// </summary>
    private List<Worker> idleWorkers;
    /// <summary>
    /// List of tasks to be made by the workers.
    /// </summary>
    private List<GameObject> tasks;
    /// <summary>
    /// Game Mode Manager that represents the player that owns this class.
    /// </summary>
    private Player gameMode;

    public Player GameMode
    {
        get
        {
            return gameMode;
        }

        set
        {
            gameMode = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        idleWorkers = new List<Worker>();
        activeWorkers = new List<Worker>();
        tasks = new List<GameObject>();
        GameMode = GetComponent<Player>();

        Worker[] workers = FindObjectsOfType(typeof(Worker)) as Worker[];
        foreach (Worker worker in workers)
        {
            if (worker.getPlayer() == GameMode)
            {
                idleWorkers.Add(worker);
                worker.setWorkerManager(this);
            }
        }

    }

    private void Update()
    {
        if (tasks.Count > 0 && tasks[0] != null)
        {
            if (idleWorkers.Count > 0)
            {
                activeWorkers.Add(idleWorkers[0]);
                idleWorkers[0].setBuilding(tasks[0]);
                idleWorkers.Remove(idleWorkers[0]);
                tasks.RemoveAt(0);
            }
        }
    }


    /// <summary>
    /// Adds the number of tasks given to the list of tasks. 
    /// </summary>
    /// <param name="building"></param>
    /// <param name="num"></param>
    public void taskWorkers(GameObject building/*Vector3 destination*/, int num)
    {
        for (int i = 0; i < num; i++)
        {
            //add task
            tasks.Add(building);


            //if (idleWorkers.Count > 0)
            //{
            //    activeWorkers.Add(idleWorkers[0]);
            //    idleWorkers[0].setDestination(destination);
            //    idleWorkers.Remove(idleWorkers[0]);
            //}
        }
    }
    /// <summary>
    /// Removes a worker from being active and makes it idle.
    /// </summary>
    /// <param name="worker"></param>
    public void makeWorkerIdle(Worker worker)
    {
        if (activeWorkers.Contains(worker))
        {
            int index = activeWorkers.IndexOf(worker);
            idleWorkers.Add(activeWorkers[index]);
            activeWorkers.RemoveAt(index);
        }
    }
    /// <summary>
    /// Adds a worker to the idleWorkers list at the beggining of the game.
    /// </summary>
    /// <param name="worker"></param>
    public void addWorker(Worker worker)
    {
        idleWorkers.Add(worker);
        worker.setWorkerManager(this);
    }
    /// <summary>
    /// Removes a worker from the list.
    /// </summary>
    /// <param name="worker"></param>
    public void RemoveWorker(Worker worker)
    {
        if (isWorkerActive(worker))
            activeWorkers.Remove(worker);
        else
            idleWorkers.Remove(worker);
    }
    /// <summary>
    /// Returns if a worker is active.
    /// </summary>
    /// <param name="worker"></param>
    /// <returns></returns>
    public bool isWorkerActive(Worker worker)
    {
        return activeWorkers.Contains(worker);
    }

    /// <summary>
    /// Recursive method that removes the given tasks form the tasks list.
    /// </summary>
    /// <param name="task"></param>
    public void RemoveTask(GameObject task)
    {
        if (tasks.Contains(task))
        {
            tasks.Remove(task);
            RemoveTask(task);
        }
    }
}
