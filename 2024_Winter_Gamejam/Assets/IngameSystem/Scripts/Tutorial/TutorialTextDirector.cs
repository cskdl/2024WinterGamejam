using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTextDirector : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_textOnPlayer;
    [SerializeField] private string[] m_tutorialTexts;
    [SerializeField] private float m_targetTime;

    [SerializeField] private TextMeshProUGUI m_exitInfoText;
    [SerializeField] private string m_exitInfo;

    private float m_timer = 0;
    private int m_index;

    private void Start()
    {
        m_exitInfoText.text = m_exitInfo;
        m_index = 0;
        m_textOnPlayer.text = m_tutorialTexts[m_index];
    }

    // Update is called once per frame
    void Update()
    {
        m_textOnPlayer.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1, 0);
        m_timer += Time.deltaTime;
        if(m_timer >= m_targetTime)
        {
            m_timer = 0;
            if(++m_index >= m_tutorialTexts.Length)
            {
                m_index = 0;
            }
            m_textOnPlayer.text = m_tutorialTexts[m_index];
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
