using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordBrushStroke : MonoBehaviour {

    A_PaintBrush paintBrush;
    Transform paintBrushPos;
    public PlayBackBrushStroke playBackScript;

    public Vector3[] positions;
    public List<Vector3> positionsList  = new List<Vector3>();
    public Vector3[] rotations;
    public List<Vector3> rotationsList = new List<Vector3>();

    bool isPainting;
    //Transform currentBrush;

    private void OnEnable()
    {
        print(Resources.FindObjectsOfTypeAll<A_PaintBrush>()[0].name);
        paintBrush = Resources.FindObjectsOfTypeAll<A_PaintBrush>()[0].GetComponent<A_PaintBrush>();
        paintBrush.onBrushStrokeStart.AddListener(StartingBrushstroke);
        paintBrush.onBrushStrokeEnd.AddListener(EndingBrushstroke);

        paintBrushPos = paintBrush.transform.GetChild(1);
       // currentBrush = this.transform.GetChild(0);
        //A_PaintBrush.OnBrushStart += StartingBrushstroke;
        //A_PaintBrush.OnBrushEnd += EndingBrushstroke;
    }

    private void OnDisable()
    {
        //A_PaintBrush.OnBrushStart -= StartingBrushstroke;
        //A_PaintBrush.OnBrushEnd -= EndingBrushstroke;
    }

    private void Update()
    {
        if (paintBrush.isPainting)
        {
            if (!isPainting)
            {
                isPainting = true;
                StartingBrushstroke();
            }
        }
        else
        {
            if (isPainting)
            {
                isPainting = false;
                EndingBrushstroke();
            }
        }
    }

    public void StartingBrushstroke()
    {
        isPainting = true;
        print("start bursh");
        StartCoroutine(WhilePainting());
    }
    
    public void EndingBrushstroke()
    {
        isPainting = false;
        print("end bursh");

        positions = positionsList.ToArray();
        playBackScript.positions = positions;

        rotations = rotationsList.ToArray();
        playBackScript.rotations = rotations;

        playBackScript.PlayBackStroke();
        //call the network brush
        //this.GetComponent<NetworkedBrush>().CallPlayStrokeOnClients();
    }

    IEnumerator WhilePainting()
    {
        while (isPainting)
        {
            positionsList.Add(paintBrushPos.position);
            rotationsList.Add(paintBrushPos.rotation.eulerAngles);
            yield return null;
        }
    }
}
