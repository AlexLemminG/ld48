using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Godrays : MonoBehaviour {
    public float rotationSpeed = 90;
    public float freq;
    public float power;
    public float offset = 1f;
    void Update () {
        transform.localEulerAngles = new Vector3 (0f, 0f, rotationSpeed * Time.unscaledTime);
        transform.localScale = Vector3.one * (offset + Mathf.Sin (Time.unscaledTime * freq) * power);
    }
}