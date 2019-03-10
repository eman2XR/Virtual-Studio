using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObject : MonoBehaviour {

    GrabbableObject grabObjectScript;

    Transform initialParent;

    float initialDistance;
    float currentDistance;
    Vector3 initialScale;
    public float sensitivity = 0.5f;

    GameObject leftController;
    GameObject rightController;

    void Start()
    {
        initialParent = this.transform.parent;

        grabObjectScript = this.GetComponent<GrabbableObject>();
        leftController = GameObject.Find("[CameraRig]").transform.GetChild(0).gameObject;
        rightController = GameObject.Find("[CameraRig]").transform.GetChild(1).gameObject;

        grabObjectScript.objectGrabbedEvent += ObjectGrabbed;
        grabObjectScript.objectReleasedEvent += ObjectReleased;
    }

    void ObjectGrabbed()
    {
        if(grabObjectScript.isGrabbedByLeftHand && grabObjectScript.isGrabbedByRightHand)
        {
            StartCoroutine(ScaleObject());
        }
    }

    void ObjectReleased()
    {
        
    }

    IEnumerator ScaleObject()
    {
        this.transform.parent = initialParent;
        initialDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
        initialScale = this.transform.localScale;
        while (grabObjectScript.isGrabbedByLeftHand && grabObjectScript.isGrabbedByRightHand)
        {
            currentDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
            float distance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
            Vector3 newScale = new Vector3(((currentDistance - initialDistance) * sensitivity) + initialScale.x, ((currentDistance - initialDistance) * sensitivity) + initialScale.y, ((currentDistance - initialDistance) * sensitivity) + initialScale.z);
            ScaleAround(this.gameObject, this.transform.GetChild(0).GetComponent<Renderer>().bounds.center, newScale);
            yield return null;
        }
    }

    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.transform.localScale.x; // relataive scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}
