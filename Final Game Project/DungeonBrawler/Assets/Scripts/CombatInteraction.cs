using UnityEngine;
using System.Collections;

public class CombatInteraction : MonoBehaviour {

    [HideInInspector]
    public bool disabled;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float knockback;
    [HideInInspector]
    public float fixedVerticalKnockback = 3;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    public bool stunned;
    [HideInInspector]
    public bool immuneToCC;
    private GameObject warrior;

    //TODO move this code to a proper place
    void Start()
    {
        warrior = GameObject.Find("Warrior");

        if (warrior == null)
        {
            warrior = GameObject.Find("Warrior(Clone)");
        }
    }
    void Update()
    {
        if (disabled)
        {
            damage = 0f;
            knockback = 0f;
        }
    }
    void OnDestroy()
    {
        GameObject room1Controller = GameObject.Find("Room1Controller");
        Room1Enemies room1Enemies = null;

        
        if (room1Controller != null) room1Enemies = GameObject.Find("Room1Controller").GetComponent<Room1Enemies>();
        
        if (tag == "Enemy")
        {
            if (room1Enemies != null)  room1Enemies.GetComponent<Room1Enemies>().numberOfEnemies--;
        }
    }

}
