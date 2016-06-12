// FadeInOut
 
var fadeTexture : Texture2D;
var fadeSpeed = 0.2;
var drawDepth = -1000;
 
private var alpha = 0.0; 
private var fadeDir = -1;
private var  time = 0.0;
 
function OnGUI(){
	time += Time.deltaTime;
 	if (time >= 60){
	    alpha -= fadeDir * fadeSpeed * Time.deltaTime;  
	    alpha = Mathf.Clamp01(alpha);   
	 
	    GUI.color.a = alpha;
	 
	    GUI.depth = drawDepth;
	 
	    GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	    }
	if (time >= 65){
		Application.LoadLevel("mainmenu");
	    }
	if (Event.current.Equals (Event.KeyboardEvent ("return"))){
		Application.LoadLevel("mainmenu");
	    }
}