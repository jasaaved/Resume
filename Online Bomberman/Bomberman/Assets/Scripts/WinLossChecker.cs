using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class WinLossChecker : MonoBehaviour {
    private bool notWaited = true;
    public bool P1Dead = false;
    public bool P2Dead = false;
    public bool P3Dead = false;
    public bool P4Dead = false;
    public bool started = false;

    public class Player_Info
    {
        public int assignment;
        public Vector3 pos = new Vector3(0,0,0);
        public float horizontal = 0;
        public float vertical = 0;
        public bool bomb = false;
        public bool dead = true;
        public int lives = 0;
    }

    private Player_Info player_info;
    private bool already_sent = false;

    void Start()
    {
        player_info = new Player_Info();
    }

    // Update is called once per frame
    void Update () {
        //StartCoroutine(Wait5());

        if (!started)
            return;

        if (!P1Dead && GameObject.Find("Player1") == null){
            P1Dead = true;
        }

        if (!P2Dead && GameObject.Find("Player2") == null){
            P2Dead = true;
        }

        if (!P3Dead && GameObject.Find("Player3") == null){
            P3Dead = true;
        }

        if (!P4Dead && GameObject.Find("Player4") == null)
        {
            P4Dead = true;
        }


        if (P1Dead && GameObject.Find("Player1") != null)
        {
            P1Dead = false;
        }

        if (P2Dead && GameObject.Find("Player2") != null)
        {
            P2Dead = false;
        }

        if (P3Dead && GameObject.Find("Player3") != null)
        {
            P3Dead = false;
        }

        if (P4Dead && GameObject.Find("Player4") != null)
        {
            P4Dead = false;
        }

        int player_assignment = GameObject.Find("Network").GetComponent<Network>().player_assignment;
        
        if(player_assignment == 1 && P1Dead && !already_sent)
        {
            player_info.assignment = player_assignment;
            string m_SendString = JsonUtility.ToJson(player_info);
            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
            int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
            int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
            int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
            byte m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
            int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
            byte error;
            NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
            already_sent = true;
        }

        if (player_assignment == 2 && P2Dead && !already_sent)
        {
            player_info.assignment = player_assignment;
            string m_SendString = JsonUtility.ToJson(player_info);
            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
            int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
            int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
            int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
            byte m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
            int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
            byte error;
            NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
            already_sent = true;
        }

        if (player_assignment == 3 && P3Dead && !already_sent)
        {
            
            player_info.assignment = player_assignment;
            string m_SendString = JsonUtility.ToJson(player_info);
            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
            int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
            int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
            int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
            byte m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
            int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
            byte error;
            NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
            already_sent = true;
        }

        if (player_assignment == 4 && P4Dead && !already_sent)
        {

            player_info.assignment = player_assignment;
            string m_SendString = JsonUtility.ToJson(player_info);
            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
            int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
            int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
            int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
            byte m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
            int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
            byte error;
            NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
            already_sent = true;
        }




        if (P1Dead && P2Dead && P3Dead && P4Dead)
        {
            GameObject.Find("Winner").GetComponent<WinnerHolder>().winner = "It's a tie!";
            DontDestroyOnLoad(GameObject.Find("Winner"));
            SceneManager.LoadScene("TieScreen");
        }

        else if (!P1Dead && P2Dead && P3Dead && P4Dead)
        {
            GameObject.Find("Winner").GetComponent<WinnerHolder>().winner = "White Player Wins!";
            DontDestroyOnLoad(GameObject.Find("Winner"));
            //SceneManager.LoadScene("TieScreen");
            if (player_assignment == 1)
                SceneManager.LoadScene("WinScreen");
            else
                SceneManager.LoadScene("LossScreen");
        }

        else if (P1Dead && !P2Dead && P3Dead && P4Dead)
        {
            GameObject.Find("Winner").GetComponent<WinnerHolder>().winner = "Red Player Wins!";
            DontDestroyOnLoad(GameObject.Find("Winner"));
            //SceneManager.LoadScene("TieScreen");
            if (player_assignment == 2)
                SceneManager.LoadScene("WinScreen");
            else
                SceneManager.LoadScene("LossScreen");
        }

        else if (P1Dead && P2Dead && !P3Dead && P4Dead)
        {
            GameObject.Find("Winner").GetComponent<WinnerHolder>().winner = "Blue Player Wins!";
            DontDestroyOnLoad(GameObject.Find("Winner"));
            //SceneManager.LoadScene("TieScreen");
            if (player_assignment == 3)
                SceneManager.LoadScene("WinScreen");
            else
                SceneManager.LoadScene("LossScreen");
        }

        else if (P1Dead && P2Dead && P3Dead && !P4Dead)
        {
            GameObject.Find("Winner").GetComponent<WinnerHolder>().winner = "Yellow Player Wins!";
            DontDestroyOnLoad(GameObject.Find("Winner"));
            //SceneManager.LoadScene("TieScreen");
            if (player_assignment == 4)
                SceneManager.LoadScene("WinScreen");
            else
                SceneManager.LoadScene("LossScreen");
        }
	}

    IEnumerator Wait5(){
        yield return new WaitForSeconds(5);
        notWaited = false;
    }
}
