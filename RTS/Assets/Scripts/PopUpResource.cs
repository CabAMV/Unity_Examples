using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Pop up with numeric information. 
/// </summary>
public class PopUpResource : MonoBehaviour {
    /// <summary>
    /// Point of spawn.
    /// </summary>
    Transform cell;

    // Update is called once per frame
    void FixedUpdate ()
    {
        transform.position+=Vector3.up*0.1f;
        transform.rotation = Camera.main.transform.rotation;
    }
    /// <summary>
    /// Text to be shown.
    /// </summary>
    /// <param name="text"></param>
    public void setText(string text)
    {
        GetComponent<TextMesh>().text = text;
        Invoke("destroy", 1);

    }
    /// <summary>
    /// Setter for the cell variable.
    /// </summary>
    /// <param name="cell"></param>
    public void setCell(Transform cell)
    {
        this.cell= cell;
        transform.position = cell.position+Vector3.up*8;
    }
    /// <summary>
    /// Destroy this game object.
    /// </summary>
    public void destroy()
    {
        Destroy(gameObject);
    }
}
