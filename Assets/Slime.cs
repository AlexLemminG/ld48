using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {
    public float moveSpeed = 5f;

    public float jumpVelocity = 10f;
    public float jumpProbability = 0.5f;
    public bool preventFalling = false;

    bool movingLeft = false;
    Rigidbody2D rb;
    float prevJumpTime = 0f;
    float noMoveChangeDuration = 0.5f;
    void Start () {
        movingLeft = Random.Range (0f, 1f) > 0.5f;
        rb = GetComponent<Rigidbody2D> ();

        int depth = -Mathf.RoundToInt (transform.position.y);
        if (depth < 50) {
            moveSpeed = 2;
            preventFalling = true;
            jumpProbability = 0f;
            sprite.sprite = level1;
        } else if (depth < 150) {
            moveSpeed = 5;
            preventFalling = false;
            jumpProbability = 0.5f;
            sprite.sprite = level2;
        } else {
            moveSpeed = 8;
            preventFalling = false;
            jumpProbability = 0.6f;
            sprite.sprite = level3;
        }
    }
    public SpriteRenderer sprite;
    public Sprite level1;
    public Sprite level2;
    public Sprite level3;

    void Update () {
        bool hasWallLeft = Enemy.HasWallAt (Block.GetPos (transform.position + Vector3.left * 0.666f));
        bool hasWallRight = Enemy.HasWallAt (Block.GetPos (transform.position + Vector3.right * 0.666f));
        bool hasFloorLeft = Enemy.HasWallAt (Block.GetPos (transform.position + Vector3.left * 0.666f + Vector3.down));
        bool hasFloorRight = Enemy.HasWallAt (Block.GetPos (transform.position + Vector3.right * 0.666f + Vector3.down));

        bool wantJump = Random.Range (0f, 1f) < jumpProbability;

        Vector2 velocity = rb.velocity;
        if (Time.time - prevJumpTime > noMoveChangeDuration) {
            if (movingLeft && (hasWallLeft || preventFalling && !hasFloorLeft)) {
                if (wantJump) {
                    velocity.y = jumpVelocity;
                    prevJumpTime = Time.time;
                } else {
                    movingLeft = false;
                }
            }
            if (!movingLeft && (hasWallRight || preventFalling && !hasFloorRight)) {
                if (wantJump) {
                    velocity.y = jumpVelocity;
                    prevJumpTime = Time.time;
                } else {
                    movingLeft = true;
                }
            }
        }
        bool noMoving = hasWallLeft && hasWallRight;
        if (noMoving) {
            velocity.x = 0f;
        } else {
            velocity.x = movingLeft ? -moveSpeed : moveSpeed;
        }
        if (velocity.x > 0.5f) {
            sprite.flipX = false;
        } else if (velocity.x < -0.5f) {
            sprite.flipX = true;
        }

        rb.velocity = velocity;
    }
}