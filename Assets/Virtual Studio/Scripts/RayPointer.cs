using System;
using System.Collections;
using UnityEngine;
using Valve;

public class RayPointer : MonoBehaviour
{

    #region Variables

    //paint brush
    A_PaintBrush paintBrush;
     
    PaintPaletteVolume PPV;
    bool palleteHover;
    public GameObject objectHit;
    GameObject lastObjectHit;

    //raycasting pointer    
    public RaycastHit hit;
    public bool busy;

    [Tooltip("this will send clickEvent() message to all object that have a child named 'RayCollider'")]
    public String OnClickBroadcastMessage = "ClickEvent";

    LineRenderer lineRenderer;

    public GameObject Tip;

    private float lineWidth = 0.005f;

    Vector3 tipStartScal;
    Coroutine rayVisCoroutine;

    bool OnHoverBroadcasted;
    bool OnHoverExitBroadcasted;

    //ray pointer global
    [Tooltip("this sets the raypointer to hit other objects and not just the paint pallete")]
    public bool raypointerGlobal;

    //lenght of ray
    public float rayLength = 0.3f;

    #endregion

    void Start()
    {
        //ray pointer
        tipStartScal = Tip.transform.localScale;

        objectHit = new GameObject();
        objectHit.transform.parent = Tip.transform;
        lastObjectHit = objectHit;  
    }

    private void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, rayLength))
        {
            //---------------------------------------------------------------------
            if (!raypointerGlobal)
            {
                //menu items
                if (hit.collider.gameObject.name == "RayCollider")
                {
                    objectHit = hit.collider.transform.parent.gameObject;
                    Show();
                    SetLength(hit.distance);
                    busy = true;
                    // print(hit.collider.gameObject.transform.parent.name);
                }

                //  else if (hit.collider == null)
                //    {
                //     Hide();
                //     busy = false;
                // }
            }
            //--------------------------------------------------------------------

            else if (raypointerGlobal)
            {
                //global objects
                if (hit.collider.transform.parent != null)
                {
                    if (hit.collider.transform.parent.GetComponent<SelectableObject>())
                    {
                        objectHit = hit.collider.transform.parent.gameObject;
                        Show();
                        SetLength(hit.distance);
                        busy = true;
                        // print(hit.collider.gameObject.name);
                    }
                }
                //menu items
                if (hit.collider.gameObject.name == "RayCollider")
                {
                    objectHit = hit.collider.transform.parent.gameObject;
                    Show();
                    SetLength(hit.distance);
                    busy = true;
                    // print(hit.collider.gameObject.transform.parent.name);
                }
            }
        }

        //turn off rayPointer when not in use
        else if (busy) { busy = false; }
        else if (!busy && rayVisible) { Hide(); }


        //broadcast OnHover----------------------------------------------------------------------
        if (paintBrush.controller != null)
        {
            if (busy)
            {
                if (objectHit.GetComponent<SelectableItem>() && !OnHoverBroadcasted)
                {
                    objectHit.SendMessage("OnHover", objectHit, SendMessageOptions.DontRequireReceiver);
                    OnHoverExitBroadcasted = false;
                    OnHoverBroadcasted = true;
                }
                else if (objectHit.GetComponent<SelectableObject>() && !OnHoverBroadcasted)
                {
                    objectHit.GetComponent<SelectableObject>().isHovered = true;
                    OnHoverExitBroadcasted = false;
                    OnHoverBroadcasted = true;
                }
            }
        }

        //broadcast OnHoverExit-------------------------------------------------------------------------------------------------
        if (busy)
        {
            if (!OnHoverExitBroadcasted && lastObjectHit != objectHit)
            {
                OnHoverBroadcasted = false;
                OnHoverExitBroadcasted = true;
                //  print("broadcasting On Hovere Exit ");
                if (lastObjectHit != null)
                {
                    lastObjectHit.SendMessage("OnHoverExit", SendMessageOptions.DontRequireReceiver);
                    lastObjectHit = objectHit;
                    if (objectHit.GetComponent<SelectableObject>() != null) { objectHit.GetComponent<SelectableObject>().isHovered = false; }
                }
            }
        }

        //broacast clickevent----------------------------------------------------------------------------------------------------
        if (paintBrush.controller != null)
        {
            if (busy && paintBrush.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (objectHit.GetComponent<SelectableItem>())
                {
                    objectHit.SendMessage("ClickEvent", objectHit, SendMessageOptions.DontRequireReceiver);
                    // print("broadcast click");
                }

                else if (objectHit.GetComponent<SelectableObject>())
                {
                    objectHit.GetComponent<SelectableObject>().OnClick();
                    OnHoverExitBroadcasted = false;
                    OnHoverBroadcasted = true;
                }
            }

            //broacast clickHold-------------------------------------------------------------------------------------------------
            if (paintBrush.controller != null)
            {
                if (busy && paintBrush.controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                {
                    if (objectHit.GetComponent<SelectableItem>())
                    {
                        objectHit.SendMessage("ClickHold", objectHit, SendMessageOptions.DontRequireReceiver);
                        // print("broadcast click");
                    }
                }
            }
            //broacast unClick
            if (paintBrush.controller != null)
            {
                if (busy && paintBrush.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    if (objectHit.GetComponent<SelectableItem>())
                    {
                        objectHit.SendMessage("Unclick", objectHit, SendMessageOptions.DontRequireReceiver);
                        // print("broadcast click");
                    }
                }
            }
        }
    }
    
    public bool rayVisible { get; private set; }

    public void Hide(bool rayOnly = false)
    {
        if (isActiveAndEnabled)
        {
            if (rayVisible)
            {
                rayVisible = false;
                rayVisCoroutine = StartCoroutine(HideRay());
                //this.StopCoroutine(m_RayVisibilityCoroutine);
            }
        }
    }

    public void Show(bool rayOnly = false)
    {
        if (isActiveAndEnabled)
        {
            if (!rayVisible)
            {
                rayVisible = true;
                rayVisCoroutine = StartCoroutine(ShowRay());
                this.StopCoroutine(rayVisCoroutine);
            }
        }
    }

    public void SetLength(float length)
    {
        if (!rayVisible)
            return;

        var scaledWidth = lineWidth;
        var scaledLength = length;

        var lineRendererTransform = lineRenderer.transform;
        lineRendererTransform.localScale = Vector3.one * scaledLength;
        lineRenderer.SetWidth(scaledWidth, scaledWidth * scaledLength);
        Tip.transform.position = transform.position + transform.forward * length;
        Tip.transform.localScale = scaledLength * tipStartScal;
    }

    private void Awake()
    {
        //get paint brush
        paintBrush = this.transform.parent.GetComponent<A_PaintBrush>();

        //line renderer
        lineRenderer = this.transform.GetChild(0).GetComponent<LineRenderer>();

        rayVisible = true;

        ///tip
        if(Tip == null)
        {
            Tip = this.transform.GetChild(1).gameObject;
        }
    }

    private IEnumerator HideRay()
    {
        Tip.transform.localScale = Vector3.zero;

        // cache current width for smooth animation to target value without snapping
        var currentWidth = lineRenderer.startWidth;
        const float kTargetWidth = 0f;
        const float kSmoothTime = 0.1875f;
        var smoothVelocity = 0f;
        var currentDuration = 0f;
        while (currentDuration < kSmoothTime)
        {
            currentDuration += Time.unscaledDeltaTime;
            currentWidth = Mathf.SmoothDamp(currentWidth, kTargetWidth, ref smoothVelocity, kSmoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            lineRenderer.SetWidth(currentWidth, currentWidth);
            yield return null;
        }

        lineRenderer.SetWidth(kTargetWidth, kTargetWidth);
        rayVisCoroutine = null;
    }

    private IEnumerator ShowRay()
    {
        Tip.transform.localScale = tipStartScal;

        float scaledWidth;
        var currentWidth = lineRenderer.startWidth;
        var smoothVelocity = 0f;
        const float kSmoothTime = 0.3125f;
        var currentDuration = 0f;
        while (currentDuration < kSmoothTime)
        {
            currentDuration += Time.unscaledDeltaTime;
            currentWidth = Mathf.SmoothDamp(currentWidth, lineWidth, ref smoothVelocity, kSmoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            scaledWidth = currentWidth;
            lineRenderer.SetWidth(scaledWidth, scaledWidth);
            yield return null;
        }

        scaledWidth = lineWidth;
        lineRenderer.SetWidth(scaledWidth, scaledWidth);
        rayVisCoroutine = null;
    }
}


