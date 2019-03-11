using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public bool outlineOnTouch = true;
    public Material outlineMaterial;

    public delegate void OnObjectGrabbed();
    public OnObjectGrabbed objectGrabbedEvent;
    public delegate void OnObjectReleased();
    public OnObjectReleased objectReleasedEvent;

    [HideInInspector]
    public bool isGrabbedByLeftHand;
    [HideInInspector]
    public bool isGrabbedByRightHand;

    GameObject highlightModel;
    bool highLightModelCreated;

    public void Touched(GameObject controller)
    {
        if (outlineOnTouch)
        {
            foreach (Transform trans in this.GetComponentInChildren<Transform>())
            {
                if (trans.GetComponent<MeshFilter>())
                {
                    if (!highLightModelCreated)
                    {
                        highLightModelCreated = true;
                        highlightModel = Instantiate(trans.gameObject, trans);
                        if (!outlineMaterial)
                            outlineMaterial = Resources.Load("Outline.mat", typeof(Material)) as Material;
                        highlightModel.GetComponent<Renderer>().material = outlineMaterial;
                    }
                    if (highlightModel.GetComponent<Collider>())
                        Destroy(highlightModel.GetComponent<Collider>());
                }
            }
        }
    }

    public void UnTouched(GameObject controller)
    {
        if (outlineOnTouch)
        {
            Destroy(highlightModel);
            highLightModelCreated = false;
        }
    }

    public void Grabbed(GameObject controller)
    {
        if (controller.name == "Controller (left)")
            isGrabbedByLeftHand = true;

        if (controller.name == "Controller (right)")
            isGrabbedByRightHand = true;

        if (objectGrabbedEvent != null) objectGrabbedEvent();
    }

    public void Released(GameObject controller)
    {
        if (controller.name == "Controller (left)")
            isGrabbedByLeftHand = false;

        if (controller.name == "Controller (right)")
            isGrabbedByRightHand = false;

        if (objectReleasedEvent != null) objectReleasedEvent();
    }
}