using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public GameObject[] lifes;

    public void UpdateLives(int playerLife)
    {
        for (int i = 0; i < lifes.Length; i++)
            this.lifes[i].SetActive(i < playerLife);
    }


    public void Init(int playerLife)
    {
        for (int i = 0; i < lifes.Length; i++)
            this.lifes[i].SetActive(false);

        for (int i = 0; i < playerLife; i++)
            this.lifes[i].SetActive(true);
    }
}