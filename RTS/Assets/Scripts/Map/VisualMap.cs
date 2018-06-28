using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMap : MonoBehaviour {

    private SpriteRenderer renderer;
    private Renderer mRenderer;

    [SerializeField]private List<Color> colors;
    [SerializeField] private List<float> limits;

    public Color[] colours;

    void Start () {
        renderer = GetComponent<SpriteRenderer>();
        mRenderer = GetComponent<Renderer>();
        //print(GetComponent<Renderer>());

    }



    public void fillTexture(float value, int column, int row, int width)
    {
        if (value >= limits[5])
        {
            colours[row * width + column] = colors[5];
        }
        if (value >= limits[4])
        {
            colours[row * width + column] = colors[4];
        }
        if (value >= limits[3])
        {
            colours[row * width + column] = colors[3];
        }
        if (value >= limits[2])
        {
            colours[row * width + column] = colors[2];
        }
        if (value >= limits[1])
        {
            colours[row * width + column] = colors[1];
        }
        if (value >= limits[0])
        {
            colours[row * width + column] = colors[0];
        }
    }

    public void applyTexture()
    {
        renderer.sprite.texture.SetPixels(colours);
        renderer.sprite.texture.Apply();
    }

    public void applyTexture(int width,int height)
    {
        Texture2D texture = new Texture2D(width, height);

        texture.SetPixels(colours);
        texture.Apply();
        mRenderer.material.SetTexture("_MainTex",texture);
    }
    public void generateTexture(float[,] values,int width,int height,float maxValue)
    {
        Color[] colours = new Color[256 * 256];
        float newValue;

        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                //print(values[i, j]);
                if (values[i, j] >= limits[5])
                {
                    colours[j * 256 + i] = colors[5];

                }
                if (values[i, j] >= limits[4])
                {
                    colours[j * 256 + i] = colors[4];
                }
                if (values[i, j] >= limits[3])
                {
                    colours[j * 256 + i] = colors[3];
                }
                if (values[i, j] >= limits[2])
                {
                    colours[j * 256 + i] = colors[2];
                }
                if (values[i, j] >= limits[1])
                {
                    colours[j * 256 + i] = colors[1];
                }
                if (values[i, j] >= limits[0])
                {
                    colours[j * 256 + i] = colors[0];
                }

            }
        }

        Texture2D texture = new Texture2D(256,256);

        texture.SetPixels(colours);
        texture.Apply();

        Rect newRect = new Rect(0, 0, texture.width, texture.height);

        Sprite sprite = Sprite.Create(texture,newRect,renderer.sprite.pivot);
        renderer.sprite = sprite;
    }
}
