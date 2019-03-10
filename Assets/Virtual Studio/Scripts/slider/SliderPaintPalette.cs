using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderPaintPalette : MonoBehaviour {

    #region variables

    //displayValue
    public Text displayValue;

    //slider sensitivity
    [Range(3f, 10f)]
    public float sliderSensitivity = 7f;
   
    //target scale
    public enum targetScaling { scaleXYZ, scaleX, scaleY, scaleZ, density };
    public targetScaling ScalingTarget;

    //Get masterPaint script
    [HideInInspector]
    public A_PaintPalette paintMaster;

    //ui slider
    [HideInInspector]
    public Slider slider;

    //tip of ray
    Vector3 iTipPos;
    float distanceDragged;
        
    //bool
    bool wasClicked;

    float oldValue;

    public float slidingValue;

    //hover 
    bool hoverEntry;
    bool hoverExit;
    #endregion

    void Start () {

        //ui slider
		slider = this.GetComponent<Slider>();

        //paintmaster
        paintMaster = this.transform.parent.parent.parent.parent.gameObject.GetComponent<A_PaintPalette>();
        //print(paintMaster.name);

        oldValue = slider.value;

    }

    void ClickEvent()
    {
        if (paintMaster.ray.objectHit == this.gameObject)
        {
            wasClicked = true;
        }
    }

    void ClickHold()
    {
        if (wasClicked)
        {
           // print("changing");
           slider.value = slidingValue;
           paintMaster.ray.busy = true;
        }
    }

    void UnClick()
    {
        wasClicked = false;
        paintMaster.ray.busy = false;
    }

    void Update()
    {
        if (paintMaster.ray.objectHit == this.gameObject)
        {
            slidingValue = Mathf.Clamp((-paintMaster.ray.Tip.transform.InverseTransformPoint(transform.position).x) / sliderSensitivity + 3, 0f, 7f);
            // print(slidingValue);
        }

        if (slider.value != oldValue)
        {
            // print("size changed");
            switch (ScalingTarget)
            {
                case targetScaling.scaleXYZ:
                    paintMaster.selectSize(slider.value, "XYZ");
                    break;
                case targetScaling.scaleX:
                    paintMaster.selectSize(slider.value, "X");
                    break;
                case targetScaling.scaleY:
                    paintMaster.selectSize(slider.value, "Y");
                    break;
                case targetScaling.scaleZ:
                    paintMaster.selectSize(slider.value, "Z");
                    break;
                case targetScaling.density:
                    paintMaster.selectDensity(slider.value / 7f);
                    displayValue.text = ((slidingValue / 7f) * 100) + " cm";
                    break;
            }

            oldValue = slider.value;
        }

        //check for OnHover
        if (paintMaster.ray.objectHit == this.gameObject)
        {
            HoverEntry();
        }
        else
        {
            HoverExit();
        }
    }

    public void HoverEntry()
    {
        if (!hoverEntry)
        {
            hoverEntry = true;
            //print("hover entry");
            slider.targetGraphic.color = slider.colors.highlightedColor;
            hoverExit = false;
        }
    }
    public void HoverExit()
    {
        if (!hoverExit)
        {
            hoverExit = true;
            //   print("hover exit");
            slider.targetGraphic.color = Color.white;
            hoverEntry = false;
        }
    }
}
