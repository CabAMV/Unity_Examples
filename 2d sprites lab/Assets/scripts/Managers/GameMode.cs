using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : Singleton<GameMode> {

    [SerializeField] private GameObject characterPrefab;
    private List<GameObject> playerList;
    private GameObject[] Spawns;

    // Use this for initialization
    void Start() {
        playerList = new List<GameObject>();
        Spawns = GameObject.FindGameObjectsWithTag("Respawn");
        spawnPlayers();
        Physics2D.IgnoreLayerCollision(2, 2);
        Physics2D.IgnoreLayerCollision(8, 8);
        Physics2D.IgnoreLayerCollision(2, 8);
        Physics2D.IgnoreLayerCollision(9, 10);

    }



    public List<GameObject> getPlayerList()
    {
        return playerList;
    }

    public void spawnPlayers()
    {
        for (int i = 0; i < Spawns.GetLength(0); i++)
        {
            GameObject player = Instantiate(characterPrefab, Spawns[i].transform.position, Quaternion.identity);
            playerList.Add(player);
            playerList[i].GetComponent<CharacterMovement>().assingID(i + 1);
        }
    }

    public void removeDropWeaponFromPlayer(GameObject gun)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<CharacterMovement>().removeFromPickableWeapons(gun);
        }

    }

}
