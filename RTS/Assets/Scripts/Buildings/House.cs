using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Building that spawns workers unitl reaches its maximum number.
/// </summary>
public class House : Building
{
    /// <summary>
    /// Prefab of the worker to spawn.
    /// </summary>
    public GameObject workerPrefab;
    /// <summary>
    /// Determines if a worker can be spawned
    /// </summary>
    private bool canSpawn;
    /// <summary>
    /// Point where the Workers should be spawned.
    /// </summary>
    private Vector3 spawnPoint;

    [SerializeField] private float workerPrice;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        spawnPoint = transform.Find("SpawnPoint").position;
        //for (int i = 0; i < numWorkers; i++)
        //{
        //    SpawnWorker();
        //}

        canSpawn = true;

    }
    protected override void Update()
    {
        base.Update();

        if (workers.Count < numWorkers)
        {
            if (canSpawn && gameMode.resources.hasEnougth(0,0,workerPrice))
            {
                gameMode.resources.FoodAmount -= workerPrice;
                StartCoroutine(Spawn());
            }
        }
    }

    /// <summary>
    /// Spawns a worker and adds it to the workers manager.
    /// </summary>
    public void SpawnWorker()
    {
        addWorker(Instantiate(workerPrefab, spawnPoint, Quaternion.identity, GameObject.Find("Workers").transform));
        workers[workers.Count - 1].GetComponent<Worker>().setHome(this);
        workers[workers.Count - 1].GetComponent<Worker>().setPlayer(gameMode);
        gameMode.getWorkersManager().addWorker(workers[workers.Count - 1].GetComponent<Worker>());

    }

    /// <summary>
    /// Iterator in time for the SpawnWorker() method to work.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Spawn()
    {
        canSpawn = false;
        yield return new WaitForSeconds(3f);
        SpawnWorker();
        canSpawn = true;
    }


}
