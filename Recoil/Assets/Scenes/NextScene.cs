using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public int levelId = 0;

    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerController.levelClear.isPlaying)
        {
            Debug.Log("NextScene");
            StartCoroutine(nextLevelPlay());
        }
    }

    IEnumerator nextLevelPlay()
    {
        playerController.levelClear.Play();

        yield return new WaitWhile(() => playerController.levelClear.isPlaying);

        SaveSystem.SaveTime(playerController);
        SceneManager.LoadScene(levelId);
    }
}
