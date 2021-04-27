using UnityEngine;

public abstract class Bonus : MonoBehaviour {
    bool isPicked = false;
    protected abstract void OnPickedUp (Player player);
    public void Pickup (Player player) {
        if (isPicked) {
            return;
        }
        isPicked = true;
        OnPickedUp (player);
        Destroy (gameObject);
    }
}