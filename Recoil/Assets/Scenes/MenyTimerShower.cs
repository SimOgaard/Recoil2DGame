using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenyTimerShower : MonoBehaviour
{
    public Text textTimer;
    public Text textShow;
    public PlayerController playerController;
    private int showingBestRun;
    public TimerData timerData;

    private bool hasFinisedRun = false;

    private void Start()
    {
        timerData = SaveSystem.LoadTime();
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();

        hasFinisedRun = timerData.hasFinisedRun;

        if (hasFinisedRun)
        {
            showingBestRun = 2;
        }
        else
        {
            showingBestRun = 0;
        }

        ChangeTime();
    }

    private string MSMs(float time){
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        return minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }

    public void ButtonClickTime()
    {
        if (!hasFinisedRun)
        {
            if (showingBestRun == 0)
            {
                showingBestRun = 1;
            }
            else
            {
                showingBestRun = 0;
            }
        }
        else
        {
            if (showingBestRun == 0)
            {
                showingBestRun = 1;
            }
            else if (showingBestRun == 1)
            {
                showingBestRun = 2;
            }
            else
            {
                showingBestRun = 0;
            }
        }
        ChangeTime();
    }

    public void ChangeTime()
    {
        if (hasFinisedRun && showingBestRun == 2)
        {
            textTimer.text = timerData.lastRunText;
            textShow.text = "SHOWING:\nLAST RUN";

            timerData.hasFinisedRun = false;

            playerController.hasFinisedRun = timerData.hasFinisedRun;
            playerController.lastRunText = timerData.lastRunText;

            playerController.currentGameTimer = timerData.currentGameTimer;
            playerController.currentLevelTimers = timerData.currentLevelTimers;

            playerController.bestGameTimer = timerData.bestGameTimer;
            playerController.bestRun = timerData.bestRun;

            playerController.bestLevelTimers = timerData.bestLevelTimers;

            SaveSystem.SaveTime(playerController);
        }
        else if (timerData.bestGameTimer != float.MaxValue && showingBestRun == 0)
        {
            textShow.text = "SHOWING:\nBEST RUN";
            textTimer.text = MSMs(timerData.bestGameTimer) + "\n";

            for (int i = 0; i < timerData.bestRun.Length; i++)
            {
                textTimer.text += "\n" + MSMs(timerData.bestRun[i]);
            }
        }
        else if (showingBestRun == 1 && timerData.bestGameTimer != float.MaxValue)
        {
            textShow.text = "SHOWING:\nBEST TIMES";
            textTimer.text = MSMs(Summing()) + "\n";

            for (int i = 0; i < timerData.bestRun.Length; i++)
            {
                textTimer.text += "\n" + MSMs(timerData.bestLevelTimers[i]);
            }
        }
        else
        {
            textTimer.text = "PLAY TO RUN AGAINST\nYOUR BEST RUN";
        }
    }

    public float Summing()
    {
        float sum = 0;
        for (int i = 0; i < timerData.bestLevelTimers.Length; i++)
        {
            sum += timerData.bestLevelTimers[i];
        }
        return sum;
    }
}
