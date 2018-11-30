//--------------------
//This script holds a list in Memory of all new Objects created during runtime
//run the fuction writeFile() to add new object
//---------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INI_NewObjectsList : MonoBehaviour {

    public GameObject newObject;

    private IniFile ini;

    bool created;
    int counter = 0;
    string ObjectToCreate;

    public void writeFile(GameObject newObject)
    {
        counter += 1;
        ini.SetString("GameObject" + counter, newObject.name);
        ini.SetInt("counter", counter);
    }

    private void OnApplicationQuit()
    {
        ini.Save("NewObjectsList");
    }

    private void Start()
    {
        StartCoroutine(delayStart());
    }

    IEnumerator delayStart()
    {
        //delay to make sure the objects(GameObject.find) are in the scene
        yield return new WaitForSeconds(1);

        //get the file
        ini = new IniFile("NewObjectsList");

        //read the counter
        counter = ini.GetInt("counter");
        //print(counter);

        if (ini.GetString("GameObject" + counter) != null)
        {
            //ini.GetString("GameObject" + counter);
            // print(ObjectToCreate);
        }

        for (int i = 1; i <= counter; i++)
        {
            //print(i);
            //find the gameObject with the same name as on the list and duplicate it 
            //print("create this gameObject:  " + ini.GetString("GameObject" + i));
            //print("create this gameObject:  " + ini.GetString("GameObject" + i));
            if(i == 1) Instantiate(GameObject.Find(ini.GetString("GameObject" + i)));
            if(i == 2) Instantiate(GameObject.Find(ini.GetString("GameObject" + i)));
            if (i == 3) Instantiate(GameObject.Find(ini.GetString("GameObject" + i)));
            if (i == 4) Instantiate(GameObject.Find(ini.GetString("GameObject" + i)));
        }
    }
}

