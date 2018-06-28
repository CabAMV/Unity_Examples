using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hook : MonoBehaviour {

    private Button hookBtn;                     //UI button that triggers connectRigidbodies
    private GameObject lastCollision;           //stores what the Hook touches
    private GameObject charge;                  //stores a charge if there is one

    private Rigidbody anchor;                   //stores the rigidBodie

    private void Start()                        
    {
        charge = null;
        GameObject temp=GameObject.FindGameObjectWithTag("Hook");
        hookBtn = temp.GetComponent<Button>();
        hookBtn.onClick.AddListener(connectRigidbodies);      //assing what the listener should call to whe press down
    }
	
    //stores what the hook has overlapped with
	private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {           
            lastCollision = collision.gameObject;
        }
    }

    //resets the variable lastCollision
	private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.Equals(lastCollision))
            lastCollision = null;
    }

    //intializes the anchor from other script
    public void setAnchor(Rigidbody rb)
    {
        anchor = rb;
    }

    //assings a new charge and a joint for it if there isn`t one and destroys it if there is one
    public void connectRigidbodies()
    {
		if (charge==null) {
			if (lastCollision != null) {
                charge = lastCollision;
                charge.AddComponent<HingeJoint> ().connectedBody = anchor;
                charge.GetComponent<HingeJoint> ().useLimits = true;
                charge.GetComponent<HingeJoint> ().autoConfigureConnectedAnchor = false;
                charge.GetComponent<HingeJoint> ().connectedAnchor = new Vector3 (0, -1, 0);

			}
		
		}
        else 
		{
			GameObject.Destroy (charge.GetComponent<HingeJoint> ());
		    charge = null;

		}



    }
}
