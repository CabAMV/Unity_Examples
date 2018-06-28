using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as a tree unit inside a forest.
/// </summary>
public class ForestTree : Building {

    Forest parent;
    GameObject worker;


    /// <summary>
    /// Initial time for the tree to be cut down.
    /// </summary>
    [SerializeField] private float baseWaitingTime;
    /// <summary>
    /// Remaining time for the tree to be cut down.
    /// </summary>
    private float timeLeft;
    /// <summary>
    /// Speed of work.
    /// </summary>
    private float vel;
    /// <summary>
    /// Amount of wood to be crafted once the tree is cut down.
    /// </summary>
    [SerializeField] private int crafteableWood;
    /// <summary>
    /// Checks if the type of resource is stone or wood
    /// </summary>
    [SerializeField] private bool isStone;
    /// <summary>
    /// Checks if chop button has been pressed.
    /// </summary>
    public bool tasked;
    /// <summary>
    /// Prefab of the pop up to be shown once the tree is cut down.
    /// </summary>
    [SerializeField] private GameObject popUpResourcesPrefab;
    /// <summary>
    /// Pop up to be shown once the tree is cut down.
    /// </summary>
    private GameObject popUpResources;

    protected override void Start()
    {
        if(worker!=null)
            vel = workers.Count;
        else
            vel =0;

        timeLeft = baseWaitingTime;
        if (!(gameMode is AIGameModeManager))
        {
            popUpResources = Instantiate(popUpResourcesPrefab, GameObject.Find("LocalUI").transform);
            popUpResources.GetComponent<PopUpResource>().setCell(transform);
            popUpResources.SetActive(false);
        }

    }


    // Update is called once per frame
    protected override void Update () {
        if (timeLeft <= 0)
        {
            StopCoroutine(startCountDown());
            addWood();
            if (parent.isCanvasActive())
            {
                (parent.getPlayer() as GameModeManager).enableSelectionMode();
            }

            //WorkersManager wMan = gameMode.getWorkersManager();
            //foreach (GameObject worker in workers)
            //{
            //    worker.SetActive(true);
            //    wMan.makeWorkerIdle(worker.GetComponent<Worker>());
            //}
            if (!(gameMode is AIGameModeManager))
            {
                popUpResources.SetActive(true);
                popUpResources.GetComponent<PopUpResource>().setText("+" + (crafteableWood).ToString());
            }
            destroySelf();
        }
    }
    
    /// <summary>
    /// Adds a task to the workers manager.
    /// </summary>
    public void chop()
    {
        if (!tasked)
        {
            gameMode.getWorkersManager().taskWorkers(this.gameObject, 1);
            tasked = true;
        }
    }
    /// <summary>
    /// Activates the pop up indicating the resources colected and add new resources to its player.
    /// </summary>
    private void addWood()
    {
        if (!(gameMode is AIGameModeManager))
        {
            popUpResources.SetActive(true);
            popUpResources.GetComponent<PopUpResource>().setText("+" + (crafteableWood).ToString());
        }
        if(isStone)
            parent.getPlayer().resources.StoneAmount += crafteableWood;
        else
            parent.getPlayer().resources.WoodAmount += crafteableWood;

    }

    /// <summary>
    /// Iterator for decrementing the time left based on the speed of work.
    /// </summary>
    /// <returns></returns>
    IEnumerator startCountDown()
    {
        yield return new WaitForSeconds(1 / vel);
        timeLeft--;
        StartCoroutine(startCountDown());

    }
    /// <summary>
    /// Removes itself from the forest and destroys itself.
    /// </summary>
    public new void destroySelf()
    {
        parent.removeTree(this);
        RemoveTask();
        WorkersManager wMan = gameMode.getWorkersManager();
        worker.SetActive(true);
        wMan.makeWorkerIdle(worker.GetComponent<Worker>());

        
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Sets the parent forest.
    /// </summary>
    /// <param name="forest"></param>
    public void setParent(Forest forest)
    {
        parent = forest;
        setGameModeManager(parent.getPlayer());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Worker>()!=null)
        {
            if (this.gameObject.Equals(other.gameObject.GetComponent<Worker>().getBuilding()))
            {
                worker = other.gameObject;
                worker.SetActive(false);
                vel = 1;
                StartCoroutine(startCountDown());
            }

        }
    }
}
