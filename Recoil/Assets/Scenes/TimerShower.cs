using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerShower : MonoBehaviour
{
    public Text textTimer;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if ((playerController.hasMovedFirstLevel && SceneManager.GetActiveScene().buildIndex == 1) || SceneManager.GetActiveScene().buildIndex != 1)
        {
            playerController.currentGameTimer += Time.deltaTime;
            playerController.currentLevelTimers[SceneManager.GetActiveScene().buildIndex-1] += Time.deltaTime;

            if (playerController.bestGameTimer == float.MaxValue)
            {
                textTimer.text = MSMs(playerController.currentGameTimer) + "\n";
            }
            else if (playerController.currentGameTimer < sumOfLastRunTime(SceneManager.GetActiveScene().buildIndex))
            {
                textTimer.text = "<color=green>" + MSMs(playerController.currentGameTimer) + "</color>\n";
            }
            else
            {
                textTimer.text = "<color=red>" + MSMs(playerController.currentGameTimer) + "</color>\n";
            }

            for (int i = 0; i < SceneManager.GetActiveScene().buildIndex; i++)
            {
                if (playerController.bestRun[i] != float.MaxValue)
                {
                    if (playerController.currentLevelTimers[i] < playerController.bestLevelTimers[i])
                    {
                        textTimer.text += "\n<color=yellow>-" + timeDiff(playerController.currentLevelTimers[i], playerController.bestRun[i]) + "</color> ";
                    }
                    else if (playerController.currentLevelTimers[i] < playerController.bestRun[i])
                    {
                        textTimer.text += "\n<color=green>-" + timeDiff(playerController.currentLevelTimers[i], playerController.bestRun[i]) + "</color> ";
                    }
                    else
                    {
                        textTimer.text += "\n<color=red>+" + timeDiff(playerController.currentLevelTimers[i], playerController.bestRun[i]) + "</color> ";
                    }
                }
                else
                {
                    textTimer.text += "\n";
                }

                textTimer.text += MSMs(playerController.currentLevelTimers[i]);
            }
        }
    }

    private float sumOfLastRunTime(int index)
    {
        float sum = 0.0f;
        for (int i = 0; i < index; i++)
        {
            sum += playerController.bestRun[i];
        }
        return sum;
    }

    private string MSMs(float time)
    {
        time = Mathf.Abs(time);
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        return minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }

    private string timeDiff(float time1, float time2)
    {
        float timeDiff = Mathf.Abs(time1 - time2);
        int seconds = Mathf.FloorToInt(timeDiff);
        int milliseconds = Mathf.FloorToInt((timeDiff * 100F) % 100F);

        return seconds.ToString("00") + ":" + milliseconds.ToString("00"); ;
    }
}
