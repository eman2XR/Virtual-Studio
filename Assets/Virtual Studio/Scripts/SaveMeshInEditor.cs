
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


public class SaveMeshInEditor : MonoBehaviour
{
    Transform selectedGameObject;

    public string savedObjectsPath = "Assets/Virtual Studio/RuntimeGeneratedObjects/";
    string savedObjectsMeshesPath;

    public static int meshCount;

    private void Start()
    {
       savedObjectsMeshesPath = savedObjectsPath + "meshes/";
    }

    public void SaveAsset()
    {
        foreach (Transform child in this.transform)
        {
            MeshFilter mf = child.GetComponent<MeshFilter>();
            if (mf)
            {
                var savePath = savedObjectsMeshesPath + this.transform.name + meshCount + ".asset";
                Debug.Log("Saved Mesh to:" + savePath);
                AssetDatabase.CreateAsset(mf.mesh, savePath);
                meshCount++;
                //AssetDatabase.AddObjectToAsset(mf.mesh, "Assets/AAVR_PaintTools/Advanced Paint/RuntimeGeneratedMeshes/" + this.gameObject.name + ".prefab");
                Destroy(this.transform.GetComponent<SaveMeshInEditor>());
            }
        }
        GameObject prefab = PrefabUtility.CreatePrefab(savedObjectsPath + this.gameObject.name + meshCount + ".prefab", (GameObject)this.gameObject, ReplacePrefabOptions.ReplaceNameBased);
    }
}

#endif