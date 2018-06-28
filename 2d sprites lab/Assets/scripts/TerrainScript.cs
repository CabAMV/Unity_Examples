using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{

    private Texture2D texture;
    private Texture2D cloneTexture;
    private SpriteMask mask;
    //private Test spriteGenerator;
    // Use this for initialization
    void Start()
    {

        texture = GetComponent<SpriteRenderer>().sprite.texture;

        cloneTexture =  new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1);
        cloneTexture.LoadRawTextureData(texture.GetRawTextureData());
        cloneTexture.wrapMode = TextureWrapMode.Clamp;
        cloneTexture.Apply();

        GetComponent<SpriteRenderer>().sprite = Sprite.Create(cloneTexture, new Rect(0.0f, 0.0f, cloneTexture.width, cloneTexture.height),new Vector2(0,0),100f);
        //Sprite sprite= Sprite.Create(cloneTexture, new Rect(0.0f, 0.0f, cloneTexture.width, cloneTexture.height), new Vector2(0, 0), 100f);
        mask = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteMask>();
        //spriteGenerator=GetComponent<Test>();
        //GetComponent<SpriteRenderer>().sprite = spriteGenerator.generateSprite(sprite);


    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                TerrainManager.Instance.destroyTerrains(hit.point.x,hit.point.y,100);
            }

        }
    }


    private void radialDamage(int OX, int OY, int blastRadius)
    {
        float time1 = Time.realtimeSinceStartup;

        Color color = new Color(0, 0, 0, 0);
        for (int i = OX - blastRadius; i < OX + blastRadius; i++)
        {
            for (int j = OY - blastRadius; j < OY + blastRadius; j++)
            {
                if (distance(new Vector2(i, j) , new Vector2(OX, OY)) <= blastRadius)
                {
                    cloneTexture.SetPixel(i, j, color);

                }

            }
        }
        cloneTexture.Apply();
        float time2 = Time.realtimeSinceStartup;
        //print(this.gameObject.name+"test 2  " + (time2 - time1));


    }


    private void resetTexture()
    {
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), GetComponent<SpriteRenderer>().sprite.pivot);
    }
    void OnApplicationQuit()
    {
        resetTexture();
    }


    private void updateSprite()
    {
        //GetComponent<SpriteRenderer>().sprite = spriteGenerator.generateSprite(GetComponent<SpriteRenderer>().sprite);

        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();

        mask.sprite = this.GetComponent<SpriteRenderer>().sprite;


    }

    private Vector2 convertToLocalPixels(float impactX, float impactY)
    {
        float distanceX = impactX - this.transform.position.x;
        float distanceY = impactY - this.transform.position.y;

        float uvX =(distanceX * cloneTexture.width) / (GetComponent<SpriteRenderer>().sprite.bounds.size.x);    
        float uvY =(distanceY * cloneTexture.height) / (GetComponent<SpriteRenderer>().sprite.bounds.size.y);

        return new Vector2(Mathf.RoundToInt(uvX), Mathf.RoundToInt(uvY));
    }


    /*
elimina pixeles del sprite
radius: en pixeles (int)
    */
    public void destroyTerrain(float pointX, float pointY, int radius)
    {    
        Vector2 UV = convertToLocalPixels(pointX , pointY);
        radialDamage((int)UV.x, (int)UV.y, radius);
        updateSprite();
    }

    private float distance(Vector2 firstPosition, Vector2 secondPosition)
    {
        Vector2 heading;
        float distance;
        float distanceSquared;

        heading.x = firstPosition.x - secondPosition.x;
        heading.y = firstPosition.y - secondPosition.y;

        distanceSquared = heading.x * heading.x + heading.y * heading.y;
        distance = Mathf.Sqrt(distanceSquared);

        return distance;
    }
}
