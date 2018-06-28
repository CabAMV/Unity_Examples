using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that groups a number of trees under a single building and manages them.
/// </summary>
public class Forest : Building {

    private List<ForestTree> Trees;
    /// <summary>
    /// Button with the associated event of creating a new task.
    /// </summary>
    private Button chopButton;

    [SerializeField] private bool isStone;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        Trees = new List<ForestTree>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<ForestTree>() != null)
            {
                Trees.Add(child.gameObject.GetComponent<ForestTree>());
                child.gameObject.GetComponent<ForestTree>().setParent(this);
            }
        }
        chopButton = canvas.transform.GetChild(0).Find("Chop").GetComponent<Button>();
        chopButton.onClick.AddListener(chop);
    }
    /// <summary>
    /// Selects a Tree from the list and tasks workers with chopping it.
    /// </summary>
    public void chop()
    {
        foreach (ForestTree tree in Trees)
        {
            if (!tree.tasked)
            {
                tree.chop();
                return;
            }

        }
    }
    /// <summary>
    /// Destroys a Tree, if there is no more trees destroys itself.
    /// </summary>
    /// <param name="tree"></param>
    public void  removeTree(ForestTree tree)
    {
        Trees.Remove(tree);
        if (Trees.Count <=0)
        {
            destroySelf();
        }

    }
    /// <summary>
    /// Returns if the type of resource is stone or not.
    /// </summary>
    /// <returns></returns>
    public bool IsStone()
    {
        return isStone;
       
    }
	

}
