using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour {

    public RectTransform background;
    public RectTransform theCanvas;

    private float height;
    private float width;

    void Start()
    {
        width = theCanvas.sizeDelta.x;
        height = theCanvas.sizeDelta.y;

        background.sizeDelta = new Vector2(width, height);

    }
}
