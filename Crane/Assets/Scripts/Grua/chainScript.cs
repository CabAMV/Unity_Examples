using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class chainScript : MonoBehaviour
{

    private List<GameObject> chain;                 //list of active links
    private List<GameObject> linkPool;              //pool of inactive links
    private float orPointY;                         //initial Y coordinate of the anchor
    [SerializeField] private int maxLinks;
    [SerializeField] private float speed;           //base speed movement of the anchor
    private float aplicableSpeed;                   //speed * direction

    private Rigidbody anchor;

    // Use this for initialization
    void Start()
    {
        orPointY = this.transform.position.y;

        chain = new List<GameObject>();
        linkPool = new List<GameObject>();
        anchor = this.GetComponent<Rigidbody>();

        for (int i = 0; i < maxLinks; i++)                                                          //create all the links
        {
            createLink();
        }
		linkPool [0].AddComponent<SphereCollider> ();
		linkPool[0].GetComponent<SphereCollider>().radius =2;
		linkPool [0].GetComponent<SphereCollider> ().center=new Vector3(0,-1,0);
		linkPool [0].GetComponent<CapsuleCollider> ().isTrigger=true;
        linkPool[0].GetComponent<SphereCollider>().isTrigger = true;
        linkPool[0].AddComponent<Hook>().setAnchor(linkPool[0].GetComponent<Rigidbody>());          //make the first link the hook

        for (int i = 0; i < maxLinks - 2; i++)                                                      //make visible the initial chain
        {
            addLink();
        }


    }



    private void FixedUpdate()
    {
        if (aplicableSpeed != 0)
            moveChain(aplicableSpeed);
    }

    //moves the anchor up and down if the chain is long enough
    public void moveChain(float orientation)
    {
        if (this.canGoDown() && aplicableSpeed < 0)
        {
            anchor.MovePosition(this.transform.position + (Vector3.up * aplicableSpeed));
            this.transform.position = anchor.position;

        }

        if (this.canGoUp() && aplicableSpeed > 0)
        {
            anchor.MovePosition(this.transform.position + (Vector3.up * aplicableSpeed));
            this.transform.position = anchor.position;
        }


    }

    //removes a link from the link pool and adds it to the chain in the correct position with a joint to the next link and the anchor
    public void addLink()
    {
        setKinematic();

        chain.Insert(0, linkPool[0]);
        linkPool.Remove(linkPool[0]);
        chain[0].SetActive(true);

        if (chain.Count > 1)
        {


            chain[1].GetComponent<HingeJoint>().connectedBody = null;
            chain[0].transform.position = chain[1].transform.position;
            chain[1].transform.position = chain[1].transform.position + (chain[1].transform.up * -1f);

            chain[0].transform.rotation = chain[1].transform.rotation;
            chain[1].GetComponent<HingeJoint>().autoConfigureConnectedAnchor = true;
            chain[1].GetComponent<HingeJoint>().connectedBody = chain[0].GetComponent<Rigidbody>();
            chain[1].GetComponent<HingeJoint>().useLimits=true;
            JointLimits limit=chain[1].GetComponent<HingeJoint>().limits;
            limit.max = 180;



        }

        chain[0].transform.position = this.transform.position + (Vector3.down);


        chain[0].GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
        chain[0].GetComponent<HingeJoint>().connectedBody = anchor;
        chain[0].GetComponent<HingeJoint>().connectedAnchor = new Vector3(0, 0, 0);
        chain[0].GetComponent<HingeJoint>().useLimits = false;

        this.transform.position = new Vector3(transform.position.x, orPointY, transform.position.z);

        setNotKinematic();

    }

    //removes the first link from the chain and adds it to the link pool
    public void removeLink()
    {
        linkPool.Insert(0, chain[0]);
        chain.Remove(chain[0]);
        linkPool[0].SetActive(false);

        chain[0].transform.position = this.transform.position + (Vector3.down);
        chain[0].GetComponent<HingeJoint>().connectedBody = anchor;
        this.transform.position = new Vector3(transform.position.x, orPointY, transform.position.z);

    }


    //creates a link and deactivates it adding it to the link pool
    private void createLink()
    {
        linkPool.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));
        linkPool[linkPool.Count - 1].AddComponent<Rigidbody>();
        linkPool[linkPool.Count - 1].GetComponent<Rigidbody>().mass = 100;
        linkPool[linkPool.Count - 1].GetComponent<CapsuleCollider>().isTrigger = false;
        linkPool[linkPool.Count - 1].AddComponent<HingeJoint>();
        linkPool[linkPool.Count - 1].GetComponent<HingeJoint>().useLimits=true;
        linkPool[linkPool.Count - 1].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        linkPool[linkPool.Count - 1].SetActive(false);
    }

    //sets all links as kinematic
    private void setKinematic()
    {
        for (int i = 0; i < chain.Count; i++)
        {
            Rigidbody rb = chain[i].GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

        }
    }
    //sets all links as not kinematic
    private void setNotKinematic()
    {
        for (int i = 0; i < chain.Count; i++)
        {
            Rigidbody rb = chain[i].GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

        }
    }

    public void setDirection(int direction)
    {
        aplicableSpeed = speed * direction;
    }

    //checks if the chain is long enough to go down
    public bool canGoDown()
    {
        if (linkPool.Count > 0)
            return true;
        else
            return false;
    }
    
    //checks if the chain is long enough to go up
    public bool canGoUp()
    {
        if (linkPool.Count < maxLinks - 1)
            return true;
        else
            return false;
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            if (other.transform.position.y < this.transform.position.y)              //if exits the trigger upwards removes a link
            {
                removeLink();
            }

            else if (other.transform.position.y > this.transform.position.y)        //if exits the trigger downwards adds a link
            {
                addLink();
            }
        }
    }

}
