using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private Button cellSelectionButton;
    [SerializeField] private float snapTimerMax = 3f;

    private bool moveRequested = false;
    private PolygonCollider2D polygonCollider;
    private BoxCastAndSnappingManager boxCastAndSnappingManager;
    private Vector3 moveDir;
    private float snappingTimer;

    private void Awake() {

        polygonCollider = GetComponent<PolygonCollider2D>();
        boxCastAndSnappingManager = GetComponent<BoxCastAndSnappingManager>();

        cellSelectionButton.onClick.AddListener(() => {
            ActivateCellMovement();
        });
    }

    private void Start() {
        EditMapOptionsUI.Instance.OnRotateButtonPressed += EditMapOptionsUI_OnRotateButtonPressed;
    }

    private void EditMapOptionsUI_OnRotateButtonPressed(object sender, EventArgs e) {
        if (SelectionManager.Instance.GetActiveObject() != null) {
            Cell activeCell = (SelectionManager.Instance.GetActiveObject()).GetComponent<Cell>();
            float activeRotationOnZAxis = activeCell.transform.eulerAngles.z;
            activeCell.transform.rotation = Quaternion.Euler(0f, 0f, activeRotationOnZAxis + 10f);
        }
    }

    public PolygonCollider2D GetCollider() {
        return polygonCollider;
    }

    private void Update() {

        if (SelectionManager.Instance.GetActiveObject() == gameObject) {
            CellMovementHandler();
        } else {
            moveRequested = false;
            ActivateBoxCast();
        }

    }

    private void CellMovementHandler() {

        moveDir = new Vector3();
        if (Input.GetKey(KeyCode.W)) {
            moveDir = Vector3.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDir = Vector3.down;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDir = Vector3.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDir = Vector3.right;
        }

        if (moveDir != Vector3.zero) {
            moveRequested = true;
        }
    }

    private void FixedUpdate() {
        if (moveRequested) {
            transform.position += moveDir * moveDistance;
            moveRequested = false;
        }

        if(snappingTimer > 0f) {
            snappingTimer -= Time.fixedDeltaTime;
        } else {
            snappingTimer = 0f;
        }
    }

    private void ActivateBoxCast() {

        for (int pathIndex = 0; pathIndex < polygonCollider.pathCount; pathIndex++) {
            Vector2[] pathPoints = polygonCollider.GetPath(pathIndex);
            int pointsCount = pathPoints.Length;

            for (int i = 0; i < pointsCount; i++) {
                Vector2 currentGlobalPoint = polygonCollider.transform.TransformPoint(pathPoints[i]);
                Vector2 nextGlobalPoint = polygonCollider.transform.TransformPoint(pathPoints[(i + 1) % pointsCount]);
                Vector2 edgeMiddlePoint = (currentGlobalPoint + nextGlobalPoint) / 2;
                Vector2 boxCastDirection = (edgeMiddlePoint - (Vector2)polygonCollider.bounds.center).normalized;

                boxCastAndSnappingManager.ActivateBoxCastAndSnapping(edgeMiddlePoint, boxCastDirection, polygonCollider);
            }
        }
    }

    public void SnapToThePointAndDeactivateObject(Vector2 snapPoint) {
        if (snappingTimer <= 0f) {
            transform.position = snapPoint;
            snappingTimer = snapTimerMax;
            SelectionManager.Instance.DeactivateObject(gameObject);
        }
    }

    public void ActivateCellMovement() {
        SelectionManager.Instance.SetActiveObject(gameObject);
    }

    public void DeactivateCellMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}