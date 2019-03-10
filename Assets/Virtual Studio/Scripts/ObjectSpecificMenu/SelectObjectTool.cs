using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectObjectTool : MonoBehaviour {
    
    private Button button;
    private Text buttonDescription;
    private Text buttonTitle;
    Color originalColor;
    Text brushSelectionText;
    bool hoverEntry;
    bool hoverExit;

    private void Start()
    {
      ///  //brush Selection text
     //   try { brushSelectionText = this.transform.parent.parent.GetChild(2).GetChild(1).GetComponent<Text>(); }
     //   catch (Exception e) { };
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
            //print("hover entry");
            button.targetGraphic.color = button.colors.highlightedColor;
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

    private void Awake()
    {
        button = this.GetComponent<Button>();
        originalColor = button.targetGraphic.color;
    }

}