using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_PaintScaler : MonoBehaviour {

    // paint_brush script
    A_PaintBrush pB;

    //size
    public float width;

	void Start () {
        pB = this.transform.parent.GetComponent<A_PaintBrush>();
	}

    void Update()
    {
        if (pB.controller != null)
        {
            if (pB.controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y >= -0.4f && pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y <= 0.4f)
            {
                Vector2 touchpad = (pB.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
                //pB.SizeX = touchpad.x + 1;
                //  print("Pressing Touchpad" + touchpad.y * 10 + " x:" + touchpad.x * 10);
                pB.SizeSelection(touchpad.x + 1, "XYZ");
            }
            
        }
    }
 }

