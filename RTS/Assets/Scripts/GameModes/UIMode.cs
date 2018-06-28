using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the activation and the activation of the current cell selected canvas. 
/// </summary>
public class UIMode : GameMode
{
    public override void initMode(SelectableObject obj)
    {
        cellSelected = obj.getCell();
        Cell cell = cellSelected as Cell;
        Bounds bounds = cell.GetComponent<Collider>().bounds;
        Vector3 anchor = cell.getAnchor();
        selector.transform.position = new Vector3(anchor.x, bounds.max.y + 1, anchor.z);

        if (cellSelected.getBuilding() is Building)
            setSelectorDimensions((cellSelected.getBuilding() as Building).getDimensions());
        else
            GetComponent<GameModeManager>().enableSelectionMode();

        if (cellSelected.getBuilding() != null)
        {
            currentSelection = cellSelected.getBuilding();
            (currentSelection as Building).getCanvas().SetActive(true);
        }
        else
            currentSelection = cellSelected;

        //GameObject [] selectores =GameObject.FindGameObjectsWithTag("SelectorVisual");
        //selectores[0].GetComponent<Renderer>().sharedMaterial.color = Color.blue;
    }

    public override void disableMode()
    {
        (currentSelection as Building).getCanvas().SetActive(false);
    }

    public override void gameModeEvents()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out hit))
            {
                //every Update gets the cell the cursor is obove and stores it
                if (hit.collider.gameObject.GetComponent<SelectableObject>())
                {
                    disableMode();
                    initMode(hit.collider.gameObject.GetComponent<SelectableObject>().getCell());

                }
            }
        }
    }



}