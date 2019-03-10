using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectSpecificMenu : MonoBehaviour
{
    #region variables
    public RayPointer rayPointer;
    //selected object text
    public Text selObjText;

    //select object tool
     bool selectObjectToolActive;
    GameObject lastObjSelected;
    int countObj;
    int countCol;

    //merged object
    GameObject mergedObjects;

    //duplicated object
    GameObject duplicatedObjectSelected;

    //prefab with all the scripts for easy management
    public GameObject mergedObjectsPrefab;

    //selected object material 
    public Material selectedObjMaterial;
    [Space(20)]

    //list of selected gameobjects
    public List<GameObject> objectsSelected = new List<GameObject>();
    List<Transform> objectsSelectedChildren = new List<Transform>();

    //initial colors and materials
    public List<Color> objectsSelectedColors = new List<Color>();
    public List<Material> objectsSelectedMaterials = new List<Material>();

    //paint brush
    public A_PaintBrush paintBrush;

    #endregion

    void Start()
    {
    }

    public void SelectObjectTool()
    {
        if (selectObjectToolActive)
        {
            rayPointer.rayLength = 0.3f;
            rayPointer.raypointerGlobal = false;
            selectObjectToolActive = false;
        }
        else
        {
            rayPointer.rayLength = 1000000f;
            rayPointer.raypointerGlobal = true;
            selectObjectToolActive = true;
        }
    }

    public void DuplicateObject()
    {
        countObj = 0;
        foreach (GameObject obj in objectsSelected)
        {
            //create duplicate
            duplicatedObjectSelected = Instantiate(obj);

            //clicked object script problem
            obj.GetComponent<SelectableObject>().isClicked = false;

            //move it a little for visibility
            duplicatedObjectSelected.transform.position = duplicatedObjectSelected.transform.position + new Vector3(0.05f, 0.05f, 0.05f);

            //change colors of the duplicate-----------------------------
            int i = 0;
            foreach (Transform trans in duplicatedObjectSelected.transform)
            {
                if (trans.GetComponent<Renderer>() != null)
                {
                        trans.GetComponent<Renderer>().material = objectsSelectedMaterials[i];
                        trans.GetComponent<Renderer>().material.color = objectsSelectedColors[i];
                    }
                i++;
            }
            //--------------------------------------------------------


            //change colors of the initial object------------------------------
            int ii = 0;
            foreach (Transform trans in obj.transform)
            {
                if (trans.GetComponent<Renderer>() != null)
                {
                    trans.gameObject.GetComponentInChildren<Renderer>().material = objectsSelectedMaterials[ii];
                    trans.gameObject.GetComponentInChildren<Renderer>().material.color = objectsSelectedColors[ii];
                }
                ii++;
            }
            //---------------------------------------------------------

            //parent it to globalpaint
            duplicatedObjectSelected.transform.parent = GameObject.Find("Paint_Global").transform;
            
            //remove from the list of seleceted objects
            countObj++;
        }
        //clear lists
        objectsSelectedMaterials.Clear();
        objectsSelectedColors.Clear();
        objectsSelected.Clear();

    }

    public void MergeObjects()
    {
        
        if (objectsSelected.Count >= 2)
        {
        print("merge " + objectsSelected.Count + " Objects");
        mergedObjects = Instantiate(mergedObjectsPrefab);
        mergedObjects.transform.parent = GameObject.Find("Paint_Global").transform;

            countObj = 0;
            foreach(GameObject obj in objectsSelected)
            {
                //restore colors and materials
                foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>())
                    {
                        //print(countCol);
                        rend.material = objectsSelectedMaterials[countCol];
                        rend.material.color = objectsSelectedColors[countCol];
                        countCol++;
                    }
                //get each child object with the mesh and add it to the list of children
                foreach (Transform trans in obj.GetComponent<Transform>())
                {
                objectsSelectedChildren.Add(trans);
                }
                //get each transform in the list of childrend and parent it to the merged object
                foreach (Transform trans in objectsSelectedChildren)
                {
                    trans.parent = mergedObjects.transform;
                    //add the save prefab script
                   // if (!mergedObjects.GetComponent<SaveMeshInEditor>())
                    //{
                    //    mergedObjects.AddComponent<SaveMeshInEditor>();
                   // }
                    //then destroy the leftover objects
                    Destroy(obj);
                }
                countObj++;
            }
           
            //countObj = 0;
            //foreach (GameObject obj in objectsSelected)
            //{
            //        if (obj.GetComponentInChildren<Renderer>() != null)
            //        {
            //            //restore colors and materials
            //            obj.gameObject.GetComponentInChildren<Renderer>().material = objectsSelectedMaterials[countObj];
            //            obj.gameObject.GetComponentInChildren<Renderer>().material.color = objectsSelectedColors[countObj];

            //            // remove from the list of colors and materials
            //            objectsSelectedMaterials.Remove(obj.GetComponentInChildren<Renderer>().material);
            //            objectsSelectedColors.Remove(obj.GetComponentInChildren<Renderer>().material.color);
            //            countObj--;
            //        }
            //        //destroy its components
            //        foreach (Component comp in obj.GetComponents<Component>())
            //        { if (!(comp is Transform)) { Destroy(comp); } }

            //        obj.transform.GetChild(0).parent = mergedObjects.transform;
            //        // if (obj.transform.GetChild(1).transform != null)
            //        // {
            //        //      obj.transform.GetChild(1).parent = mergedObjects.transform;
            //        //  }
            //        //get the objects with the mesh and parent it directly to the merged object
            //        foreach (Transform trans in obj.transform)
            //        {
            //            print(obj.transform.GetChildCount());
            //            trans.parent = mergedObjects.transform;
            //        }

            //    countObj++;
            //    Destroy(obj);
            //}

            objectsSelected.Clear();
            objectsSelectedChildren.Clear();
            objectsSelectedMaterials.Clear();
            objectsSelectedColors.Clear();
            countCol = 0;
        }

    }

    public void ObjectSelected(GameObject incomingObjSelected)
    {
        lastObjSelected = incomingObjSelected;
        lastObjSelected.name = incomingObjSelected.name; 

        //add it to the list
        objectsSelected.Add(lastObjSelected);

        //display its name in the panel
        selObjText.text = selObjText.text.Replace("no objects currently selected", "");
        selObjText.text = selObjText.text + "\n" + "- " + incomingObjSelected.name;

        //change its material
        foreach (Transform obj in incomingObjSelected.transform)
        {
            //add to the list
            if (obj.GetComponent<Renderer>() != null)
            {
                objectsSelectedMaterials.Add(obj.GetComponent<Renderer>().material);
                objectsSelectedColors.Add(obj.GetComponent<Renderer>().material.color);

                //asign the selObjMat
                obj.GetComponent<Renderer>().material = selectedObjMaterial;

                //change the colors of each mesh renderer in the list to match to initial colors
                int i = 0;
                foreach (Material mat in objectsSelectedMaterials)
                {
                    mat.color = objectsSelectedColors[i];
                    obj.GetComponent<Renderer>().material.color = objectsSelectedColors[i];
                    i++;
                }
            }
        }
    }

    public void ObjectDeselected(GameObject incomingObjDeselected)
    {
    //change the materials and colors of each mesh renderer in the list to match to initial colors and mat------------------------------

       // for each child of the object DeSelected
        foreach (Transform obj in incomingObjDeselected.transform)
            {
            //change the colors and materials back
            if (obj.gameObject.GetComponent<Renderer>() != null)
            {
              //  print(objectsSelected.IndexOf(obj.gameObject));
                obj.gameObject.GetComponent<Renderer>().material = objectsSelectedMaterials[objectsSelected.IndexOf(incomingObjDeselected.gameObject)];
                obj.gameObject.GetComponent<Renderer>().material.color = objectsSelectedColors[objectsSelected.IndexOf(incomingObjDeselected)];
            }

            // remove from the list of colors and materials
            objectsSelectedMaterials.Remove(obj.GetComponent<Renderer>().material);
            objectsSelectedColors.Remove(obj.GetComponent<Renderer>().material.color);
        }
    //-----------------------------------------------------------

        //remove from the list of seleceted objects
        objectsSelected.Remove(incomingObjDeselected);

        //delete name from display panel 
        selObjText.text = selObjText.text.Replace("\n" + "- " + incomingObjDeselected.name, "");

    }

    public void SaveObject()
    {
   

        if (objectsSelected.Count == 0)
        {
            selObjText.text = "!!!No objects selected to save!!!";
        }
        else
        {
            foreach (GameObject paintObject in objectsSelected)
            {
            #if UNITY_EDITOR
                if (paintObject.GetComponent<SaveMeshInEditor>())
                {
                    paintObject.GetComponent<SaveMeshInEditor>().SaveAsset();
                    selObjText.text = paintObject.name + " saved";
                }
            #endif
            }
        }
    }
}