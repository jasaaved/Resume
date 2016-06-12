using UnityEngine;
using System.Collections;

public class BattleMenuScript : MonoBehaviour {
	private bool choice = true;
	private bool choice1 = true;
	private bool choice2 = true;
	private bool choice3 = true;
	private bool isplayed = false;
	static public bool monsteralive = true;
	private float screenh;
	private float screenw;
	public AudioClip bgmusic;
	public AudioClip cursor;
	public AudioClip go;
	static public int monsterHP3 = 4000; //change
	static public  int playerHP3 = 4000;//change
	private bool attack1 = true;
	private bool attack2 = true;
	private bool skill1 = true;
	private bool skill2 = true;
	private bool skill3 = true;
	private bool skill4 = true;
	private bool freeze = true;
	private bool boil = true;
	private bool formula1 = true;
	private bool formula2 = true;
	private bool formula3 = true;
	static public bool inmotion = false;
	public Texture Texture1;
	public Texture stats;
	private string hpstring;
	public bool scan = false;
	private bool attacking = false;
	private bool hit = false;
	private bool goplaying = false;
	private bool incorrect = false;
	private bool correct = false;
	public AudioClip victory;
	private bool boil_gui = false;
	private bool freeze_gui = false;
	public string f_answer = "";
	public string b_answer = "";
	private bool valence_gui = false;
	private bool weight_gui = false;
	public string w_answer = "";
	public string v_answer = "";
	public Texture hints;
	

	// Use this for initialization
	void Start(){

		monsteralive = true;
	
		monsterHP3 = 4000;//change
		playerHP3 = 4000;

		inmotion = false;//change
	}


	void OnGUI() {
		screenh = Screen.height;
		screenw = Screen.width;
		hpstring = "HP: " + playerHP3 + "/4000";//change
		string enstring = "HP: " +monsterHP3+"/4000";//change


		if (monsteralive) {
			if (!isplayed){
			GetComponent<AudioSource>().PlayOneShot(bgmusic);
			isplayed = true;
			}
		}
		if (playerHP3 <= 0){
				inmotion = true;
			if (goplaying == false){
				GetComponent<AudioSource>().Stop();
				GetComponent<AudioSource>().PlayOneShot(go);
				goplaying = true;
				}

			GUI.Box (new Rect(36*screenw/100, 35*screenh/100, 20*screenw/100, 12*screenh/100), "Game Over.");
			if (GUI.Button (new Rect(40*screenw / 100, 40*screenh/100, screenw /8, screenh/25), "Continue")){
				Application.LoadLevel("mainmenu");
			}
		}
		if (monsterHP3 <= 0){
			inmotion = true;
			if (goplaying == false){
				GetComponent<AudioSource>().Stop();
				GetComponent<AudioSource>().PlayOneShot(victory);
				goplaying = true;
			}

			GUI.Box (new Rect(36*screenw/100, 35*screenh/100, 20*screenw/100, 12*screenh/100), "You win!");
			if (GUI.Button (new Rect(40*screenw / 100, 40*screenh/100, screenw /8, screenh/25), "Continue")){
				Application.LoadLevel("mainmenu");
			}
		}


		GUI.Box (new Rect(20*screenw/100, 45*screenh/100, screenw/10, screenh/18), hpstring);
		GUI.Box (new Rect(75*screenw/100, 30*screenh/100, screenw/10, screenh/18), enstring);
		if (formula1) {
			if (GUI.Button (new Rect (750 * screenw / 1000, screenh / 13, screenw / 8, screenh / 20), "Hints")) {
				formula2 = false;
				formula1 = false;
				GetComponent<AudioSource>().PlayOneShot(cursor);
			}
			if (GUI.Button (new Rect (750 * screenw / 1200, screenh / 13, screenw / 8, screenh / 20), "PTable")) {
				formula3 = false;
				formula1 = false;
				GetComponent<AudioSource>().PlayOneShot(cursor);
			}
		}
		if (!inmotion){
		if (choice1) {
			GUI.Box (new Rect (screenw/16, 3*screenh/4-(screenh/16), screenw/4, screenh/4+screenh/16), "Battle Menu");
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4, screenw/4, screenh/16), "Attack")) {	
				choice = false;
				choice1 = false;
					GetComponent<AudioSource>().PlayOneShot(cursor);
			}

			if (GUI.Button (new Rect (screenw/16, 3*screenh/4+screenh/16, screenw/4, screenh/16), "Skill")) {
				choice1 = false;
				choice2 = false;
					GetComponent<AudioSource>().PlayOneShot(cursor);
			}
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4+screenh/16+screenh/16, screenw/4, screenh/16), "Magic")) {
				choice1 = false;
				choice3 = false;
					GetComponent<AudioSource>().PlayOneShot(cursor);
			}
				if (GUI.Button (new Rect(screenw/16, 3*screenh/4+screenh/16+screenh/16+screenh/16, screenw/4, screenh/16), "Scan")){
					GetComponent<AudioSource>().PlayOneShot(cursor);
					scan = true;


				}
			
		}
		if (!choice) {
			GUI.Box (new Rect (screenw/16, 3*screenh/4-(screenh/16), screenw/4, screenh/4), "Attack Menu");
			if (attack1 == true){
				if (GUI.Button (new Rect (screenw/16, 3*screenh/4, screenw/4, screenh/16), "Weight Attack")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						weight_gui = true;
				}
				}
			if (attack2 == true){
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4+screenh/16, screenw/4, screenh/16), "Valence Attack")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						valence_gui = true;
				}
				}
			if (GUI.Button (new Rect(screenw/16, 3*screenh/4+screenh/16+screenh/16, screenw/4, screenh/16), "Back")){
				choice1 = true;
				choice = true;
					GetComponent<AudioSource>().PlayOneShot(cursor);
				}

			}
		if (!choice2) {
			GUI.Box (new Rect (screenw/16, 3*screenh/4-screenh/16-screenh/16, screenw/4, screenh/4+screenh/16+screenh/16), "Skill Menu");
			if (skill1==true){
				if (GUI.Button (new Rect (screenw/16, 3*screenh/4, screenw/4, screenh/16), "Break Single Bonds")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						incorrect = true;

				}
				}
			if (skill2 == true){
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4+screenh/16, screenw/4, screenh/16), "Break Double Bonds")) {
				GetComponent<AudioSource>().PlayOneShot(cursor);
						incorrect = true;
				}
				}
			if (skill3 == true){
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4+screenh/16+screenh/16, screenw/4, screenh/16), "Break Triple Bonds")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						incorrect = true;
			}
				}
			if (GUI.Button (new Rect(screenw/16, 3*screenh/4+screenh/16+screenh/16+screenh/16, screenw/4, screenh/16), "Back")){
				choice1 = true;
				choice2 = true;
					GetComponent<AudioSource>().PlayOneShot(cursor);
			}
			if (skill4== true){
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4-screenh/16, screenw/4, screenh/16), "Break Ionic Bonds")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						correct = true;
						skill4 = false;
			}
			}
			}

		if (!choice3) {
			GUI.Box (new Rect (screenw/16, 3*screenh/4-(screenh/16), screenw/4, screenh/4), "Magic Menu");
			if (boil == true){
				if (GUI.Button (new Rect (screenw/16, 3*screenh/4, screenw/4, screenh/16), "Boil")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						boil_gui = true;
				}
				}
			if (freeze == true){
			if (GUI.Button (new Rect (screenw/16, 3*screenh/4+screenh/16, screenw/4, screenh/16), "Freeze")) {
						GetComponent<AudioSource>().PlayOneShot(cursor);
						freeze_gui = true;
				}
				}
			if (GUI.Button (new Rect(screenw/16, 3*screenh/4+screenh/16+screenh/16, screenw/4, screenh/16), "Back")){
				choice1 = true;
				choice3 = true;
					GetComponent<AudioSource>().PlayOneShot(cursor);
			}
			
			}
		}
		
	

	if (scan == true){
			inmotion = true;
			GUI.Box (new Rect(30*screenw/100, screenh/10, 40*screenw/100, 45*screenh/100), stats);
			if (GUI.Button (new Rect(45*screenw / 100, 50*screenh/100, screenw /8, screenh/25), "Back")){
				GetComponent<AudioSource>().PlayOneShot(cursor);
				hit = true;
				scan = false;
			}
	}
		if (incorrect == true){
			inmotion = true;
			GUI.Box (new Rect(36*screenw/100, 35*screenh/100, 20*screenw/100, 12*screenh/100), "Incorrect!");
			if (GUI.Button (new Rect(40*screenw / 100, 40*screenh/100, screenw /8, screenh/25), "Continue")){
				GetComponent<AudioSource>().PlayOneShot(cursor);
				hit = true;
				incorrect = false;
			}
		}
		if (correct == true){
			inmotion = true;
			GUI.Box (new Rect(36*screenw/100, 35*screenh/100, 20*screenw/100, 12*screenh/100), "Correct!");
			if (GUI.Button (new Rect(40*screenw / 100, 40*screenh/100, screenw /8, screenh/25), "Continue")){
					GetComponent<AudioSource>().PlayOneShot(cursor);
					attacking = true;
					correct = false;
		}
			}
	if (hit == true){
			saltenemy.saltidle = false;
			saltenemy.saltattack = true;
			playerscript.playeridle3 = false;
			playerscript.playerhit3 = true;
			hit = false;
		}
	if (attacking == true){
				playerscript.playeridle3 = false;
				playerscript.playerattack3 = true;
			saltenemy.saltidle = false;
			saltenemy.salthit = true;
			attacking = false;

	}
		if (freeze_gui == true){
			inmotion = true;
			GUI.Box (new Rect(30*screenw/100, 35*screenh/100, 40*screenw/100, 20*screenh/100),"What is the freezing point of this monster\n in Fahrenheit(round to the nearest whole number)?");
			f_answer = GUI.TextField(new Rect(30*screenw/100, 42*screenh/100, 40*screenw/100, 6*screenh/100), f_answer);
			if (GUI.Button (new Rect(40*screenw / 100, 50*screenh/100, 20*screenw /100, screenh/25), "Submit")){
					if (f_answer=="1474"){
						correct = true;
						freeze = false;
						freeze_gui = false;

		}
				if (f_answer != "1474"){
					incorrect = true;
					f_answer = "";
					freeze_gui = false;
	}
			}
		}
		if (boil_gui == true){
			inmotion = true;
			GUI.Box (new Rect(30*screenw/100, 35*screenh/100, 40*screenw/100, 20*screenh/100),"What is the boiling point of this monster\n in Fahrenheit(round to the nearest whole number)?");
			b_answer = GUI.TextField(new Rect(30*screenw/100, 42*screenh/100, 40*screenw/100, 6*screenh/100), b_answer);
			if (GUI.Button (new Rect(40*screenw / 100, 50*screenh/100, 20*screenw /100, screenh/25), "Submit")){
				if (b_answer=="2575"){
					correct = true;
					boil = false;
					boil_gui = false;
					
				}
				if (b_answer != "2575"){
					incorrect = true;
					b_answer = "";
					boil_gui = false;
				}
			}
		}
		if (weight_gui == true){
			inmotion = true;
			GUI.Box (new Rect(30*screenw/100, 35*screenh/100, 40*screenw/100, 20*screenh/100),"What is the molecular weight of one molecule\n in this monster(round to the nearest whole number)?");
			w_answer = GUI.TextField(new Rect(30*screenw/100, 42*screenh/100, 40*screenw/100, 6*screenh/100), w_answer);
			if (GUI.Button (new Rect(40*screenw / 100, 50*screenh/100, 20*screenw /100, screenh/25), "Submit")){
				if (w_answer=="58"){
					correct = true;
					attack1 = false;
					weight_gui = false;
					
				}
				if (w_answer != "58"){
					incorrect = true;
					w_answer = "";
					weight_gui = false;
				}
			}
		}
		if (valence_gui == true){
			inmotion = true;
			GUI.Box (new Rect(30*screenw/100, 35*screenh/100, 40*screenw/100, 20*screenh/100),"How many valence electrons are in one molecule\n in this monster?");
			v_answer = GUI.TextField(new Rect(30*screenw/100, 42*screenh/100, 40*screenw/100, 6*screenh/100), v_answer);
			if (GUI.Button (new Rect(40*screenw / 100, 50*screenh/100, 20*screenw /100, screenh/25), "Submit")){
				if (v_answer=="8"){
					correct = true;
					attack2 = false;
					valence_gui = false;
					
				}
				if (v_answer != "8"){
					incorrect = true;
					v_answer = "";
					valence_gui = false;
				}
			}
		}
		if(!formula2){
			GUI.Box (new Rect(30*screenw/100, screenh/10, 40*screenw/100, 80*screenh/100), hints);
			if (GUI.Button (new Rect(45*screenw / 100, 85*screenh/100, screenw /8, screenh/25), "Back")){
				formula2 = true;
				formula1 = true;
				GetComponent<AudioSource>().PlayOneShot(cursor);
			}
			
		}
		if (!formula3) {
			GUI.Box (new Rect(screenw/8, screenh/10, 3*screenw/4, 3*screenh/4), Texture1);
			if (GUI.Button (new Rect(45*screenw / 100, 4*screenh/5, screenw /8, screenh/25), "Back")){
				///playerscript.playeridle = false;
				formula3 = true;
				formula1 = true;
				GetComponent<AudioSource>().PlayOneShot(cursor);
			}
		}
		}
}