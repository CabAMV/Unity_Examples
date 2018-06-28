using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Component attached to the object being the objective to be proteted by the player.
/// </summary>
public class Base : Building {

    protected override void Start()
    {
        base.Start();

    }
    /// <summary>
    /// Calls to reaload scene once the building is destroyed.
    /// </summary>
    protected override void CheckDestruction()
    {
        //llamar a game over aqui
        print("GAME OVER");
        SceneManager.LoadScene("TestManu");
    }
}
