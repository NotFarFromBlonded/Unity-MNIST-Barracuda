using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drawOnTexture : MonoBehaviour
{
    public Texture2D baseTexture;
    public bool windows;
    public bool android;
    public Text predictedValue;
    public Text predictedValueScore;

    // Update is called once per frame
    void Update()
    {
       
        DrawingOnBlank();  
    }

    private void DrawingOnBlank()
    {
        if(Camera.main == null)
        {
            throw new Exception("Cannot find camera");
        }

        if((Input.GetMouseButton(0) && Input.GetMouseButton(1))||((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(mouseRay, out hit))
            {
                return;
            }

            if (hit.collider.transform != transform)
            {
                return;
            }

            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= baseTexture.width;
            pixelUV.y *= baseTexture.height;

            Color colorToSet = Input.GetMouseButton(0) ? Color.white : Color.black;

            baseTexture.SetPixel((int)pixelUV.x, (int)pixelUV.y, colorToSet);
            baseTexture.Apply();
        }

        
    }

    public void ResetBoardValue()
    {
        var colors = baseTexture.GetPixels();
        for (int i = 0; i < colors.Length; i++)
            if (colors[i] == Color.white)
                colors[i] = Color.black;
        baseTexture.SetPixels(colors);
        baseTexture.Apply();
        predictedValue.text = "";
        predictedValueScore.text = "";
    }
}
