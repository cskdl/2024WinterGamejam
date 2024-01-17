using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundChange : MonoBehaviour
{
    public void ChangeSecondScene()
    {
        SceneManager.LoadScene("SoundManagerScene");
    }
}
