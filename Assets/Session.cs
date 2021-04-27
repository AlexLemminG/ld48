using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : Singleton<Session> {
    [EditorButton]
    public void NewGame () {
        SceneManager.LoadScene ("Game");
    }
    int lowestDepth = 0;
    public int GetLowestDepth () {
        return lowestDepth;
    }

    public void UpdateLowestDepth (int currentDepth) {
        lowestDepth = Mathf.Max (currentDepth, lowestDepth);
    }
}