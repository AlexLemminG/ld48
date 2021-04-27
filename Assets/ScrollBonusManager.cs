using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollBonusManager : Singleton<ScrollBonusManager> {
    [System.Serializable]
    public class UpgradableVar {
        [System.Serializable]
        public class Level {
            public int value;
            public string message;
            public string quoteOverride;
            public int minDepth;
        }

        public List<Level> levels;

        int currentLevel = 0;
        public bool IsLastLevel () {
            return currentLevel == levels.Count - 1;
        }
        public bool CanUpgrade (int depth) {
            return !IsLastLevel () && levels[currentLevel + 1].minDepth <= depth;
        }
        public void Upgrade (int depth) {
            Debug.Assert (CanUpgrade (depth));
            currentLevel++;
        }
        public string GetMessage () {
            return levels[currentLevel].message;
        }
        public string GetQuoteOverride () {
            return levels[currentLevel].quoteOverride;
        }
        public int GetVar () {
            return levels[currentLevel].value;
        }
    }

    internal int GetTotalScrollsCount () {
        return totalCount;
    }

    internal int GetPickedScrollsCount () {
        return pickedCount;
    }

    public UpgradableVar viewDistance;
    public UpgradableVar initialShovels;
    public UpgradableVar initialDynamites;
    public UpgradableVar initialStoners;
    public UpgradableVar doubleJump;

    int pickedCount = 0;
    int totalCount = 0;

    List<UpgradableVar> allVars;

    protected override void OnAwake () {
        allVars = new List<UpgradableVar> () {
            viewDistance,
            initialShovels,
            initialDynamites,
            initialStoners,
            doubleJump
        };
        totalCount = 1;
        foreach (var v in allVars) {
            totalCount += v.levels.Count - 1;
        }
    }
    public string notDeepEnoughMessage = "not deep enough for upgrade";

    public void UpgradeRandom (Vector2Int pos) {
        int depth = -pos.y;
        var upgradables = allVars.Where (x => x.CanUpgrade (depth)).ToList ();
        bool noUpgradesLeft = allVars.Where (x => !x.IsLastLevel ()).Count () == 0;

        if (noUpgradesLeft) {
            UI.instance.scrollPanel.ShowFinal ();
            return;
        } else {
            if (upgradables.Count == 0) {
                UI.instance.scrollPanel.Show (notDeepEnoughMessage, notDeepEnough : true);
            } else {
                pickedCount++;
                var upgrade = upgradables[Random.Range (0, upgradables.Count)];
                upgrade.Upgrade (depth);
                UI.instance.scrollPanel.Show (upgrade.GetMessage (), quoteOverride : upgrade.GetQuoteOverride ());
            }
        }
    }
}