using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Player3Move : MonoBehaviour
{
    public class Player_Info
    {
        public int assignment;
        public Vector3 pos;
        public float horizontal;
        public float vertical;
        public bool bomb;
        public bool dead;
        public int lives = 3;
    }

    public Player_Info player_info;
    private string m_SendString;
    private bool first_sent = false;

    void Start()
    {
        player_info = new Player_Info();
    }


    void Update()
    {
        int player_assignment = GameObject.Find("Network").GetComponent<Network>().player_assignment;

        if (player_assignment == 3)
        {
            // check input axes
            // Check Input Axes
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            GetComponent<Rigidbody2D>().velocity = new Vector2(h, v) * 6;

            // Set Animation Parameters
            GetComponent<Animator>().SetInteger("X", (int)h);
            GetComponent<Animator>().SetInteger("Y", (int)v);

            player_info.assignment = player_assignment;
            player_info.horizontal = h;
            player_info.vertical = v;
            player_info.pos = transform.position;

            if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("s")
                            || Input.GetKeyUp("d") || Input.GetKeyUp("up") || Input.GetKeyUp("left") || Input.GetKeyUp("right") || Input.GetKeyUp("down") || !first_sent)
            {
                m_SendString = JsonUtility.ToJson(player_info);

                byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
                int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
                int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
                byte m_CommunicationChannel;
                m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
                int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
                byte error;
                NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                if (player_info.bomb)
                    player_info.bomb = false;
                first_sent = true;
            }
            

            if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s")
                           || Input.GetKeyDown("d") || Input.GetKeyDown("up") || Input.GetKeyDown("left") || Input.GetKeyDown("right") || Input.GetKeyDown("down"))
            {
                m_SendString = JsonUtility.ToJson(player_info);

                byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
                System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
                int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
                int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
                int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
                byte m_CommunicationChannel;
                m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
                int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
                byte error;
                NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
                if (player_info.bomb)
                    player_info.bomb = false;
            }

        }
    }
    void OnApplicationQuit()
    {
        player_info.dead = true;
        m_SendString = JsonUtility.ToJson(player_info);

        byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
        System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
        int recHostId = GameObject.Find("Network").GetComponent<Network>().recHostId;
        int connectionId = GameObject.Find("Network").GetComponent<Network>().connectionId;
        int channelId = GameObject.Find("Network").GetComponent<Network>().channelId;
        byte m_CommunicationChannel = GameObject.Find("Network").GetComponent<Network>().m_CommunicationChannel;
        int m_ConnectionId = GameObject.Find("Network").GetComponent<Network>().m_ConnectionId;
        byte error;
        NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
    }
}
