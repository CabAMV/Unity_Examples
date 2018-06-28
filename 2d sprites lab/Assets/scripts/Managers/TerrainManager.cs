using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager>
{
    private TerrainScript[] Terrains;

    void Start () {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Terrain");
        Terrains = new TerrainScript[objs.Length]; 
        for (int i=0;i<objs.Length ;i++)
        {
            Terrains[i]=objs[i].GetComponent<TerrainScript>();
        }
    }

    public void destroyTerrains(float pointX, float pointY, int radius)
    {
        foreach (TerrainScript terrain in Terrains)
        {
            terrain.destroyTerrain(pointX, pointY, radius);  
        }
        DecalsManager.Instance.instanciateBulletHole(pointX, pointY, radius);
      
    }

}
