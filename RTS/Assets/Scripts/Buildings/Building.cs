using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Father class for all the objects that can be placed in the world and be interactuable.
/// </summary>
public class Building : SelectableObject,IDamageable
{
    /// <summary>
    /// Cell the building is associated with.
    /// </summary>
    protected Cell cell;
    /// <summary>
    /// Canvas to create on spawn.
    /// </summary>
    [SerializeField] protected GameObject canvasPrefab;
    /// <summary>
    /// building canvas
    /// </summary>
    protected GameObject canvas;
    /// <summary>
    /// Button tasked with closing the canvas.
    /// </summary>
    private Button exitButton;

    [SerializeField] private float life;

    private Text maxLifeText;

    private Text currentLifeText;

    /// <summary>
    /// Dimension in cells of this building.
    /// </summary>
    [SerializeField] protected Vector2Int Dimension;
    /// <summary>
    /// Orientation of the building.
    /// </summary>
    protected orientation _orientation;
    /// <summary>
    /// Location of the canvas relative to the building.
    /// </summary>
    protected Vector3 UIPlacement;
    /// <summary>
    /// List of workers "working" on this building.
    /// </summary>
    protected List<GameObject> workers;
    /// <summary>
    /// Max number of workers that can be associated with this building.
    /// </summary>
    [SerializeField]protected int numWorkers;
    /// <summary>
    /// Amount of resources needed for this building to be constructed.
    /// </summary>
    [SerializeField] protected int wood,stone;

    public int Wood
    {
        get
        {
            return wood;
        }

        set
        {
            wood = value;
        }
    }

    public int Stone
    {
        get
        {
            return stone;
        }

        set
        {
            stone = value;
        }
    }

    protected virtual void Start()
    {
        canvas = Instantiate(canvasPrefab, GameObject.Find("LocalUI").transform);

        //initialize the exit button
        exitButton = canvas.transform.GetChild(0).Find("Exit").GetComponent<Button>();
        exitButton.onClick.AddListener(hideInterface);

        maxLifeText = canvas.transform.GetChild(0).Find("MaxLife").GetComponent<Text>();
        maxLifeText.text = life.ToString();
        currentLifeText = canvas.transform.GetChild(0).Find("CurrentLife").GetComponent<Text>();
        currentLifeText.text = life.ToString();

        canvas.SetActive(false);
        UIPlacement = transform.Find("UIPlacement").position;
        workers = new List<GameObject>();

        gameMode.AddBuilding(this);
    }

    protected virtual void Update()
    {
        canvas.transform.Find("UIPlacement").GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(UIPlacement);
    }

    /// <summary>
    /// Sets the dimension in cells of this building.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void setDimensions(int x = 1, int y = 1)
    {
        Dimension = new Vector2Int(x, y);
    }
    /// <summary>
    /// Sets the dimension in cells of this building.
    /// </summary>
    /// <param name="vector"></param>
    public void setDimensions(Vector2Int vector)
    {
        Dimension = vector;
    }
    /// <summary>
    /// Returns the dimension of this building based on the orientation.
    /// </summary>
    /// <returns></returns>
    public Vector2Int getDimensions()
    {
        if (_orientation == orientation.North || _orientation == orientation.South)
            return Dimension;
        if (_orientation == orientation.East || _orientation == orientation.West)
            return new Vector2Int(Dimension.y, Dimension.x);

        return Dimension;
    }
    /// <summary>
    /// Sets the building orientation.
    /// </summary>
    /// <param name="_orientation"></param>
    public void setOrientation(orientation _orientation)
    {
        this._orientation = _orientation;
    }
    /// <summary>
    /// Adds the given object to worker list.
    /// </summary>
    /// <param name="worker"></param>
    public virtual void addWorker(GameObject worker)
    {
        workers.Add(worker);
    }
    /// <summary>
    /// Destroys itself liberating the cells that was occupiing.
    /// </summary>
    public void destroySelf()
    {
        ConstructionMode constructMode = gameMode.getConstructionMode() as ConstructionMode;
        constructMode.setCellsOcupied(getCell() as Cell, null, getDimensions(), false);
        Destroy(canvas);

        RemoveTask();

        WorkersManager wMan = gameMode.getWorkersManager();

        foreach (GameObject worker in workers)
        {
            worker.SetActive(true);
            wMan.makeWorkerIdle(worker.GetComponent<Worker>());

        }
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Removes the given worker from the workers list. 
    /// </summary>
    /// <param name="worker"></param>
    public virtual void removeWorker(GameObject worker)
    {
        workers.Remove(worker);
    }
    /// <summary>
    /// removes itself from the tasks list.
    /// </summary>
    public void RemoveTask()
    {
        gameMode.getWorkersManager().RemoveTask(this.gameObject);
    }
    /// <summary>
    /// Applies damage tho itself.
    /// </summary>
    /// <param name="instigator"></param>
    public void ApplyDamage(IShooter instigator)
    {
        currentLifeText.text = life.ToString();
        life-=instigator.getDamage();
        if (life <= 0)
        {
            CheckDestruction();
        }
    }

    protected virtual void CheckDestruction()
    {
        destroySelf();
    }

    /// <summary>
    /// overwritten methods from SelectableObject.
    /// </summary>
    #region
    /// <summary>
    /// Sets this building current cell. 
    /// </summary>
    public override void setCell(SelectableObject other)
    {
        cell = other as Cell;
    }
    /// <summary>
    /// Returns this building current cell. 
    /// </summary>
    /// <returns></returns>
    public override SelectableObject getCell()
    {
        return cell;
    }

    #endregion

    //Canvas methods
    #region
    /// <summary>
    /// Returns if this canvas is active
    /// </summary>
    public bool isCanvasActive()
    {
        return canvas.activeSelf;
    }
    /// <summary>
    /// Returns this building canvas.
    /// </summary>
    /// <returns></returns>
    public GameObject getCanvas()
    {
        return canvas;
    }
    /// <summary>
    /// Disables the UIMode in the GameModeManager and enables the SelectionMode.
    /// </summary>
    public virtual void hideInterface()
    {
        canvas.SetActive(false);
        if (gameMode is GameModeManager)
            (gameMode as GameModeManager).enableSelectionMode();
        else
        {
            GameModeManager instigator= GameObject.Find("Player").GetComponent<GameModeManager>();
            instigator.enableSelectionMode();
        }
    }
    #endregion
}
