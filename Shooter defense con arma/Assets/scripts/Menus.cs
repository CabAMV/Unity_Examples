using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menus : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("Scene principal");
       

    }
    
}
