using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pp_PaintSave : MonoBehaviour {

    Vector3[] newPos;


    void Start()
    {
        LineRenderer oldLineComponent = this.GetComponent<LineRenderer>();

        //Get old Position Length
        newPos = new Vector3[oldLineComponent.positionCount];
        //Get old Positions
        oldLineComponent.GetPositions(newPos);

        //Copy Old postion to the new LineRenderer
       // newLine.GetComponent<LineRenderer>().SetPositions(newPos);
       // print(newPos.ToString());
        
        StartCoroutine(delayStart());
    }

    IEnumerator delayStart()
    {
        //wait until evertyhing loads
        yield return new WaitForSeconds(1f);

        //When we load the application we want to load our position and rotation
        if (PlayerPrefsX.GetVector3Array(this.gameObject.name + "PaintPos") != null)
        {
           // print("data found");
            this.GetComponent<LineRenderer>().SetPositions(PlayerPrefsX.GetVector3Array(this.gameObject.name + "PaintPos"));
        }
    }

    void OnApplicationQuit()
    {
        //When we quit the application we want to save our position and rotation
        PlayerPrefsX.SetVector3Array(this.gameObject.name + "PaintPos", newPos);      
    }

}
