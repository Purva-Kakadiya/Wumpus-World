using System;
using System.Collections;
using UnityEngine;

public class WaitingTimer : MonoBehaviour {

    private float timer;
    private Cell cell;
    private bool isWaiting = false;

    private void Awake() {
        cell = GetComponent<Cell>();
    }

    public void WaitForFewSecond(Behaviour scriptName, float delay, Action onComplete = null) {
        StartCoroutine(DisableScript(scriptName, delay, onComplete));
    }

    IEnumerator DisableScript(Behaviour scriptName, float delay, Action onComplete) {
        scriptName.enabled = false;

        yield return new WaitForSeconds(delay);
        scriptName.enabled = true;
        onComplete?.Invoke();
    }

}