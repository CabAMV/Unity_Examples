using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class in charge of controlling the actions of a player either human or AI.
/// </summary>
public class Player : MonoBehaviour {

    /// <summary>
    /// Construction gamemode functionality.
    /// </summary>
    protected GameMode constructionMode;
    /// <summary>
    /// Manager responsible for the workers.
    /// </summary>
    protected WorkersManager workersManager;
    /// <summary>
    /// Storage of the resources of the game.
    /// </summary>
    [NonSerialized] public Resources resources;

    protected Base rivalBase;


    protected virtual void Start()
    {
        //Base[] bases = FindObjectsOfType(typeof(Base)) as Base[];
        //foreach (Base _base in bases)
        //{
        //    if (_base.getPlayer() != this)
        //    {
        //        rivalBase = _base;
        //    }
        //}
        //print(rivalBase);
    }

    /// <summary>
    /// Retunrs the workersmanager 
    /// </summary>
    /// <returns></returns>
    public WorkersManager getWorkersManager()
    {
        return workersManager;
    }

    /// <summary>
    /// Returns the current active game mode
    /// </summary>
    /// <returns></returns>
    public virtual GameMode getCurrentMode()
    {
        return constructionMode;
    }
    /// <summary>
    /// Returns the construction game mode to use its funtionality.
    /// </summary>
    /// <returns></returns>
    public GameMode getConstructionMode()
    {
        return constructionMode;
    }

    public void setRivalBase(Base _base)
    {
        rivalBase = _base;
    }

    public Base getRivalBase()
    {
        return rivalBase;
    }

    public virtual void AddBuilding(Building building) { }

}
