using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pp_TransformSave : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(delayStart());
    }

    IEnumerator delayStart()
    {
        //wait until evertyhing loads
        yield return new WaitForSeconds(1.2f);

        //When we load the application we want to load our position and rotation
        if (PlayerPrefsX.GetVector3(this.name + "_Scale") != Vector3.zero)
        {
            transform.localPosition = PlayerPrefsX.GetVector3(this.name + "_Position");
            transform.localRotation = PlayerPrefsX.GetQuaternion(this.name + "_Rotation");
            transform.localScale = PlayerPrefsX.GetVector3(this.name + "_Scale");
        }
    }

        void OnApplicationQuit()
    {
        //When we quit the application we want to save our position and rotation
        PlayerPrefsX.SetVector3(this.name + "_Position", transform.localPosition);
        PlayerPrefsX.SetQuaternion(this.name + "_Rotation", transform.localRotation);
        PlayerPrefsX.SetVector3(this.name + "_Scale", transform.localScale);
    }

}
