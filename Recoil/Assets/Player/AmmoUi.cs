using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUi : MonoBehaviour
{
    private PlayerController playerController;

    public Image[] bullets;
    [SerializeField]
    private Sprite bullet;
    [SerializeField]
    private Sprite shell;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i < playerController.amountOfShotsLeft)
            {
                bullets[i].sprite = bullet;
            }
            else
            {
                bullets[i].sprite = shell;
            }

            if (i < playerController.amountOfShotsInit)
            {
                bullets[i].enabled = true;
            }
            else
            {
                bullets[i].enabled = false;
            }
        }
        if (playerController.amountOfShotsInit == 0)
        {
            bullets[0].enabled = true;
        }
    }
}
