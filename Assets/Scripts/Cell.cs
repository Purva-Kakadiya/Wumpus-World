using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [SerializeField] private Button cellSelectionButton;
    [SerializeField] private float waitingTimerMax = 0.5f;
    [SerializeField] private Transform snappedVisual;
    [SerializeField] private Transform movingCellDoor;
    //[SerializeField] private List<Transform> doorSnappedList = new List<Transform>();

    private PolygonCollider2D polygonCollider;
    private MovementManager movementManager;
    private WaitingTimer waitingTimer;
    private BoxCastManager boxCastManager;
    private RoutingManager routingManager;
    private bool isWaiting = false;
    private bool isSnappedVisualActive = false;


    private Transform lastCellTransform;
    private Transform boxCastOriginPoint;

    private void Awake() {

        polygonCollider = GetComponent<PolygonCollider2D>();
        movementManager = GetComponent<MovementManager>();
        waitingTimer = GetComponent<WaitingTimer>();
        boxCastManager = GetComponent<BoxCastManager>();
        routingManager = GetComponent<RoutingManager>();

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
        int snapPointRotation = movementManager.GetSnapPointRotation(boxCastDirectionNormalized);

        ShowSnappedVisual.Instance.ShowSnapVisual(cellSnapPoint, snapPointRotation, snappedVisual);
        isSnappedVisualActive = true;
        this.boxCastOriginPoint = boxCastOriginPoint;

        //movementManager.GoToPoint(cellSnapPoint, boxCastDirectionNormalized);
        //waitingTimer.WaitFor(waitingTimerMax);
        //isWaiting = true;
    }

    public void SetRoute(Vector3 boxCastDirectionNormalized, Transform boxCastOriginPoint, Cell boxCastingCell) {

        int doorIndex = 2;
        int wantedRotation = movementManager.GetSnapPointRotation(boxCastDirectionNormalized);
        int currentRotation = GetNumInRange((int)transform.eulerAngles.z);

        while (wantedRotation != currentRotation) {
            doorIndex = doorIndex + 1;
            if (doorIndex == 4) {
                doorIndex = 1;
            }
            wantedRotation = GetNumInRange(wantedRotation - 120);
        }

        Transform innerDoor = boxCastManager.GetBoxCastOriginPoint(doorIndex - 1);
        routingManager.SetRoutePair(innerDoor, boxCastOriginPoint);
        boxCastingCell.SetRoutePair(boxCastOriginPoint, innerDoor);
    }

    public void SetRoutePair(Transform boxCastOriginPoint, Transform hitDoor) {
        routingManager.SetRoutePair(boxCastOriginPoint, hitDoor);
    }

    public int GetNumInRange(int num) {
        if(num > 180) {
            num = num - 360;
        }
        if(num <= -180) {
            num = 360 + num;
        }
        return num;
    }

    public int ConvertToAngle(Vector3 directionVector) {
        float angleInDegree = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        return GetNumInRange(Mathf.RoundToInt(angleInDegree));
    }

    public void SetSnapVisualInactive() {
        ShowSnappedVisual.Instance.SetSnapVisualInactive(snappedVisual);
        isSnappedVisualActive = false;
    }

    public void ActivateCellMovement() {
        //if(doorSnappedList.Count != 0) {
        //    doorSnappedList.Clear();
        //}
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

            SetRouting(movingCellDoor, boxCastOriginPoint);

            GameObject castingObject = boxCastOriginPoint.parent.gameObject;
            Cell boxCastingCell = castingObject.GetComponent<Cell>();
            boxCastingCell.SetRouting(boxCastOriginPoint, movingCellDoor);

            //doorSnappedList.Add(movingCellDoor);
        }
        SelectionManager.Instance.SetActiveObject(null);

    }

    private void SetRouting(Transform movingCellDoor, Transform boxCastOriginPoint) {
        routingManager.SetRoutePair(movingCellDoor, boxCastOriginPoint);
    }

    public void CellMovementCanceled() {
        movementManager.MoveCellTo(lastCellTransform);
        SelectionManager.Instance.SetActiveObject(null);
    }

    public bool BoxCastInRouteManager(Transform boxCastOriginPoint) {
        if (routingManager.IsDoorInRoutePair(boxCastOriginPoint)) {
            return true;
        }
        return false;
    }

    public void DeactivateCellMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}