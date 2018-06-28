using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class in charge of manage the selector when nothing is being done and the access to the ui mode.
/// </summary>
public class SelectionMode : GameMode
{

    private void Start()
    {
        //cellSelected = Map.Instance.getCell(0, 0, 1);
    }

    public override void initMode(SelectableObject obj)
    {
        base.initMode(obj);
        GetComponent<CameraController>().setCanRotate(true);

    }

    public override void setCellSelected()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            //every Update gets the cell the cursor is obove and stores it
            if (hit.collider.gameObject.GetComponent<SelectableObject>())
            {
                cellSelected = hit.collider.gameObject.GetComponent<SelectableObject>().getCell();

            }
        }

    }

    public override void gameModeEvents()
    {
        Cell cell = cellSelected as Cell;
        Bounds bounds=cell.GetComponent<Collider>().bounds;
        Vector3 anchor = cell.getAnchor();
        selector.transform.position = new Vector3(anchor.x,bounds.max.y+1,anchor.z);


        if (cell.getIsOcupied())
        {
            Building build = cell.getBuilding() as Building;
            setSelectorDimensions(build.getDimensions());
        }
        else
            setSelectorDimensions();


        //Sets the object selected, actually not in use
        if (Input.GetButtonDown("Fire1"))
        {
            if (cellSelected != null)
            {
                cellSelected.action(hit);
                if(hit.collider.GetComponent<SelectableObject>().getBuilding())
                    currentSelection = hit.collider.GetComponent<SelectableObject>().getBuilding();
                else
                    currentSelection = hit.collider.GetComponent<SelectableObject>().getCell();

                if (currentSelection is Building &&(currentSelection as Building).getPlayer()==this.getGameModeManager())
                    GetComponent<GameModeManager>().enableUIMode();

                //(currentSelection as Building).showInterface();

                //currentSelection = cellSelected.getBuilding();
            }

        }
    }

}
