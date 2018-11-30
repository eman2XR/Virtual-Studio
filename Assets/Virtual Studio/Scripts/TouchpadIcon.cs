using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TouchpadIcon : MonoBehaviour {

    public Button brush;
    public Button scaler;
    public Button eraser;

    bool reverted;
    A_PaintBrush pB;

    // Use this for initialization
    void Start () {
        if ( this.transform.parent.GetComponent<A_PaintBrush>() == null)
        { print("[Paint_Brush] doesn't seem to exist or has been moved?"); }
        else { pB = this.transform.parent.GetComponent<A_PaintBrush>();}
    }
	
	// Update is called once per frame
	void Update () {
        if(pB.controller != null)
        {
            if (pB.controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                checkBrush();
                checkEraser();
                checkScaler();
            }
        }
        
      }

    void checkEraser()
    {
        if (pB.controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) && pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y <= -0.5f)
        {
           // print("touching eraser");
            eraser.targetGraphic.color = eraser.colors.highlightedColor;
            reverted = false;
        }
        else if (!reverted){
            eraser.targetGraphic.color = eraser.colors.normalColor;
        }
    }
    void checkBrush()
    {
        if (pB.controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) && pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y >= 0.5f)
        {
            brush.targetGraphic.color = brush.colors.highlightedColor;
            reverted = false;
        }
        else if (!reverted)
        {
            brush.targetGraphic.color = brush.colors.normalColor;
        }
    }
    void checkScaler()
    {
        if (pB.controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad) && pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y >= -0.4f && pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y <= 0.4f)
        { scaler.targetGraphic.color = scaler.colors.highlightedColor;
            reverted = false;
        }
        else if (!reverted)
            {
                scaler.targetGraphic.color = scaler.colors.normalColor;
            }
    }
}
