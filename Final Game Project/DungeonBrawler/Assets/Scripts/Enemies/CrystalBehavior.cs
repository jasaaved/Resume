using UnityEngine;
using System.Collections;

public class CrystalBehavior : MonoBehaviour
{
    public GameObject golem;
    public Color flashColor = Color.red;

    private CombatInteraction combatInteraction;
    private EnemyStats enemyStats;
    private Color mainColor;
    private float alpha;
    private Color color;
    private float currentRemainTime;
    private float fadeTime = 1f;
    private SpriteRenderer m_Renderer;
    private bool isDead;

    void Start()
    {
        transform.parent = null;
        enemyStats = GetComponent<EnemyStats>();
        combatInteraction = GetComponent<CombatInteraction>();
        mainColor = GetComponent<SpriteRenderer>().color;
        m_Renderer = GetComponent<SpriteRenderer>();
        isDead = false;

        golem.GetComponent<GolemBehavior>().SetInvincible(true);
    }
    void Update()
    {
        if (!isDead)
        {
            CombatInteractionHandler();

            if (enemyStats.health <= 0)
            {
                golem.GetComponent<GolemBehavior>().SetInvincible(false);
                isDead = true;
                currentRemainTime = fadeTime;
            }
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

    private void CombatInteractionHandler()
    {
        if (combatInteraction.damage > 0)
        {
            // Deal the damage
            enemyStats.health -= combatInteraction.damage;

            // Show damage has been taken
            StartCoroutine(DisplayDamageTaken());
        }

        // ALWAYS RESET VALUES
        combatInteraction.damage = 0;
    }
    private IEnumerator DisplayDamageTaken()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = flashColor;

        yield return new WaitForSeconds(0.05f);

        if (renderer != null)
        {
            renderer.color = mainColor;
        }
    }

    private void fadingOut()
    {
        alpha = currentRemainTime / fadeTime;
        color = m_Renderer.color;
        color.a = alpha;
        m_Renderer.color = color;
    }
}
