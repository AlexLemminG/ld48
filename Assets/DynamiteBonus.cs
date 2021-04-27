using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBonus : Bonus {
    public int amount = 2;

    protected override void OnPickedUp (Player player) {
        player.dynamiteCount += amount;
        UI.instance.dynamitePanel.PunchScale ();
        SoundManager.instance.Play (SoundManager.instance.getSmallBonusClips);
    }
}