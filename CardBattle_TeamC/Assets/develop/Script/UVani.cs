using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVani :MonoBehaviour
{
    RawImage rawImage;
    //Rect uvrect;
    public float UVS = 5f;
    public bool Vertical = false;
    public bool VReverse = false;
    public bool Side = false;
    public bool SReverse = false;
    public bool Alpha = false;
    public float AlphaS = 0f;

    private float speed = 0.001f;
    private float Rect_x = 0f;
    private float Rect_y = 0f;
    private float alpha = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        //引数は順にX,Y,W,H
        rawImage.uvRect = new Rect(0f, 0f, 1f, 1f);

        speed = speed * UVS;
        alpha = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Side)
        {
            if (SReverse)
            {
                if (Rect_x > 1f)
                {
                    Rect_x = 0f;
                }
                else
                {
                    Rect_x += speed;
                }
            }
            else
            {
                if (Rect_x < 0f)
                {
                    Rect_x = 1f;
                }
                else
                {
                    Rect_x -= speed;
                }
            }
        }
        if (Vertical)
        {
            if (VReverse)
            {
                if (Rect_y > 1f)
                {
                    Rect_y = 0f;
                }
                else
                {
                    Rect_y += speed;
                }
            }
            else
            {
                if (Rect_y < 0f)
                {
                    Rect_y = 1f;
                }
                else
                {
                    Rect_y -= speed;
                }
            }
        }
        if (Alpha)
        {
            alpha = alpha + AlphaS;
            if (alpha < 0f || alpha > 1f)
            {
                AlphaS = AlphaS * -1;
            }
        }
        
        rawImage.uvRect =  new Rect(Rect_x, Rect_y, 1, 1);
        rawImage.color = new Color(1f, 1f, 1f, alpha);
    }
}
