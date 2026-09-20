using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float distanceBeforeJump = 1f;
    [SerializeField] private Animator animator;

    private const string IS_MOVING = "isMoving";
    private const string HAS_JUMPED = "hasJumped";

    private bool isMoving = false;
    private bool hasJumped = false;
    private Vector3 targetPosition;

    private void Update() {
        if(isMoving) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    public void PlayerMovementAnimation(Vector3 destination, Vector3 jumpPosition) {
        targetPosition = destination;
        isMoving = true;
        hasJumped = false;
        //animator.SetBool(IS_MOVING, true);

        float distanceToJump = Vector3.Distance(transform.position, jumpPosition);
        if(distanceToJump <= distanceBeforeJump && hasJumped == false) {
            hasJumped = true;
            //animator.SetTrigger(HAS_JUMPED);
        }

        float distanceToDestination = Vector3.Distance(transform.position, destination);
        if(distanceToDestination <= 0.1f) {
            isMoving = false;
            //animator.SetBool(IS_MOVING, false);
            transform.position = destination;
            Debug.Log("Player stopped Running!");
        }
    }

}