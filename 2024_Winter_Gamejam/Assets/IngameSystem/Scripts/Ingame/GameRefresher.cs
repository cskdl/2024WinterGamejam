using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRefresher : MonoBehaviour
{
    public void Refresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
