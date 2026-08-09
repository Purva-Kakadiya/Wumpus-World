using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [SerializeField] private Button cellSelectionButton;
    [SerializeField] private float waitingTimerMax = 0.5f;
    [SerializeField] private Transform snappedVisual;
    [SerializeField] private Transform movingCellDoor;

    private PolygonCollider2D polygonCollider;
    private MovementManager movementManager;
    private WaitingTimer waitingTimer;
    private BoxCastManager boxCastManager;
    private bool isWaiting = false;
    private bool isSnappedVisualActive = false;

    private Transform lastCellTransform;
    private Transform boxCastOriginPoint;

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

        //if(isSnappedVisualActive) {
        //    if (boxCastManager.IsSnappingActive() == false) {
        //        ShowSnappedVisual.Instance.SetSnapVisualInactive(snappedVisual);
        //        isSnappedVisualActive = false;
        //    }
        //}
    }

    //public bool IsSnappingVisualActive() {
    //    return isSnappedVisualActive;
    //}

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

    public void SnapAtPoint(Vector3 cellSnapPoint, Vector3 boxCastDirectionNormalized, Transform boxCastOriginPoint) {
        float snapPointRotation = movementManager.GetSnapPointRotation(boxCastDirectionNormalized);

        Debug.Log("snapping at: " + cellSnapPoint + " with rotation: " + snapPointRotation);
        ShowSnappedVisual.Instance.ShowSnapVisual(cellSnapPoint, snapPointRotation, snappedVisual);
        isSnappedVisualActive = true;
        this.boxCastOriginPoint = boxCastOriginPoint;

        //movementManager.GoToPoint(cellSnapPoint, boxCastDirectionNormalized);
        //waitingTimer.WaitFor(waitingTimerMax);
        //isWaiting = true;
    }

    public void SetSnapVisualInactive() {
        ShowSnappedVisual.Instance.SetSnapVisualInactive(snappedVisual);
        isSnappedVisualActive = false;
    }

    public void ActivateCellMovement() {
        lastCellTransform = gameObject.transform;
        SelectionManager.Instance.SetActiveObject(gameObject);
        EditMapOptionsUI.Instance.SetConfirmPanelActive(this);
    }

    public void CellMovementConfirmed() {
        if(isSnappedVisualActive) {
            Transform newCellTransform = ShowSnappedVisual.Instance.GetSnappedVisualTransform();
            movementManager.MoveCellTo(newCellTransform);
            ShowSnappedVisual.Instance.SetSnapVisualInactive(snappedVisual);
            isSnappedVisualActive = false;

            RoutingManager.Instance.SetRountePair(movingCellDoor, boxCastOriginPoint);
        }
        SelectionManager.Instance.SetActiveObject(null);

    }

    public void CellMovementCanceled() {
        movementManager.MoveCellTo(lastCellTransform);
        SelectionManager.Instance.SetActiveObject(null);
    }

    public bool BoxCastInRouteManager(Transform boxCastOriginPoint) {
        if (RoutingManager.Instance.IsDoorInRoutePair(boxCastOriginPoint)) {
            return true;
        }
        return false;
    }

    public void DeactivateCellMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}