using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{

    public Texture2D[] atlasTextures;

    // could make these public for debugging purposes
    public Rect[] rects;
    public Texture2D atlas;
    Sprite sprite;
    SpriteRenderer spriteRenderer;

    public Sprite generateSprite(Sprite orSprite)
    {

        /*spriteRenderer = GetComponent<SpriteRenderer>();

        if (!spriteRenderer)
            return;*/
        atlasTextures = new Texture2D[1];
        atlasTextures[0] = orSprite.texture;
        // pack textures
        atlas = new Texture2D(8192, 8192);
        rects = atlas.PackTextures(atlasTextures, 2, 8192);
        
        // create sprite using rects[0]
        Vector2 atlasSize = new Vector2(atlas.width, atlas.height);
        Rect spriteRect = new Rect(Vector2.Scale(rects[0].position, atlasSize), Vector2.Scale(rects[0].size, atlasSize));
        sprite = Sprite.Create(atlas, spriteRect, new Vector2(0, 0));

        // create new shape for sprite using array of vertices
        Vector2[] vertices = new Vector2[6];
        
        vertices[0] = new Vector2(0, sprite.rect.size.y);
        vertices[1] = new Vector2(sprite.rect.size.x, 0);
        vertices[2] = new Vector2(0, 0);
        vertices[3] = new Vector2(sprite.rect.size.x, 0);
        vertices[4] = new Vector2(0, sprite.rect.size.y);
        vertices[5] = new Vector2(sprite.rect.size.x, sprite.rect.size.y);

        ushort[] triangles = new ushort[6];
        
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 3;
        triangles[4] = 4;
        triangles[5] = 5;
        // set sprite vertices
        orSprite.OverrideGeometry(vertices, triangles);

        return orSprite;
        // send new sprite to the sprite renderer
        //spriteRenderer.sprite = sprite;
    }
}
