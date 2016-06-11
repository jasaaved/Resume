using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SimpleCrypto;

public class Login : MonoBehaviour {

	///////////////////////////////////////////
	// MEMBERS
	///////////////////////////////////////////
	public GUISkin guiSkin;
	public System.Boolean showLoadOut;
	public string loadOutText = "";
	public GameObject emptyObject;
	public System.Boolean doLogin = true;
	private string loginUsername = ""; //this is the field where the player will put the name to login
	private string loginPassword = ""; //this is his password
	private string loginIP = ""; //this is the IP the client will use to connect to the Server
	private string regUsername = ""; //this is the field where the player will put the name to login
	private string regPassword = "";
	private string regDisplayName = "";
	private string confirmRegPassword = "";
	private string formText = ""; //this field is where the messages sent by PHP script will be in
	private string URL = "http://alpha-wolf-squadron-abagg.c9users.io/Login.php";
	private Rect textrect = new Rect (10, 150, 500, 500); //just make a GUI object rectangle
	private IEnumerator loadOut;
	private ICryptoService cryptoService = new PBKDF2();


	///////////////////////////////////////////
	// LIFE CYCLE
	///////////////////////////////////////////
	void Start () {
		PlayerPrefs.DeleteAll();
	}
	void OnGUI(){
		GUI.skin = guiSkin;

		if(showLoadOut){
			GUI.Box (new Rect(0, Screen.height/2 - 30, Screen.width, 60), loadOutText);
		}else{
			if(doLogin){
				GUI.Window (0, new Rect (Screen.width/2 - 250, Screen.height/2 - 100, 500, 350), attemptLogin, "Login");
			}else{
				GUI.Window (0, new Rect (Screen.width/2 - 250, Screen.height/2 - 100, 500, 350), attemptLogin, "Create Account");
			}
		}
	}


	///////////////////////////////////////////
	// UI METHODS
	///////////////////////////////////////////
	void attemptLogin (int id){
		if(doLogin){
			GUILayout.BeginVertical();
			GUILayout.Space(50);

			GUILayout.BeginHorizontal();  
			GUILayout.Label( "Username:" );
			GUILayout.Space(20);
			loginUsername = GUILayout.TextField  (loginUsername, 30,  GUILayout.Width(345), GUILayout.Height(35) );
			GUILayout.EndHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label( "Password:" );
			GUILayout.Space(20);
			loginPassword = GUILayout.PasswordField ( loginPassword, "*"[0], 30,  GUILayout.Width(345), GUILayout.Height(35) ); //same as above, but for password
			GUILayout.EndHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label( "IP:" );
			GUILayout.Space(20);
			loginIP = GUILayout.TextField ( loginIP, 39,  GUILayout.Width(345), GUILayout.Height(35) ); //same as above, but for password
			GUILayout.EndHorizontal();
			GUILayout.Space(100);

			GUILayout.BeginHorizontal();
			if ( GUILayout.Button ("Login" ) ){ //just a button
				StartCoroutine(performAction("Login"));
			}
			GUILayout.Space(10);
			if ( GUILayout.Button ( "Sign Up" ) ){ //just a button
				doLogin = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

		}else{
			GUILayout.BeginVertical();
			GUILayout.Space(50);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Username:" );
			GUILayout.Space(20);
			regUsername = GUILayout.TextField (regUsername, 30, GUILayout.Width(300), GUILayout.Height(35) );

			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Password:" );
			GUILayout.Space(20);
			regPassword = GUILayout.PasswordField (regPassword, "*"[0], 30, GUILayout.Width(300), GUILayout.Height(35) );
			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Confirm Password:" );
			GUILayout.Space(20);
			confirmRegPassword = GUILayout.PasswordField (confirmRegPassword, "*"[0], 30, GUILayout.Width(300), GUILayout.Height(35) );
			GUILayout.EndHorizontal();

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.Label("Display Name:" );
			GUILayout.Space(20);
			regDisplayName = GUILayout.TextField (regDisplayName, 20, GUILayout.Width(300), GUILayout.Height(35) );
			GUILayout.EndHorizontal();
			GUILayout.Space(51);

			GUILayout.BeginHorizontal();
			if ( GUILayout.Button ("Finish" ) ){ //just a button
				print("onFinishClick");
				StartCoroutine(performAction("Register"));
			}
			GUILayout.Space(10);
			if ( GUILayout.Button ("Back" ) ){ //just a button
				doLogin = true;

			}
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}
	}


	///////////////////////////////////////////
	// NETWORK METHODS
	///////////////////////////////////////////
	IEnumerator performAction(string Act) {
		print("Performing Action = " + Act);
		string tempURL = "";
		if(Act =="Login"){
			if (validateLoginEntries ()) {
				tempURL = URL + "?username=" + loginUsername + "&password=" + loginPassword + "&loggedIn=1" + "&Act=" + Act;
			} else {
				yield return loadOut;
				yield break;
			}
		}else{
			if (validateRegEntries()) {
				string salt = cryptoService.GenerateSalt();
				tempURL = URL + "?username=" + regUsername + "&password=" + regPassword + "&salt=" + salt + "&displayName=" + regDisplayName + "&Act=" + Act;
			} else {
				yield return loadOut;
				yield break;
			}
		}

		print("Attempting connection to: " + tempURL);

		WWW w = new WWW(tempURL); //here we create a var called 'w' and we sync with our URL and the form
		yield return w; //we wait for the form to check the PHP file, so our game dont just hang

		if (w.error != null) {
			yield return showLoadOutMessage ("Unable to Connect", 2);
			print(w.error); //if there is an error, tell us
		} else {
			print("Connected");
			if(w.text.Contains("LOGIN_SUCCESS")){
				yield return showLoadOutMessage ("Logging in...", 2);

				string[] text = w.text.Split ('&');

				if(text.Length < 2){
					yield return showLoadOutMessage ("Unable to Login", 2);
					yield break;
				}

				string userDisplayName = text[1]; 

				GameObject gameObj = GameObject.Find ("UserIPObj");

				UserIP userIP = gameObj.GetComponent<UserIP> ();
				userIP.IP = loginIP;
				userIP.display_name = userDisplayName;
				userIP.username = loginUsername;

				print ("User IP = " + loginIP);
				print ("User display name = " + userDisplayName);

				DontDestroyOnLoad (gameObj);
				SceneManager.LoadScene ("ServerBoardScene");
			}
			if(w.text == "LOGIN_INVALID"){
				yield return showLoadOutMessage("Incorrect Username or Password", 2);
			}
			if(w.text == "ALREADY_LOGGED_IN"){
				yield return showLoadOutMessage("User is Currently Logged in", 2);
			}
			if(w.text == "NO_USER_FOUND"){
				yield return showLoadOutMessage("No Registered User Found", 2);
			}
			if (w.text == "EMPTY_ENTRY") {
				yield return showLoadOutMessage("Cannot Submit Empty Entry", 2);
			}
			if(w.text == "USERNAME_TAKEN"){
				yield return showLoadOutMessage("Username is Taken", 2);
				SceneManager.LoadScene("Login");
			}	
			if(w.text == "ILLEGAL_REQUEST"){
				yield return showLoadOutMessage("Server Error", 2);
			}
			if(w.text == "REGISTERED"){
				yield return showLoadOutMessage("Account Created", 2);
				SceneManager.LoadScene("Login");
			}
			if(w.text == "ERROR"){
				yield return showLoadOutMessage("An Error Occurred", 2);
				SceneManager.LoadScene("Login");
			}

			print(w.text);
			w.Dispose(); //clear our form in game
		}
	}


	///////////////////////////////////////////
	// INTERNAL METHODS
	///////////////////////////////////////////
	IEnumerator showLoadOutMessage(string message, int seconds){
		showLoadOut = true;
		loadOutText = message;
		yield return new WaitForSeconds(seconds);
		showLoadOut = false;
	}
	bool validateRegEntries(){
		if(regPassword != confirmRegPassword){
			loadOut = showLoadOutMessage ("Passwords do not Match", 2);
			return false;
		}

		return true;
	}
	bool validateLoginEntries(){
		if(loginUsername == ""){
			loadOut = showLoadOutMessage ("No Username Entered", 2);
			return false;
		}
		else if(loginPassword == ""){
			loadOut = showLoadOutMessage ("No Password Entered", 2);
			return false;
		}
		else if(loginIP == ""){
			loadOut = showLoadOutMessage ("No IP Address Entered", 2);
			return false;
		}

		return true;
	}
}
