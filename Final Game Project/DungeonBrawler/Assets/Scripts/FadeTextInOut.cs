using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTextInOut : MonoBehaviour {

    public Text text;
    public float fadeTime = 1;

	void Update ()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.PingPong(Time.time, fadeTime));
    }

}
