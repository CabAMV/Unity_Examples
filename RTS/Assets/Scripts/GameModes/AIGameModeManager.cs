using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Player in charge of being the ai. Always does the exact same match.
/// </summary>
public class AIGameModeManager : Player
{
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject farmland;
    [SerializeField] private GameObject barrack;
    [SerializeField] private GameObject _base;

    private GameObject auxObject;

    private List<Forest> forests;
    private List<Farmland> farms;
    private List<Barrack> barracks;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        workersManager = GetComponent<WorkersManager>();
        constructionMode = GetComponent<ConstructionMode>();
        constructionMode.setGameModeManager(this);
        resources = new Resources(3000, 3000, 0);
        farms = new List<Farmland>();
        barracks = new List<Barrack>();



        StartCoroutine(InitializeForests(1));
        MultiForestTask(2, 6);

        StartCoroutine(PlaceBuilding(house, 11, 75, 30));
        StartCoroutine(PlaceBuilding(farmland, 6, 75, 40));
        StartCoroutine(FarmTask(0, 65));

        MultiForestTask(66, 10);


        StartCoroutine(PlaceBuilding(house, 9, 75, 120));
        StartCoroutine(PlaceBuilding(farmland, 3, 75, 200));

        MultiFarmTask(0, 201, 2);
        MultiForestTask(301, 10);


        StartCoroutine(PlaceBuilding(farmland, 6, 77, 300));
        StartCoroutine(PlaceBuilding(house, 11, 77, 300));
        StartCoroutine(PlaceBuilding(turret, 14, 54, 300));

        MultiFarmTask(1, 301, 3);

        StartCoroutine(PlaceBuilding(barrack, 14, 70, 301));
        MultiFarmTask(2, 325, 3);
        StartCoroutine(BarrackTask(0,330));
        StartCoroutine(PlaceBuilding(turret, 16, 54, 330));
        StartCoroutine(PlaceBuilding(farmland, 3, 77, 340));
        MultiFarmTask(3, 350, 3);

    }

    private void Update()
    {
        forests.RemoveAll(forest => forest == null);
        forests.RemoveAll(forest => forest.gameObject.activeSelf == false);

        if (farms != null)
        {
            farms.RemoveAll(farm => farm == null);
            farms.RemoveAll(farm => farm.gameObject.activeSelf == false);
        }
        if (barracks != null)
        {
            barracks.RemoveAll(barrack => barrack == null);
            barracks.RemoveAll(barrack => barrack.gameObject.activeSelf == false);
        }


    }
    /// <summary>
    /// Adds a new building to the player to be managed.
    /// </summary>
    /// <param name="building"></param>
    public override void AddBuilding(Building building)
    {
        if (building is Farmland)
        {
            farms.Add(building as Farmland);
        }
        if (building is Barrack)
        {
            barracks.Add(building as Barrack);
            print("cuartel hecho");
        }
    }
    /// <summary>
    /// Gets a reference of all the forest of its field.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator InitializeForests(float delay)
    {
        yield return new WaitForSeconds(delay);
        forests = new List<Forest>();
        Forest[] auxArray = FindObjectsOfType(typeof(Forest)) as Forest[];

        foreach (Forest forest in auxArray)
        {
            if (forest.getPlayer() == this)
            {
                if (!forest.IsStone())
                    forests.Add(forest);
            }
        }
    }

    /// <summary>
    /// Places a new building where indicated
    /// </summary>
    /// <param name="building"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator PlaceBuilding(GameObject building, int x, int y, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("ahora building");

        Cell cell = Map.Instance.getCell(x, y);
        Building _component = building.GetComponent<Building>();
        Vector2Int dimension = _component.getDimensions();

        if ((constructionMode as ConstructionMode).canConstruct(cell, dimension) && resources.hasEnougth(_component.Wood, _component.Stone, 0f))
        {
            auxObject = (constructionMode as ConstructionMode).placeBuilding(cell, cell.CalculateConstructionPlace(dimension), building, Quaternion.identity, orientation.South);
            resources.WoodAmount -= _component.Wood;
            resources.StoneAmount -= _component.Stone;
        }
        else
        {
            print("ñope" + (constructionMode as ConstructionMode).canConstruct(cell, dimension) + "  " + resources.hasEnougth(_component.Wood, _component.Stone, 0f));
        }
    }
    /// <summary>
    /// Creates new task for the workers.
    /// </summary>
    /// <param name="numWorkers"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator MakeTask(int numWorkers, float delay)
    {
        yield return new WaitForSeconds(delay);
        workersManager.taskWorkers(auxObject, numWorkers);
    }
    /// <summary>
    /// Creates multiple tasks for the indicated farmland.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="delay"></param>
    /// <param name="num"></param>
    private void MultiFarmTask(int index, float delay, int num)
    {
        for (int i = 0; i < num; i++)
        {
            StartCoroutine(FarmTask(index, delay));
        }
    }
    /// <summary>
    /// Creates multiple tasks for the indicated forest.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="num"></param>
    private void MultiForestTask(float delay, int num)
    {
        for (int i = 0; i < num; i++)
        {
            StartCoroutine(ForestTask(delay));
        }
    }
    /// <summary>
    /// Creates a task fora a farmland.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator FarmTask(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        print("ahora farm");
        if (index < farms.Count)
            farms[index].sendWorker();
        else
        {
            StartCoroutine(FarmTask(index, 2));
        }
    }
    /// <summary>
    /// Creates a task for a forest.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator ForestTask(float delay)
    {
        yield return new WaitForSeconds(delay);
        print("ahora forest");
        forests[0].chop();
    }
    /// <summary>
    /// Creates a task for a barrack.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator BarrackTask(int index,float delay)
    {
        yield return new WaitForSeconds(delay);
        if (barracks != null && index < barracks.Count)
        {
            barracks[index].Spawn();
            StartCoroutine(BarrackTask(index, 0.5f));
        }
        else
        {
            StartCoroutine(BarrackTask(index, 3));
        }
    }

}
