using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building that produces food over the time base on the number of workers it has.
/// </summary>
public class Farmland : Building
{
    /// <summary>
    /// Button in charge to task a worker with going to work to this farm.
    /// </summary>
    private Button goButton;
    /// <summary>
    /// Button that frees a worker of work in this farm.
    /// </summary>
    private Button unGoButton;
    /// <summary>
    /// Shows the number of workers tasked with working in this farm.
    /// </summary>
    private Text workersText;
    /// <summary>
    /// Number of workers required with working in this farm.
    /// </summary>
    private int workersSent;
    /// <summary>
    /// Amount of food to produce without multipliers.
    /// </summary>
    private float baseProduction = 0.5f;
    /// <summary>
    /// Amount of food to produce after appling multipliers.
    /// </summary>
    private float production;
    /// <summary>
    /// Base speed of production.
    /// </summary>
    private float baseVel = 5;
    /// <summary>
    /// Current speed of production.
    /// </summary>
    private float vel;
    /// <summary>
    /// Determines if the farm can increase the resources amount of food.
    /// </summary>
    private bool canProduce;
    /// <summary>
    /// Prefab of the pop up that shows the amount of food produced.
    /// </summary>
    [SerializeField] private GameObject popUpResourcesPrefab;
    /// <summary>
    /// Pop that show the amount of food produced.
    /// </summary>
    private GameObject popUpResources;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        setSpeed();

        goButton = canvas.transform.GetChild(0).Find("go").GetComponent<Button>();
        unGoButton = canvas.transform.GetChild(0).Find("UnGo").GetComponent<Button>();
        workersText = canvas.transform.GetChild(0).Find("Counter").GetChild(0).GetComponent<Text>();

        goButton.onClick.AddListener(sendWorker);
        unGoButton.onClick.AddListener(unSendWorker);

        canProduce = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (canProduce)
            StartCoroutine(Collect());
    }
    /// <summary>
    /// Setter for the speed of production (vel) base on the number of workers.
    /// </summary>
    private void setSpeed()
    {

        if (workers.Count > 0)
        {
            production = baseProduction * (getCell() as Cell).ProductionBoost;
            vel = baseVel / workers.Count;
        }
        else
            production = 0;
    }
    /// <summary>
    /// Iterator that increassed the amount of food of the player thought time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Collect()
    {
        canProduce = false;
        yield return new WaitForSeconds(vel);
        gameMode.resources.FoodAmount += production;
        if(production !=0 && !(gameMode is AIGameModeManager))
        {
            popUpResources = Instantiate(popUpResourcesPrefab, GameObject.Find("LocalUI").transform);
            popUpResources.GetComponent<PopUpResource>().setCell(transform);
            popUpResources.GetComponent<PopUpResource>().setText("+" + production.ToString());
        }

        canProduce = true;
    }
    /// <summary>
    /// Creates a task for send a worker to the farmland location.
    /// </summary>
    public void sendWorker()
    {
        if (workersSent < numWorkers)
        {
            gameMode.getWorkersManager().taskWorkers(this.gameObject, 1);
            workersSent++;
            workersText.text = workersSent.ToString();
        }
    }
    /// <summary>
    /// Untasks a worker whos working in this farmland.
    /// </summary>
    public void unSendWorker()
    {
        if (workers.Count > 0)
        {
            WorkersManager wMan = gameMode.getWorkersManager();
            workers[workers.Count - 1].SetActive(true);
            workers[workers.Count - 1].GetComponent<Worker>().setBuilding();
            wMan.makeWorkerIdle(workers[workers.Count - 1].GetComponent<Worker>());
            workers.RemoveAt(workers.Count - 1);
            setSpeed();
            workersSent--;
            workersText.text = workersSent.ToString();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (workers.Count < numWorkers)
        {
			if(other.gameObject.GetComponent<Worker>() != null)
			{
	            if (this.gameObject.Equals(other.gameObject.GetComponent<Worker>().getBuilding()))
	            {
	                workers.Add(other.gameObject);
	                workers[workers.Count - 1].SetActive(false);
	                setSpeed();
				}
			}
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (workers.Count < numWorkers)
        {
            if (other.gameObject.GetComponent<Worker>() != null)
            {
                if (this.gameObject.Equals(other.gameObject.GetComponent<Worker>().getBuilding()))
                {
                    workers.Add(other.gameObject);
                    workers[workers.Count - 1].SetActive(false);
                    setSpeed();
                }
            }
        }
    }
}
