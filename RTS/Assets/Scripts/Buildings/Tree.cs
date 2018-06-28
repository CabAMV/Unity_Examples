using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building with the funtionality of a tree.
/// </summary>
public class Tree : Building {
    /// <summary>
    /// Button with the associated event of creating a new task.
    /// </summary>
    private Button chopButton;
    /// <summary>
    /// Amount of wood to be crafted once the tree is cut down.
    /// </summary>
    [SerializeField] private int crafteableWood;
    /// <summary>
    /// Amount of stone to be crafted once the tree is cut down.
    /// </summary>
    [SerializeField] private int crafteableStone;
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
    /// Checks if chop button has been pressed.
    /// </summary>
    private bool tasked;
    /// <summary>
    /// Prefab of the pop up to be shown once the tree is cut down.
    /// </summary>
    [SerializeField]private GameObject popUpResourcesPrefab;
    /// <summary>
    /// Pop up to be shown once the tree is cut down.
    /// </summary>
    private GameObject popUpResources;

    protected override void Start()
    {
        //Takes the chop button for this object canvas and adds a listener
        chopButton = canvas.transform.GetChild(0).Find("Chop").GetComponent<Button>();
        chopButton.onClick.AddListener(chop);

        vel = workers.Count;
        timeLeft = baseWaitingTime;

        popUpResources = Instantiate(popUpResourcesPrefab,GameObject.Find("LocalUI").transform);
        popUpResources.GetComponent<PopUpResource>().setCell(getCell().transform);
        popUpResources.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (timeLeft <= 0)
        {
            StopCoroutine(startCountDown());
            addWood();
            if (canvas.activeSelf)
            {
                (gameMode as GameModeManager).enableSelectionMode();
            }

            //WorkersManager wMan = gameMode.getWorkersManager();
            //foreach (GameObject worker in workers)
            //{
            //    worker.SetActive(true);
            //    wMan.makeWorkerIdle(worker.GetComponent<Worker>());
            //}
            popUpResources.SetActive(true);
            popUpResources.GetComponent<PopUpResource>().setText("+" + (crafteableWood+crafteableStone).ToString());
            destroySelf();
        }

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
    /// Adds a task to the workers manager.
    /// </summary>
    private void chop()
    {
        if (!tasked)
        {
            gameMode.getWorkersManager().taskWorkers(this.gameObject, numWorkers);
            tasked = true;
        }
    }
    /// <summary>
    /// Increases the Amount of wood of the player by the variable crafteableWood.
    /// </summary>
    private void addWood()
    {
        gameMode.resources.WoodAmount += crafteableWood;
        gameMode.resources.StoneAmount += crafteableStone;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (workers.Count < numWorkers)
        {
			if (other.gameObject.GetComponent<Worker> () != null) 
			{
				if (this.gameObject.Equals (other.gameObject.GetComponent<Worker> ().getBuilding ())) 
				{
					workers.Add (other.gameObject);
					workers [workers.Count - 1].SetActive (false);
					vel = workers.Count;
					if (workers.Count > 0) {
						StartCoroutine (startCountDown ());
						//isConstructing = true;
					}
					if (workers.Count <= 0) {
						StopCoroutine (startCountDown ());
						//isConstructing = false;
					}
				}
			}
        }
    }
}
