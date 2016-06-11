using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {
    private bool DecrementLock = true;
    public int lives = 3;

	public void Respawn () {
        if (DecrementLock){
            DecrementLock = false;
            StartCoroutine(Wait3());
        }
	}

    IEnumerator Wait3(){
        Vector3 disappear = new Vector3(-500, 500, 0);
        transform.position = disappear;

        yield return new WaitForSeconds(3);

        if (lives <= 1){
            Destroy(gameObject);
        }

        else if (gameObject.name == "Player1"){
            Vector3 temp = new Vector3(-4, 4, 0);
            transform.position = temp;
        }

        else if (gameObject.name == "Player2"){
            Vector3 temp = new Vector3(4, -4, 0);
            transform.position = temp;
        }

        else if (gameObject.name == "Player3"){
            Vector3 temp = new Vector3(-4, -4, 0);
            transform.position = temp;
        }

        else if (gameObject.name == "Player4"){
            Vector3 temp = new Vector3(4, 4, 0);
            transform.position = temp;
        }
        int player_assignment = GameObject.Find("Network").GetComponent<Network>().player_assignment;
        if (player_assignment == 1 && gameObject.name == "Player1")
        {
            lives = lives - 1;
            GameObject.Find("Network").GetComponent<Network>().player1lives--;
            GameObject.Find("Player1").GetComponent<Player1Move>().player_info.lives--;
        }

        if (player_assignment == 2 && gameObject.name == "Player2")
        {
            lives = lives - 1;
            GameObject.Find("Network").GetComponent<Network>().player2lives--;
            GameObject.Find("Player2").GetComponent<Player2Move>().player_info.lives--;
        }

        if (player_assignment == 3 && gameObject.name == "Player3")
        {
            lives = lives - 1;
            GameObject.Find("Network").GetComponent<Network>().player3lives--;
            GameObject.Find("Player3").GetComponent<Player3Move>().player_info.lives--;
        }
        if (player_assignment == 4 && gameObject.name == "Player4")
        {
            lives = lives - 1;
            GameObject.Find("Network").GetComponent<Network>().player4lives--;
            GameObject.Find("Player4").GetComponent<Player4Move>().player_info.lives--;
        }


        DecrementLock = true;
    }
}
