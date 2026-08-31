using UnityEngine;
using UnityEngine.UI;

public class ShowSnappedVisual : MonoBehaviour {

    public static ShowSnappedVisual Instance { get; private set; }

    [SerializeField] private Transform visualSnapPoint;
    [SerializeField] private float snapPointDistanceFromCenter;

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

    public void ShowSnapVisual(Vector3 snapPoint,Vector3 boxCastDirectionNormalized, float snapPointRotation, Transform snappedVisual) {
        Vector3 snappedVisualPoint = snapPoint + (boxCastDirectionNormalized * snapPointDistanceFromCenter);
        snappedVisual.transform.position = snappedVisualPoint;
        snappedVisual.rotation = Quaternion.Euler(0f, 0f, snapPointRotation);
        snappedVisual.gameObject.SetActive(true);
    }

    public Transform GetSnappedVisualTransform() {
        return visualSnapPoint;
    }

    public void SetSnapVisualInactive(Transform snappedVisual) {
        //isSnappedVisual = false;

        snappedVisual.gameObject.SetActive(false);
    }

}