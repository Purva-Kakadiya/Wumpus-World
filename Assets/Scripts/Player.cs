using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    private Cell parentCell;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("There are more than one player!");
        }
        Instance = this;
    }

    private void Start() {
        //gameInput.OnMoveUpActions += GameInput_OnMoveUpAction;
    }

    private void Update() {
        parentCell = transform.parent.GetComponent<Cell>();
    }

    public void SetCurrentCellUnvisitable() {
        Cell parentCell = transform.parent.GetComponent<Cell>();
        parentCell.SetCellUnvisitable();
    }

    //private void GameInput_OnMoveUpAction(object sender, EventArgs e) {
    //    //transform.SetParent();
    //}

}