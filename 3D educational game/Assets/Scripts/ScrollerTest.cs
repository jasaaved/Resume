using UnityEngine;
using System.Collections;
 
 public class ScrollerTest : MonoBehaviour
{
	public Font myfont;
 
    void OnGUI() {
		GUI.skin.font = myfont;
		GUI.Label (new Rect(Screen.width/4,Screen.height/6,Screen.width/2,3*Screen.height/4), "Tired of teaching and working in computer science, Prof. Snow wanted to try something new. One day, he broke into one of the chemistry labs in the university he worked at. Hoping to discover cold fusion, Prof. Snow started mixing chemicals randomly. However, instead of discovering cold fusion, Prof. Snow accidentally created five monsters created from water, oxygen, salt, carbon dioxide, and hydrogen cyanide. The monsters killed Prof. Snow and all the chemistry experts at the university. The monsters have now invaded a local high school: your high school. The monsters have killed your chemistry teachers, so it's up to you, a student of chemistry, to defeat these monsters. Suit up, research, and use your knowledge about these important molecules in chemistry to defeat these monsters!");
		}
		
	}