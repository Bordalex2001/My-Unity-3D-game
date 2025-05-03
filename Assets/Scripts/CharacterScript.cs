using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterScript : MonoBehaviour
{
    private Animator animator;
    private AudioSource walkSound;
    private AudioSource runSound;
    private AudioSource jumpStartSound;
    private AudioSource jumpFinishSound;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -9.81f;
    private MoveStates prevMoveState = MoveStates.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        walkSound = GetComponent<AudioSource>();
        runSound = GetComponents<AudioSource>()[1];
        jumpStartSound = GetComponents<AudioSource>()[2];
        jumpFinishSound = GetComponents<AudioSource>()[3];
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveStates moveState = (MoveStates)animator.GetInteger("MoveState");

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            if (prevMoveState == MoveStates.Jumping)
            {
                moveState = MoveStates.JumpFinish;
            }
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float sprintValue = sprintAction.ReadValue<float>();

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        if (cameraForward != Vector3.zero)
        {
            cameraForward.Normalize();
        }

        Vector3 moveStep = playerSpeed * Time.deltaTime * (1.0f + sprintValue) * (
            moveValue.x * Camera.main.transform.right +
            moveValue.y * cameraForward
        );
        if (moveState != MoveStates.JumpStart &&
            moveState != MoveStates.Jumping &&
            moveState != MoveStates.JumpFinish)
        {
            if (moveStep.magnitude > 0)
            {
                transform.forward = cameraForward;
                moveState = Mathf.Abs(moveValue.x) > Mathf.Abs(moveValue.y) ? (sprintValue > 0 ? MoveStates.SideRun : MoveStates.SideWalk) : (sprintValue > 0 ? MoveStates.Run : MoveStates.Walk);
            }
            else
            {
                moveState = MoveStates.Idle;
            }
        }
        characterController.Move(moveStep);

        // Makes the player jump
        if (jumpAction.ReadValue<float>() > 0 && groundedPlayer)
        {
            moveState = MoveStates.JumpStart;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        if (moveState != prevMoveState)
        {
            animator.SetInteger("MoveState", (int)moveState);
            prevMoveState = moveState;

            if (walkSound.isPlaying) walkSound.Stop();
            if (runSound.isPlaying) runSound.Stop();
            if (jumpStartSound.isPlaying) jumpStartSound.Stop();
            if (jumpFinishSound.isPlaying) jumpFinishSound.Stop();

            switch (moveState)
            {
                case MoveStates.Walk:
                case MoveStates.SideWalk:
                    walkSound.Play();
                    break;

                case MoveStates.Run:
                case MoveStates.SideRun:
                    runSound.Play();
                    break;

                case MoveStates.JumpStart:
                    jumpStartSound.Play();
                    break;

                case MoveStates.JumpFinish:
                    jumpFinishSound.Play();
                    break;
            }
        }
    }

    private void OnJumpStartAnimationEnds()
    {
        animator.SetInteger("MoveState", (int)MoveStates.Jumping);
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
    }
    private void OnJumpFinishAnimationEnds()
    {
        animator.SetInteger("MoveState", (int)MoveStates.Idle);
    }
}

enum MoveStates
{
    Idle = 1,
    Walk = 2,
    SideWalk = 3,
    Run = 4,
    SideRun = 5,
    JumpStart = 6,
    Jumping = 7,
    JumpFinish = 8
}