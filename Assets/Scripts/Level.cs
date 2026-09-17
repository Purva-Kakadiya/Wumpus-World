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
        Vector3 playerTransform = startingCell.GetCellCenter();
        player = Instantiate(playerPrefab, startingCell.transform);
        player.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        player.transform.position = playerTransform - new Vector3(0, 0.2f, 0);
        MakePlayerRotationZero();
    }

    private void MakePlayerRotationZero() {
        Transform currentPlayerCell = player.transform.parent;
        float cellRotation = currentPlayerCell.eulerAngles.z;
        player.transform.rotation = Quaternion.Euler(0f, 0f, cellRotation - cellRotation);
    }

}
