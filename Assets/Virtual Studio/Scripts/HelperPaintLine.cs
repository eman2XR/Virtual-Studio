using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperPaintLine : MonoBehaviour {

    A_PaintBrush pb;

    LineRenderer lineRenderer;

    Vector3 distance;

    bool reset;
    // Use this for initialization
    void Start () {
        
        if(this.transform.parent.gameObject.GetComponent<A_PaintBrush>() == null)
        {
            print("[Paint Brush Script not found");
        }
        pb = this.transform.parent.gameObject.GetComponent<A_PaintBrush>();

        lineRenderer = this.GetComponent<LineRenderer>();

        pb.helperLineRenderer.SetPosition(0, Vector3.zero);
        pb.helperLineRenderer.SetPosition(1, Vector3.zero);
    }

    // Update is called once per frame
    void Update ()
    {
       // print(pb.paintDensity);
        //helper line
        if (pb.controller != null)
        {
            if (pb.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (!pb.ray.busy)
                {
                    pb.helperLineRenderer.SetPosition(0, pb.paintBrushHolder.position);
                }
            }
            if (pb.controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (!pb.ray.busy)
                {
                    pb.helperLineRenderer.SetPosition(1, pb.paintBrushHolder.position);

                    if (Vector3.Distance(pb.helperLineRenderer.GetPosition(0), pb.helperLineRenderer.GetPosition(1)) > (pb.paintDensity))
                    {
                        pb.helperLineRenderer.SetPosition(0, pb.paintBrushHolder.position);
                    }
                   
                }
            }
            if (pb.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
             //   print("resetPos");
                   pb.helperLineRenderer.SetPosition(0, pb.paintBrushHolder.position);
                    pb.helperLineRenderer.SetPosition(1, pb.paintBrushHolder.position);
            }
        }


        

    }
}
