using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MathCalaculation : MonoBehaviour {

    [SerializeField] private TMP_InputField inputFieldPoint1x;
    [SerializeField] private TMP_InputField inputFieldPoint1y;
    [SerializeField] private TMP_InputField inputFieldPoint2x;
    [SerializeField] private TMP_InputField inputFieldPoint2y;
    [SerializeField] private TextMeshProUGUI angleText;
    [SerializeField] private Button confirmButton;

    private Vector2 point1;
    private Vector2 point2;

    private void Awake() {
        confirmButton.onClick.AddListener(() => {
            OnConfirmClick();
        });
    }

    private void OnConfirmClick() {
        float.TryParse(inputFieldPoint1x.text, out point1.x);
        float.TryParse(inputFieldPoint1y.text, out point1.y);
        float.TryParse(inputFieldPoint2x.text, out point2.x);
        float.TryParse(inputFieldPoint2y.text, out point2.y);

        Vector2 direction = point1 - point2;
        float angleInRadius = Mathf.Atan2(direction.y, direction.x);
        float angleInDegree = angleInRadius * Mathf.Rad2Deg;
        angleText.text = angleInDegree.ToString();
    }

}