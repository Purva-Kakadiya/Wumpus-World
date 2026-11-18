using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    private Cell parentCell;
    private PlayerAnimation playerAnimation;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("There are more than one player!");
        }
        Instance = this;

        playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Start() {
        //gameInput.OnMoveUpActions += GameInput_OnMoveUpAction;
    }

    private void Update() {
        parentCell = transform.parent.GetComponent<Cell>();
    }

    public Cell GetCurrentlyVisitedCell() {
        return parentCell;
    }

    public void SetCurrentCellUnvisitable() {
        Cell parentCell = transform.parent.GetComponent<Cell>();
        parentCell.SetCellUnvisitable();
    }

    public void StartAnimation(Vector3 startPosition, float jumpDistance) {
        playerAnimation.PlayerMovementAnimation(startPosition, jumpDistance);
    }

    //private void GameInput_OnMoveUpAction(object sender, EventArgs e) {
    //    //transform.SetParent();
    //}

}