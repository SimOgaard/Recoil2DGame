using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRecharge : MonoBehaviour
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
            playerController.ammoPickupSound.Play();
            playerController.amountOfShotsLeft = playerController.amountOfShotsInit;
            Destroy(gameObject);
        }
    }
}
