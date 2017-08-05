using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSprite : MonoBehaviour {
    
    public Color mainColor;

	void Start ()
    {
        mainColor = GetComponent<SpriteRenderer>().color;
	}
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        float x = 0;
        float y = 0;
        float z = transform.localPosition.z;
        transform.localPosition = new Vector3(x, y, z);
    }

}
