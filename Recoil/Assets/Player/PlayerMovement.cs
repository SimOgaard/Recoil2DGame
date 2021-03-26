using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Movement Forces")]
        [SerializeField]
        private float horizontalSpeedMax = 8.0f;
        [SerializeField]
        private float acceleration = 100.0f;

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vkey);

    private const int LEFT_ARROW_KEY = 0x41;
    private const int RIGHT_ARROW_KEY = 0x44;
 
    private bool rightbt;
    private bool leftbt;
    private float newMovementX;

    private float accelerationFixed;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        accelerationFixed = acceleration;
    }

    private void Update()
    {
        if (!playerController.isPlayingNoMovement)
        {
            if ((GetAsyncKeyState(LEFT_ARROW_KEY) & 0x8000) > 0)
            {
                leftbt = true;
            }
            else
            {
                leftbt = false;
            }

            if ((GetAsyncKeyState(RIGHT_ARROW_KEY) & 0x8000) > 0)
            {
                rightbt = true;
            }
            else
            {
                rightbt = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (playerController.rb.velocity.x > 0 || playerController.rb.velocity.x < 0)
        {
            acceleration = 100;
            playerController.hasMovedFirstLevel = true;
        }
        else
        {
            acceleration = accelerationFixed;
        }

        if (rightbt && !leftbt)
        {
            if (playerController.rb.velocity.x < 0.0f)
            {
                newMovementX = 0.0f;
            }
            else
            {
                if (!playerController.isFacingRight)
                {
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                    playerController.isFacingRight = true;
                }

                newMovementX = (acceleration * Time.deltaTime) + playerController.rb.velocity.x;
            }
        }
        else if (leftbt && !rightbt)
        {
            if (playerController.rb.velocity.x > 0.0f)
            {
                newMovementX = 0.0f;
            }
            else
            {
                if (playerController.isFacingRight)
                {
                    transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                    playerController.isFacingRight = false;
                }

                newMovementX = -(acceleration * Time.deltaTime) + playerController.rb.velocity.x;
            }
        }
        else
        {
            newMovementX = 0.0f;
        }

        //        if (!playerController.shootingMovement)
        //        {
        playerController.rb.velocity = new Vector2(Mathf.Clamp(newMovementX, -horizontalSpeedMax, horizontalSpeedMax) + playerController.shootingMovement.x, playerController.shootingMovement.y == 0.0f ? playerController.rb.velocity.y : playerController.shootingMovement.y);
//        }
//        else
//        {
//            playerController.rb.velocity = new Vector2(Mathf.Clamp(newMovementX, -horizontalSpeedMax, horizontalSpeedMax), playerController.rb.velocity.y);
//        }
    }
}
