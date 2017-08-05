using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Rewired;

public class ReticleMovement : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    private GameObject player;

    public float angle;

    [HideInInspector] public bool cscheme1;
    [HideInInspector] public bool cscheme2;
    [HideInInspector] public bool cscheme3;
    private float m_Horizontal;
    private float m_Vertical;
    private bool m_Left;
    private bool m_Right;
    private int playerNumber;
    private Vector2 mag_check;

    private Player p1;
    private Player p2;
    private Player p3;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start ()
    {
        player = transform.parent.gameObject;
        cscheme1 = player.GetComponent<PlayerController>().cscheme1;
        cscheme2 = player.GetComponent<PlayerController>().cscheme2;
        cscheme3 = player.GetComponent<PlayerController>().cscheme3;

        playerNumber = player.GetComponent<PlayerController>().playerNumber;


        p1 = ReInput.players.GetPlayer(0);
        p2 = ReInput.players.GetPlayer(1);
        p3 = ReInput.players.GetPlayer(2);

    }

    void Update()
    {
        playerNumber = player.GetComponent<PlayerController>().playerNumber;
        switch (playerNumber)
        {
            case 1:
                Reticle1Control();
                break;
            case 2:
                Reticle2Control();
                break;
            case 3:
                Reticle3Control();
                break;
        }

        angle = Mathf.Atan2(m_Vertical, m_Horizontal);
        float posx = player.transform.position.x;
        float posy = player.transform.position.y;

        transform.position = new Vector3(Mathf.Cos(angle) * 1.5f + posx, Mathf.Sin(angle) * 1.5f + posy, 0);
        transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        

            // Orient its scale to face left
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
 
        
    }
    private void Reticle1Control()
    {

        if (cscheme1 || cscheme3)
        {
            if (cscheme3 && (p1.GetAxis("RS Axis X") != 0 || p1.GetAxis("RS Axis Y") != 0))
            {
                m_Horizontal = p1.GetAxis("RS Axis X");
                m_Vertical = p1.GetAxis("RS Axis Y");
            }
            else
            {
                m_Vertical = p1.GetAxis("RS Axis Y");
                m_Horizontal = p1.GetAxis("RS Axis X");
            }


        }

        else if (cscheme2)
        {

            mag_check = new Vector2(p1.GetAxis("RS Axis X"), p1.GetAxis("RS Axis Y"));
            if (mag_check.magnitude >= 0.90)
            {
                m_Horizontal = p1.GetAxis("RS Axis X");
                m_Vertical = p1.GetAxis("RS Axis Y");
            }



        }
    }
    private void Reticle2Control()
    {

        if (cscheme1 || cscheme3)
        {
            if (cscheme3 && (p2.GetAxis("RS Axis X") != 0 || p2.GetAxis("RS Axis Y") != 0))
            {
                m_Horizontal = p2.GetAxis("RS Axis X");
                m_Vertical = p2.GetAxis("RS Axis Y");
            }
            else
            {
                m_Vertical = p2.GetAxis("RS Axis Y");
                m_Horizontal = p2.GetAxis("RS Axis X");
            }


        }

        else if (cscheme2)
        {

            mag_check = new Vector2(p2.GetAxis("RS Axis X"), p2.GetAxis("RS Axis Y"));
            if (mag_check.magnitude >= 0.90)
            {
                m_Horizontal = p2.GetAxis("RS Axis X");
                m_Vertical = p2.GetAxis("RS Axis Y");
            }



        }
    }
    private void Reticle3Control()
    {

        if (cscheme1 || cscheme3)
        {
            if (cscheme3 && (p3.GetAxis("RS Axis X") != 0 || p3.GetAxis("RS Axis Y") != 0))
            {
                m_Horizontal = p3.GetAxis("RS Axis X");
                m_Vertical = p3.GetAxis("RS Axis Y");
            }
            else
            {
                m_Vertical = p3.GetAxis("RS Axis Y");
                m_Horizontal = p3.GetAxis("RS Axis X");
            }


        }

        else if (cscheme2)
        {

            mag_check = new Vector2(p3.GetAxis("RS Axis X"), p3.GetAxis("RS Axis Y"));
            if (mag_check.magnitude >= 0.90)
            {
                m_Horizontal = p3.GetAxis("RS Axis X");
                m_Vertical = p3.GetAxis("RS Axis Y");
            }



        }
    }

    void LateUpdate()
    {
        if (player.transform.localScale.x < 0)
        {
            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
