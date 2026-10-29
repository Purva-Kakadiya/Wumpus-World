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

    private void Update() {
        if(isMoving) {
            //Debug.Log("Player is moving");
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            float distanceToDestination = Vector3.Distance(transform.localPosition, targetPosition);
            if (distanceToDestination <= playerSnapDistance) {
                isMoving = false;
                //animator.SetBool(IS_MOVING, false);
                transform.localPosition = targetPosition;
                Debug.Log("Player stopped Running!");
                animator.ResetTrigger(START_RUNNING_TRIGGER);
                animator.Play(DEFAULT_ANIMATION);
            }
        }
    }

    public void PlayerMovementAnimation(Vector3 destination, Vector3 jumpPosition) {
        destination = new Vector3(0f, 0f, -0.5f);
        jumpPosition = new Vector3(jumpPosition.x, jumpPosition.y, -0.5f);
        targetPosition = destination;
        isMoving = true;
        hasJumped = false;
        animator.SetTrigger(START_RUNNING_TRIGGER);
        Debug.Log("Player is moving");

        float distanceToJump = Vector3.Distance(transform.localPosition, jumpPosition);
        if(distanceToJump <= distanceBeforeJump && hasJumped == false) {
            hasJumped = true;
            Debug.Log("Player Jumped");
            //animator.SetTrigger(HAS_JUMPED);
        }

    }

}