using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    //[SerializeField] private float m_targetDistance = 0.2f;
    [field : SerializeField] public float m_moveSpeed { get; private set; }

    [SerializeField] private float m_turnSpeed = 180;
    [SerializeField] private List<GameObject> m_dragonBody; //include head
    [SerializeField] private GameObject m_bodyObj;
    [SerializeField] private Transform m_player;

    private float m_countUp = 0;

    //처음 시작할 때 카운트 다운(움직이지 말라고)
    public float CountDown = 3;
    public bool IsCrashed = false;

    private void Start()
    {
        InitBody();
    }

    private void FixedUpdate()
    {
        if(CountDown > 0)
        {
            CountDown -= Time.fixedDeltaTime;
        }
        DragonMovement();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(m_dragonBody[0].transform.position.x, m_dragonBody[0].transform.position.y, -10);
    }

    private void DragonMovement()
    {
        if(CountDown > 0)
        {
            if (IsCrashed)
            {
                m_dragonBody[0].GetComponent<Rigidbody2D>().velocity = m_dragonBody[0].transform.right * -m_moveSpeed / 2 * Time.deltaTime;
            }

            Camera.main.transform.position = new Vector3(m_dragonBody[0].transform.position.x, m_dragonBody[0].transform.position.y, -10);
            return;
        }
        if(CountDown <= 0 && IsCrashed)
        {
            IsCrashed = false;
        }

        m_dragonBody[0].GetComponent<Rigidbody2D>().velocity = m_dragonBody[0].transform.right * m_moveSpeed * Time.deltaTime;
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
                if (markManager.MarkerList.Count < 1) continue;
                m_dragonBody[i].transform.position = markManager.MarkerList[0].Position;
                m_dragonBody[i].transform.rotation = markManager.MarkerList[0].Rotation;
                if (markManager.MarkerList.Count > 1)
                {
                    markManager.MarkerList.RemoveAt(0);
                }
            }
        }
    }

    public void CreateBodyParts()
    {
        MarkerManager markManager = m_dragonBody[m_dragonBody.Count - 1].GetComponent<MarkerManager>();

        m_moveSpeed *= 0.975f;
        GameObject temp = Instantiate(m_bodyObj, markManager.MarkerList[0].Position, markManager.MarkerList[0].Rotation);
        if (!temp.GetComponent<MarkerManager>())
        {
            temp.AddComponent<MarkerManager>();
        }
        if (!temp.GetComponent<Rigidbody2D>())
        {
            temp.AddComponent<Rigidbody2D>();
        }
        temp.transform.SetParent(m_player);
        temp.transform.localScale = temp.transform.localScale * m_player.localScale.x;
        m_dragonBody.Add(temp);
        if (markManager.MarkerList.Count > 1)
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
