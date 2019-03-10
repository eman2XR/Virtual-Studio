using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class A_PaintBrush : MonoBehaviour
{

    #region variables
    //material 
    Material material;

    //paint brush toggle
    [HideInInspector]
    public bool paintBrushActive;

    //eraser
    [HideInInspector]
    public GameObject eraser;
    [HideInInspector]
    public bool eraserActive;

    //sizes
    float SizeX = 1;
    float SizeY = 1;
    float SizeZ = 1;

    //rayPointer
    [HideInInspector]
    public RayPointer ray;
    bool rayState;

    //controller
    [Tooltip("the controller the [Paint Brush] object is attached to")]
    public GameObject VRController;
    SteamVR_TrackedObject trackedObj;
    [HideInInspector]
    public SteamVR_Controller.Device controller;

    //paint master
    public A_PaintPalette paintPallete;
    [Space(20)]

    [HideInInspector]
    public GameObject activeBrush;
    [HideInInspector]
    public Color color;
    [HideInInspector]
    public Transform paintBrushHolder;

    //display brush
    [HideInInspector]
    public GameObject displayBrush;

    bool paintOn;
    bool extrudeOff;
    GameObject curPaint;

    //extrusion target
    Transform extrusionTarget;

    //paint density
    [HideInInspector]
    public float paintDensity;
    public float paintDensityS;

    //helper line
    [HideInInspector]
    public LineRenderer helperLineRenderer;

    //check if is painting;
    [HideInInspector]
    public bool isPainting;

    //rotate brush toggle
    [HideInInspector]
    public bool rotateBrush;

    //straight line
    [HideInInspector]
    public bool straightLines;

    // freezing positions and rotations
    Vector3 initialPaintContactPos;
    bool frozenX;
    bool frozenY;
    bool frozenZ;
    bool frozenRot;

    ExtrudeMesh extrudeMesh;
    GameObject paintGlobal;

    //network extended----------------------
    public UnityEvent onBrushStrokeStart;
    public UnityEvent onBrushStrokeEnd;
    public delegate void OnBrushStrokeStart();
    public static event OnBrushStrokeStart OnBrushStart;
    public delegate void OnBrushStrokeEnd();
    public static event OnBrushStrokeEnd OnBrushEnd;
    //-------------------------------------------------------

    #endregion

    private void Awake()
    {
        //REFERENCES
        //eraser
        if (this.transform.GetChild(3).gameObject.GetComponent<A_Eraser>() != null)
        { eraser = this.transform.GetChild(3).gameObject; }
        else { print("Eraser object doesn't seem to exist or has been moved?"); };

        //active brush
        try { activeBrush = paintPallete.paintBrushType; }
        catch (Exception e) { print("please attach the [Paint_Palette] object in the inspector"); };

        //rayPointer
        try { ray = this.transform.GetChild(0).GetComponent<RayPointer>(); }
        catch (Exception e) { print("Eraser object doesn't seem to exist or has been moved?"); };

        //controller
        try { trackedObj = VRController.GetComponent<SteamVR_TrackedObject>(); }
        catch (Exception e) { print("VRController object doesn't seem to exist or has been moved?"); };

        //paintBrush holder
        paintBrushHolder = this.transform.GetChild(1).transform;

        //line renderer
        if (this.transform.GetChild(5).gameObject.GetComponent<LineRenderer>() != null)
        { helperLineRenderer = this.transform.GetChild(5).gameObject.GetComponent<LineRenderer>(); }
        else { print("Helper Line Renderer object doesn't seem to exist or has been moved?"); };

    }

    private void Start()
    {
        //get initial values from PaintPallete
        material = paintPallete.material;
        color = paintPallete.color;
        paintDensity = paintPallete.paintDensity;
        frozenX = paintPallete.drawStraightOnX;
        frozenY = paintPallete.drawStraightOnY;
        frozenZ = paintPallete.drawStraightOnZ;
        frozenRot = paintPallete.StraightEnds;
        straightLines = paintPallete.straightLines;

        //
        SizeX = paintPallete.brushSize;
        SizeY = paintPallete.brushSize;
        SizeZ = paintPallete.brushSize;

        //
        if (paintPallete.paintBrushType)
        {
            paintBrushHolder.gameObject.SetActive(true);
            paintBrushActive = true;
            displayBrush = Instantiate(activeBrush, paintBrushHolder.transform.position, paintBrushHolder.rotation, paintBrushHolder.transform);
            displayBrush.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);
            displayBrush.transform.GetChild(0).GetComponent<Renderer>().material = paintPallete.material;
            displayBrush.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
            #if UNITY_EDITOR
            Destroy(displayBrush.GetComponent<SaveMeshInEditor>());
            #endif
        }
    }

    #region starting paint Update
    private void Update()
    {
        //get controller buttons
        controller = SteamVR_Controller.Input((int)trackedObj.index);

        //painting
        if (controller != null && !ray.busy)
        {
            if (paintBrushActive)
            {
                if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x > 0.8f)
                {
                    isPainting = true;
                    Paint();

                    //ray point stuff so it doesnt hit things while painting
                    rayState = ray.raypointerGlobal;
                    ray.raypointerGlobal = false;

                    //continious scale
                    //if (extrusionTarget) { extrusionTarget.localScale = new Vector3(SizeX, SizeY, SizeZ); }

                    // freezing positions toggles
                    if (!frozenX && !frozenY && !frozenZ)
                    {
                        extrusionTarget.transform.position = paintBrushHolder.position;
                        if (!rotateBrush)
                        {
                            extrusionTarget.transform.rotation = paintBrushHolder.rotation;
                        }
                    }
                    else if (frozenX) { extrusionTarget.transform.position = new Vector3(paintBrushHolder.transform.position.x, initialPaintContactPos.y, initialPaintContactPos.z); }
                    else if (frozenY) { extrusionTarget.transform.position = new Vector3(initialPaintContactPos.x, paintBrushHolder.transform.position.y, initialPaintContactPos.z); }
                    else if (frozenZ) { extrusionTarget.transform.position = new Vector3(initialPaintContactPos.x, initialPaintContactPos.y, paintBrushHolder.transform.position.z); }

                    //Zero out rotation
                    if (frozenRot)
                    {
                        if (frozenX) { extrusionTarget.transform.rotation = Quaternion.Euler(90, 90, 0); }
                        else if (frozenY) { extrusionTarget.transform.rotation = Quaternion.Euler(0, 160, 0); }
                        else if (frozenZ) { extrusionTarget.transform.rotation = Quaternion.Euler(0, 90, -90); }
                    }
                }

                //straight line
                if (straightLines)
                {

                    float paintDensityS = Vector3.Distance(initialPaintContactPos, paintBrushHolder.position);
                    if (extrudeMesh != null && controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x > 0.81f)
                    {
                        //print("trigger half way up");
                        paintDensity = paintDensityS;
                        extrudeMesh.minDistance = paintDensity;
                    }
                }
            }
        }


        //release the paint object
        if (curPaint != null)
        {
            if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (curPaint != null)
                {
                    if (straightLines)
                    {
                        paintDensity = paintDensity - 0.1f;
                        extrudeMesh.minDistance = paintDensity;
                    }
                    if (extrusionTarget != null)
                    {
                        StartCoroutine(destroyComponentsAfterPainting());
                    }
                    paintOn = false;
                    isPainting = false;

                    //network extended----------------------
                    onBrushStrokeEnd.Invoke();
                    //OnBrushStart();
                }
            }
        }

        //turn paintBrush ON/Off
        if (controller != null)
        {
            if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y >= 0.5f)
            {
                if (!paintBrushActive)
                {
                    paintBrushHolder.gameObject.SetActive(true);
                    paintBrushActive = true;
                    if (displayBrush != null)
                    {
                        displayBrush.SetActive(true);
                    }
                    if (eraserActive)
                    {
                        eraser.GetComponent<A_Eraser>().DeActivateEraser();
                        eraserActive = false;
                    }
                }
                else
                {
                    paintBrushHolder.gameObject.SetActive(false);
                    paintBrushActive = false;
                }
            }
        }
    }
    #endregion

    IEnumerator destroyComponentsAfterPainting()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(curPaint.transform.GetChild(0).GetComponent<ExtrudeMesh>());
        curPaint.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
        Destroy(extrusionTarget.gameObject);
    }

    void Paint()
    {
        if (!paintOn && !ray.busy)
        {
            //make a global paint holder for all created objects
            if (paintGlobal == null)
                paintGlobal = new GameObject("Paint_Global");

            //instantiate paint and parent to paintGlobal object
            curPaint = (GameObject)Instantiate(activeBrush, paintBrushHolder.transform.position, paintBrushHolder.rotation);
            curPaint.transform.parent = paintGlobal.transform;

            //get the extrusion target and parent it to controller
            extrusionTarget = curPaint.transform.GetChild(1);
            //  extrusionTarget.transform.parent = paintBrushHolder.transform; //(this now has been moved in the Update for doing freezongPos)

            //turn mesh extrusion on
            extrudeMesh = curPaint.transform.GetChild(0).GetComponent<ExtrudeMesh>();
            extrudeMesh.time = Mathf.Infinity;

            //caps
            if (paintPallete.buildCaps)
                curPaint.transform.GetChild(0).GetComponent<ExtrudeMesh>().buildCaps = true;

            //assign size
             curPaint.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);

            //assign material
            curPaint.transform.GetChild(0).GetComponent<Renderer>().material = material;

            //assign color
            curPaint.transform.GetChild(0).GetComponent<Renderer>().material.color = color;

            //assign density
            extrudeMesh.minDistance = paintDensity;//(this now has been moved in the Update for straight line)

            //make extrusion target rotate if 'rotateBrush' is On
            if (rotateBrush)
                extrusionTarget.GetComponent<Rotate>().enabled = true;

            //test freezing pos and rot - initital transforms
            initialPaintContactPos = paintBrushHolder.transform.position;
            // print(paintBrushHolder.transform.position);

            paintOn = true;

            //network extended----------------------
            onBrushStrokeStart.Invoke();
            //OnBrushEnd();
        }
    }

    public void BrushSelection()
    {
        paintBrushHolder.gameObject.SetActive(true);

        paintBrushActive = true;

        if (displayBrush != null)
        {
            Destroy(displayBrush.gameObject);
        }

        displayBrush = Instantiate(activeBrush, paintBrushHolder.transform.position, paintBrushHolder.rotation, paintBrushHolder.transform);
        displayBrush.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);
        displayBrush.transform.GetChild(0).GetComponent<Renderer>().material = material;
        displayBrush.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
#if UNITY_EDITOR
        Destroy(displayBrush.GetComponent<SaveMeshInEditor>());
#endif

        // print("brushChanged");

        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
    }

    public void ColorSelection(Color incomingColor)
    {
        if (displayBrush != null)
        {
            displayBrush.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        }
        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }
    }

    public void SizeSelection(float incomingSize, string scaleTarget)
    {
        if (scaleTarget == "XYZ")
        {
            SizeX = incomingSize;
            SizeY = incomingSize;
            SizeZ = incomingSize;

            if (displayBrush != null)
            {
                displayBrush.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);
            }
        }
        else if (scaleTarget == "X")
        {
            SizeX = incomingSize;
            if (displayBrush != null)
            {
                displayBrush.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);
            }
        }
        else if (scaleTarget == "Y")
        {
            SizeY = incomingSize;
            if (displayBrush != null)
            {
                displayBrush.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);
            }
        }
        else if (scaleTarget == "Z")
        {
            SizeZ = incomingSize;
            if (displayBrush != null)
            {
                displayBrush.transform.localScale = new Vector3(SizeX, SizeY, SizeZ);
            }
        }

        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }

    }

    public void DensitySelection(float incomingDensity)
    {
        paintDensity = incomingDensity;
        if (displayBrush != null)
        {
            // displayBrush.transform.GetChild(0).GetComponent<ExtrudeMesh>().minDistance = paintDensity;
        }
        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }
    }

    public void MaterialSelection(Material incomingMaterial)
    {
        if (displayBrush != null)
        {
            material = incomingMaterial;
            displayBrush.transform.GetChild(0).GetComponent<Renderer>().material = incomingMaterial;
            displayBrush.transform.GetChild(0).GetComponent<Renderer>().material.color = color;
        }

        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }
    }

    public void RotateBrushSelection()
    {
        if (!rotateBrush)
        {
            rotateBrush = true;
            if (displayBrush != null)
            {
                displayBrush.transform.GetChild(1).GetComponent<Rotate>().enabled = true;
            }
        }
        else
        {
            rotateBrush = false;
            if (displayBrush != null)
            {
                displayBrush.transform.GetChild(1).GetComponent<Rotate>().enabled = true;
            }
        }
        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }
    }

    public void FreezePositions(string axis)
    {
        if (axis == "X")
        {
            if (!frozenX)
            {
                frozenX = true;
            }
            else { frozenX = false; }

        }
        else if (axis == "Y")
        {
            if (!frozenY)
            {
                frozenY = true;
            }
            else { frozenY = false; }
        }
        else if (axis == "Z")
        {
            if (!frozenZ)
            {
                frozenZ = true;
            }
            else { frozenZ = false; }
        }
        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }
    }

    public void FreezeRotations()
    {
        if (!frozenRot)
        {
            frozenRot = true;
        }
        else
        { frozenRot = false; }
        //deactivete eraser if active
        if (eraserActive)
        {
            eraser.GetComponent<Renderer>().enabled = false;
            eraser.GetComponent<Collider>().enabled = false;
            eraserActive = false;
            paintBrushActive = true;
            eraserActive = false;
        }
        //activete paintBrush if inactive
        if (displayBrush != null)
        {
            displayBrush.SetActive(true);
        }
    }

    public void StraightLines()
    {
        if (straightLines == true)
        {
            straightLines = false;
            //bug!! doesnt seem to return to previous paint density
            paintDensity = 0.01f;
            //
        }
        else
        {
            straightLines = true;
        }
    }
}
