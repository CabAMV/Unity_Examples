using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumeration used to control the state of rotation of a building.
/// </summary>
public enum orientation
{
    North,
    East,
    West,
    South
}

/// <summary>
/// Class used in the construction mode to show were the building will be placed.
/// </summary>
public class BuildingPlaceHolder : MonoBehaviour {
    /// <summary>
    /// Prefab this object will be sustitued with.
    /// </summary>
    [SerializeField] private GameObject ObjectToCreate;
    /// <summary>
    /// Reference to the building component of the new object, needed for initialization.
    /// </summary>
    private Building building;
    /// <summary>
    /// Dimensions in cells of this object and the future building.
    /// </summary>
    private Vector2Int Dimension;
    /// <summary>
    /// Variable storing the current orientation of the object.
    /// </summary>
    private orientation _orientation;


    // Use this for initialization
    void Start () {
        building = ObjectToCreate.GetComponent<Building>();
        Dimension = building.getDimensions();
        _orientation = orientation.South;
        this.GetComponent<MeshFilter>().sharedMesh = ObjectToCreate.GetComponent<MeshFilter>().sharedMesh;

        transform.localScale = ObjectToCreate.transform.localScale;

        _orientation = orientation.South;
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        _orientation = orientation.South;

    }
    /// <summary>
    /// Returns the dimensions of the object based on the current orientation.
    /// </summary>
    /// <returns></returns>
    public Vector2Int getDimension()
    {
        if (_orientation==orientation.North  || _orientation == orientation.South)
            return Dimension;
        if (_orientation == orientation.East || _orientation == orientation.West)
            return new Vector2Int(Dimension.y,Dimension.x);

        return Dimension;
    }
    /// <summary>
    /// Changes the orientation of the object.
    /// </summary>
    public void Rotate()
    {
        transform.Rotate(Vector3.up,90);

        switch (_orientation)
        {
            case orientation.North:
                _orientation = orientation.East;
                break;
            case orientation.East:
                _orientation = orientation.South;
                break;
            case orientation.South:
                _orientation = orientation.West;
                break;
            case orientation.West:
                _orientation = orientation.North;
                break;

        }

    }
    /// <summary>
    /// Return the future object to create.
    /// </summary>
    /// <returns></returns>
    public GameObject getBuilding()
    {
        return ObjectToCreate;
    }
    /// <summary>
    /// Returns the current orientation of the object.
    /// </summary>
    /// <returns></returns>
    public orientation getOrientation()
    {
        return _orientation;
    }
}
