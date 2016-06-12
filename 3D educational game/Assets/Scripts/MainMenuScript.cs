using UnityEngine;
using System.Collections;


public class MainMenuScript : MonoBehaviour {
	public Texture boss3;
	private float screenh;
	private float screenw;
	public Font myfont2;
	public AudioClip mainmenumusic;


	// Use this for initialization
	void OnGUI() {
		GUI.skin.font = myfont2;
		screenh = Screen.height;
		screenw = Screen.width;

		if (GUI.Button (new Rect(screenw/8,625*screenh/1000,Screen.width/4,Screen.height/4), boss3)){
				Application.LoadLevel("Battle3");
	
	}
		if (GUI.Button (new Rect(screenw/8,screenh/8,Screen.width/4,Screen.height/4), "1")){
				Application.LoadLevel("Battle7");
		}
		if (GUI.Button (new Rect(625*screenw/1000,Screen.height/8,Screen.width/4,Screen.height/4), "2")){
			Application.LoadLevel("Battle4");
			
	}
		if (GUI.Button (new Rect(625*screenw/1000,625*screenh/1000,Screen.width/4,Screen.height/4), "3")){
			Application.LoadLevel("Battle6");
		}

			if (GUI.Button (new Rect(375*Screen.width/1000,375*Screen.height/1000,Screen.width/4,Screen.height/4), "5")){
			Application.LoadLevel("Battle5");
		}

}
}
