using DG.Tweening;
using UnityEngine;

public class ItemPanel : MonoBehaviour {
    public TMPro.TMP_Text text;
    public GameObject shakeRoot;
    public GameObject scaleRoot;

    Vector3 originalShakePos;
    Vector3 originalScaleScale;

    bool isInit = false;
    void Init () {
        if (isInit) {
            return;
        }
        isInit = true;
        originalScaleScale = scaleRoot.transform.localScale;
        originalShakePos = shakeRoot.transform.localPosition;
    }

    public void SetAmount (int amount, int maxAmount) {
        text.text = amount + "/" + maxAmount;
        text.color = Color.white;
    }
    public void SetAmount (int amount) {
        text.text = amount + "";
        text.color = amount == 0 ? Color.red : Color.white;
    }

    public void Shake () {
        Init ();
        shakeRoot.transform.localPosition = originalShakePos;
        shakeRoot.transform.DOKill ();
        shakeRoot.transform.DOShakePosition (0.2f, 10f, 100);
    }

    public void PunchScale () {
        Init ();
        scaleRoot.transform.localScale = originalScaleScale;
        scaleRoot.transform.DOKill ();
        scaleRoot.transform.DOPunchScale (Vector3.one * 0.5f, 0.2f, 2);
    }

    public override int GetHashCode () {
        return base.GetHashCode ();
    }

    public override bool Equals (object other) {
        return base.Equals (other);
    }

    public override string ToString () {
        return base.ToString ();
    }
}