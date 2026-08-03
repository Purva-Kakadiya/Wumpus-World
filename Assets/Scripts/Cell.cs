using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [SerializeField] private Button cellSelectionButton;
    [SerializeField] private float waitingTimerMax = 0.5f;

    private PolygonCollider2D polygonCollider;
    private MovementManager movementManager;
    private WaitingTimer waitingTimer;
    private BoxCastManager boxCastManager;
    private bool isWaiting = false;

    private void Awake() {

        polygonCollider = GetComponent<PolygonCollider2D>();
        movementManager = GetComponent<MovementManager>();
        waitingTimer = GetComponent<WaitingTimer>();
        boxCastManager = GetComponent<BoxCastManager>();

        cellSelectionButton.onClick.AddListener(() => {
            ActivateCellMovement();
        });
    }

    private void Update() {
        if (isWaiting) {
            movementManager.enabled = false;
            boxCastManager.enabled = false;
        }
    }

    public Transform GetTransform(Cell cell) {
        //Transform cellTransform = movementManager.GetObjectTransform(cell);
        //return cellTransform;

        return cell.transform;
    }

    public bool IsObjectSelected() {
        if(gameObject == SelectionManager.Instance.GetActiveObject()) {
            return true;
        } else {
            return false;
        }
    }

    public void SetIsWaiting(bool waitingFlag) {
        isWaiting = waitingFlag;
        movementManager.enabled = true;
        boxCastManager.enabled = true;
    }

    public PolygonCollider2D GetCollider() {
        return polygonCollider;
    }

    public void SnapAtPoint(Vector3 snapPoint) {
        movementManager.GoToPoint(snapPoint);
        waitingTimer.WaitFor(waitingTimerMax);
        isWaiting = true;
    }

    public void ActivateCellMovement() {
        SelectionManager.Instance.SetActiveObject(gameObject);
    }

    public void DeactivateCellMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}