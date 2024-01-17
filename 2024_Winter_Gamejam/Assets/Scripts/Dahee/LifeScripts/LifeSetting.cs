using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeSetting : MonoBehaviour
{
    public int playerLife = 3;
    public GameDirector gameDirector;

   
    void Start()
    {
        this.gameDirector.UpdateLives(this.playerLife);
        this.gameDirector.Init(this.playerLife);
    }

    void Update()
    {
        LifeTest();
    }


    void LifeTest()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerLife -= 1;
            this.gameDirector.UpdateLives(this.playerLife);
        }

        if (playerLife <= 0)
        {
            SceneManager.LoadScene("ClearScene");
        }
    }
}

