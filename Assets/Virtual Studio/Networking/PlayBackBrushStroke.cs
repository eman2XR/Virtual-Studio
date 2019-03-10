using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackBrushStroke : MonoBehaviour {

    public RecordBrushStroke recorderScript;
    bool playbackOn;
    public Vector3[] positions;
    public Vector3[] rotations;
    int current = 0;
    float WPradius = 1;
    public float speed = 8;

    public Transform currentBrushStroke;
    Transform newBrushStroke;

    private void OnEnable()
    {
        currentBrushStroke = this.transform.GetChild(0);
    }

    [ContextMenu("PlayBackStroke")]
    public void PlayBackStroke()
    {
        CreateNewBrush();
        //move into position and then make the extrusion permanent
        currentBrushStroke.position = positions[0];
        currentBrushStroke.rotation = Quaternion.Euler(rotations[0]);
        current = 0;
        playbackOn = true;
    }

    private void Update()
    {
        if (playbackOn)
        {
            if (Vector3.Distance(positions[current], currentBrushStroke.position) < WPradius)
            {
                current++;
                if(current == 2)
                {
                    currentBrushStroke.GetComponentInChildren<ExtrudeMesh>().time = 100;
                }
                if (current == positions.Length)
                {
                    playbackOn = false;
                    DestroyOldBrush();
                }
                currentBrushStroke.position = Vector3.MoveTowards(currentBrushStroke.position, positions[current - 1], Time.deltaTime * speed);
                currentBrushStroke.rotation = Quaternion.RotateTowards(currentBrushStroke.rotation, Quaternion.Euler(rotations[current - 1]), Time.deltaTime * speed);
            }
        }
    }

    void CreateNewBrush()
    {
        newBrushStroke = Instantiate(currentBrushStroke, this.transform).transform;
    }

    void DestroyOldBrush()
    {
        Destroy(currentBrushStroke.GetComponentInChildren<ExtrudeMesh>());
        currentBrushStroke = newBrushStroke;
        recorderScript.positionsList.Clear();
        recorderScript.rotationsList.Clear();
    }
}
