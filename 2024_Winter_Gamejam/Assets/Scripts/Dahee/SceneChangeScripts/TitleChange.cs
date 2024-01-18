using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleChange : MonoBehaviour
{

   public void ChangeFirstScene()
    {
        SceneManager.LoadScene("ClearScene");

    }

    public void ChangeSecondScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GoToTotorial()
    {
        SceneManager.LoadScene("TutorialTest");
    }
}
