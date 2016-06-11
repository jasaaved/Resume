using UnityEngine;
using System.Collections;

public class WinnerScript : MonoBehaviour {
    // Update is called once per frame
	void Update () {
        GameObject.Find("Canvas/Text").GetComponent<UnityEngine.UI.Text>().text = GameObject.Find("Winner").GetComponent<WinnerHolder>().winner;
        
	}
}
