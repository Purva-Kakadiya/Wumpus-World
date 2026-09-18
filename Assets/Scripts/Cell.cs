 using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [SerializeField] private Button cellSelectionButton;
    [SerializeField] private Transform snappedVisual;
    [SerializeField] private Transform movingCellDoor;
    [SerializeField] private int defaultSnapDoorNumber;
    [SerializeField] private int numberOfDoors;

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
        boxCastManager = GetComponent<BoxCastManager>();

        if (GameModeManager.Instance.GetCurrentGameMode() == GameMode.PlayGame) {
            cellSelectionButton.interactable = false;
        }

        cellSelectionButton.onClick.AddListener(() => {
            if (GameModeManager.Instance.GetCurrentGameMode() == GameMode.EditMap) {
                ActivateCellMovement();
            } else {
                Level gameLevel = transform.parent.GetComponent<Level>();
                if(gameLevel != null) {
                    Debug.Log(gameLevel.name);
                } else {
                    Debug.Log("Level not found!");
                }
            }
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

    public Vector3 GetCellCenter() {
        Vector3 cellCenter = boxCastManager.GetCellCenter();
        return cellCenter;
    }

    public void SetVisitableCell() {
        routingManager.SetRoutePairActive();
    }

    public void SetRouteVisitability(bool isVisitable) {
        cellSelectionButton.interactable = isVisitable;
    }

    public void SnapAtPoint(Vector3 cellSnapPoint, Vector3 boxCastDirectionNormalized, Transform boxCastOriginPoint) {
        int snapPointRotation = movementManager.GetSnapPointRotation(boxCastDirectionNormalized);

        ShowSnappedVisual.Instance.ShowSnapVisual(cellSnapPoint,boxCastDirectionNormalized, snapPointRotation, snappedVisual);
        isSnappedVisualActive = true;
        this.boxCastOriginPoint = boxCastOriginPoint;
    }

    public void SetRoute(Vector3 boxCastDirectionNormalized, Transform boxCastOriginPoint, Cell boxCastingCell) {
        int doorIndex = defaultSnapDoorNumber;
        int wantedRotation = movementManager.GetSnapPointRotation(boxCastDirectionNormalized);
        int currentRotation = GetNumInRange((int)transform.eulerAngles.z);
        int maxLoops = numberOfDoors;
        bool foundOtherDoor = false;

        if (wantedRotation == currentRotation) {
            foundOtherDoor = true;
        }

        while (wantedRotation != currentRotation) {
            doorIndex = doorIndex + 1;
            if (doorIndex > numberOfDoors) {
                doorIndex = 1;
            }
            
            wantedRotation = GetNumInRange(wantedRotation - (360 / numberOfDoors));
            if(wantedRotation == currentRotation) {
                foundOtherDoor = true;
                break;
            }
            if (maxLoops == 0) {
                break;
            }
            maxLoops--;
        }

        if (foundOtherDoor == true) {
            Transform innerDoor = boxCastManager.GetBoxCastOriginPoint(doorIndex - 1);
            Debug.Log("Added boxcast pair " + innerDoor.parent.gameObject.name + "." + innerDoor.name + " and " + boxCastOriginPoint.parent.gameObject.name + "." + boxCastOriginPoint.name);
            SetRouting(innerDoor, boxCastOriginPoint);
            boxCastingCell.SetRouting(boxCastOriginPoint, innerDoor);
        }
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
        routingManager.ResetSnappedDoorList();
    }

    public void RemoveSnapPairWithKey(Transform innerDoor, Transform outerDoor) {
        routingManager.RemoveSnapPair(innerDoor, outerDoor);
    }

    public void CellMovementConfirmed() {
        if(isSnappedVisualActive) {
            //Transform newCellTransform = ShowSnappedVisual.Instance.GetSnappedVisualTransform();
            movementManager.MoveCellTo(snappedVisual);
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

        waitingTimer.WaitForFewSecond(boxCastManager);
        waitingTimer.WaitForFewSecond(polygonCollider);
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