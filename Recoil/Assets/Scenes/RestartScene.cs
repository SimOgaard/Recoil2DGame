using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerController.deathSound.isPlaying)
        {
            Debug.Log("RestartScene");
            StartCoroutine(deathSoundPlay());
        }
    }

    IEnumerator deathSoundPlay()
    {
        playerController.deathSound.Play();

        playerController.transform.position = new Vector3(100, 100, 0);

        yield return new WaitWhile(() => playerController.deathSound.isPlaying);

        SaveSystem.SaveTime(playerController);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
