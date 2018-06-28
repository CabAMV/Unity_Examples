using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of constructing new buildings.
/// </summary>
public class ConstructionMode : GameMode
{
    int layerMask = 1 << 0;
    /// <summary>
    /// List of posible place holders.
    /// </summary>
    [SerializeField] private List<BuildingPlaceHolder> buildingPlaceHolders;

    /// <summary>
    /// Current placeholder of the building to build.
    /// </summary>
    private BuildingPlaceHolder building;
    /// <summary>
    /// Point of location for the place holder.
    /// </summary>
    protected Vector3 middlePoint;



    private void Start()
    {
        building = buildingPlaceHolders[0];
    }

    public override void disableMode()
    {
        enablePlaceHolder(false);
    }


    public override void setCellSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //raycast that ignores the buildings
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //every Update gets the cell the cursor is obove and stores it
            if (hit.collider.gameObject.GetComponent<SelectableObject>())
            {
                cellSelected = hit.collider.gameObject.GetComponent<SelectableObject>().getCell();
            }
        }
        middlePoint = selector.transform.GetChild(0).transform.position;

    }

    public override void gameModeEvents()
    {
        Cell cell = cellSelected as Cell;
        Bounds bounds = cell.GetComponent<Collider>().bounds;

        //Do not change for getAnchor if so will locate the building placeholder incorrectly.
        //4 is half the size of a cell, if localscale is used the system will work wrongly.
        selector.transform.position = new Vector3(cell.transform.position.x - 4, bounds.max.y + 1, cell.transform.position.z - 4);
        setSelectorDimensions(building.getDimension());

        //Places an object in the desired cell if possible
        if (Input.GetButtonDown("Fire1"))
        {
            building.Rotate();

        }

        Building Temp = building.getBuilding().GetComponent<Building>();
        if (canConstruct(cellSelected as Cell, building.getDimension()) && gameModeManager.resources.hasEnougth(Temp.Wood, Temp.Stone,0f))
        {
            building.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.7f);
            if (Input.GetButtonDown("Fire2"))
            {
                if (cellSelected != null)
                {
                    placeBuilding(cellSelected as Cell, (cellSelected as Cell).CalculateConstructionPlace(building.getDimension()), building.getBuilding(), building.transform.rotation, building.getOrientation());
                    gameModeManager.resources.WoodAmount -= Temp.Wood;
                    gameModeManager.resources.StoneAmount -= Temp.Stone;

                }
            }
        }

        else
        {
            building.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.7f);
        }

    }

    /// <summary>
    /// Places a building by desired params.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="position"></param>
    /// <param name="building"></param>
    /// <param name="rotation"></param>
    /// <param name="_orientation"></param>
    public GameObject placeBuilding(Cell cell, Vector3 position, GameObject building, Quaternion rotation, orientation _orientation)
    {
        GameObject tempBuild = Instantiate(building, position, rotation, GameObject.Find("Buildings").transform);
        Building build = tempBuild.GetComponent<SelectableObject>() as Building;
        build.setCell(cell);
        //Building _build = build as Building;
        build.setOrientation(_orientation);
        setCellsOcupied(cell, build, build.getDimensions(), true);
        build.setGameModeManager(this.gameModeManager);
        return tempBuild;

    }

    /// <summary>
    /// Sets the cells indicated ocuppied.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="building"></param>
    /// <param name="dimensions"></param>
    /// <param name="isOcupied"></param>
    public void setCellsOcupied(Cell cell, Building building, Vector2Int dimensions, bool isOcupied)
    {
        int columns = dimensions.x, rows = dimensions.y;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Cell mapCell = Map.Instance.getCell(cell.getCoords().x + j, cell.getCoords().y + i, cell.Level);
                mapCell.setIsOcupied(isOcupied);
                mapCell.setBuilding(building);
                if (isOcupied)
                    mapCell.setAnchor(cell.transform.position);
                else
                    mapCell.setAnchor(mapCell.transform.position);
            }
        }
    }


    /// <summary>
    /// Determines if the desired location is suitable for construction.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public bool canConstruct(Cell cell, Vector2Int dimension)
    {
        int columns = dimension.x, rows = dimension.y;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (!Map.Instance.cellExist(cell, cell.getCoords().x + j, cell.getCoords().y + i) || Map.Instance.getCell(cell.getCoords().x + j, cell.getCoords().y + i, cell.Level).getIsOcupied() || Map.Instance.getCell(cell.getCoords().x + j, cell.getCoords().y + i, cell.Level).getPlayer() != gameModeManager)
                {
                    return false;
                }

            }
        }
        return true;

    }

    /// <summary>
    /// Enables or disables the place holder.
    /// </summary>
    /// <param name="flag"></param>
    public void enablePlaceHolder(bool flag)
    {
        middlePoint = selector.transform.GetChild(0).transform.position;
        building.gameObject.SetActive(flag);
        building.transform.position = middlePoint;
    }

    /// <summary>
    /// Sets the building to construct in enable this mode
    /// </summary>
    /// <param name="id"></param>
    public void setBuilding(int id)
    {
        building = buildingPlaceHolders[id];
    }


}
