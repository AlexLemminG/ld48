using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelBonus : Bonus {
    public int amount = 2;

    protected override void OnPickedUp (Player player) {
        player.shovelsCount += amount;
        UI.instance.shovelPanel.PunchScale ();
        SoundManager.instance.Play (SoundManager.instance.getSmallBonusClips);
    }
}