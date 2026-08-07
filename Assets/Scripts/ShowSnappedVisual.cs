using UnityEngine;
using UnityEngine.UI;

public class ShowSnappedVisual : MonoBehaviour {

    public static ShowSnappedVisual Instance { get; private set; }

    private Vector3 snapPoint;
    private bool isSnappedVisual = false;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        //if(isSnappedVisual) {
        //    ShowSnapVisual();
        //}
    }

    public void SetSnapVisualActive(Vector3 snapPoint) {
        isSnappedVisual = true;
        this.snapPoint = snapPoint;
    }

    public void ShowSnapVisual(Vector3 snapPoint, float snapPointRotation, Transform snappedVisual) {
        Debug.Log("Show snap Visual at: " + snapPoint);
        snappedVisual.transform.position = snapPoint;
        snappedVisual.rotation = Quaternion.Euler(0f, 0f, snapPointRotation);
        snappedVisual.gameObject.SetActive(true);
    }

    public void SetSnapVisualInactive(Transform snappedVisual) {
        //isSnappedVisual = false;

        snappedVisual.gameObject.SetActive(false);
    }

}