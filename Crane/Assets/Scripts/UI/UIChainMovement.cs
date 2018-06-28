using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIChainMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    // Movement direction 
    [SerializeField] private int direction;

    // Reference to the chain
    private chainScript chain;

    // Use this for initialization
    void Start()
    {
        GameObject _chain = GameObject.FindGameObjectWithTag("Chain");
        chain = _chain.GetComponent<chainScript>();
    }


    // Called when press down
    public void OnPointerDown(PointerEventData eventData)
    {
        chain.setDirection(direction);
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        chain.setDirection(0);
    }
}
