using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRemove : MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.ammoRemoveSound.Play();
            playerController.amountOfShotsLeft = 0;
            Destroy(gameObject);
        }
    }
}
