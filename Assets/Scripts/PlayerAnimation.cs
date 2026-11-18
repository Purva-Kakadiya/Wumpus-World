using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float playerSnapDistance = 0.1f;
    [SerializeField] private float distanceBeforeJump = 0.2f;
    [SerializeField] private Animator animator;

    private const string START_RUNNING_TRIGGER = "startRunningTrigger";
    private const string DEFAULT_ANIMATION = "PlayerIdleAnimation";
    //private const string IS_MOVING = "isMoving";
    //private const string HAS_JUMPED = "hasJumped";

    private bool isMoving = false;
    private bool hasJumped = false;
    private Vector3 targetPosition;
    private float distanceToJump;
    private float distanceMoved = 0f;

    private void Update() {
        if(isMoving) {
            //Debug.Log("Player is moving");
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            float distanceToDestination = Vector3.Distance(transform.localPosition, targetPosition);
            if (distanceToDestination <= playerSnapDistance) {
                isMoving = false;
                hasJumped = false;
                distanceMoved = 0;
                //animator.SetBool(IS_MOVING, false);
                transform.localPosition = targetPosition;
                animator.ResetTrigger(START_RUNNING_TRIGGER);
                animator.Play(DEFAULT_ANIMATION);
            }

            distanceMoved += moveSpeed * Time.deltaTime;
            //float distanceToJump = Vector3.Distance(transform.localPosition, this.distanceToJump);
            if (Mathf.Abs(distanceMoved - distanceToJump) <= distanceBeforeJump && hasJumped == false) {
                hasJumped = true;
                Debug.Log("Player Jumped");
                //animator.SetTrigger(HAS_JUMPED);
            }
            //Debug.Log(Mathf.Abs(distanceMoved - distanceToJump));
        }
    }

    public void PlayerMovementAnimation(Vector3 destination, float jumpDistance) {
        destination = new Vector3(0f, 0f, -0.5f);
        targetPosition = destination;
        distanceToJump = jumpDistance;
        Debug.Log(distanceToJump);
        isMoving = true;
        hasJumped = false;
        animator.SetTrigger(START_RUNNING_TRIGGER);
    }

}