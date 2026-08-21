using System.Collections;
using UnityEngine;

public class WaitingTimer : MonoBehaviour {

    private float timer;
    private Cell cell;
    private bool isWaiting = false;

    private void Awake() {
        cell = GetComponent<Cell>();
    }

    public void WaitForFewSecond(MonoBehaviour scriptName) {
        StartCoroutine(DisableScript(scriptName));
    }

    IEnumerator DisableScript(MonoBehaviour scriptName) {
        scriptName.enabled = false;

        yield return new WaitForSeconds(2f);
        scriptName.enabled = true;
    }

}