using UnityEngine;
using UnityEngine.UI;

public class LevelSpawner : MonoBehaviour {

    [SerializeField] private Transform levelSpawnLocation;
    [SerializeField] private Button level1Button;
    [SerializeField] private Level level1;

    private void Awake() {
        level1Button.onClick.AddListener(() => {
            Level spawnLevel = Instantiate(level1, levelSpawnLocation);
            spawnLevel.OnLevelLoad();
            this.gameObject.SetActive(false);
        });
    }

}