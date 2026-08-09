using UnityEngine;
using UnityEngine.UI;

public class ShowSnappedVisual : MonoBehaviour {

    public static ShowSnappedVisual Instance { get; private set; }

    [SerializeField] private Transform visualSnapPoint;

    private bool isSnappedVisual = false;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        //if(isSnappedVisual) {
        //    ShowSnapVisual();
        //}
    }

    //public void SetSnapVisualActive(Vector3 snapPoint) {
    //    isSnappedVisual = true;
    //    this.snapPoint = snapPoint;
    //}

    public void ShowSnapVisual(Vector3 snapPoint, float snapPointRotation, Transform snappedVisual) {
        snappedVisual.transform.position = snapPoint;
        snappedVisual.rotation = Quaternion.Euler(0f, 0f, snapPointRotation);
        Debug.Log(snappedVisual.gameObject.activeSelf);

        visualSnapPoint.transform.position = snapPoint;
        visualSnapPoint.transform.rotation = snappedVisual.rotation;
    }

    public Transform GetSnappedVisualTransform() {
        return visualSnapPoint;
    }

    public void SetSnapVisualInactive(Transform snappedVisual) {
        //isSnappedVisual = false;

        snappedVisual.gameObject.SetActive(false);
    }

}