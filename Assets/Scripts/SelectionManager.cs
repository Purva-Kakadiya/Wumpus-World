using System;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public static SelectionManager Instance { get; private set; }

    [SerializeField] private PerceptUI perceptUI;

    private GameObject activeObject;

    private void Awake() {
        Instance = this;
    }

    public void SetActiveObject(GameObject activeObject) {
        this.activeObject = activeObject;
    }

    public void SetEveryObjectDeactive() {
        activeObject = null;
    }

    public GameObject GetActiveObject() {
        return activeObject;
    }

}