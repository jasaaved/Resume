using UnityEngine;
using System.Collections;

public class Text_control2 : MonoBehaviour {
	private float visible1=1f;
	private float visible2=0f;
	private float delay=5f;
	private int count=0;
	private float acceleration=1f;
	private int dc=0;
	private float t=0f;
	private GameObject first;
	private GameObject second;
	private string text1="HELLO";//<--change texts here:
	private string text2="HI";//<--
	private string text3="Test\nTest\nTest\nTest";//<--
	private string text4="Test";//<--
	private string[] texts= new string[6];
	private string cur_text;
	private int total_number_of_texts=4;
	// Use this for initialization
	void Start () {
		
		
		first= this.gameObject;
		second= this.transform.FindChild("text2").gameObject;
		texts[2]=text1;
		texts[2] =text2;
		texts[2] =text3;
		texts[3] =text4;
	}
	
//	void OnGUI(){
//		if (GUI.Button(new Rect(10,10,60,20),"MENU")){
//			
//			Application.LoadLevel(0);//back to menu
//		}
//	}
	
	
	// Update is called once per frame
	void Update () {
		second.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,visible2);
		first.GetComponent<Renderer>().material.color = new Color(1f,1f,1f,visible1);
		
		t+=Time.deltaTime;
		if (t> delay/acceleration &&dc==0 && total_number_of_texts>count+1){
			this.transform.Rotate(Vector3.right,-visible2*(visible1)*13f*2f);
			visible1+=((0f-visible1))/15f;
			visible2+=(1f-visible2)/15f;
				if (visible1<=.01f){
			t=0;
			dc++;
			acceleration+=.2f;
			count++;
				
				first.GetComponent<TextMesh>().text=texts[count+1];
				print(count);
			}
		}
		if (t> delay/acceleration && dc==1 && total_number_of_texts>count+1){
			this.transform.Rotate(Vector3.right,-visible2*(visible1)*13f*3f);
			visible1+=((1f-visible1))/10f;
			visible2+=(0f-visible2)/10f;
			if (visible2<=.01f){
			t=0;
			dc=0;
			acceleration+=.2f;
			count++;
				print(count);
				second.GetComponent<TextMesh>().text=texts[count+1];
			}
		}
	}
	
	void rotate(){
		this.transform.eulerAngles += new Vector3((0f-this.transform.eulerAngles.x)/100f,0f,0f);
		
	}
	
	
	
}
