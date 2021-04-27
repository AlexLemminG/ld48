using UnityEngine;

public class SuperBlock : MonoBehaviour {
    public bool generate = true;
    const int size = 10;

    public GameObject skyBackground;
    public GameObject groundBackground;

    void Add () {
        Level.instance.superblocks.Add (GetPos (), this);
    }

    static Vector2Int GetPos (Vector3 pos) {
        return new Vector2Int (Mathf.RoundToInt (pos.x / size), Mathf.RoundToInt (pos.y / size));
    }

    Vector2Int GetPos () {
        return GetPos (transform.position);
    }

    void Remove () {
        Level.instance.superblocks.Remove (GetPos ());
    }

    public static void EnsureCreated (Vector3 pos, float radius) {
        int radiusInt = Mathf.CeilToInt (radius / size);
        Vector2Int posInt = GetPos (pos);

        for (int dx = -radiusInt; dx <= radiusInt; dx++) {
            for (int dy = -radiusInt; dy <= radiusInt; dy++) {
                var sbPos = posInt + new Vector2Int (dx, dy);
                if (Level.instance.superblocks.ContainsKey (sbPos)) {
                    continue;
                }
                var block = Instantiate (Level.instance.superBlockPrefab, (Vector3) (Vector2) sbPos * size, Quaternion.identity).GetComponent<SuperBlock> ();
                block.Add ();
            }
        }
    }

    Vector2Int GetMinBlock () {
        return GetPos () * size;
    }

    Vector2Int GetMaxBlock () {
        return GetMinBlock () + new Vector2Int (size - 1, size - 1);
    }

    void Generate () {
        var min = GetMinBlock ();
        var max = GetMaxBlock ();
        var generator = Level.instance.generator;
        for (int x = min.x; x <= max.x; x++) {
            for (int y = min.y; y <= max.y; y++) {
                var pos = new Vector2Int (x, y);
                BlockType blockType = generator.GetBlockTypeAt (pos);
                Level.instance.CreateBlock (blockType, pos);
            }
        }
    }

    void Start () {
        if (transform.position.y >= -0.1f) {
            skyBackground.SetActive (true);
            groundBackground.SetActive (false);
        } else {
            skyBackground.SetActive (false);
            groundBackground.SetActive (true);
        }
        if (generate) {
            Generate ();
        }
    }
}