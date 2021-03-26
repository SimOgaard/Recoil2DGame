using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text textTimer;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerController.lastRunText = textTimer.text;
        playerController.hasFinisedRun = true;

        if (collision.CompareTag("Player"))
        {
            for (int i = 0; i < playerController.currentLevelTimers.Length; i++)
            {
                if (playerController.currentGameTimer < playerController.bestGameTimer)
                {
                    playerController.bestRun[i] = playerController.currentLevelTimers[i];
                }
                if (playerController.currentLevelTimers[i] < playerController.bestLevelTimers[i])
                {
                    Debug.Log("PB on " + i);

                    playerController.bestLevelTimers[i] = playerController.currentLevelTimers[i];
                }
                playerController.currentLevelTimers[i] = 0.0f;
            }
            if (playerController.currentGameTimer < playerController.bestGameTimer)
            {
                Debug.Log("PB!");
                playerController.bestGameTimer = playerController.currentGameTimer;
            }

            SaveSystem.SaveTime(playerController);
            SceneManager.LoadScene(0);
        }
    }
}
