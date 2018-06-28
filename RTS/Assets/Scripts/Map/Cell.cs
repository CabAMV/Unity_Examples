using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that represents a cell in the level grid.
/// </summary>
public class Cell : SelectableObject
{
    /// <summary>
    /// Coordinates of this cell.
    /// </summary>
    private int X, Y;
    /// <summary>
    /// Building this cell is associated with.
    /// </summary>
    private Building building;
    /// <summary>
    /// Determines if the cell has a building associated.
    /// </summary>
    private bool isOcupied;
    /// <summary>
    /// Where the Selector should be located if this cell is selected.
    /// </summary>
    private Vector3 anchor;
    /// <summary>
    /// Level where this cell is located in the map grid.
    /// </summary>
    private int level;
    /// <summary>
    /// Multiplayer of the production of the farms.
    /// </summary>
    private float productionBoost;

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public float ProductionBoost
    {
        get
        {
            return productionBoost;
        }

        set
        {
            productionBoost = value;
        }
    }

    //Building building; 

    // Use this for initialization
    protected void Start()
    {
        isOcupied = false;
        setAnchor(transform.position);
        //GameModeManager[] aux = FindObjectsOfType(typeof(GameModeManager)) as GameModeManager[];
        //setGameModeManager(aux[0]);
    }
    /// <summary>
    /// Setter of the coords of the cell.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void setCoords(int x, int y)
    {
        X = x;
        Y = y;
    }
    /// <summary>
    /// Returns the cell coordinates.
    /// </summary>
    /// <returns></returns>
    public Vector2Int getCoords()
    {
        return new Vector2Int(X, Y);
    }
    /// <summary>
    /// Sets if the cell is ocupied.
    /// </summary>
    /// <param name="ocupied"></param>
    public void setIsOcupied(bool ocupied)
    {
        isOcupied = ocupied;
    }
    /// <summary>
    /// Returns if the cell is ocuppied.
    /// </summary>
    /// <returns></returns>
    public bool getIsOcupied()
    {
        return isOcupied;
    }
    /// <summary>
    /// Sets the variable anchor.
    /// </summary>
    /// <param name="anchor"></param>
    public void setAnchor(Vector3 anchor)
    {
        //4 is half the size of a cell in world units
        this.anchor = new Vector3(anchor.x - 4, anchor.y + 1, anchor.z - 4);
    }
    /// <summary>
    /// Return the anchor variable.
    /// </summary>
    /// <returns></returns>
    public Vector3 getAnchor()
    {
        return anchor;
    }
    /// <summary>
    /// Returns where the building should be placed based on the anchor and the dimension given.
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public Vector3 CalculateConstructionPlace(Vector2Int dimension)
    {
        //8 =cell width
        Bounds bounds = GetComponent<Collider>().bounds;
        return new Vector3(transform.position.x + ((dimension.x - 1) * 8) / 2, bounds.max.y, transform.position.z + ((dimension.y - 1) * 8) / 2);
    }

    /// <summary>
    /// overwritten methods from SelectableObject
    /// </summary>
    #region
    public override void action(RaycastHit hit)
    {
        base.action(hit);
    }

    public override void setBuilding(SelectableObject other)
    {
        this.building = other as Building;
    }

    public override SelectableObject getBuilding()
    {
        return this.building;
    }
    #endregion

}


