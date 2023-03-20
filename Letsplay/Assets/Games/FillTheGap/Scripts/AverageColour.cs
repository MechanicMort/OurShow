using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AverageColour : MonoBehaviour
{


    private Image thisButton;
    private void Start()
    {
        
       
    }

    public void FixedUpdate()
    {
        // thisButton.transform.position = GetComponentInParent<SpriteRenderer>().sprite.pivot / centreAdjuster;
        thisButton = GetComponentInChildren<Image>();
        thisButton.color = AverageColorFromTexture(GetComponentInParent<SpriteRenderer>().sprite.texture, GetComponentInParent<SpriteRenderer>());
    }

    Color32 AverageColorFromTexture(Texture2D tex,SpriteRenderer spriteRenderer)
    {

        Color32[] texColors = tex.GetPixels32();
 
        

        int total = Mathf.RoundToInt(spriteRenderer.sprite.pivot.x) * Mathf.RoundToInt(spriteRenderer.sprite.pivot.y);

        float r = 0;
        float g = 0;
        float b = 0;
        r = tex.GetPixels(Mathf.RoundToInt(spriteRenderer.sprite.pivot.x), Mathf.RoundToInt(spriteRenderer.sprite.pivot.y), 1, 1)[0].r * 255;
        //texColors[total].r;

        g = tex.GetPixels(Mathf.RoundToInt(spriteRenderer.sprite.pivot.x), Mathf.RoundToInt(spriteRenderer.sprite.pivot.y), 1, 1)[0].g * 255;
        //texColors[total].g;

        b = tex.GetPixels(Mathf.RoundToInt(spriteRenderer.sprite.pivot.x), Mathf.RoundToInt(spriteRenderer.sprite.pivot.y), 1, 1)[0].b * 255;
        //texColors[total].b;

        return new Color32((byte)(r ), (byte)(g ), (byte)(b), 255);

    }
}
