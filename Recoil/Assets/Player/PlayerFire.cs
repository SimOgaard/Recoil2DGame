using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject gun;

    private Vector3 v3Pos;
    private Vector3 heading;
    private float rotation;

    [SerializeField]
    private float offset = 0;

    [SerializeField]
    private float shootForceY = 12f;
    [SerializeField]
    private float shootForceX = 10f;
    [SerializeField]
    private float maxForceX = 24f;

    [SerializeField]
    private float shootMovementTimeOrigin = 0.3f;
    private float shootMovementTime = 0.0f;

    [SerializeField]
    private float timeBetweenShotsOrigin = 0.05f;
    private float timeBetweenShots = 0.0f;

    public GameObject projectile;
    public Transform shotPoint;

    public AudioSource fireSound;

    public float acceleration = 5.0f;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        gun = GameObject.Find("Gun");
    }

    private void Update()
    {
        v3Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heading = v3Pos - transform.position;
        rotation = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg + offset;

        if (playerController.isFacingRight)
        {
            gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);
        }
        else
        {
            gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation-180);
        }

        if (Input.GetMouseButtonDown(0) && playerController.amountOfShotsLeft != 0 && timeBetweenShots <= 0.0f && !playerController.deathSound.isPlaying)
        {
            fireSound.Play();

            shootMovementTime = 0.0f;
            timeBetweenShots = timeBetweenShotsOrigin;
            playerController.isJumping = false;

            if (!playerController.infiniteShots)
            {
                playerController.amountOfShotsLeft--;
            }

            if (playerController.isFacingRight)
            {
                Instantiate(projectile, shotPoint.position, gun.transform.rotation);

                playerController.shootingMovement = new Vector2(Mathf.Clamp(-gun.transform.right.x * shootForceX * (playerController.isGrounded == true ? 0.75f : 1) + playerController.shootingMovement.x * 0.75f, -maxForceX, maxForceX), -gun.transform.right.y * shootForceY + playerController.shootingMovement.y * 0.75f);
            }
            else
            {
                Instantiate(projectile, shotPoint.position, Quaternion.Euler(0.0f, 0.0f, rotation));

                playerController.shootingMovement = new Vector2(Mathf.Clamp(gun.transform.right.x * shootForceX * (playerController.isGrounded == true ? 0.75f : 1) + playerController.shootingMovement.x * 0.75f, -maxForceX, maxForceX), gun.transform.right.y * shootForceY + playerController.shootingMovement.y * 0.75f);
            }
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        shootMovementTime += Time.deltaTime;
        if (shootMovementTime > shootMovementTimeOrigin)
        {
            playerController.shootingMovement = new Vector2(0.0f, 0.0f);
        }

        if (playerController.shootingMovement.x > 0.0f)
        {
            playerController.shootingMovement = new Vector2(playerController.shootingMovement.x - acceleration * Time.deltaTime, playerController.shootingMovement.y);
        }
        else if (playerController.shootingMovement.x < 0.0f)
        {
            playerController.shootingMovement = new Vector2(playerController.shootingMovement.x + acceleration * Time.deltaTime, playerController.shootingMovement.y);
        }

        if (playerController.shootingMovement.y > 0.0f)
        {
            playerController.shootingMovement = new Vector2(playerController.shootingMovement.x, playerController.shootingMovement.y - acceleration * Time.deltaTime);
        }
        else if (playerController.shootingMovement.y < 0.0f)
        {
            playerController.shootingMovement = new Vector2(playerController.shootingMovement.x, playerController.shootingMovement.y - acceleration * Time.deltaTime);
        }

        fireSound.transform.position = new Vector3(0, 0, 0);
    }
}
