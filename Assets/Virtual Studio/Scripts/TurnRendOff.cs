using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnRendOff : MonoBehaviour {

    public Renderer objectToTurnOff;

    public void TurnOff()
    {
        if (objectToTurnOff.enabled == false)
        {
            objectToTurnOff.enabled = true;
        }
        else
        {
            objectToTurnOff.enabled = false;
        }
    }
}
