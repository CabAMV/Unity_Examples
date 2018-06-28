using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalsManager : Singleton<DecalsManager>
{

    [SerializeField]
    private GameObject bulletHole;
    private List<GameObject> deadDecalList;
    private List<GameObject> aliveDecalList;

    // Use this for initialization
    void Start()
    {
        initDecals(100);

    }


    private void initDecals(int num)
    {
        deadDecalList = new List<GameObject>();
        for (int i = 0; i < num; i++)
        {
            deadDecalList.Add(Instantiate(bulletHole));
            deadDecalList[i].SetActive(false);
        }
        aliveDecalList = new List<GameObject>();
    }
    public void instanciateBulletHole(float x, float y, float blastRadius)
    {
        aliveDecalList.Add(deadDecalList[deadDecalList.Count - 1]);
        deadDecalList.Remove(deadDecalList[deadDecalList.Count - 1]);
        aliveDecalList[aliveDecalList.Count - 1].SetActive(true);
        aliveDecalList[aliveDecalList.Count - 1].transform.position = new Vector3(x, y, 0);
        float scale = aliveDecalList[aliveDecalList.Count - 1].GetComponent<BulletDecal>().getDecalScale(blastRadius);
        aliveDecalList[aliveDecalList.Count - 1].transform.localScale = new Vector3(scale, scale, 1);
        print("1");
    }

}
