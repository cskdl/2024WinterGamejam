using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private float m_counter = 0;
    private List<Vector3> m_foodPoses = new List<Vector3>();
    private List<GameObject> m_foods = new List<GameObject>();

    private void Update()
    {
        m_counter += Time.deltaTime;
        if(m_counter >= m_generationDistance)
        {
            GenerateFood();
            m_counter = 0;
        }
    }

    public void UpdateFoodCondition()
    {
        for(int i = 0; i < m_foodPoses.Count; i++)
        {
            if (m_foodPoses[i].x >= m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                && m_foodPoses[i].x <= m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2)
                && m_foodPoses[i].y >= m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                && m_foodPoses[i].y <= m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1
                && !m_foods[i].activeSelf)
            {
                m_foods[i].SetActive(true);
            }
            else if((m_foodPoses[i].x < m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                || m_foodPoses[i].x > m_mapGenerator.RecordedChunk.x * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2)
                || m_foodPoses[i].y < m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize - (m_mapGenerator.ChunkSize * 3 / 2)
                || m_foodPoses[i].y > m_mapGenerator.RecordedChunk.y * m_mapGenerator.ChunkSize + (m_mapGenerator.ChunkSize * 3 / 2) + 1)
                && m_foods[i].activeSelf)
            {
                m_foods[i].SetActive(false);
            }
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
                temp.transform.position = m_backgroundTilemap.CellToWorld(new Vector3Int(x, y));
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
                temp.transform.position = m_backgroundTilemap.CellToWorld(new Vector3Int(x, y));
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
}
