using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour {

    public delegate void OnObjectGrabbed();
    public OnObjectGrabbed objectGrabbedEvent;
    public delegate void OnObjectReleased();
    public OnObjectReleased objectReleasedEvent;

    [HideInInspector]
    public bool isGrabbedByLeftHand;
    [HideInInspector]
    public bool isGrabbedByRightHand;

   
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
