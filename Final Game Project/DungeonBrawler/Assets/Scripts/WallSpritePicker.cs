using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpritePicker : MonoBehaviour {
    public Sprite[] WallSprites;
    private SpriteRenderer SR;

	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = WallSprites[Random.Range(0, WallSprites.Length)];
        SR.color = Color.white;

        if (transform.parent != null)
        {
            if (transform.parent.transform.parent != null)
            {
                if (transform.parent.transform.parent.name == "BossDoors")
                {
                    SR.color = Color.green;
                }

                if (transform.parent.transform.parent.name == "BossDoors2")
                {
                    SR.color = Color.red;
                }
            }
        }
	}
}
