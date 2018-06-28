using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMovement : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{
    
    // Axis of movement
    [SerializeField] private bool axisX;
    // Direction of movement
    [SerializeField] private int direction;

    // Reference to the crane
	private GruaMovement grua;

    // Use this for initialization
    void Start () {
		GameObject _grua = GameObject.FindGameObjectWithTag ("Grua");
		grua = _grua.GetComponent<GruaMovement>();
	}
    
    // Called when press down
    public void OnPointerDown(PointerEventData eventData)
    {
		grua.setMovementSpeed(direction);
		grua.setaxis (axisX);
    }
    
    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
	{
		grua.setMovementSpeed(0);
	}
}
