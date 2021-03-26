using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Jump Forces")]
        [SerializeField]
        private float jumpForce = 20.0f;

    [Header("Jump Times")]
        [SerializeField]
        private float minJumpTime = 0.075f;

    [Header("Jump Leeway")]
        [SerializeField]
        private float jumpBeforeGroundDelay = 0.16f;
        [SerializeField]
        private float jumpAfterFallingDelay = 0.07f;

    [SerializeField]
    private float maxSpeedY = 40.0f;

    private float timerForJumpBeforeGround = 0.0f;
    private float timerForJumpAfterFalling = 0.0f;
    private float gravityScaleOrigin = 0.0f;

    private bool wantsToJump = false;

    public AudioSource jumpSound;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        gravityScaleOrigin = playerController.rb.gravityScale;
    }

    private void Update()
    {
        if (!playerController.isPlayingNoMovement)
        {
            if (playerController.isGrounded)
            {
                timerForJumpAfterFalling = 0.0f;
            }
            else if (!playerController.isJumping)
            {
                timerForJumpAfterFalling += Time.deltaTime;
            }

            timerForJumpBeforeGround += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timerForJumpBeforeGround = 0.0f;
                wantsToJump = true;
            }
            if (timerForJumpBeforeGround >= jumpBeforeGroundDelay)
            {
                wantsToJump = false;
            }

            if ((playerController.isGrounded && wantsToJump) || ((timerForJumpAfterFalling < jumpAfterFallingDelay) && Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded))
            {
                if (!jumpSound.isPlaying)
                {
                    jumpSound.Play();
                }
                timerForJumpAfterFalling = jumpAfterFallingDelay;
                playerController.isJumping = true;
                playerController.airTime = 0.0f;
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpForce);
            }

            if ((playerController.isJumping && playerController.rb.velocity.y <= 0.0f) || (!Input.GetKey(KeyCode.Space) && playerController.isJumping && (playerController.airTime >= minJumpTime)) || playerController.hitCeiling)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, 0.0f);
                playerController.isJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, Mathf.Clamp(playerController.rb.velocity.y, -maxSpeedY, maxSpeedY));
        jumpSound.transform.position = new Vector3(0, 0, 0);
    }
}
