using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private List<GameObject> childs;
    private bool isJumping;
    private bool isFliped;
    private float horizontalSpeed;
    private List<GameObject> pickableWeapon;
    public string playerID;
    private int health;

    void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        // childs= getChilds();
        isJumping = true;
        isFliped = false;
        horizontalSpeed = 5;
        health = 100;

        pickableWeapon = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        inputs();

    }

    private void inputs()
    {
        if (Input.GetAxis("Horizontal_P"+playerID) > 0 && rb.velocity.x < horizontalSpeed)
        {
            rb.velocity = new Vector2(horizontalSpeed*Input.GetAxis("Horizontal_P" + playerID), rb.velocity.y);
            if (isFliped)
            {
                flip();
                isFliped = false;
            }

        }
        if (Input.GetAxis("Horizontal_P"+ playerID) == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (Input.GetAxis("Horizontal_P"+ playerID) < 0 && rb.velocity.x > -horizontalSpeed)
        {
            rb.velocity = new Vector2(horizontalSpeed* Input.GetAxis("Horizontal_P" + playerID), rb.velocity.y);

            if (!isFliped)
            {
                flip();
                isFliped = true;
            }
        }

        if (Input.GetButtonDown("Jump_P"+ playerID) && !this.isJumping)
        {
            rb.velocity = rb.velocity + new Vector2(0, 10);
            this.isJumping = true;

        }
        if (Input.GetButtonDown("Interact_P"+playerID))
        {
            replaceWeapon(pickableWeapon[0]);
        }
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<weapon>().weaponImputs("Fire1_P"+playerID);

    }




    private void flip()
    {
        this.gameObject.transform.GetChild(0).Rotate(0, 180, 0);
        //this.transform.Find("Arms").gameObject.GetComponent<weapon>().flip();
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<weapon>().flip();
    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        
            Bounds bounds = this.GetComponent<CapsuleCollider2D>().bounds;

            RaycastHit2D hitInfo;
            if (Physics2D.Raycast(new Vector3(transform.position.x, bounds.min.y), Vector2.down, 1f))
            {
                hitInfo = Physics2D.Raycast(new Vector3(transform.position.x,bounds.min.y) , Vector2.down, 1f);
                if (hitInfo.collider.gameObject)
                {
                    this.isJumping = false;
                }
                Debug.DrawRay(this.transform.position,Vector2.down,Color.green,1);
            }

            RaycastHit2D hitInfoR;
            if (Physics2D.Raycast(new Vector3(bounds.min.x, bounds.min.y), Vector2.down, 1f))
            {
                hitInfoR = Physics2D.Raycast(new Vector3(bounds.min.x, bounds.min.y), Vector2.down, 1f);
                if (hitInfoR.collider.gameObject)
                {
                    this.isJumping = false;
                }
                Debug.DrawRay(new Vector3(bounds.min.x, bounds.min.y), Vector2.down, Color.red, 1);
            }

            RaycastHit2D hitInfoL;
            if (Physics2D.Raycast(new Vector3(bounds.max.x, bounds.min.y), Vector2.down, 1f))
            {
                hitInfoL = Physics2D.Raycast(new Vector3(bounds.max.x, bounds.min.y), Vector2.down, 1f);
                if (hitInfoL.collider.gameObject)
                {
                    this.isJumping = false;
                }
                Debug.DrawRay(new Vector3(bounds.max.x, bounds.min.y), Vector2.down, Color.blue, 1);
            }

        


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("droppedWeapon"))
        {
            pickableWeapon.Add(collision.gameObject);
            print("triggerON");
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("droppedWeapon"))
            removeFromPickableWeapons(collision.gameObject);
    }

    private void replaceWeapon(GameObject obj)
    {
        Vector3 position=this.transform.GetChild(1).position;
        GameObject newWeapon=Instantiate(obj.GetComponent<droppedWeapon>().getWeapon(),this.transform.position,this.transform.rotation);
        GameMode.Instance.removeDropWeaponFromPlayer(obj);
        newWeapon.transform.Find("ArmR").GetComponent<HingeJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        newWeapon.transform.Find("ArmL").GetComponent<HingeJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

        newWeapon.transform.position =position;
        Destroy(this.transform.GetChild(1).gameObject);
        newWeapon.transform.parent = this.transform;
        Destroy(obj.transform.parent.gameObject);
        Destroy(obj);
    }

    public void assingID(int ID)
    {
        this.playerID = ID.ToString();
    }

    public bool IsFlipped()
    {
        return isFliped;
    }

    public void removeFromPickableWeapons(GameObject gun)
    {
        if (pickableWeapon.Contains(gun))
        {
            pickableWeapon.Remove(gun);
        }

    }

    public void applyDamage(int damage,GameObject instigator)
    {
        this.health -= damage;
        print(this.health +" "+this.playerID+" " + instigator.GetComponent<CharacterMovement>().playerID);
        Debug.Break();
    }

}
