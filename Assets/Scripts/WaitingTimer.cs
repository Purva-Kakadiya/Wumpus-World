using UnityEngine;

public class WaitingTimer : MonoBehaviour {

    private float timer;
    private Cell cell;
    private bool isWaiting = false;

    private void Awake() {
        cell = GetComponent<Cell>();
    }

    private void Update() {
        if(timer > 0f) {
            timer -= Time.deltaTime;
        } else {
            timer = 0f;
        }

        if(isWaiting && timer <= 0f) {
            cell.SetIsWaiting(false);
            isWaiting = false;
        }
    }

    public void WaitFor(float timerMax) {
        timer = timerMax;
        isWaiting = true;
    }

}