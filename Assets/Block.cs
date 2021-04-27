using System;
using UnityEngine;

public class Block : MonoBehaviour {
    public GameObject destroyParticlesPrefab;
    public BlockType type;
    static bool IsTraversable (Block block) {
        return !(block != null && block.type == BlockType.sand || block.type == BlockType.stone);
    }

    public static bool IsCharTraversable (Block block) {
        if (!block) {
            return true;
        }

        return block.type != BlockType.sand && block.type != BlockType.stone;
    }

    void Awake () {
        Add ();
    }

    public void Destroy () {
        if (destroyParticlesPrefab != null) {
            Instantiate (destroyParticlesPrefab, transform.position, Quaternion.identity);
        }
        var pos = GetPos ();
        DestoyInternal ();
        HandleDestroyedAt (pos);
    }

    static void HandleDestroyedAt (Vector2Int originalPos) {
        var pos = originalPos;
        Block block = GetAt (pos);
        if (block) {
            return;
        }
        while (!block) {
            pos += Vector2Int.up;
            var upperBlock = GetAt (pos);
            if (!upperBlock) {
                break;
            }
            if (CanDrop (upperBlock.type)) {
                upperBlock.MoveTo (pos - Vector2Int.up);
            }
            block = GetAt (pos);
        }
        DG.Tweening.DOVirtual.DelayedCall (0.01f, () => HandleDestroyedAt (originalPos - Vector2Int.up));
    }

    static bool CanDrop (BlockType type) {
        switch (type) {
            case BlockType.sand:
            case BlockType.scrollBonus:
            case BlockType.shovelBonus:
            case BlockType.stonerBonus:
            case BlockType.dynamiteBonus:
                return true;
            default:
                return false;
        }
    }

    void MoveTo (Vector2Int newPos) {
        Remove ();
        transform.position = (Vector3) (Vector2) newPos;
        Add ();
    }

    public static Vector2Int GetPos (Vector3 pos) {
        return Vector2Int.RoundToInt (pos);
    }

    Vector2Int GetPos () {
        return GetPos (transform.position);
    }

    void Remove () {
        Level.instance.blocks.Remove (GetPos ());
    }

    void Add () {
        Level.instance.blocks.Add (GetPos (), this);
    }

    public static Block GetAt (Vector2 pos) {
        Level.instance.blocks.TryGetValue (GetPos (pos), out var block);
        return block;
    }

    void DestoyInternal () {
        Remove ();
        Destroy (gameObject);
    }

    internal void ReplaceWith (BlockType blockType) {
        var pos = GetPos ();
        DestoyInternal ();
        Level.instance.CreateBlock (blockType, pos);
    }
}