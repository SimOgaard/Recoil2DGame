using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelShower : MonoBehaviour
{
    public Text text;
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
        text.text = (SceneManager.GetActiveScene().buildIndex).ToString() + "/" + (SceneManager.sceneCountInBuildSettings-1).ToString();
    }
}
