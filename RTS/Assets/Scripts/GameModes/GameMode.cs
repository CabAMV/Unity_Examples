using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Father class for the different game modes or states the player can be in.
/// </summary>
public class GameMode : MonoBehaviour {
    /// <summary>
    /// Hit result of the cursor.
    /// </summary>
    protected RaycastHit hit;
    /// <summary>
    /// What object is currently selected.
    /// </summary>
    protected SelectableObject currentSelection;
    /// <summary>
    /// Actual cell selected.
    /// </summary>
    protected SelectableObject cellSelected;

    /// <summary>
    /// Visual representation of the cursor in the world.
    /// </summary>
    protected GameObject selector;

    /// <summary>
    /// Game mode manager.
    /// </summary>
    protected Player gameModeManager;

    //[SerializeField] private List<BuildingPlaceHolder> buildingPlaceHolders;
    //private BuildingPlaceHolder building;

    /// <summary>
    /// Sets the cellSelected.
    /// </summary>
    public virtual void setCellSelected()
    {
    }
    /// <summary>
    /// Events of the Game mode.
    /// </summary>
    public virtual void gameModeEvents()
    {
    }
    /// <summary>
    /// Enables the game mode.
    /// </summary>
    /// <param name="obj"></param>
    public virtual void initMode(SelectableObject obj)
    {
    }
    /// <summary>
    /// Disables the game mode.
    /// </summary>
    public virtual void disableMode()
    {
    }

    /// <summary>
    /// Returns the cell selected.
    /// </summary>
    /// <returns></returns>
    public SelectableObject getCellSelected()
    {
        return cellSelected;
    }
    /// <summary>
    /// Returns the current selection.
    /// </summary>
    /// <returns></returns>
    public SelectableObject getCurrentSelection()
    {
        return currentSelection;
    }
    /// <summary>
    /// Sets the selector of the game mode.
    /// </summary>
    /// <param name="selector"></param>
    public void setSelector(GameObject selector)
    {
        this.selector = selector;
    }
    /// <summary>
    /// Sets the selector dimensions.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void setSelectorDimensions(int x = 1, int y = 1)
    {
        selector.transform.localScale = new Vector3(x, 1, y);
    }
    /// <summary>
    /// Sets the selector dimensions.
    /// </summary>
    /// <param name="d"></param>
    public void setSelectorDimensions(Vector2 d)
    {
        selector.transform.localScale = new Vector3(d.x, 1, d.y);
    }
    /// <summary>
    /// Sets the game mode manager.
    /// </summary>
    /// <param name="gameModeManager"></param>
    public void setGameModeManager(Player gameModeManager)
    {
        this.gameModeManager = gameModeManager;
    }
    /// <summary>
    /// Returns the game mode manager.
    /// </summary>
    /// <returns></returns>
    public Player getGameModeManager()
    {
        return gameModeManager;
    }
}
