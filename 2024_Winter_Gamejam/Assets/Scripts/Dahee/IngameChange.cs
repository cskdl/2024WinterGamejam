using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameChange : MonoBehaviour
{

   public void ChangeFirstScene()
    {
        SceneManager.LoadScene("TitleScene");

    }

    public void ChangeSecondScene()
    {
        SceneManager.LoadScene("IngameScene");
    }
}
