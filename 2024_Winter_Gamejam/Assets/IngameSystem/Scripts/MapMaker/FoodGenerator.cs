using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

//처음에 5개, 3초에 1개씩 생성
//최초 생성은 현재 청크, 이후 생성은 9칸 청크 중 1개
public class FoodGenerator : MonoBehaviour
{
    [SerializeField] private RandomPerlinMap m_mapGenerator;
    [SerializeField] private GameObject m_foodPrefab;
    [SerializeField] private Transform m_foodParent;
    [SerializeField] private int m_initialCount = 5;
    [SerializeField] private float m_generationDistance = 3;
    [SerializeField] private Tilemap m_backgroundTilemap;
    [SerializeField] private GameObject m_dragonballHelperPrefab;

    private float m_counter = 0;
    private List<Vector3> m_foodPoses = new List<Vector3>();
    private List<GameObject> m_foods = new List<GameObject>();

    private GameObject[] m_dragonballHelpers;
    private Vector3[] m_positionHelps = new Vector3[9];

    private void Awake()
    {
        m_dragonballHelpers = new GameObject[9];
        for(int i = 0; i < m_dragonballHelpers.Length; i++)
        {
            m_dragonballHelpers[i] = Instantiate(m_dragonballHelperPrefab);
            m_dragonballHelpers[i].SetActive(false);
            m_positionHelps[i] = new Vector3((float)2e9, (float)2e9);
        }
    }

    private void Update()
    {
        m_counter += Time.deltaTime;
        if(m_counter >= m_generationDistance)
        {
            GenerateFood();
            m_counter = 0;
        }
        MoveDragonHelper();
    }

    public void UpdateFoodCondition()
    {
        int posCount = m_foodPoses.Count;
        for(int i = 0; i < posCount; i++)
        {
            if (m_foodPoses[i].x >= m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                && m_foodPoses[i].x <= m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2)
                && m_foodPoses[i].y >= m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                && m_foodPoses[i].y <= m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1
                && m_foods[i] != null && !m_foods[i].activeSelf)
            {
                m_foods[i].SetActive(true);
            }
            else if((m_foodPoses[i].x < m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                || m_foodPoses[i].x > m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1
                || m_foodPoses[i].y < m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                || m_foodPoses[i].y > m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1)
                && m_foods[i] != null && m_foods[i].activeSelf)
            {
                m_foods[i].SetActive(false);
                //RemoveHelper(m_foods[i]);
            }
            if (m_foodPoses.Count >= i) break;
        }
    }

    public void InitFood()
    {
        int x, y;
        while (m_initialCount > 0)
        {
            x = Random.Range(m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize / 2),
                m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize / 2) + 1);
            y = Random.Range(m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize / 2),
                m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize / 2) + 1);
            if (m_mapGenerator.GetIdUsingPerlin(x, y) == 0 && !m_foodPoses.Contains(new Vector3(x, y)))
            {
                m_initialCount--;
                m_foodPoses.Add(new Vector3(x, y));
                GameObject temp = Instantiate(m_foodPrefab, m_foodParent);
                temp.transform.position = m_backgroundTilemap.CellToLocal(new Vector3Int(x, y));
                m_foods.Add(temp);
            }
            else
            {
                continue;
            }
        }
    }

    private void GenerateFood()
    {
        int x, y;
        while (true)
        {
            x = Random.Range(m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2),
                m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1);
            y = Random.Range(m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2),
                m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1);
            if (m_mapGenerator.GetIdUsingPerlin(x, y) == 0 && !m_foodPoses.Contains(new Vector3(x, y)))
            {
                m_foodPoses.Add(new Vector3(x, y));
                GameObject temp = Instantiate(m_foodPrefab, m_foodParent);
                temp.transform.position = m_backgroundTilemap.CellToLocal(new Vector3Int(x, y));
                m_foods.Add(temp);
                break;
            }
        }
    }

    public void RemoveFood(GameObject pFood)
    {
        if (!m_foodPoses.Contains(pFood.transform.position))
        {
            return;
        }
        int index = m_foodPoses.FindIndex(0, m_foodPoses.Count, n => n.Equals(pFood.transform.position));
        m_foodPoses.RemoveAt(index);
        m_foods.RemoveAt(index);
    }

    public void UpdateDragonballHelper()
    {
        int i = 0;
        for(int count = 0; count < m_dragonballHelpers.Length; count++)
        {
            if ((m_dragonballHelpers[count].activeSelf && !m_foodPoses.Contains(m_positionHelps[count])) 
                || (m_foodPoses.Contains(m_positionHelps[count]) && m_foods[m_foodPoses.IndexOf(m_positionHelps[count])] != null 
                && !m_foods[m_foodPoses.IndexOf(m_positionHelps[count])].activeSelf) 
                /*|| (m_dragonballHelpers[count].activeSelf && (Camera.main.WorldToViewportPoint(m_positionHelps[count]).x < -1f
                || Camera.main.WorldToViewportPoint(m_positionHelps[count]).x > 2f
                || Camera.main.WorldToViewportPoint(m_positionHelps[count]).y < -1f
                || Camera.main.WorldToViewportPoint(m_positionHelps[count]).y > 2f))*/)
            {
                m_dragonballHelpers[count].gameObject.SetActive(false);
            }
            if (m_dragonballHelpers[count].activeSelf)
            {
                continue;
            }
            for (; i < m_foods.Count; i++)
            {
                if (m_foods[i] == null)
                {
                    m_foods.RemoveAt(i);
                    continue;
                }
                if (m_positionHelps.Contains(m_foods[i].transform.position))
                {
                    i++;
                    break;
                }
                if (m_foods[i] != null && !m_foods[i].activeSelf)
                {
                    if (m_dragonballHelpers.FirstOrDefault(n => n != null && !n.activeSelf) == null) return;

                    int index = GetIndexofHelper(m_dragonballHelpers.FirstOrDefault(n => n != null && n.activeSelf));
                    if (index == -1 || !m_dragonballHelpers[index].activeSelf)
                    {
                        i++;
                        continue;
                    }
                    //Debug.LogError($"꺄악{index}");
                    //m_dragonballHelpers[index].gameObject.SetActive(false);
                    //Debug.Log("여기와서 지워짐");
                }
                Vector3 pos = Camera.main.WorldToViewportPoint(m_foods[i].transform.position);
                if(pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
                {
                    if (m_dragonballHelpers.FirstOrDefault(n => n != null && !n.activeSelf) == null) return;
                    int index = GetIndexofHelper(m_dragonballHelpers.FirstOrDefault(n => !n.activeSelf));
                    if (m_dragonballHelpers[index].activeSelf) return;
                    if (index == -1) return;
                    //카메라 위에 위치
                    if (pos.y > 1)
                    {
                        //좌측 상단
                        if (pos.x < 0)
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 135));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
                            helperPos.z = 0;
                            helperPos.x += 0.5f;
                            helperPos.y -= 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                        //중앙 상단
                        else if (pos.x <= 1)
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(pos.x, 1, 0));
                            helperPos.z = 0;
                            //helperPos.x += 0.5f;
                            helperPos.y -= 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                        //우측 상단
                        else
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
                            helperPos.z = 0;
                            helperPos.x -= 0.5f;
                            helperPos.y -= 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                    }
                    else if(pos.y >= 0)
                    {
                        //좌측 중앙
                        if (pos.x < 0)
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(0, pos.y, 0));
                            helperPos.z = 0;
                            helperPos.x += 0.5f;
                            //helperPos.y -= 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                        //완전 중앙(불가)
                        else if (pos.x <= 1)
                        {
                            Debug.Log("it can't be");
                            m_dragonballHelpers[index].SetActive(false);
                        }
                        //우측 중앙
                        else
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(1, pos.y, 0));
                            helperPos.z = 0;
                            helperPos.x -= 0.5f;
                            //helperPos.y -= 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                    }
                    else
                    {
                        //좌측 하단
                        if (pos.x < 0)
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 225));
                            var helperPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
                            helperPos.z = 0;
                            helperPos.x += 0.5f;
                            helperPos.y += 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                        //중앙 하단
                        else if (pos.x <= 1)
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                            var helperPos = Camera.main.ViewportToWorldPoint(new Vector3(pos.x, 0, 0));
                            helperPos.z = 0;
                            //helperPos.x += 0.5f;
                            helperPos.y += 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                        //우측 하단
                        else
                        {
                            m_dragonballHelpers[index].SetActive(true);
                            m_dragonballHelpers[index].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 315));
                            var helperPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
                            helperPos.z = 0;
                            helperPos.x -= 0.5f;
                            helperPos.y += 0.5f;
                            m_dragonballHelpers[index].transform.position = helperPos;
                            m_positionHelps[index] = m_foodPoses[i];
                        }
                    }
                    i++;
                }
                else
                {
                    //Debug.Log("여기와서 지워짐");
                    if (m_positionHelps.FirstOrDefault(n => n != null && n == m_foods[i].transform.position) != null) return;
                    int index = m_positionHelps.Select((n, index) => (n, index)).FirstOrDefault(n => n.n != null && n.n == m_foods[i].transform.position).index;
                    Debug.Log("꺄악");
                    m_dragonballHelpers[index].gameObject.SetActive(false);
                }
                if (i >= m_foods.Count) i--;
            }
        }
    }

    public void MoveDragonHelper()
    {
        UpdateDragonballHelper();
        for(int count = 0; count < m_dragonballHelpers.Length; count++)
        {
            if (!m_foodPoses.Contains(m_positionHelps[count]))
            {
                m_dragonballHelpers[count].gameObject.SetActive(false);
            }
            if (m_dragonballHelpers[count].activeSelf)
            {
                Vector3 pos = Camera.main.WorldToViewportPoint(m_positionHelps[count]);
                if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
                {
                    if (m_dragonballHelpers.FirstOrDefault(n => n != null && !n.activeSelf) == null) return;
                    int index = GetIndexofHelper(m_dragonballHelpers.FirstOrDefault(n => !n.activeSelf));
                    if (m_dragonballHelpers[index].activeSelf) return;
                    if (index == -1) return;
                    m_dragonballHelpers[count].SetActive(true);
                    //카메라 위에 위치
                    if (pos.y > 1)
                    {
                        //좌측 상단
                        if (pos.x < 0)
                        {
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 135));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
                            helperPos.z = 0;
                            helperPos.x += 0.5f;
                            helperPos.y -= 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                        //중앙 상단
                        else if (pos.x <= 1)
                        {
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(pos.x, 1, 0));
                            helperPos.z = 0;
                            //helperPos.x += 0.5f;
                            helperPos.y -= 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                        //우측 상단
                        else
                        {
                            m_dragonballHelpers[count].SetActive(true);
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
                            var helperPos = /*Camera.main.transform.position + */ Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
                            helperPos.z = 0;
                            helperPos.x -= 0.5f;
                            helperPos.y -= 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                    }
                    else if (pos.y >= 0)
                    {
                        //좌측 중앙
                        if (pos.x < 0)
                        {
                            m_dragonballHelpers[count].SetActive(true);
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(0, pos.y, 0));
                            helperPos.z = 0;
                            helperPos.x += 0.5f;
                            //helperPos.y -= 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                        //완전 중앙(불가)
                        else if (pos.x <= 1)
                        {
                            Debug.Log("it can't be");
                        }
                        //우측 중앙
                        else
                        {
                            m_dragonballHelpers[count].SetActive(true);
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(1, pos.y, 0));
                            helperPos.z = 0;
                            helperPos.x -= 0.5f;
                            //helperPos.y -= 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                    }
                    else
                    {
                        //좌측 하단
                        if (pos.x < 0)
                        {
                            m_dragonballHelpers[count].SetActive(true);
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 225));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
                            helperPos.z = 0;
                            helperPos.x += 0.5f;
                            helperPos.y += 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                        //중앙 하단
                        else if (pos.x <= 1)
                        {
                            m_dragonballHelpers[count].SetActive(true);
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(pos.x, 0, 0));
                            helperPos.z = 0;
                            //helperPos.x += 0.5f;
                            helperPos.y += 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                        //우측 하단
                        else
                        {
                            m_dragonballHelpers[count].SetActive(true);
                            m_dragonballHelpers[count].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 315));
                            var helperPos = /*Camera.main.transform.position +*/ Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
                            helperPos.z = 0;
                            helperPos.x -= 0.5f;
                            helperPos.y += 0.5f;
                            m_dragonballHelpers[count].transform.position = helperPos;
                        }
                    }
                }
                else
                {
                    m_dragonballHelpers[count].gameObject.SetActive(false);
                }
            }
        }
    }

    private int GetIndexofHelper(GameObject pTarget)
    {
        if (pTarget == null) return -1;

        for(int i = 0; i < m_dragonballHelpers.Length; i++)
        {
            if (m_dragonballHelpers[i].transform.position == pTarget.transform.position)
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveHelper(GameObject pTarget)
    {
        //if (pTarget == null) return;
        //if (m_positionHelps.FirstOrDefault(n => n != null && n == pTarget.transform.position) == null
        //    /*&& (m_dragonballHelpers[0] == null)*/) return;
        //int index = m_positionHelps.Select((n, index) => (n, index)).FirstOrDefault(n => n.n != null && n.n == pTarget.transform.position).index;
        //Debug.Log($"꺄악{index}");
        //m_dragonballHelpers[index].gameObject.SetActive(false);
        //UpdateDragonballHelper();
        ////Debug.Log(m_dragonballHelpers[index].transform.position);
    }
}
