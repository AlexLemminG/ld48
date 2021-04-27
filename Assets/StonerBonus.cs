using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonerBonus : Bonus {
    public int amount = 2;

    protected override void OnPickedUp (Player player) {
        player.stonersCount += amount;
        UI.instance.stonerPanel.PunchScale ();
        SoundManager.instance.Play (SoundManager.instance.getSmallBonusClips);
    }
}