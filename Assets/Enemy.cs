using UnityEngine;

public class Enemy : MonoBehaviour {
    public static bool HasWallAt (Vector2Int pos) {
        if (!Block.IsCharTraversable (Block.GetAt (pos))) {
            return true;
        } else {
            return false;
        }
    }

    public static bool HasWallAtDir (Vector2 pos, Vector2 dir) {
        float spread = 0.666f;
        float spreadPerp = 0.333f;
        var perp = Vector2.Perpendicular (dir.normalized);
        var posA = Vector2Int.RoundToInt (pos + dir * spread + perp * spreadPerp);
        var posB = Vector2Int.RoundToInt (pos + dir * spread);
        var posC = Vector2Int.RoundToInt (pos + dir * spread - perp * spreadPerp);
        return HasWallAt (posA) || HasWallAt (posB) || HasWallAt (posC);
    }
}