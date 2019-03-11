using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVRObjectGrab : MonoBehaviour
{
    Transform initialParent;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

    bool touched;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    GameObject collidingObject;//To keep track of what objects have rigidbodies
    GameObject objectInHand;//To track the object you're holding

    void OnTriggerEnter(Collider other)//Activate function in trigger zone, checking rigidbodies and ignoring if no rigidbodies 
    {
        if (!touched)
        {
            touched = true;
            if (other.GetComponentInParent<GrabbableObject>())
            {
                collidingObject = other.transform.parent.gameObject;
                initialParent = collidingObject.transform.parent;
                collidingObject.GetComponent<GrabbableObject>().Touched(this.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (touched)
        {
            if (other.GetComponentInParent<GrabbableObject>())
            {
                touched = false;
                if(collidingObject)
                    collidingObject.GetComponent<GrabbableObject>().UnTouched(this.gameObject);
                collidingObject = null;
            }
        }
    }

    void Update()
    {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))// Push grip buttons and touching object, set GrabObject function
        {
            //print("grip pressed");
            if (collidingObject)
                GrabObject();
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))// If release grip buttons and holding object, set to release
        {
            if (objectInHand)
                ReleaseObject();
        }
    }

    private void GrabObject() // Picking up object and assigning objectInHand variable
    {
        objectInHand = collidingObject;
        objectInHand.transform.SetParent(this.transform);
        objectInHand.GetComponent<GrabbableObject>().Grabbed(this.gameObject);
    }

    // Releasing object 
    private void ReleaseObject()
    {
        objectInHand.transform.SetParent(initialParent);
        objectInHand.GetComponent<GrabbableObject>().Released(this.gameObject);
    }

}