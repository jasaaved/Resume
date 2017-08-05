using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpritePicker : MonoBehaviour {
    public Sprite[] PlatformSprites;
    private SpriteRenderer SR;

	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = PlatformSprites[Random.Range(0, PlatformSprites.Length)];
        SR.color = Color.white;
	}
}
