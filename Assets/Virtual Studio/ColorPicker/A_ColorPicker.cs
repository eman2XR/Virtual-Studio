using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_ColorPicker : MonoBehaviour {

    //paintMaster
    public A_PaintPalette paintPalette;

    //Rbg text
    public Text RGBtext;

    Image colorPreview;

    RayPointer ray;
    Texture2D tex;
    
    public Color tempColor;
    public Color SelectedColor;

    void Start()
    {
        //paintMaster
        if (paintPalette == null)
        { paintPalette = this.transform.parent.parent.parent.GetComponent<A_PaintPalette>(); }
        
        //get raypoint script
        ray = paintPalette.ray.GetComponent<RayPointer>();

        //get the texture
        tex = (Texture2D)this.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture;

        //color preview
        colorPreview = this.transform.GetChild(1).gameObject.GetComponent<Image>();

        //RGB text
        if (RGBtext == null)
        { RGBtext = this.transform.GetChild(2).GetChild(2).GetComponent<Text>(); }
    }

    void ClickEvent()
    {
        if (ray.objectHit == this.gameObject)
        {
           // print("colorSelected");
            SelectedColor = tempColor;
            paintPalette.color = SelectedColor;
            paintPalette.selectColor(SelectedColor);
        }
    }

    void Update()
    {
        //chech if rayPointer is hitting the color panel
        if (ray.objectHit != null)
        {
            if (ray.objectHit == this.gameObject)
            {
                //print("onhover color picker");
                // print(tex.GetPixelBilinear(ray.hit.textureCoord.x, ray.hit.textureCoord.y ));
                tempColor = tex.GetPixelBilinear(ray.hit.textureCoord.x, ray.hit.textureCoord.y);
                colorPreview.color = tempColor;
                RGBtext.text = tempColor.ToString();
            }
        }
    }

   
}


