using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerData
{
    public bool hasFinisedRun;
    public string lastRunText;

    public float currentGameTimer;
    public float[] currentLevelTimers;

    public float bestGameTimer;
    public float[] bestRun;

    public List<Vector2>[] bestRunGhost = new List<Vector2>[20];
    public List<Vector2>[] recordedGhostRun = new List<Vector2>[20];

    public float[] bestLevelTimers;

    public TimerData(PlayerController playerController)
    {
        hasFinisedRun = playerController.hasFinisedRun;
        lastRunText = playerController.lastRunText;

        currentGameTimer = playerController.currentGameTimer;
        currentLevelTimers = playerController.currentLevelTimers;

        bestGameTimer = playerController.bestGameTimer;
        bestRun = playerController.bestRun;

        bestLevelTimers = playerController.bestLevelTimers;

        isPlayingNoMovement = playerController.isPlayingNoMovement;
        infiniteShots = playerController.infiniteShots;

//      bestRunGhost = playerController.bestRunGhost;
//      recordedGhostRun = playerController.bestRunGhost;
        instantReset = playerController.instantReset;

    }

    public bool instantReset;

    public bool isPlayingNoMovement;
    public bool infiniteShots;
}
