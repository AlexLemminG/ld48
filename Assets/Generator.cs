using UnityEngine;

public class Generator {
    float perlinOffset = 0f;

    public Generator () {
        perlinOffset = Random.Range (-100f, 100f);
    }

    public BlockType GetBlockTypeAt (Vector2Int pos) {
        if (pos.y >= 0) {
            return BlockType.none;
        }

        if (!IsCave (pos) && IsCave (pos + Vector2Int.down)) {
            return BlockType.stone;
        }

        bool isGround = IsCave (pos) && !IsCave (pos + Vector2Int.down);
        if (IsCave (pos) && !isGround) {
            return BlockType.none;
        }
        if (!IsCave (pos)) {
            return GetRandomSolidBlock (pos);
        }

        float random = Random.Range (0f, 1f);
        if (random < Mathf.Lerp (0.00f, 0.04f, Mathf.InverseLerp (0, 400f, -pos.y))) {
            return BlockType.scrollBonus;
        }
        random = Random.Range (0f, 1f);
        if (random < Mathf.Lerp (0.04f, 0.08f, Mathf.InverseLerp (0, 300f, -pos.y))) {
            return GetRandomEnemy (pos);
        }
        random = Random.Range (0f, 1f);
        if (random < Mathf.Lerp (0.1f, 0.05f, Mathf.InverseLerp (0, 300f, -pos.y))) {
            return GetRandomBonus (pos);
        }

        return BlockType.none;
    }

    private BlockType GetRandomBonus (Vector2Int pos) {
        float random = Random.Range (0f, 1f);
        if (random < 0.666f) {
            return BlockType.shovelBonus;
        }
        random = Random.Range (0f, 1f);
        if (random < 0.666f) {
            return BlockType.stonerBonus;
        }
        return BlockType.dynamiteBonus;
    }

    private BlockType GetRandomEnemy (Vector2Int pos) {
        float random = Random.Range (0f, 1f);
        return random > 0.5f ? BlockType.slimeEnemy : BlockType.surikenEnemy;
    }

    BlockType GetRandomSolidBlock (Vector2Int pos) {
        return Random.Range (0f, 1f) > 0.9f ? BlockType.stone : BlockType.sand;
    }

    bool IsCave (Vector2Int pos) {
        float threshold = Mathf.Lerp (0.1f, 0.4f, Mathf.InverseLerp (0f, 300f, -pos.y));
        Vector3 perlinPos = new Vector3 (pos.x * 0.1f, pos.y * 0.2f, perlinOffset);
        return Perlin.Noise (perlinPos) > threshold;
    }
}