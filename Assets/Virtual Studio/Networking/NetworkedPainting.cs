using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class NetworkedPainting : NetworkBehaviour
{
    //[SyncVar]
    //public string positions;

    //[SyncVar]
    //public string rotations;

    //public Vector3[] positionsArray;
    //List<Vector3> positionsList = new List<Vector3>();
    //public Vector3[] rotationsArray;
    //List<Vector3> rotationsList = new List<Vector3>();

    ////recorder stuff
    //bool isPainting;
    //A_PaintBrush paintBrush;
    //Transform paintBrushPos;

    ////playback stuff
    //bool playbackOn;
    //int current = 0;
    //float WPradius = 1;
    //public float speed = 8;

    //GameObject currentBrush;

    //private void OnEnable()
    //{
    //    print(Resources.FindObjectsOfTypeAll<A_PaintBrush>()[0].name);
    //    paintBrush = Resources.FindObjectsOfTypeAll<A_PaintBrush>()[0].GetComponent<A_PaintBrush>();
    //    paintBrush.onBrushStrokeStart.AddListener(StartingBrushstroke);
    //    paintBrush.onBrushStrokeEnd.AddListener(EndingBrushstroke);

    //    paintBrushPos = paintBrush.transform.GetChild(1);

    //    currentBrush = this.transform.GetChild(0).gameObject;
    //    //A_PaintBrush.OnBrushStart += StartingBrushstroke;
    //    //A_PaintBrush.OnBrushEnd += EndingBrushstroke;
    //}

    //public void PlayBackStroke()
    //{
    //    //move into position and then make the extrusion permanent
    //  //  currentBrush.transform.position = positions[0];
    //  //  this.transform.rotation = Quaternion.Euler(rotations[0]);
    //    playbackOn = true;
    //}

    //void Update ()
    //{
    //    if (paintBrush.isPainting)
    //    {
    //        if (!isPainting)
    //        {
    //            isPainting = true;
    //            StartingBrushstroke();
    //        }
    //    }
    //    else
    //    {
    //        if (isPainting)
    //        {
    //            isPainting = false;
    //            EndingBrushstroke();
    //        }
    //    }

    //    ///-----playback brush stroke
    //    if (playbackOn)
    //    {
    //        if (transform.position == positions[1])
    //        {
    //            this.transform.GetChild(0).GetComponent<ExtrudeMesh>().time = 100;
    //        }
    //        if (Vector3.Distance(positions[current], transform.position) < WPradius)
    //        {
    //            current++;
    //            if (current == positions.Length)
    //            {
    //                playbackOn = false;
    //            }
    //        }
    //        transform.position = Vector3.MoveTowards(transform.position, positions[current - 1], Time.deltaTime * speed);
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotations[current - 1]), Time.deltaTime * speed);
    //    }
    //}

    //public void StartingBrushstroke()
    //{
    //    isPainting = true;
    //    print("start bursh");
    //    StartCoroutine(WhilePainting());
    //}

    //public void EndingBrushstroke()
    //{
    //    isPainting = false;
    //    print("end bursh");

    //    positionsArray = positionsList.ToArray();
    //    positions = SerializeVector3Array(positionsArray);

    //    rotationsArray = rotationsList.ToArray();
    //    rotations = SerializeVector3Array(rotationsArray);

    //    //call the network brush
    //    //this.GetComponent<NetworkedBrush>().CallPlayStrokeOnClients();
    //}

    //IEnumerator WhilePainting()
    //{
    //    while (isPainting)
    //    {
    //        positionsList.Add(paintBrushPos.position);
    //        rotationsList.Add(paintBrushPos.rotation.eulerAngles);
    //        yield return null;
    //    }
    //}

    //public static string SerializeVector3Array(Vector3[] aVectors)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    foreach (Vector3 v in aVectors)
    //    {
    //        sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z).Append("|");
    //    }
    //    if (sb.Length > 0) // remove last "|"
    //        sb.Remove(sb.Length - 1, 1);
    //    return sb.ToString();
    //}

    //public static Vector3[] DeserializeVector3Array(string aData)
    //{
    //    string[] vectors = aData.Split('|');
    //    Vector3[] result = new Vector3[vectors.Length];
    //    for (int i = 0; i < vectors.Length; i++)
    //    {
    //        string[] values = vectors[i].Split(' ');
    //        if (values.Length != 3)
    //            throw new System.FormatException("component count mismatch. Expected 3 components but got " + values.Length);
    //        result[i] = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
    //    }
    //    return result;
    //}
}
