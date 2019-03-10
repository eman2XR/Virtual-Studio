using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestNetMessage : NetworkBehaviour
{
    [SyncVar]
    public int testInt;
    int oldTestInt;

    public Text text;

    [ClientRpc]
    void RpcChangeInt(int amount)
    {
        Debug.Log("Changed int by:" + amount);
        text.text = amount.ToString();
    }

    private void Start()
    {
        oldTestInt = testInt;
    }

    private void Update()
    {
        if (oldTestInt != testInt)
        {
            print("test int changed");
            RpcChangeInt(testInt - oldTestInt);
            oldTestInt = testInt;
        }
    }
}

