using System.Collections.Generic;
using UnityEngine;

public class Level : Singleton<Level>, ISceneSingleton {
    public Dictionary<Vector2Int, Block> blocks = new Dictionary<Vector2Int, Block> ();
    public Dictionary<Vector2Int, SuperBlock> superblocks = new Dictionary<Vector2Int, SuperBlock> ();
    public Generator generator;

    protected override void OnAwake () {
        generator = new Generator ();
    }

    public GameObject prefabSand;
    public GameObject prefabStone;
    public GameObject prefabSlimeEnemy;
    public GameObject prefabSurikenEnemy;
    public GameObject prefabScrollBonus;
    public GameObject prefabDynamiteBonus;
    public GameObject prefabStonerBonus;
    public GameObject prefabShovelBonus;

    public GameObject dynamitePrefab;
    public GameObject superBlockPrefab;

    public void CreateBlock (BlockType type, Vector2Int pos) {
        GameObject prefab = null;
        switch (type) {
            case BlockType.none:
                return;
            case BlockType.sand:
                prefab = prefabSand;
                break;
            case BlockType.stone:
                prefab = prefabStone;
                break;
            case BlockType.slimeEnemy:
                prefab = prefabSlimeEnemy;
                break;
            case BlockType.surikenEnemy:
                prefab = prefabSurikenEnemy;
                break;
            case BlockType.scrollBonus:
                prefab = prefabScrollBonus;
                break;
            case BlockType.dynamiteBonus:
                prefab = prefabDynamiteBonus;
                break;
            case BlockType.stonerBonus:
                prefab = prefabStonerBonus;
                break;
            case BlockType.shovelBonus:
                prefab = prefabShovelBonus;
                break;
        }
        Instantiate (prefab, (Vector3) (Vector2) pos, Quaternion.identity);
    }
}