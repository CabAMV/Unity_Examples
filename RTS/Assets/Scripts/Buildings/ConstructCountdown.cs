using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Building that occupies an space while the building itself is being constructed.
/// </summary>
public class ConstructCountdown : Building
{
    /// <summary>
    /// Future building to ocupie this building space.
    /// </summary>
    [SerializeField] private GameObject prefab;
    /// <summary>
    /// Base to wait until the building finish its construction.
    /// </summary>
    [SerializeField] private float baseWaitingTime;
    /// <summary>
    /// Remaining time of construction.
    /// </summary>
    private float timeLeft;
    /// <summary>
    /// Speed of construction.
    /// </summary>
    private float vel;
    /// <summary>
    /// Determines if the timeleft is decrementing or not.
    /// </summary>
    private bool isConstructing;
    /// <summary>
    /// Test that shows tehe timeLeft variable in the canvas.
    /// </summary>
    private Text timerText;




    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        gameMode.getWorkersManager().taskWorkers(this.gameObject, numWorkers);

        Transform panel = canvas.transform.GetChild(0).Find("Panel");
        timerText = panel.GetChild(0).gameObject.GetComponent<Text>();

        vel = workers.Count;
        timeLeft = baseWaitingTime;

        isConstructing = false;
    }

    protected override void Update()
    {
        base.Update();

        timerText.text = timeLeft.ToString();
        if (timeLeft <= 0)
        {
            StopCoroutine(startCountDown());
            placeBuilding();
            if (canvas.activeSelf)
            {
                (gameMode as GameModeManager).enableSelectionMode();
            }

            //destroySelf();
            WorkersManager wMan = gameMode.getWorkersManager();
            foreach (GameObject worker in workers)
            {
                worker.SetActive(true);
                wMan.makeWorkerIdle(worker.GetComponent<Worker>());
            }
            RemoveTask();
            Destroy(this.gameObject);
        }

    }
    /// <summary>
    /// Iterator for decrementing the time left based on the speed of construction.
    /// </summary>
    /// <returns></returns>
    IEnumerator startCountDown()
    {
        yield return new WaitForSeconds(1 / vel);
        timeLeft--;
        StartCoroutine(startCountDown());

    }
    /// <summary>
    /// Calls the construction mode for placeing a new building based on this building params such as cell, dimension, orientation and rotation.
    /// </summary>
    public void placeBuilding()
    {
        ConstructionMode mode = gameMode.getConstructionMode() as ConstructionMode;
        mode.placeBuilding(this.cell, transform.position, prefab, transform.rotation, _orientation);
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
					if (workers.Count > 0 && !isConstructing) {
						StartCoroutine (startCountDown ());
						isConstructing = true;
					}
					if (workers.Count <= 0) {
						StopCoroutine (startCountDown ());
						isConstructing = false;
					}
				}
			}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (workers.Count < numWorkers && !workers.Contains(other.gameObject))
        {
            if (other.gameObject.GetComponent<Worker>() != null)
            {
                if (this.gameObject.Equals(other.gameObject.GetComponent<Worker>().getBuilding()))
                {
                    workers.Add(other.gameObject);
                    workers[workers.Count - 1].SetActive(false);
                    vel = workers.Count;
                    if (workers.Count > 0 && !isConstructing)
                    {
                        StartCoroutine(startCountDown());
                        isConstructing = true;
                    }
                    if (workers.Count <= 0)
                    {
                        StopCoroutine(startCountDown());
                        isConstructing = false;
                    }
                }
            }
        }
    }

}
