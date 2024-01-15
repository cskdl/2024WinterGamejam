using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    //[SerializeField] private float m_targetDistance = 0.2f;
    [SerializeField] private float m_moveSpeed = 280;
    [SerializeField] private float m_turnSpeed = 180;
    [SerializeField] private List<GameObject> m_dragonBody; //include head
    [SerializeField] private GameObject m_bodyObj;

    private float m_countUp = 0;

    private void Start()
    {
        InitBody();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateBodyParts();
        }
        DragonMovement();
    }

    private void DragonMovement()
    {
        m_dragonBody[0].GetComponent<Rigidbody2D>().velocity = m_dragonBody[0].transform.right * m_moveSpeed * Time.deltaTime;
        Camera.main.transform.position = new Vector3(m_dragonBody[0].transform.position.x, m_dragonBody[0].transform.position.y, -10);
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = 0;

        float dy = MousePos.y - m_dragonBody[0].transform.position.y;
        float dx = MousePos.x - m_dragonBody[0].transform.position.x;
        float rotateDg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        m_dragonBody[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotateDg));

        if(m_dragonBody.Count > 1)
        {
            for(int i = 1; i < m_dragonBody.Count; i++)
            {
                MarkerManager markManager = m_dragonBody[i - 1].GetComponent<MarkerManager>();
                m_dragonBody[i].transform.position = markManager.MarkerList[0].Position;
                m_dragonBody[i].transform.rotation = markManager.MarkerList[0].Rotation;
                markManager.MarkerList.RemoveAt(0);
            }
        }
    }

    public void CreateBodyParts()
    {
        MarkerManager markManager = m_dragonBody[m_dragonBody.Count - 1].GetComponent<MarkerManager>();

        GameObject temp = Instantiate(m_bodyObj, markManager.MarkerList[0].Position, markManager.MarkerList[0].Rotation);
        if (!temp.GetComponent<MarkerManager>())
        {
            temp.AddComponent<MarkerManager>();
        }
        if (!temp.GetComponent<Rigidbody2D>())
        {
            temp.AddComponent<Rigidbody2D>();
        }
        m_dragonBody.Add(temp);
        markManager.ClearMarkerList();
    }

    public void InitBody()
    {
        if (!m_dragonBody[0].GetComponent<MarkerManager>())
        {
            m_dragonBody[0].AddComponent<MarkerManager>();
        }
        if (!m_dragonBody[0].GetComponent<Rigidbody2D>())
        {
            m_dragonBody[0].AddComponent<Rigidbody2D>();
        }
    }
}
