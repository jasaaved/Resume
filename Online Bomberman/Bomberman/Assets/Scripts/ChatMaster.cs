
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ChatMaster : MonoBehaviour
{

    class ChatEntry
    {
        public string name = "";
        public string message = "";
        public string timeTag = "";
    }

    ArrayList entries;
    Vector2 currentScrollPos = new Vector2();
    string inputField = "";
    bool chatInFocus = true;
    string inputFieldFocus = "CIFT";
    bool absPos = false;
    public GUISkin guiSkin;
    private string m_SendString;
 
    void Awake()
    {
        InitializeChat();
    }

    void InitializeChat()
    {
        entries = new ArrayList();

    }

    //draw the chat box in size relative to your GUIlayout
    public void Draw()
    {
        ChatWindow();
    }

    void ChatWindow()
    {
        GUILayout.BeginVertical("box");
        currentScrollPos = GUILayout.BeginScrollView(currentScrollPos, GUILayout.MaxWidth(1000), GUILayout.MinWidth(1000)); //limits the chat window size to max 1000x1000, remove the restraints if you want

        foreach (ChatEntry ent in entries)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.wordWrap = true;
            GUILayout.Label(ent.timeTag + " " + ent.name + ": " + ent.message);
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        GUILayout.EndScrollView();
        if (chatInFocus)
        {
            GUI.SetNextControlName(inputFieldFocus);
            inputField = GUILayout.TextField(inputField, GUILayout.MaxWidth(1000), GUILayout.MinWidth(1000));
            GUI.FocusControl(inputFieldFocus);
        }
        GUILayout.EndVertical();

        if (chatInFocus)
        {
            HandleNewEntries();
        }
        else {
            checkForInput();
        }

    }


    void checkForInput()
    {
        if (Event.current.type == EventType.KeyDown ||  Event.current.character == '\n' || !chatInFocus){
            GUI.FocusControl(inputFieldFocus);
            chatInFocus = true;
            currentScrollPos.y = float.PositiveInfinity;
        }
    }

    void HandleNewEntries()
    {
        if (Event.current.type == EventType.KeyDown ||  Event.current.character == '\n'){
            if (inputField.Length <= 0)
            {

                Debug.Log("unfocusing chat (empty entry)");
                return;
            }
            AddChatEntry(GameObject.Find("UserIPObj").GetComponent<UserIP>().display_name, inputField);
            m_SendString = "chat;" + GameObject.Find("UserIPObj").GetComponent<UserIP>().display_name + ";" + inputField;
            byte[] bytes = new byte[m_SendString.Length * sizeof(char)];
            System.Buffer.BlockCopy(m_SendString.ToCharArray(), 0, bytes, 0, bytes.Length);
            int recHostId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().recHostId;
            int connectionId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().connectionId;
            int channelId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().channelId;
            byte m_CommunicationChannel;
            m_CommunicationChannel = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().m_CommunicationChannel;
            int m_ConnectionId = GameObject.Find("LobbyNetwork").GetComponent<LobbyNetwork>().m_ConnectionId;
            byte error;
            NetworkTransport.Send(recHostId, m_ConnectionId, m_CommunicationChannel, bytes, bytes.Length, out error);
            //AddChatEntry("Cookie monster", inputField); //for offline testing
            inputField = "";
            //Debug.Log("unfocusing chat and entry sent");
        }
    }


    public void AddChatEntry(string name, string msg)
    {
        ChatEntry newEntry = new ChatEntry();
        newEntry.name = name;
        newEntry.message = msg;
        newEntry.timeTag = "[" + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + "]";
        entries.Add(newEntry);
        currentScrollPos.y = float.PositiveInfinity;
    }

    void Start()
    {
        Awake();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 100, 500, 250));
        Draw();
        GUILayout.EndArea();
    }

    
}

