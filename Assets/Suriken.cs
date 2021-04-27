using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken : Enemy {
    public float speed = 1f;
    public float rotationSpeed = 90f;
    Rigidbody2D rb;
    Vector2 velocity;
    float rotationOffset;
    void Start () {
        rb = GetComponent<Rigidbody2D> ();

        int depth = -Mathf.RoundToInt (transform.position.y);
        if (depth < 50) {
            speed = 1;
        } else if (depth < 150) {
            speed = 3;
        } else {
            speed = 8;
        }

        velocity = Random.insideUnitCircle.normalized * speed;
        rotationOffset = Random.Range (0f, 360f);
    }

    void Update () {
        if (velocity.y > 0f && Enemy.HasWallAtDir (transform.position, Vector2.up)) {
            velocity.y = -velocity.y;
        }
        if (velocity.y < 0f && Enemy.HasWallAtDir (transform.position, Vector2.down)) {
            velocity.y = -velocity.y;
        }
        if (velocity.x < 0f && Enemy.HasWallAtDir (transform.position, Vector2.left)) {
            velocity.x = -velocity.x;
        }
        if (velocity.x > 0f && Enemy.HasWallAtDir (transform.position, Vector2.right)) {
            velocity.x = -velocity.x;
        }

        transform.localEulerAngles = new Vector3 (0f, 0f, Time.time * rotationSpeed + rotationOffset);

        rb.velocity = velocity;
    }
}