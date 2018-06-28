using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIRotation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Direction of rotation
    [SerializeField] private int direction;

    // Reference to the crane
    private GruaMovement grua;

    // Use this for initialization
    void Start()
    {
        GameObject _grua = GameObject.FindGameObjectWithTag("Grua");
        grua = _grua.GetComponent<GruaMovement>();
    }

    // Called when press down
    public void OnPointerDown(PointerEventData eventData)
    {
        grua.setRotationSpeed(direction);
    }
    // Called when the button is released

    public void OnPointerUp(PointerEventData eventData)
    {
        grua.setRotationSpeed(0);
    }
}
