using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    Camera camera; 
    GameObject[] players;
    Vector3 newPosition;

	// Use this for initialization
	void Start () {
        camera= this.GetComponent<Camera>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        players = GameObject.FindGameObjectsWithTag("Player");
        newPosition = calculatePosition();
        this.transform.position =new Vector3(newPosition.x,newPosition.y,this.transform.position.z) ;
        camera.orthographicSize = calculateSize();
	}

    private Vector3 calculatePosition()
    {
        Vector3 vector=players[0].transform.position;
        if (players.GetLength(0) > 1)
        {
            for (int i =1;i< players.GetLength(0); i++)
            {
                vector = (vector + players[i].transform.position)/2;
                
            }
        }
        
        return vector;
    }

    private float calculateSize()
    {
        Vector3 point =new Vector3(this.transform.position.x,this.transform.position.y,0);
        float distance=0;
        float maxim = 0;
        for (int i = 0; i < players.GetLength(0); i++)
        {
            distance = Mathf.Abs((players[i].transform.position - point).magnitude);
            if (distance > maxim)
                maxim = distance;
        }
        if (maxim > 2)
            return maxim * 1.1f;
        else
            return camera.orthographicSize;
    }
}
