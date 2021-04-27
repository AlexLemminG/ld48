using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartDialog : MonoBehaviour {
    public List<string> dialog;
    public int moveScrollIdx = 0;
    public TMP_Text mesh;
    public GameObject player;
    public float scrollDelay = 3f;
    public GameObject scroll;
    TextPrinter printer;

    IEnumerator Start () {
        for (int i = 0; i < dialog.Count; i++) {
            printer = new TextPrinter (mesh, dialog[i]);
            printer.timePerChar *= 1.5f;
            if (i == dialog.Count - 1) {
                printer.timePerChar *= 10f;
            }
            SoundManager.instance.Play (SoundManager.instance.sayClips);
            MovePlayer ();
            float startTime = Time.unscaledTime;
            bool movedScroll = false;
            while (!printer.IsFinished ()) {
                printer.Update (Time.unscaledDeltaTime);
                if (i == moveScrollIdx && !movedScroll && Time.unscaledTime - startTime > scrollDelay) {
                    movedScroll = true;
                    MoveScroll ();
                }
                yield return null;
            }
            yield return new WaitForSeconds (1.5f);
        }
        yield return new WaitForSeconds (1f);
        Session.instance.NewGame ();
    }

    public float dur = 1f;
    public float str = 10f;
    public int vib = 10;
    [EditorButton]
    void MoveScroll () {
        //scroll.transform.DOShakeRotation (dur, new Vector3 (0f, 0f, str), vib);
        scroll.transform.DOPunchRotation (new Vector3 (0f, 0f, str), dur, vib);
    }

    public float dur2 = 1f;
    public float str2 = 10f;
    public int vib2 = 10;
    [EditorButton]
    void MovePlayer () {
        player.transform.DOPunchRotation (new Vector3 (0f, 0f, str2), dur2, vib2);
        //player.transform.DOShakeRotation (dur2, new Vector3 (0f, 0f, str2), vib2);
    }
}