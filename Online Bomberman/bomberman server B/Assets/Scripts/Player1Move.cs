using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Player1Move : MonoBehaviour
{
    public  class Player_Info
    {
        public int assignment;
        public Vector2 pos;
        public float horizontal;
        public float vertical;
        public bool bomb;
        public bool dead;
    }

    public Player_Info player_info;

    void Start()
    {
        player_info = new Player_Info();
    }


    void FixedUpdate()
    {
        int player_assignment = GameObject.Find("Main Camera").GetComponent<StWindow>().player_assignment;

        if (player_assignment == 1)
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
            player_info.pos = transform.position;
            player_info.horizontal = h;
            player_info.vertical = v;

        }
    }
}
