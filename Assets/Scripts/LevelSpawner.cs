using UnityEngine;
using UnityEngine.UI;

public class LevelSpawner : MonoBehaviour {

    [SerializeField] private Transform levelSpawnLocation;
    [SerializeField] private Button level1Button;
    [SerializeField] private Level level1;

    private GameModeManager gameModeManager;

    private void Awake() {
        Transform gameModeManagerTransform = GameObject.Find("GameModeManager").transform;
        gameModeManager = gameModeManagerTransform.GetComponent<GameModeManager>();

        level1Button.onClick.AddListener(() => {
            gameModeManager.SetGameMode(GameMode.PlayGame);

            Level spawnLevel = Instantiate(level1, levelSpawnLocation);
            spawnLevel.LevelSpawned();
            level1Button.gameObject.SetActive(false);
        });
    }

}