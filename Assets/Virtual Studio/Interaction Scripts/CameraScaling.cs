using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour {

    public Transform cameraRig;
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;

    bool bothGripPressed;
    bool leftGripPressed;
    bool rightGripPressed;

    float initialDistance;
    float currentDistance;

    Vector3 initialScale;
    public float sensitivity = 0.5f;

    void Start()
    {
        if (!cameraRig)
            cameraRig = this.transform;
        leftController.Gripped += LeftGrip;
        leftController.Ungripped += LeftUnGrip;
        rightController.Gripped += RightGrip;
        rightController.Ungripped += RightUnGrip;
    }

    void LeftGrip(object sender, ClickedEventArgs e)
    {
        leftGripPressed = true;
        Debug.Log("Left Grip has been pressed");
        if (rightGripPressed)
        {
            if (!bothGripPressed)
            {
                bothGripPressed = true;
                print("both grip pressed");
                StartCoroutine(IsScaling());
            }
        }
    }
    void LeftUnGrip(object sender, ClickedEventArgs e)
    {
        leftGripPressed = false;
        bothGripPressed = false;
    }

    void RightGrip(object sender, ClickedEventArgs e)
    {
        rightGripPressed = true;
        Debug.Log("Right Grip has been pressed");
        if (leftGripPressed)
        {
            if (!bothGripPressed)
            {
                bothGripPressed = true;
                print("both grip pressed");
                StartCoroutine(IsScaling());
            }
        }
    }
    void RightUnGrip(object sender, ClickedEventArgs e)
    {
       rightGripPressed = false;
       bothGripPressed = false;
    }

    IEnumerator IsScaling()
    {
        initialDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
        initialScale = this.transform.localScale;
        while (bothGripPressed)
        {
            currentDistance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
            float distance = Vector3.Distance(leftController.transform.position, rightController.transform.position);
            this.transform.localScale = new Vector3(((currentDistance - initialDistance) * sensitivity) + initialScale.x, ((currentDistance - initialDistance) * sensitivity) + initialScale.y, ((currentDistance - initialDistance) * sensitivity) + initialScale.z);
            yield return null;
        }
    }
}
