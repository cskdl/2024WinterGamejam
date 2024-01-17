using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownStarter : MonoBehaviour
{
    [SerializeField] private float m_targetTime = 3;
    [SerializeField] private Text m_text;

    private void Start()
    {
        m_text.transform.position = Camera.main.WorldToScreenPoint(new Vector3(0, 1, 0));
        m_text.text = ((int)m_targetTime + 1).ToString();
    }

    private void Update()
    {
        m_targetTime -= Time.deltaTime;
        if(m_targetTime <= 0)
        {
            Destroy(this.gameObject);
        }
        if(m_text.text != ((int)m_targetTime + 1).ToString())
        {
            m_text.text = ((int)m_targetTime + 1).ToString();
        }
    }
}
