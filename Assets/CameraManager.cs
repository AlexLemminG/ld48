using UnityEngine;
using UnityEngine.U2D;

public class CameraManager : Singleton<CameraManager>, ISceneSingleton {
    public Vector2 GetMousePosition () {
        return GetComponent<Camera> ().ScreenToWorldPoint (Input.mousePosition);
    }

    public float offsetY;
    public float maxDistanceX;
    public float maxDistanceY;

    void Update () {
        var player = Player.instance;
        if (!player) {
            return;
        }

        var currentPos = (Vector2) transform.position;
        var targetPos = (Vector2) player.transform.position;
        float lowMult = Mathf.Lerp (3f, 1f, Mathf.InverseLerp (0f, -5f, targetPos.y));
        targetPos.y += offsetY;

        currentPos.x = Mathf.Clamp (currentPos.x, -maxDistanceX + targetPos.x, targetPos.x + maxDistanceX);
        currentPos.y = Mathf.Clamp (currentPos.y, -maxDistanceY * lowMult + targetPos.y, targetPos.y + maxDistanceY * lowMult);

        transform.position = new Vector3 (currentPos.x, currentPos.y, -10f);

        GetComponent<PixelPerfectCamera> ().assetsPPU = ScrollBonusManager.instance.viewDistance.GetVar ();
    }
}