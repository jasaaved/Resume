using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageInOut : MonoBehaviour {

    public Image image;
    public float fadeTime = 1;

	void Update ()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.PingPong(Time.time, fadeTime));
    }

}
