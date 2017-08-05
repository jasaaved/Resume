using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FanOfDaggers : MonoBehaviour {

    /////////////////////////////////////////////////////////////////
    // MEMBERS
    /////////////////////////////////////////////////////////////////
    public GameObject dagger;
    public int count = 3;

    private List<GameObject> m_DaggerList;
    private GameObject m_Reticle;
    private ReticleMovement m_ReticleMovement;
    private float m_Angle;
    private WeaponStats m_WeaponStats;


    /////////////////////////////////////////////////////////////////
    // MONOBEHAVIOUR
    /////////////////////////////////////////////////////////////////
    void Start()
    {
        m_WeaponStats = GetComponent<WeaponStats>();
        Destroy(gameObject,m_WeaponStats.projectileLifeSpan);

        m_DaggerList = new List<GameObject>();
        
        m_Reticle = GameObject.Find("Rogue(Clone)");

        if (m_Reticle == null)
        {
            m_Reticle = GameObject.Find("Rogue").transform.FindChild("Reticle").gameObject;
        }
        else
        {
            m_Reticle = GameObject.Find("Rogue(Clone)").transform.FindChild("Reticle").gameObject;
        }

        m_ReticleMovement = m_Reticle.GetComponent<ReticleMovement>();
        m_Angle = m_ReticleMovement.angle * Mathf.Rad2Deg;

        float bound1 = m_Angle - 90;
        float bound2 = bound1 + 180;

        // Create angles for daggers to "fan out" in
        float divAngle = 180 / count;

        List<float> angleList = new List<float>();

        angleList.Add(m_Angle + 30);
        angleList.Add(m_Angle);
        angleList.Add(m_Angle - 30);

        for (int i = 0; i < count; i++)
        {
            GameObject dag = Instantiate(dagger);
            dag.transform.parent = transform;
            dag.GetComponent<Dagger>().weaponStats = m_WeaponStats;

            float posx = m_Reticle.transform.position.x;
            float posy = m_Reticle.transform.position.y;

            dag.transform.position = new Vector3(Mathf.Cos(m_Angle * Mathf.Deg2Rad) * m_WeaponStats.range + posx, Mathf.Sin(m_Angle * Mathf.Deg2Rad) * m_WeaponStats.range + posy, 0);
            dag.transform.eulerAngles = new Vector3(0, 0, m_Angle);
            dag.transform.parent = gameObject.transform;

            m_DaggerList.Add(dag);
        }

        for (int i = 0; i < count; i++)
        {
            Rigidbody2D rb = m_DaggerList[i].GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2();

            if (i == 0)
            {
                m_DaggerList[i].transform.Rotate(new Vector3(0, 0, -30));
                force.x = Mathf.Cos((m_Angle - 30) * Mathf.Deg2Rad);
                force.y = Mathf.Sin((m_Angle - 30) * Mathf.Deg2Rad);
            }
            if (i == 1)
            {
                force.x = Mathf.Cos(m_Angle * Mathf.Deg2Rad);
                force.y = Mathf.Sin(m_Angle * Mathf.Deg2Rad);
            }
            if (i == 2)
            {
                m_DaggerList[i].transform.Rotate(new Vector3(0, 0, 30));
                force.x = Mathf.Cos((m_Angle + 30) * Mathf.Deg2Rad);
                force.y = Mathf.Sin((m_Angle + 30) * Mathf.Deg2Rad);
            }

            force.x = force.x * m_WeaponStats.projectileVelocity;
            force.y = force.y * m_WeaponStats.projectileVelocity;
            rb.velocity = new Vector2(force.x, force.y) * m_WeaponStats.projectileVelocity;
        }
    }
    
}
