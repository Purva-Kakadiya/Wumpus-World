using UnityEngine;
using UnityEngine.UI;

public class RotationManager : MonoBehaviour {

    [SerializeField] private Vector3 rotationAngle = new Vector3(0f, 0f, 10f);
    [SerializeField] private Image cellImage;

    private Cell cell;
    private bool rotationRequested = false;
    //private PolygonCollider2D polygonCollider;

    private void Awake() {
        cell = GetComponent<Cell>();
        //polygonCollider = cellImage.GetComponent<PolygonCollider2D>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R) && cell.IsObjectSelected()) {
            rotationRequested = true;
        }
    }

    private void FixedUpdate() {
        if (rotationRequested) {
            //transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z + 10f);
            transform.Rotate(rotationAngle);
            rotationRequested = false;
        }
    }

    //public PolygonCollider2D GetPolygonCollider() {
    //    return polygonCollider;
    //}
}