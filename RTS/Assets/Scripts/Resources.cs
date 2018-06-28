using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that stores the amount of resources of a player.
/// </summary>
public class Resources{
    /// <summary>
    /// Amount of wood and stone.
    /// </summary>
    private int woodAmount,stoneAmount;
    /// <summary>
    /// Amount of food.
    /// </summary>
    private float foodAmount;
 
    public int WoodAmount
    {
        get
        {
            return woodAmount;
        }

        set
        {
            woodAmount = value;
        }
    }

    public int StoneAmount
    {
        get
        {
            return stoneAmount;
        }

        set
        {
            stoneAmount = value;
        }
    }

    public float FoodAmount
    {
        get
        {
            return foodAmount;
        }

        set
        {
            foodAmount = value;
        }
    }
    /// <summary>
    /// Default constructor.
    /// </summary>
    public Resources()
    {

    }
    /// <summary>
    /// Initialization for a given number of resources
    /// </summary>
    /// <param name="wood"></param>
    /// <param name="stone"></param>
    public Resources(int wood,int stone,float food)
    {
        WoodAmount = wood;
        StoneAmount = stone;
        FoodAmount = food;
    }

    /// <summary>
    /// Returns if is stored enougth amount of the resources required.
    /// </summary>
    /// <param name="wood"></param>
    /// <param name="stone"></param>
    /// <param name="food"></param>
    /// <returns></returns>
    public bool hasEnougth(int wood,int stone,float food)
    {
        if (WoodAmount >= wood && StoneAmount >= stone && FoodAmount>=food)
            return true;
        else
            return false;

    }
}
