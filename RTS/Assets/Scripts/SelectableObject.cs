using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that as father for those objects susceptibles of being selected.
/// </summary>
public class SelectableObject : MonoBehaviour
{
    /// <summary>
    /// Game Mode Manager that acts as identifier of the player.
    /// </summary>
    protected Player gameMode;
    /// <summary>
    /// Debug method for the child classes.
    /// </summary>
    /// <param name="hit"></param>
    public virtual void action(RaycastHit hit) { }
    /// <summary>
    /// Set the cell where this object is located or this object is associated with.
    /// </summary>
    /// <param name="other"></param>
    public virtual void setCell(SelectableObject other) { }
    /// <summary>
    /// Returns the cell where this object is located or this object is associated with.
    /// </summary>
    /// <returns></returns>
    public virtual SelectableObject getCell() { return this; }
    /// <summary>
    /// Sets the building that is associated with this object.
    /// </summary>
    /// <param name="other"></param>
    public virtual void setBuilding(SelectableObject other) { }
    /// <summary>
    /// Returns this as a building or if is not a building returns the one that this object is associated with.
    /// </summary>
    /// <returns></returns>
    public virtual SelectableObject getBuilding() { return this; }
    /// <summary>
    /// Sets this objects gamemode
    /// </summary>
    /// <param name="gameMode"></param>
    public void setGameModeManager(Player gameMode)
    {
        this.gameMode = gameMode;
    }
    /// <summary>
    /// Returns the variable gameMode that acts as identifier of the player.
    /// </summary>
    /// <returns></returns>
    public Player getPlayer()
    {
        return gameMode;
    }
}
