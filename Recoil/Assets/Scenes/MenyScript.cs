using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenyScript : MonoBehaviour
{
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

    public GameObject projectile;
    public Transform shotPoint;

    public Rigidbody2D rb;
    public Image bulletAmount_0;
    public Sprite shell;

    public PlayerController playerController;

    public AudioSource shootSound;
    public AudioSource deathSound;

    private bool hasShoot = false;

    //    Vector2 hotspot = Vector2.zero;
    //    public Texture2D _Cursor;

    public Text isPlayingNoMovementText;
    public Text infiniteShotsText;
    public Text instantResetText;
    public Text menuText;

    void Start()
    {
//        Cursor.SetCursor(_Cursor, hotspot, CursorMode.Auto);

        Cursor.lockState = CursorLockMode.Confined;
        gun = GameObject.Find("Gun");
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
        LoadTimer();

        if (playerController.instantReset)
        {
            instantResetText.text = "FULL RESET:\nINSTANT";
            menuText.text = "ESC: TO MAIN MENY / QUIT\n\nR: RESTART GAME\n\nCLICK ON FULL RESET OR YOUR TIME";
        }
        else
        {
            instantResetText.text = "FULL RESET:\n0.25 SECONDS";
            menuText.text = "ESC: TO MAIN MENY / QUIT\n\nR: RESET LEVEL\nHOLD R > 0.25 SECOND: RESTART GAME\n\nCLICK ON FULL RESET OR YOUR TIME";
        }

        if (playerController.isPlayingNoMovement)
        {
            isPlayingNoMovementText.text = "DISABLED MOVEMENT";
        }
        if (playerController.infiniteShots)
        {
            infiniteShotsText.text = "DISABLED INFINITE SHOTS";
        }
    }

    public void LoadTimer()
    {
        TimerData timerData = SaveSystem.LoadTime();

        if (timerData == null)
        {
            playerController.hasFinisedRun = true;
            playerController.lastRunText = "00:00:00";

            playerController.currentGameTimer = 0.0f;
            playerController.currentLevelTimers = new float[20] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

            playerController.bestGameTimer = float.MaxValue;
            playerController.bestRun = new float[20] { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };

            playerController.bestLevelTimers = new float[20] { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };

            playerController.isPlayingNoMovement = false;
            playerController.infiniteShots = false;

            SaveSystem.SaveTime(playerController);
        }
        else
        {
            playerController.hasFinisedRun = timerData.hasFinisedRun;
            playerController.lastRunText = timerData.lastRunText;

            playerController.currentGameTimer = timerData.currentGameTimer;
            playerController.currentLevelTimers = timerData.currentLevelTimers;

            playerController.bestGameTimer = timerData.bestGameTimer;
            playerController.bestRun = timerData.bestRun;

            playerController.bestLevelTimers = timerData.bestLevelTimers;

            playerController.isPlayingNoMovement = timerData.isPlayingNoMovement;
            playerController.infiniteShots = timerData.infiniteShots;

            playerController.instantReset = timerData.instantReset;
        }
    }

    private void Update()
    {
        v3Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heading = v3Pos - transform.position;
        rotation = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg + offset;
        gun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Shoot()
    {
        if (hasShoot)
        {
            return;
        }
        hasShoot = true;
        bulletAmount_0.sprite = shell;
        Instantiate(projectile, shotPoint.position, gun.transform.rotation);
        rb.velocity = new Vector2(-gun.transform.right.x * shootForceX, -gun.transform.right.y * shootForceY);
        shootSound.Play();
    }

    public void DisableMovement()
    {
        playerController.isPlayingNoMovement = !playerController.isPlayingNoMovement;
    }
    public void InfiniteShots()
    {
        playerController.infiniteShots = !playerController.infiniteShots;
    }

    public void InstantResetButton()
    {
        playerController.instantReset = !playerController.instantReset;
        if (playerController.instantReset)
        {
            instantResetText.text = "FULL RESET:\nINSTANT";
            menuText.text = "ESC: TO MAIN MENY / QUIT\n\nR: RESTART GAME\n\nCLICK ON FULL RESET OR YOUR TIME";
        }
        else
        {
            instantResetText.text = "FULL RESET:\n0.25 SECONDS";
            menuText.text = "ESC: TO MAIN MENY / QUIT\n\nR: RESET LEVEL\nHOLD R > 0.25 SECOND: RESTART GAME\n\nCLICK ON FULL RESET OR YOUR TIME";
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StartGameScene"))
        {
            StartCoroutine(deathSoundPlay());
        }
    }

    IEnumerator deathSoundPlay()
    {
        deathSound.Play();

        transform.position = new Vector3(100,100,0);

        yield return new WaitWhile(() => deathSound.isPlaying);
        SaveSystem.SaveTime(playerController);
        SceneManager.LoadScene(1);
    }
}
