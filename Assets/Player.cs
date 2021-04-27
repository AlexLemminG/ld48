using UnityEngine;

public class Player : Singleton<Player>, ISceneSingleton {
    public int health;
    public int dynamiteCount;
    public int shovelsCount;
    public int stonersCount;

    public GameObject selectionObj;

    Rigidbody2D rb;

    protected override void OnAwake () {
        rb = GetComponent<Rigidbody2D> ();
        selectionObj.SetActive (false);
        selectionObj.transform.parent = null;

        dynamiteCount = ScrollBonusManager.instance.initialDynamites.GetVar ();
        shovelsCount = ScrollBonusManager.instance.initialShovels.GetVar ();
        stonersCount = ScrollBonusManager.instance.initialStoners.GetVar ();
    }

    float lastDigTime = -10f;
    float digPeriod = 0.1f;
    void Update () {
        if (health <= 0) {
            return;
        }
        if (Time.unscaledTime > 0.1f) {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        SuperBlock.EnsureCreated (transform.position, 30);

        UpdateMovement ();

        var block = GetCurrentBlock ();
        if (Input.GetButtonDown ("Boom") || (Input.GetButton ("Dig") && Input.GetButtonDown ("Stone") || Input.GetButtonDown ("Dig") && Input.GetButton ("Stone"))) {
            CreateDynamite (block);
        }
        if (Input.GetButtonDown ("Stone")) {
            MakeStone (block);
        }
        if (Time.time - lastDigTime > digPeriod && Input.GetButton ("Dig")) {
            lastDigTime = Time.time;
            Dig (block);
        }

        if (block != null) {
            selectionObj.transform.position = block.transform.position;
            selectionObj.SetActive (true);
        } else {
            selectionObj.SetActive (false);
        }

        maxDepth = Mathf.Max (maxDepth, -Block.GetPos (transform.position).y);
        Session.instance.UpdateLowestDepth (maxDepth);

        if (IsNoDamage ()) {
            sprite.color = Time.time % 0.25f > 0.125f ? Color.white : Color.clear;
        } else {
            sprite.color = Color.white;
        }
    }

    void Dig (Block block) {
        if (!block) {
            return;
        }
        if (block.type != BlockType.sand) {
            return;
        }
        if (shovelsCount == 0) {
            UI.instance.shovelPanel.Shake ();
        } else {
            shovelsCount--;
            block.Destroy ();
            SoundManager.instance.Play (SoundManager.instance.hitSandClips);
        }
    }

    void MakeStone (Block block) {
        if (!block) {
            return;
        }
        if (block.type != BlockType.sand) {
            return;
        }
        if (stonersCount == 0) {
            UI.instance.stonerPanel.Shake ();
        } else {
            stonersCount--;
            block.ReplaceWith (BlockType.stone);
        }
    }
    void CreateDynamite (Block block) {
        if (!block) {
            return;
        }
        if (block.type != BlockType.stone) {
            return;
        }
        if (dynamiteCount == 0) {
            UI.instance.dynamitePanel.Shake ();
        } else {
            dynamiteCount--;
            Instantiate (Level.instance.dynamitePrefab, block.transform.position, Quaternion.identity);
        }
    }

    Block GetCurrentBlock () {
        var blockPos = Block.GetPos (CameraManager.instance.GetMousePosition ());
        var playerPos = Block.GetPos (transform.position);
        var delta = blockPos - playerPos;
        delta.x = Mathf.Clamp (delta.x, -1, 1);
        delta.y = Mathf.Clamp (delta.y, -1, 1);
        blockPos = playerPos + delta;

        var block = Block.GetAt (blockPos);
        if (!block) {
            return null;
        }
        if (block.type == BlockType.sand || block.type == BlockType.stone) {
            return block;
        }
        return null;
    }

    public float moveSpeed = 10f;
    public float moveSpeedLerp = 10f;
    public float jumpVelocity = 10f;

    bool hasSecondJump = false;
    float jumpMinPeriod = 0.2f;
    float prevJumpTime = -1f;
    public SpriteRenderer sprite;

    bool IsOnGround () {
        return Enemy.HasWallAtDir (transform.position, Vector2.down);
    }

    void UpdateMovement () {
        float moveX = Input.GetAxis ("Horizontal") * moveSpeed;

        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.Lerp (velocity.x, moveX, Time.deltaTime * moveSpeedLerp);

        if (velocity.x > 0.5f) {
            sprite.flipX = false;
        } else if (velocity.x < -0.5f) {
            sprite.flipX = true;
        }

        bool isOnGround = IsOnGround ();
        if (isOnGround) {
            hasSecondJump = ScrollBonusManager.instance.doubleJump.GetVar () > 0;
        }

        if (Input.GetButtonDown ("Jump") && (isOnGround || hasSecondJump) && Time.time - prevJumpTime > jumpMinPeriod) {
            prevJumpTime = Time.time;
            if (!isOnGround) {
                hasSecondJump = false;
            }
            SoundManager.instance.Play (SoundManager.instance.jumpClips);
            velocity.y = jumpVelocity;
        }
        //TODO hold jump to jump higher

        rb.velocity = velocity;
    }

    void OnTriggerStay2D (Collider2D collider) {
        var bonus = collider.GetComponentInParent<Bonus> ();
        if (bonus) {
            bonus.Pickup (this);
        }

        var damager = collider.GetComponentInParent<PlayerDamager> ();
        if (damager) {
            HandleDamage (damager);
        }
    }

    float prevDamageTime = -100f;
    public float noDamgeDuration = 1f;
    public float damageVelocity;

    bool IsNoDamage () {
        return Time.time - prevDamageTime < noDamgeDuration;
    }

    void HandleDamage (PlayerDamager damager) {
        if (IsNoDamage ()) {
            return;
        }
        prevDamageTime = Time.time;
        health--;
        UI.instance.healthPanel.Shake ();
        if (health <= 0) {
            //TODO death sound
            sprite.transform.localEulerAngles = new Vector3 (0f, 0f, 90f);
            DG.Tweening.DOVirtual.DelayedCall (1f, () => {
                UI.instance.scrollPanel.ShowDeath ();
                SoundManager.instance.Play (SoundManager.instance.loseClips);
            });
        }

        Vector2 velocity = transform.position - damager.transform.position;
        velocity = velocity.normalized;
        velocity += Vector2.up;
        velocity = velocity.normalized;
        velocity *= damageVelocity;

        rb.velocity = Vector2.Lerp (rb.velocity, velocity, 0.9f);
        SoundManager.instance.Play (SoundManager.instance.getDamageClips);
    }

    int maxDepth = 0;
    public int GetMaxDepth () {
        return maxDepth;
    }
}