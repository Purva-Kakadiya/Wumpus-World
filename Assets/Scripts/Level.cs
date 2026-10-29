using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour {

    public static Level Instance { get; private set; }

    [SerializeField] private Cell startingCell;
    [SerializeField] private Cell endCell;
    [SerializeField] private Player playerPrefab;
    //[SerializeField] private float playerSpawnDelay = 15f;

    private Player player;
    private Vector3 playerTransform;
    private WaitingTimer waitingTimer;
    private bool buttonClicked = false;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("more that one instance of level found!");
        }
        Instance = this;

        waitingTimer = GetComponent<WaitingTimer>();
    }

    public void LevelSpawned() {
        PlayerSpawn(startingCell);
    }

    public void SetButtonClicked(bool flag) {
        buttonClicked = flag;
    }

    private void PlayerSpawn(Cell spawnCell) {
        Debug.Log("Player has Spawned!");
        player = Instantiate(playerPrefab, spawnCell.transform);
        player.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        player.transform.localPosition = new Vector3(0f, 0f, -0.5f);
        PlayerVisitCell(spawnCell);
    }

    public void PlayerVisitCell(Cell visitingCell) {
        if(Player.Instance == null) {
            Debug.LogError("Player Instance does not exist!");
        }
        visitingCell.SetVisitableCell();
        player.transform.SetParent(visitingCell.transform, true);
        playerTransform = visitingCell.GetCellCenter();
        //player.transform.position = playerTransform - new Vector3(0, 0.2f, 0);

        //Vector3 destination = playerTransform - new Vector3(0, 0.2f, 0);
        Vector3 destination = visitingCell.GetPlayerSnapPoint();
        Vector3 moveDir = (destination - player.transform.position).normalized;
        Cell currentPlayerCell = player.transform.parent.GetComponent<Cell>();
        float distanceBetweenCenterAndBorder = currentPlayerCell.GetDistanceToBorderFromCenter();
        Vector3 jumpPosition = player.transform.position + moveDir * distanceBetweenCenterAndBorder;

        player.StartAnimation(destination, jumpPosition);
        MakePlayerRotationZero();
    }

    private void MakePlayerRotationZero() {
        Transform currentPlayerCell = player.transform.parent;
        float cellRotation = currentPlayerCell.eulerAngles.z;
        player.transform.rotation = Quaternion.Euler(0f, 0f, cellRotation - cellRotation);
    }

#if UNITY_EDITOR
    private bool contextMenuRunning = false;

    [ContextMenu("Connect cells")]

    private void StartConnectingCells() {
        if(contextMenuRunning == true) {
            return;
        }

        contextMenuRunning = true;
        EditorApplication.update += Tick;
        Debug.Log("Connection Started");
    }

    [ContextMenu("Stop Connecting cells")]

    private void StopConnectionCells() {
        if(contextMenuRunning == false) {
            return;
        }

        EditorApplication.update -= Tick;
        contextMenuRunning = false;
        EditorUtility.SetDirty(this);
        Debug.Log("Connection Stopped");
    }

    [ContextMenu("Stop Connecting cells", true)]

    private bool ValidateStop() => contextMenuRunning;

    private void Tick() {
        if(this == null) {
            EditorApplication.update -= Tick;
            return;
        }

        ConnectCells();
    }

#endif

    private void ConnectCells() {
        Cell[] cells = GetComponentsInChildren<Cell>(true);

        foreach (Cell cell in cells) {
            cell.ActivateBoxCast();
        }
    }

}
