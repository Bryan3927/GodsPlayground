using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom : MonoBehaviour
{
    public int zoomAmount;
    public Text zoomText;
    public float lowerLimit = 5;
    public float upperLimit = 40;

    private void Start()
    {
        zoomAmount = (int) ((upperLimit - 20) / (upperLimit - lowerLimit) * 100); //20 is a starting zoom
        Debug.Log(zoomAmount);
    }

    public void SetZoom(float zoom)
    {
        zoomAmount = (int) ((upperLimit - zoom) / (upperLimit - lowerLimit) * 100);
    }

    public void DisplayZoom()
    {
        zoomText.text = "" + zoomAmount + "%";
        Debug.Log(zoomText.text);
    }
}