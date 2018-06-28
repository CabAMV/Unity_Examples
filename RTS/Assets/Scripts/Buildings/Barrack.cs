using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class in charge to spawn new soldiers.
/// </summary>
public class Barrack : Building
{
    /// <summary>
    /// Prefab of the soldier to spawn.
    /// </summary>
    public GameObject soldierPrefab;
    /// <summary>
    /// Determines if a soldier can be spawned
    /// </summary>
    private bool canSpawn;
    /// <summary>
    /// Point where the Workers should be spawned.
    /// </summary>
    private Vector3 spawnPoint;
    /// <summary>
    /// Button in charge to call the Spawn method.
    /// </summary>
    private Button spawnButton;
    /// <summary>
    /// Button in charge of upgrading the soldiers stats.
    /// </summary>
    private Button updateButton;

    [Header("Soldier Params")]

    [SerializeField]private float soldierPrice;
    [SerializeField] private float soldierLife;
    [SerializeField] private float soldierDamage;
    [SerializeField] private float upgradePrice;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        spawnPoint = transform.Find("SpawnPoint").position;
        canSpawn = true;
        spawnButton = canvas.transform.GetChild(0).Find("Spawn").GetComponent<Button>();
        spawnButton.onClick.AddListener(Spawn);
        updateButton = canvas.transform.GetChild(0).Find("Update").GetComponent<Button>();
        updateButton.onClick.AddListener(updateSoldiers);



    }
    /// <summary>
    /// Creates a new soldier.
    /// </summary>
    public void Spawn()
    {
        if (gameMode.resources.hasEnougth(0, 0, soldierPrice))
        {
            gameMode.resources.FoodAmount -= soldierPrice;
            GameObject temp=Instantiate(soldierPrefab,spawnPoint,Quaternion.identity,GameObject.Find("Soldiers").transform);

            //initialize soldier
            Soldier soldier = temp.GetComponent<Soldier>();
            soldier.setPlayer(getPlayer());
            soldier.setDamage(soldierDamage);
            soldier.setLife(soldierLife);
            soldier.setDestination(gameMode.getRivalBase().transform.position);
            print("new soldier");
        }

    }
    /// <summary>
    /// Upgrades the new soldiers stats.
    /// </summary>
    public void updateSoldiers()
    {
        if (gameMode.resources.hasEnougth(0, 0, upgradePrice))
        {
            gameMode.resources.FoodAmount -= upgradePrice;
            soldierLife *= 1.2f;
            soldierDamage *= 1.5f;
        }
    }



}
