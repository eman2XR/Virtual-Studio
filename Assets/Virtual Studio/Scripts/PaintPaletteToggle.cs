using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintPaletteToggle : MonoBehaviour
{
    bool hoverEntry;
    bool hoverExit;

    private Toggle toggle;

    private Text toggleDescription;

    Color originalColor;


    void ClickEvent(GameObject objectClicked)
    {
        if (objectClicked == this.gameObject)
        {
            if (!toggle.isOn)
            {
                toggle.isOn = true;
               // print("turn on");
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }
    }
    

    public void OnHover(GameObject objectHovered)
    {
        if (objectHovered == this.gameObject)
        {
            if (!hoverEntry)
            {
                hoverEntry = true;
                //print("hover entry");
                toggle.targetGraphic.color = toggle.colors.highlightedColor;
                hoverExit = false;
            }
        }
    }

    public void OnHoverExit()
    {
        if (!hoverExit)
        {
            hoverExit = true;
            //   print("hover exit");
            toggle.targetGraphic.color = originalColor;
            hoverEntry = false;
        }
    }

    public bool selected
    {
        set
        {
            if (value)
            {
                toggle.transition = Selectable.Transition.None;
                toggle.targetGraphic.color = toggle.colors.highlightedColor;
            }
            else
            {
                toggle.transition = Selectable.Transition.ColorTint;
                toggle.targetGraphic.color = originalColor;
            }

            // HACK: Force update of target graphic color
            toggle.enabled = false;
            toggle.enabled = true;
        }
    }

    private void Awake()
    {
        toggle = this.GetComponent<Toggle>();
        originalColor = toggle.targetGraphic.color;
    }

    public void SetData(string name, string description)
    {
        //   m_ButtonTitle.text = name;
        toggleDescription.text = description;
    }

    public void Deaactivate()
    {
        if (toggle.isOn)
        {
            toggle.isOn = false;
        }
    }
    public void Activate()
    {
        if (!toggle.isOn)
        {
            toggle.isOn = true;
        }
    }
} 