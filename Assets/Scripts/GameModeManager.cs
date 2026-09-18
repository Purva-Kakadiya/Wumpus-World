using System;
using UnityEngine;
using UnityEngine.Rendering;

public enum GameMode {
    EditMap,
    PlayGame
}

public class GameModeManager : MonoBehaviour {

    public static GameModeManager Instance { get; private set; }

    [SerializeField] private GameMode currentGameMode = GameMode.EditMap;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("More than one instance of GameModeManager Found!");
        }
        Instance = this;
    }

    public GameMode GetCurrentGameMode() {
        return currentGameMode;
    }

    public void SetGameMode(GameMode newGameMode) {
        if(currentGameMode != newGameMode) {
            currentGameMode = newGameMode;
        }
    }

}