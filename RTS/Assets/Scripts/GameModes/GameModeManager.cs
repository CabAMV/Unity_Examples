using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Class that controls the different modes the player can be in.
/// </summary>
public class GameModeManager : Player
{
    /// <summary>
    /// Visual representation of the cursor.
    /// </summary>
    [SerializeField] private GameObject selector;

    /// <summary>
    /// UI texts of the resources.
    /// </summary>
    public Text woodText, stoneText, foodText;

    /// <summary>
    /// Selection gamemode functionality.
    /// </summary>
    GameMode selectionMode;
    /// <summary>
    /// Interface gamemode functionality.
    /// </summary>
    GameMode uiMode;
    /// <summary>
    /// Turret Possesion gamemode functionality.
    /// </summary>
    GameMode shootMode;
    /// <summary>
    /// Current active game mode.
    /// </summary>
    GameMode currentMode;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        workersManager = GetComponent<WorkersManager>();
        resources = new Resources(0, 0, 0);

        constructionMode = GetComponent<ConstructionMode>();
        selectionMode = GetComponent<SelectionMode>();
        uiMode = GetComponent<UIMode>();
        shootMode = GetComponent<ShootMode>();

        constructionMode.setGameModeManager(this);
        selectionMode.setGameModeManager(this);
        uiMode.setGameModeManager(this);
        shootMode.setGameModeManager(this);

        constructionMode.setSelector(selector);
        selectionMode.setSelector(selector);
        uiMode.setSelector(selector);
        shootMode.setSelector(selector);

        currentMode = selectionMode;
    }

    // Update is called once per frame
    void Update()
    {
        woodText.text = resources.WoodAmount.ToString();
        stoneText.text = resources.StoneAmount.ToString();
        foodText.text = resources.FoodAmount.ToString();

        if (!IsPointerOverUIObject())
        {
            currentMode.setCellSelected();
            currentMode.gameModeEvents();
        }

    }


    /// <summary>
    /// Enables the construction mode deactivating the current mode given an id of the building to construct.
    /// </summary>
    /// <param name="buildingID"></param>
    public void enableConstructionMode(int buildingID)
    {
        //if (currentMode != constructionMode)
        //{
        currentMode.disableMode();
        currentMode = constructionMode;
        ConstructionMode mode = currentMode as ConstructionMode;
        mode.enablePlaceHolder(false);
        mode.setBuilding(buildingID);
        mode.enablePlaceHolder(true);
        GetComponent<CameraController>().setCanRotate(false);
        //}

    }

    /// <summary>
    /// Enables the selection mode deactivating the current mode.
    /// </summary>
    public void enableSelectionMode()
    {

        currentMode.disableMode();
        selectionMode.initMode(currentMode.getCellSelected());
        currentMode = selectionMode;


    }

    /// <summary>
    /// Enables the interface mode deactivating the current mode.
    /// </summary>
    public void enableUIMode()
    {
        currentMode.disableMode();
        uiMode.initMode(currentMode.getCellSelected());
        currentMode = uiMode;

    }

    /// <summary>
    /// Allows to posses a turret, deactivating the active game mode in the process. 
    /// </summary>
    public void enableShootMode()
    {
        currentMode.disableMode();
        shootMode.initMode(currentMode.getCellSelected());
        currentMode = shootMode;
    }

    /// <summary>
    /// Returns the current active game mode
    /// </summary>
    /// <returns></returns>
    public override GameMode getCurrentMode()
    {
        return currentMode;
    }
		
    /// <summary>
    /// Checks if the pointer is over a ui element.
    /// </summary>
    /// <returns></returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
