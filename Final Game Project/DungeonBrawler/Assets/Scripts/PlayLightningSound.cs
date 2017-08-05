using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLightningSound : MonoBehaviour {


    private void Start()
    {
        this.GetComponent<AudioSource>().PlayDelayed(0.5f);
        Destroy(this.gameObject, 3.5f);
    }
}
