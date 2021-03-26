using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Timer")]
        public bool hasFinisedRun;
        public string lastRunText;

        public float currentGameTimer;
        public float[] currentLevelTimers = new float[20] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };

        public float bestGameTimer = float.MaxValue;
        public float[] bestRun = new float[20] { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };

        public float[] bestLevelTimers = new float[20] { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };

    private float keyDownTime = 0.0f;
    [SerializeField]
    private float keyDownTimeMax = 1.0f;

    // ghost mode, spela mot ditt bästa run
    // save player posision

    [Header("Player Movement")]
        public double speedX = 0.0d;
        public double speedY = 0.0d;
        public double airTime = 0.0d;

    [Header("Player Orientation")]
        public bool isFacingRight = true;

    [Header("Player Grounded")]
        public bool isGrounded;
        public bool hitCeiling;

    [Header("Player Jumping")]
        public bool isJumping;

    [Header("Player Rigidbody2D")]
        public Rigidbody2D rb;

    [Header("Player Ground Collisions")]
        public LayerMask whatIsGround;
        public Transform GroundCheck;
        public Transform CeilingCheck;

    [Header("Player Ground Ranges")]
        public float GroundRangeX = 0.0f;
        public float GroundRangeY = 0.0f;
        public float CeilingRangeX = 0.0f;
        public float CeilingRangeY = 0.0f;

    [Header("Player Shoot")]
        public Vector2 shootingMovement;
        public int amountOfShotsInitNoMovement;
        public int amountOfShotsInitNormal;
        public int amountOfShotsInit;
        public int amountOfShotsLeft;

    [Header("Sound")]
        public AudioSource deathSound;
        public AudioSource breakBlockSound;
        public AudioSource levelClear;
        public AudioSource ammoPickupSound;
        public AudioSource ammoRemoveSound;

    public bool isPlayingNoMovement = false;
    public bool infiniteShots = false;

    public bool hasMovedFirstLevel = false;

    public bool instantReset = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            keyDownTime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.R) && !deathSound.isPlaying)
        {
            Debug.Log("PlayerControllerR");
            StartCoroutine(deathSoundPlay());
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void Start()
    {
        hasMovedFirstLevel = false;

        rb = GetComponent<Rigidbody2D>();

        TimerData timerData = SaveSystem.LoadTime();
        hasFinisedRun = timerData.hasFinisedRun;
        lastRunText = timerData.lastRunText;

        currentGameTimer = timerData.currentGameTimer;
        currentLevelTimers = timerData.currentLevelTimers;

        bestGameTimer = timerData.bestGameTimer;
        bestRun = timerData.bestRun;

        bestLevelTimers = timerData.bestLevelTimers;
        instantReset = timerData.instantReset;

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            currentGameTimer = 0.0f;
            hasFinisedRun = false;
            System.Array.Clear(currentLevelTimers, 0, 20);
        }

        isPlayingNoMovement = timerData.isPlayingNoMovement;
        infiniteShots = timerData.infiniteShots;

        if (infiniteShots)
        {
            amountOfShotsInit = 1;
        }
        else if (isPlayingNoMovement)
        {
            amountOfShotsInit = amountOfShotsInitNoMovement;
        }
        amountOfShotsLeft = amountOfShotsInit;
        rb.drag = 0.0f;
    }

    void FixedUpdate()
    {
        speedX = rb.velocity.x;
        speedY = rb.velocity.y;

        isGrounded = Physics2D.OverlapBox(GroundCheck.position, new Vector2(GroundRangeX, GroundRangeY), 0, whatIsGround);
        hitCeiling = Physics2D.OverlapBox(CeilingCheck.position, new Vector2(CeilingRangeX, CeilingRangeY), 0, whatIsGround);

        if (isGrounded)
        {
            airTime = 0.0f;
        }
        else
        {
            airTime += Time.deltaTime;
        }

        if (!deathSound.isPlaying && (transform.position.x > 11.0f || transform.position.x < -11.0f || transform.position.y > 7.25f || transform.position.y < -6.5f) && !(transform.position.x > 50))
        {
            Debug.Log("PlayerControllerDistance");
            deathSound.Play();
            StartCoroutine(deathSoundPlayOutScreen());
        }
        deathSound.transform.position = new Vector3(0, 0, 0);
        breakBlockSound.transform.position = new Vector3(0, 0, 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(GroundCheck.position, new Vector2(GroundRangeX, GroundRangeY));
        Gizmos.DrawWireCube(CeilingCheck.position, new Vector2(CeilingRangeX, CeilingRangeY));
    }

    IEnumerator deathSoundPlay()
    {
        deathSound.Play();

        transform.position = new Vector3(100, 100, 0);

        yield return new WaitWhile(() => deathSound.isPlaying);

        if (keyDownTime > keyDownTimeMax || instantReset)
        {
            SaveSystem.SaveTime(this);
            SceneManager.LoadScene(1);
        }
        else
        {
            SaveSystem.SaveTime(this);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator deathSoundPlayOutScreen()
    {
        transform.position = new Vector3(100, 100, 0);

        yield return new WaitWhile(() => deathSound.isPlaying);

        SaveSystem.SaveTime(this);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void breakBlock()
    {
        breakBlockSound.Play();
    }
}
