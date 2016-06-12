using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
	private string formUsername = ""; //this is the field where the player will put the name to login
	private string formPassword = ""; //this is his password
	private string formIp = ""; //this is the IP the client will use to connect to the Server
	private string RformUsername = ""; //this is the field where the player will put the name to login
	private string RformPassword = "";
	private string RdisplayName = "";
	private string TRformPassword = "";
	public GUISkin guiSkin;
	public System.Boolean LoadOut;
	public string LoadOutText = "";
	private string formText = ""; //this field is where the messages sent by PHP script will be in
	public GameObject emptyObject;

	private string URL = "http://alpha-wolf-squadron-abagg.c9users.io/Login.php"; //change for your URL
	public System.Boolean DoLogin;

	private Rect textrect = new Rect (10, 150, 500, 500); //just make a GUI object rectangle

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll();
	}
	
	void OnGUI(){
		GUI.skin = guiSkin;

		if(LoadOut){
			GUI.Box (new Rect(Screen.width/2 - 150, Screen.height/2 - 30, 300, 60), LoadOutText);


		}else{
			if(DoLogin){
				GUI.Window (0, new Rect (Screen.width/2 - 250, Screen.height/2 - 200, 500, 400), AttemptLogin, "Login");
			}else{
				GUI.Window (0, new Rect (Screen.width/2 - 250, Screen.height/2 - 200, 500, 400), AttemptLogin, "Create Account");
			}
		}
	}

	void AttemptLogin (int id){
		if(DoLogin){
			GUILayout.BeginVertical();
			GUILayout.Space(50);

			GUILayout.BeginHorizontal();  
			GUILayout.Label( "Username:" );
			GUILayout.Space(10);
			formUsername = GUILayout.TextField ( formUsername, 30,  GUILayout.Width(345), GUILayout.Height(35));
			GUILayout.EndHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label( "Password:" );
			GUILayout.Space(10);
			formPassword = GUILayout.PasswordField (formPassword, "*"[0], 30,  GUILayout.Width(345), GUILayout.Height(35) ); //same as above, but for password
			GUILayout.EndHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label( "IP:" );
			GUILayout.Space(10);
			formIp = GUILayout.TextField (formIp, 39,  GUILayout.Width(345), GUILayout.Height(35) ); //same as above, but for password
			GUILayout.EndHorizontal();
			GUILayout.Space(150);

			GUILayout.BeginHorizontal();
			if ( GUILayout.Button ("Login" ) ){ //just a button
				StartCoroutine(Action("Login"));
			}
			GUILayout.Space(10);
			if ( GUILayout.Button ( "Sign Up" ) ){ //just a button
				DoLogin = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

		}else{
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Username:" );
			GUILayout.Space(10);
			RformUsername = GUILayout.TextField (RformUsername, 30, GUILayout.Width(300), GUILayout.Height(35) );

			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Password:" );
			GUILayout.Space(10);
			RformPassword = GUILayout.PasswordField (RformPassword, "*"[0], 30, GUILayout.Width(300), GUILayout.Height(35) );
			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Password:" );
			GUILayout.Space(10);
			TRformPassword = GUILayout.PasswordField (TRformPassword, "*"[0], 30, GUILayout.Width(300), GUILayout.Height(35) );
			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Display Name:" );
			GUILayout.Space(10);
			RdisplayName = GUILayout.TextField (RdisplayName, 20, GUILayout.Width(300), GUILayout.Height(35) );
			GUILayout.EndHorizontal();
			GUILayout.Space(101);

			GUILayout.BeginHorizontal();
			if ( GUILayout.Button ("Finish" ) ){ //just a button
				print("onFinishClick");
				StartCoroutine(Action("Register"));
			}
			GUILayout.Space(10);
			if ( GUILayout.Button ("Back" ) ){ //just a button
				DoLogin = true;

			}
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}
		//GUI.TextArea( textrect, formText );
	}

	IEnumerator Action(string Act) {
		print("Performing Action = " + Act);

		WWWForm form = new WWWForm(); //here you create a new form connection
		//add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
		string tempURL;
		if(Act =="Login"){
			tempURL = URL + "?username="+formUsername+"&password="+formPassword+"&Act="+Act;
		}else{
			tempURL = URL + "?username="+RformUsername+"&password="+RformPassword+"&Act="+Act+"&displayName="+RdisplayName;
		}

		print("Attempting connection to: " + tempURL);

		WWW w = new WWW(tempURL); //here we create a var called 'w' and we sync with our URL and the form
		yield return w; //we wait for the form to check the PHP file, so our game dont just hang

		if (w.error != null) {
			print(w.error); //if there is an error, tell us
		} else {
			print("Connected");
			if(w.text == "Success"){
				LoadOut = true;
				LoadOutText = "Logging In...";
				yield return new WaitForSeconds(2);
				PlayerPrefs.SetString("RegUser",formUsername);

				GameObject gameObj = GameObject.Find("UserIPObj");

				UserIP userIp = gameObj.GetComponent<UserIP>();
				userIp.IP = formIp;

				print("User IP = " + formIp);

				DontDestroyOnLoad(gameObj);

				SceneManager.LoadScene("MainScene");
			}
			if(w.text == "Wrong"){
				LoadOut = true;
				LoadOutText = "Wrong Password";
				yield return new WaitForSeconds(2);
				LoadOut = false;
			}
			if(w.text == "No User"){
				LoadOut = true;
				LoadOutText = "No Registered User Found";
				yield return new WaitForSeconds(2);
				LoadOut = false;
			}
			if(w.text == "TAKEN"){
				LoadOut = true;
				LoadOutText = "Username is taken";
				yield return new WaitForSeconds(2);
				SceneManager.LoadScene("Login");
			}	
			if(w.text == "ILLEGAL REQUEST"){
				LoadOut = true;
				LoadOutText = "Server Error";
				yield return new WaitForSeconds(2);
				LoadOut = false;
			}
			if(w.text == "Registered"){
				LoadOut = true;
				LoadOutText = "Account Created";
				PlayerPrefs.SetString("RegUser",RformUsername);
				yield return new WaitForSeconds(2);
				SceneManager.LoadScene("Login");
			}
			if(w.text == "ERROR"){
				LoadOut = true;
				LoadOutText = "Login Error. Restarting...";
				yield return new WaitForSeconds(2);
				SceneManager.LoadScene("Login");
			}
			print(w.text);

			// formText = w.data; //here we return the data our PHP told us
			w.Dispose(); //clear our form in game
		}
	}
}
