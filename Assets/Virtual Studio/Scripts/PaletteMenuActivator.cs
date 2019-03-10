using System;
using System.Collections;
using UnityEngine;

  public class PaletteMenuActivator : MonoBehaviour
	{
    //object specific menu
    public bool useObjectSpecificMenu;

        bool alreadyClicked;

		readonly Vector3 activatorOriginalPos = new Vector3(0f, 0f, -0.075f);
	
		 public Transform Icon;
    
		Coroutine HighlightCoroutine;
		Coroutine ActivatorMoveCoroutine;
		
    bool hoverEntry;
    bool hoverExit;

   
    public void OnHover()
    {
        if (!hoverEntry)
        {
            hoverEntry = true;
           // print("hover entry");
            OnPointerEnter();
            hoverExit = false;
        }
    }

    public void OnHoverExit()
    {
        if (!hoverExit)
        {
            hoverExit = true;
            //   print("hover exit");
            OnPointerExit();
            hoverEntry = false;
        }
    }

    void ClickEvent()
    {
        if (useObjectSpecificMenu)
        {
            if (!alreadyClicked)
            {
            this.transform.parent.GetChild(0).gameObject.SetActive(false);
            alreadyClicked = true;
            this.transform.parent.GetChild(2).gameObject.SetActive(true);
            }
            else if (alreadyClicked)
            {
                this.transform.parent.GetChild(0).gameObject.SetActive(true);
                this.transform.parent.GetChild(2).gameObject.SetActive(false);
                alreadyClicked = false;
            }
        }
        else
            if (!alreadyClicked)
        {
            this.transform.parent.GetChild(0).gameObject.SetActive(false);
            alreadyClicked = true;
        }
        else if (alreadyClicked)
        {
            this.transform.parent.GetChild(0).gameObject.SetActive(true);
            alreadyClicked = false;
        }

    }

    public void OnPointerEnter()
		{
			if (HighlightCoroutine != null)
				StopCoroutine(HighlightCoroutine);

			HighlightCoroutine = null;
			HighlightCoroutine = StartCoroutine(Highlight());
		}

	public void OnPointerExit()
	{
    Icon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    if (HighlightCoroutine != null)
			StopCoroutine(HighlightCoroutine);
	}

		
	IEnumerator Highlight(bool transitionIn = true)
	{
		var amount = 0f;
		var currentScale = Icon.localScale;
		var currentPosition = Icon.localPosition;
		var speed = 8; 

		while (amount < 1f)
		{
			amount += Time.unscaledDeltaTime * speed;
			Icon.localScale = Vector3.Lerp(currentScale, Vector3.one,  Mathf.SmoothStep(0f, 1f, amount));
			
			yield return null;
		}

		Icon.localScale = Vector3.one;
	}
}