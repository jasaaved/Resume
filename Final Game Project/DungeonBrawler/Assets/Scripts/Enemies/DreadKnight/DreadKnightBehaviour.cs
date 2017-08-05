using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadKnightBehaviour : MonoBehaviour {

    [HideInInspector] public Vector3[] teleportPositions = new Vector3[4];
    [HideInInspector] public Vector3 spawnPosition;
    private CombatInteraction m_CombatInteraction;
    [HideInInspector] public Vector3 newPosition;
    private LightPillarSpawner LPS;
    private EnemyStats m_EnemyStats;
    private Color flashColor;
    private Color m_MainColor;
    public GameObject Hammer;
    public GameObject LightBlastVertical;
    public GameObject LightBlastHorizontal;
    public GameObject TeleportMarker;
    public GameObject Slime;
    public GameObject DreadKnightSlash;
    public bool repositioning;
    public bool attacking;
    private float idleTimer;
    private float teleportTimer;
    public float maxIdleTime;

    public bool TestHammer;
    public bool TestCrossBlast;
    public bool TestTripleRain;
    public bool TestRandomAttack;
    public bool TestSpawnSlimes;
    public bool TestSlash;
    public bool CrusaderMode;

    private float alpha;
    private Color color;
    private float currentRemainTime;
    public float fadeTime = 3f;
    private SpriteRenderer m_Renderer;
    public bool isDead;

    private void Awake()
    {
        spawnPosition = transform.position;
        SetTeleportPositions();
        newPosition = GetNewPosition();
        m_Renderer = GetComponent<SpriteRenderer>();
        isDead = false;
    }
    private void Start()
    {
        repositioning = false;
        attacking = false;
        LPS = GetComponent<LightPillarSpawner>();
        idleTimer = 0;
        m_CombatInteraction = GetComponent<CombatInteraction>();
        m_EnemyStats = GetComponent<EnemyStats>();
        flashColor = Color.red;
        m_MainColor = GetComponent<SpriteRenderer>().material.GetColor("_Color");
        SpawnMarker();  
    }
    private void Update()
    {
        if (!isDead)
        {
            if (m_EnemyStats.health <= 0)
            {
                isDead = true;
                currentRemainTime = fadeTime;
                return;
            }
            Act();
            CombatInteractionHandler();
            DisplayDamageTaken();
            TestMoves();
        }
        else
        {
            currentRemainTime -= Time.deltaTime;
            if (currentRemainTime <= 0f)
            {
                Destroy(this.gameObject);
                return;
            }

            fadingOut();
        }
    }

    ///////////////////////////////////
    /// DREAD KNIGHT MOVES AND ATTACKS
    ///////////////////////////////////
    private void Teleport()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, 0.05f);
        if(InRange(newPosition, 0.25f))
        {
            repositioning = false;
            attacking = true;
        }
        //teleportTimer -= Time.deltaTime;
        //if (teleportTimer <= 0)
        //{
        //    newPosition = GetNewPosition();
        //    teleportTimer = 3f;
        //}
        //if (teleportTimer > 0)
        //{
        //    transform.position = Vector3.Lerp(transform.position, newPosition, 0.1f);
        //}
        //print(Vector3.Distance(transform.position, newPosition));
    }
    private void HammerThrow()
    {
        GameObject.Instantiate(Hammer);
    }
    private void CrossBlast()
    {
        float x = GetRandomX();
        float y = GetRandomY();
        Vector3 hPosition = new Vector3(0, y, 0) + spawnPosition;
        Vector3 vPosition = new Vector3(x, 0, 0) + spawnPosition;
        GameObject.Instantiate(LightBlastHorizontal, hPosition, transform.rotation);
        GameObject.Instantiate(LightBlastVertical, vPosition, transform.rotation);
    }
    private void TripleRain()
    {
        float x1 = GetRandomX();
        float x2 = GetRandomX();
        float x3 = GetRandomX();
        GameObject.Instantiate(LightBlastVertical, new Vector3(x1, 0, 0) + spawnPosition, transform.rotation);
        GameObject.Instantiate(LightBlastVertical, new Vector3(x2, 0, 0) + spawnPosition, transform.rotation);
        GameObject.Instantiate(LightBlastVertical, new Vector3(x3, 0, 0) + spawnPosition, transform.rotation);
    }
    private void SpawnMarker()
    {
        GameObject.Instantiate(TeleportMarker, transform.position, transform.rotation);
    }
    private void SpawnSlimes()
    {
        for(int i = 0; i < teleportPositions.Length; i++)
        {
            Vector3 position = teleportPositions[i];
            Vector3 right = new Vector3(position.x + 1, position.y, 0);
            Vector3 left = new Vector3(position.x - 1, position.y, 0);
            GameObject.Instantiate(Slime, right, transform.rotation);
            GameObject.Instantiate(Slime, left, transform.rotation);
            /*
            Vector3 position = teleportPositions[i];
            GameObject.Instantiate(Slime, position, transform.rotation);
            */
        }
    }
    private void Slash()
    {
        Vector3 right = new Vector3(transform.position.x + 2.5f, transform.position.y, 0);
        Vector3 left = new Vector3(transform.position.x - 2.5f, transform.position.y, 0);
        Vector3 leftRotate = new Vector3(-2, 3, 1);
        GameObject.Instantiate(DreadKnightSlash, right, transform.rotation);
        GameObject leftSwing = GameObject.Instantiate(DreadKnightSlash, left, transform.rotation);
        leftSwing.transform.localScale = leftRotate;
    }

    //////////////////////////////////
    /// HELPER FUNCTIONS
    //////////////////////////////////
    private void SetTeleportPositions()
    {
        teleportPositions = new Vector3[4];
        teleportPositions[0] = new Vector3(13, 1, 0) + spawnPosition;
        teleportPositions[1] = new Vector3(-13, 1, 0) + spawnPosition;
        teleportPositions[2] = new Vector3(0, -5, 0) + spawnPosition;
        teleportPositions[3] = new Vector3(0, 7, 0) + spawnPosition;
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<CombatInteraction>().damage = 3f;
        }
    }
    private void CombatInteractionHandler()
    {
        if (m_CombatInteraction.damage > 0)
        {
            // Deal the damage
            m_EnemyStats.health -= m_CombatInteraction.damage;
            GameObject blood = Resources.Load<GameObject>("Prefabs/BloodSplatter");
            GameObject.Instantiate(blood, transform.position, transform.rotation);
            // Show damage has been taken
            StartCoroutine(DisplayDamageTaken());
            flashColor = Color.red;
        }

        // ALWAYS RESET VALUES
        m_CombatInteraction.damage = 0;
        m_CombatInteraction.knockback = 0;
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.SetColor("_Color", flashColor);

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.material.SetColor("_Color", m_MainColor);
        }
    }  
    private void TestMoves()
    {
        if (TestHammer)
        {
            HammerThrow();
            TestHammer = false;
        }
        if (TestCrossBlast)
        {
            CrossBlast();
            TestCrossBlast = false;
        }
        if (TestTripleRain)
        {
            TripleRain();
            TestTripleRain = false;
        }
        if(TestRandomAttack)
        {
            CastRandomAttack();
            TestRandomAttack = false;
        }
        if(TestSpawnSlimes)
        {
            SpawnSlimes();
            TestSpawnSlimes = false;
        }
        if(TestSlash)
        {
            Slash();
            TestSlash = false;
        }
        if (CrusaderMode)
        {
            HammerThrow();
        }
    }

    //////////////////////////////////
    /// AI RELATED FUNCTIONS
    //////////////////////////////////
    private void Act()
    {
        if(repositioning)
        {
            Teleport();
        }
        if (attacking)
        {
            CastRandomAttack();
        }
        if(idleTimer <= 0 && !repositioning)
        {
            if(!MarkerOnScreen())
            {
                newPosition = GetNewPosition();
                SpawnMarker();
            }
        }
        if(m_EnemyStats.health <= 500 && !LPS.enabled)
        {
            LPS.enabled = true;
        }
        idleTimer -= Time.deltaTime;
    }
    private void CastRandomAttack()
    {
        int attack = Random.Range(0, 5);
        if(attack == 0)
        {
            TestHammer = true;
        }
        else if(attack == 1)
        {
            TestCrossBlast = true;
        }
        else if(attack == 2)
        {
            TestTripleRain = true;
        }
        else if(attack == 3)
        {
            if(!SlimesOnScreen())
            {
                TestSpawnSlimes = true;
            }
        }
        else if(attack == 4)
        {
            TestSlash = true;
        }
        attacking = false;
        idleTimer = maxIdleTime;
    }
    private bool MarkerOnScreen()
    {
        GameObject marker = GameObject.Find("TeleportMarker(Clone)");
        if(marker != null)
        {
            return true;
        }
        return false;
    }
    private bool SlimesOnScreen()
    {
        GameObject Slimes = GameObject.Find("Slime(Clone)");
        if(Slimes != null)
        {
            return true;
        }
        return false;
    }
    private float GetRandomX()
    {
        return Random.Range(-17f, 17f);
    }
    private float GetRandomY()
    {
        return Random.Range(-6f, 7f);
    }
    public Vector3 GetNewPosition()
    {
        int positionIndex = Random.Range(0, 4);
        if (teleportPositions[positionIndex] != transform.position)
        {
            return teleportPositions[positionIndex];
        }
        else
        {
            positionIndex = Random.Range(0, 4);
            return teleportPositions[positionIndex];
        }
    }
    private bool InRange(Vector3 newPosition, float minimum)
    {
        if (Vector3.Distance(transform.position, newPosition) <= minimum)
        {
            return true;
        }
        return false;
    }
    private void fadingOut()
    {
        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;
    }
}
