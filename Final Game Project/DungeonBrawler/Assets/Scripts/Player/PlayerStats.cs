using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    public float maxHealth = 25f;
    public float health = 25f;
    public float maxSpeed = 10f;
    public float maxJumpForce = 600f;
    public bool isPoisoned = false;
    public float poisonTimer = 2.0f;
    public static int warriorNum = 1;
    public static int rogueNum = 2;
    public static int wizardNum = 3;
}
