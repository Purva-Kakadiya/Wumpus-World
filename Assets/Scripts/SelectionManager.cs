using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public static SelectionManager Instance { get; private set; }

    private GameObject activeObject;

    private void Awake() {
        Instance = this;
    }

    public void SetActiveObject(GameObject activeObject) {
        this.activeObject = activeObject;
    }

    public void DeactivateObject(GameObject deactivingObject) {
        if(activeObject == deactivingObject) {
            activeObject = null;
        }
    }

    public void SetEveryObjectDeactive() {
        activeObject = null;
    }

    public GameObject GetActiveObject() {
        return activeObject;
    }

}