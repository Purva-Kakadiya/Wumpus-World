using UnityEngine;

public class Level : MonoBehaviour {

    public static Level Instance { get; private set; }

    [SerializeField] private Cell startingCell;
    [SerializeField] private Cell endCell;
    [SerializeField] private Player playerPrefab;

    private Player player;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("more that one instance of level found!");
        }
        Instance = this;
    }

    public void OnLevelLoad() {
        player = Instantiate(playerPrefab, startingCell.transform);
        MakePlayerRotationZero();
    }

    private void MakePlayerRotationZero() {
        Transform currentPlayerCell = player.transform.parent;
        float cellRotation = currentPlayerCell.eulerAngles.z;
        player.transform.rotation = Quaternion.Euler(0f, 0f, cellRotation - cellRotation);
    }

}
