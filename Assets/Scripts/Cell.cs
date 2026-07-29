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

    public bool IsObjectSelected() {
        if(gameObject == SelectionManager.Instance.GetActiveObject()) {
            return true;
        } else {
            return false;
        }
    }

    private void Start() {
        EditMapOptionsUI.Instance.OnRotateButtonPressed += EditMapOptionsUI_OnRotateButtonPressed;
    }

    private void EditMapOptionsUI_OnRotateButtonPressed(object sender, EventArgs e) {
        movementManager.RotateObject();
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