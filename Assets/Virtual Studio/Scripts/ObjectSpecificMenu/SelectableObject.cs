using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {

  //  [HideInInspector]
    public bool isHovered;
   // [HideInInspector]
    public bool isClicked;
    //
    ObjectSpecificMenu objMenu;

    private void Awake()
    {
        isClicked = false;
        isHovered = false;
    }
    public void OnClick()
    {
      //  print(this.gameObject.name + " has beenclicked");

        if (isClicked)
        {
            isClicked = false;
            GameObject.Find("[2]Object Specific Menu").SendMessage("ObjectDeselected", this.gameObject);
        }
        else
        {
            isClicked = true;
            GameObject.Find("[2]Object Specific Menu").SendMessage("ObjectSelected", this.gameObject);

        }
    }
}
