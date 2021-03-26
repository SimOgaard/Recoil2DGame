using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Character").GetComponent<PlayerController>();
    }


    public void destroyWall()
    {
        playerController.breakBlock();
        Destroy(gameObject);
    }
}
