//======= Modified version from unity wiki TransformSaver ===============
//
// it looks for GameObjects with tags "saveable" and records their transforms
// when application closes. By using the menu TransformSave the recorded 
// transforms can be applied in the editor mode
//
// How to use: Drag on a gameobject in the scene. That's it!
//=============================================================================

using UnityEngine;
using UnityEditor;
using System.Collections;

public class TransformSaver : MonoBehaviour
{
    public bool showRecordWindow;

    public class TransformSave
    {
        public int instanceID;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;

        public TransformSave(int instanceID, Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            this.instanceID = instanceID;
            this.position = position;
            this.rotation = rotation;
            this.localScale = localScale;
        }
    }

    private static ArrayList transformSaves = new ArrayList();

    [MenuItem("TransfromSaver/Transform Saver/Record Selected Transforms")]
    void OnApplicationQuit()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("saveable");
        Transform[] selection = new Transform[objects.Length];

        for (int i = 0; i < objects.Length; ++i)
           selection[i] = objects[i].transform;

        transformSaves = new ArrayList(selection.Length);

        foreach (Transform selected in selection)
        {
            TransformSave transformSave = new TransformSave(selected.GetInstanceID(), selected.position, selected.rotation, selected.localScale);
            transformSaves.Add(transformSave);
        }

        if (showRecordWindow)
        {
            EditorUtility.DisplayDialog("Transform Saver Record", "Recorded " + transformSaves.Count + " Transforms.", "OK", "");
        }
    }

    [MenuItem("TransfromSaver/Transform Saver/Apply Saved Transforms")]
    public static void DoApply()
    {
        Transform[] transforms = FindObjectsOfType(typeof(Transform)) as Transform[];
        int numberApplied = 0;

        foreach (Transform transform in transforms)
        {
            TransformSave found = null;

            for (int i = 0; i < transformSaves.Count; i++)
            {
                if (((TransformSave)transformSaves[i]).instanceID == transform.GetInstanceID())
                {
                    found = (TransformSave)transformSaves[i];
                    break;
                }
            }

            if (found != null)
            {
                transform.position = found.position;
                transform.rotation = found.rotation;
                transform.localScale = found.localScale;
                numberApplied++;
            }
        }

        //EditorUtility.DisplayDialog("Transform Saver Apply", "Applied " + numberApplied + " Transforms successfully out of " + transformSaves.Count + " possible.", "OK", "");
    }
}