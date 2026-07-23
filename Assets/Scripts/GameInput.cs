using System;
using UnityEngine;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }

    public EventHandler OnMoveUpActions;
    public EventHandler OnMoveDownActions;
    public EventHandler OnMoveLeftActions;
    public EventHandler OnMoveRightActions;

    private InputSystem_Actions playerInputActions;

    private void Awake() {
        Instance = this;

        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.MoveUp.performed += MoveUp_performed;
        playerInputActions.Player.MoveDown.performed += MoveDown_performed;
        playerInputActions.Player.MoveLeft.performed += MoveLeft_performed;
        playerInputActions.Player.MoveRight.performed += MoveRight_performed;
    }

    private void MoveRight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMoveRightActions?.Invoke(this, EventArgs.Empty);
    }

    private void MoveLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMoveLeftActions?.Invoke(this, EventArgs.Empty);
    }

    private void MoveDown_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMoveDownActions?.Invoke(this, EventArgs.Empty);
    }

    private void MoveUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnMoveUpActions?.Invoke(this, EventArgs.Empty);
    }
}