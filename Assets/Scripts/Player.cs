using System;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private GameInput gameInput;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("There are more than one player!");
        }
        Instance = this;
    }

    private void Start() {
        //gameInput.OnMoveUpActions += GameInput_OnMoveUpAction;
    }

    //private void GameInput_OnMoveUpAction(object sender, EventArgs e) {
    //    //transform.SetParent();
    //}

}