using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Highscore : MonoBehaviour {
    public TMP_Text mesh;
    // Start is called before the first frame update
    void Start () {
        mesh.text = "Highscore:\n" + Session.instance.GetLowestDepth () + "m";
    }

    // Update is called once per frame
    void Update () {

    }
}