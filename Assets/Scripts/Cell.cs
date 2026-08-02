using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [SerializeField] private Button cellSelectionButton;

    private PolygonCollider2D polygonCollider;
    private MovementManager movementManager;

    private void Awake() {

        polygonCollider = GetComponent<PolygonCollider2D>();
        movementManager = GetComponent<MovementManager>();

        cellSelectionButton.onClick.AddListener(() => {
            ActivateCellMovement();
        });
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

    public PolygonCollider2D GetCollider() {
        return polygonCollider;
    }

    public void SnapAtPoint(Vector3 snapPoint) {
        movementManager.GoToPoint(snapPoint);
    }

    public void ActivateCellMovement() {
        SelectionManager.Instance.SetActiveObject(gameObject);
    }

    public void DeactivateCellMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}