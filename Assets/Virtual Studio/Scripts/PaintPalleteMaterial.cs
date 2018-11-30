using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PaintPalleteMaterial : MonoBehaviour {
    
    bool hoverEntry;
    bool hoverExit;

    private Button button;

    private Text buttonDescription;
    private Text buttonTitle;

    Color originalColor;
    Text materialSelectionText;

    private void Awake()
    {
        button = this.GetComponent<Button>();
        originalColor = button.targetGraphic.color;
    }

    private void Start()
    {
        //brush Selection text
        try { materialSelectionText = this.transform.parent.parent.GetChild(2).GetChild(1).GetComponent<Text>(); }
        catch (Exception e) { print("Material selection text doesn't exist or has been moved?"); };
    }

    void ClickEvent(GameObject objectClicked)
    {
        if (objectClicked == this.gameObject)
        {
            button.onClick.Invoke();
        }
    }

    public void OnHover()
    {
        if (!hoverEntry)
        {
            hoverEntry = true;
            //  print("hover entry");
            button.targetGraphic.color = button.colors.highlightedColor;
            materialSelectionText.text = this.gameObject.name;
            hoverExit = false;
        }
    }

    public void OnHoverExit()
    {
        if (!hoverExit)
        {
            hoverExit = true;
            //   print("hover exit");
            button.targetGraphic.color = originalColor;
            hoverEntry = false;
        }
    }

}