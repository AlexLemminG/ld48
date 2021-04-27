using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour {
    public GameObject boomParticlesRoot;
    public GameObject smokeParticles;
    public GameObject boomParticles;
    public float boomDelay = 1f;
    // Start is called before the first frame update
    void Start () {
        boomParticlesRoot.transform.parent = null;
        Invoke ("Boom", boomDelay);
    }

    void Boom () {
        var block = Block.GetAt (transform.position);
        if (block != null) {
            block.Destroy ();
        }
        SoundManager.instance.Play (SoundManager.instance.explosionClips);
        boomParticles.SetActive (true);
        smokeParticles.SetActive (false);
        Destroy (gameObject);
        Destroy (boomParticlesRoot, 10f);
    }
}