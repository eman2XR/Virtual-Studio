using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pp_NewObject : MonoBehaviour {

    public INI_NewObjectsList newObjectsList;

     GameObject newObject;
     static GameObject sourceOfObject;
     string nameOfNewObject;

    public void createNewObject(GameObject sourceObject, GameObject createdObject) {

        createdObject = newObject;
        sourceOfObject = sourceObject;

        if (PlayerPrefsX.GetVector3(createdObject.name + "_Scale") != Vector3.zero)
        {
            print("gameObject exists!");
        }
    }

    private void Start()
    {
        //nameOfNewObject = PlayerPrefsPlus.GetString()
        //newObject = Instantiate(GameObject.Find("");
    }
     
}
