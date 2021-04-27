using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBonus : Bonus {
    protected override void OnPickedUp (Player player) {
        ScrollBonusManager.instance.UpgradeRandom (Block.GetPos (transform.position));
        SoundManager.instance.Play (SoundManager.instance.getScrollClips);
    }
}