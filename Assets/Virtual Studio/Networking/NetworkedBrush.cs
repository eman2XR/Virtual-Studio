using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;

public class NetworkedBrush : NetworkBehaviour {

    [SyncVar]
    public string positions;

    [SyncVar]
    public string rotations;

    PlayBackBrushStroke playBackBrushStroke;

    [ClientRpc]
    void RpcPlayBackOnClients()
    {
        playBackBrushStroke.positions = DeserializeVector3Array(positions);
        playBackBrushStroke.rotations = DeserializeVector3Array(rotations);

        playBackBrushStroke.PlayBackStroke();
    }

    public void CallPlayStrokeOnClients()
    {
        positions = SerializeVector3Array(playBackBrushStroke.positions);
        rotations = SerializeVector3Array(playBackBrushStroke.rotations);

        //wait to make sure data has been converted to string and updated on client
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        RpcPlayBackOnClients();
    }

    private void Start()
    {
        playBackBrushStroke = this.GetComponent<PlayBackBrushStroke>();
    }

    public static string SerializeVector3Array(Vector3[] aVectors)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Vector3 v in aVectors)
        {
            sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append("|");
        }
        if (sb.Length > 0) // remove last "|"
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    public static Vector3[] DeserializeVector3Array(string aData)
    {
        string[] vectors = aData.Split('|');
        Vector3[] result = new Vector3[vectors.Length];
        for (int i = 0; i < vectors.Length; i++)
        {
            string[] values = vectors[i].Split(' ');
            if (values.Length != 3)
                throw new System.FormatException("component count mismatch. Expected 3 components but got " + values.Length);
            result[i] = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }
        return result;
    }
}
