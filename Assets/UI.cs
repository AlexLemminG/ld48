using TMPro;
using UnityEngine;

public class UI : Singleton<UI>, ISceneSingleton {
    public ScrollPanel scrollPanel;

    public ItemPanel scrollsCountPanel;
    public ItemPanel dynamitePanel;
    public ItemPanel shovelPanel;
    public ItemPanel stonerPanel;
    public ItemPanel healthPanel;

    public TMP_Text depthMesh;

    public GameObject restartObject;

    void Update () {
        var player = Player.instance;
        if (player == null) {
            return;
        }
        scrollsCountPanel.SetAmount (ScrollBonusManager.instance.GetPickedScrollsCount (), ScrollBonusManager.instance.GetTotalScrollsCount ());
        dynamitePanel.SetAmount (player.dynamiteCount);
        shovelPanel.SetAmount (player.shovelsCount);
        stonerPanel.SetAmount (player.stonersCount);
        healthPanel.SetAmount (player.health);

        depthMesh.text = player.GetMaxDepth () + "m";

        if (player.shovelsCount == 0) {
            restartObject.SetActive (true);
            if (Input.GetButtonDown ("Restart")) {
                Session.instance.NewGame ();
            }
        } else {
            restartObject.SetActive (false);
        }
    }
}