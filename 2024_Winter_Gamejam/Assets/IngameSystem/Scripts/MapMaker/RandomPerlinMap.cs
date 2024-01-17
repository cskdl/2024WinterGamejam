using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPerlinMap : MonoBehaviour
{
    private int m_randomSeed = 1;

    [SerializeField] private Tilemap[] m_tileMaps = new Tilemap[2];
    //[SerializeField] private Sprite[] m_tilemapImg = new Sprite[2];
    [SerializeField] private TileBase[] m_tileBases = new TileBase[2];
    [SerializeField] private FoodGenerator m_foodGenerator;

    public int ChunkSize { get; private set; } = 24;

    private float m_magnification = 7.0f;
    private int m_offsetX = 0;
    private int m_offsetY = 0;

    private List<Vector3> m_currentChunks = new List<Vector3>();
    private List<Vector3> m_recordedChunks = new List<Vector3>();

    public Vector3Int RecordedChunk { get; private set; }

    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GameObject m_enemyPrefab;

    private GameObject m_player;
    private GameObject m_enemy;

    private void Start()
    {
        m_randomSeed = Random.Range(0, 1000);
        Random.InitState(m_randomSeed);   //Seed값에 따라 초기화 시킴
        m_offsetX = Random.Range(-10000, 10000);
        m_offsetY = Random.Range(-10000, 10000);
        SetPlayer();
        SetEnemy();
        GenerateMapWithoutCheck();
        m_foodGenerator.InitFood();
    }

    private void Update()
    {
        GenerateMap();
        if (Vector3.Distance(m_player.transform.GetChild(0).position, m_enemy.transform.position) > 25)
        {
            SetEnemyPosition();
        }
    }

    private void SetPlayer()
    {
        if (m_playerPrefab != null)
        {
            GameObject player = Instantiate(m_playerPrefab);
            int x = 0, y = 0;
            //플레이어가 벽에 부딪히면 안 되기 때문에 벽 외의 공간에 생성
            if (GetIdUsingPerlin(x, y) == 0)
            {
                player.transform.position = new Vector3(x, y);
            }
            else
            {
                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    while (GetIdUsingPerlin(--x, y) != 0)
                    {
                        continue;
                    }
                }
                else
                {
                    while (GetIdUsingPerlin(++x, y) != 0)
                    {
                        continue;
                    }
                }
                player.transform.position = new Vector3(x, y);
            }
            //카메라 이슈로 임시조치
            player.transform.localScale = new Vector3(1, 1, 1) * 3;
            m_player = player;
            m_playerPrefab = null;
        }
    }


    private void SetEnemy()
    {
        if (m_enemyPrefab != null)
        {
            GameObject enemy = Instantiate(m_enemyPrefab);
            Vector3 playerPos = m_player.transform.position;
            int x = (int)playerPos.x - (ChunkSize / 2);
            int y = (int)playerPos.y - (ChunkSize / 2);

            //플레이어가 벽에 부딪히면 안 되기 때문에 벽 외의 공간에 생성
            if (GetIdUsingPerlin(x, y) == 0)
            {
                enemy.transform.position = new Vector3(x, y);
            }
            else
            {
                while (GetIdUsingPerlin(--x, y) != 0)
                {
                    continue;
                }
                enemy.transform.position = new Vector3(x, y);
            }
            //enemy.transform.localScale = new Vector3(1, 1, 1) * 3;
            m_enemy = enemy;
            m_enemyPrefab = null;
        }
    }

    private void SetEnemyPosition()
    {
        if(m_enemyPrefab != null)
        {
            return;
        }
        //Debug.Log($"Distance is now {Vector3.Distance(m_player.transform.GetChild(0).position, m_enemy.transform.position)}! Reset Enemy Position");
        Vector3 playerPos = m_player.transform.GetChild(0).position;
        int x = (int)playerPos.x - (ChunkSize / 3);
        int y = (int)playerPos.y - (ChunkSize / 3);
        if (GetIdUsingPerlin(x, y) == 0)
        {
            m_enemy.transform.position = new Vector3(x, y);
        }
        else
        {
            while (GetIdUsingPerlin(--x, y) != 0)
            {
                continue;
            }
            m_enemy.transform.position = new Vector3(x, y);
        }
    }

    private void GenerateMap()
    {
        var camPos = Camera.main.transform.position;
        if (RecordedChunk != new Vector3Int((int)camPos.x / ChunkSize * 2, (int)camPos.y / ChunkSize * 2))
        {
            camPos.x /= ChunkSize;
            camPos.y /= ChunkSize;
            m_recordedChunks = m_currentChunks.ToList();
            m_currentChunks.Clear();
            for (int x = (int)camPos.x - 1; x <= (int)camPos.x + 1; x++)
            {
                for (int y = (int)camPos.y - 1; y <= (int)camPos.y + 1; y++)
                {
                    m_currentChunks.Add(new Vector3(x, y));
                    if (m_recordedChunks.Contains(new Vector3(x, y)))
                    {
                        continue;
                    }
                    LoadChunk(x, y);
                }
            }
            for(int x = RecordedChunk.x - 2; x <= RecordedChunk.x + 2; x++)
            {
                for(int y = RecordedChunk.y - 2; y <= RecordedChunk.y + 2; y++)
                {
                    if (m_currentChunks.Contains(new Vector3(x, y)))
                        continue;
                    UnloadChunk(x, y);
                }
            }
            m_foodGenerator.UpdateFoodCondition();
            RecordedChunk = new Vector3Int((int)camPos.x, (int)camPos.y);
        }
    }
    private void GenerateMapWithoutCheck()
    {
        var camPos = Camera.main.transform.position;
        camPos.x /= ChunkSize;
        camPos.y /= ChunkSize;
        for (int x = (int)camPos.x - 1; x <= (int)camPos.x + 1; x++)
        {
            for (int y = (int)camPos.y - 1; y <= (int)camPos.y + 1; y++)
            {
                LoadChunk(x, y);
            }
        }
        RecordedChunk = new Vector3Int((int)camPos.x / ChunkSize, (int)camPos.y / ChunkSize);
    }


    private void LoadChunk(int pX, int pY)
    {
        for(int x = pX * (int)ChunkSize - ((int)ChunkSize / 2); x < pX * (int)ChunkSize + ((int)ChunkSize / 2); x++)
        {
            for(int y = pY * (int)ChunkSize - ((int)ChunkSize / 2); y < pY * (int)ChunkSize + ((int)ChunkSize / 2); y++)
            {
                if(!m_tileMaps[GetIdUsingPerlin(x, y)].HasTile(m_tileMaps[GetIdUsingPerlin(x, y)].LocalToCell(new Vector3Int(x, y))))
                    m_tileMaps[GetIdUsingPerlin(x, y)].SetTile(m_tileMaps[GetIdUsingPerlin(x, y)].LocalToCell(new Vector3Int(x, y)), m_tileBases[GetIdUsingPerlin(x, y)]);
            }
        }
    }

    private void UnloadChunk(int pX, int pY)
    {
        for (int x = pX * (int)ChunkSize - ((int)ChunkSize / 2); x < pX * (int)ChunkSize + ((int)ChunkSize / 2); x++)
        {
            for (int y = pY * (int)ChunkSize - ((int)ChunkSize / 2); y < pY * (int)ChunkSize + ((int)ChunkSize / 2); y++)
            {
                if (m_tileMaps[GetIdUsingPerlin(x, y)].HasTile(m_tileMaps[GetIdUsingPerlin(x, y)].LocalToCell(new Vector3Int(x, y))))
                    m_tileMaps[GetIdUsingPerlin(x, y)].SetTile(m_tileMaps[GetIdUsingPerlin(x, y)].LocalToCell(new Vector3Int(x, y)), null);
            }
        }
    }

    public int GetIdUsingPerlin(int pX, int pY)
    {
        float rawPerlin = Mathf.PerlinNoise((pX - m_offsetX) / m_magnification, (pY - m_offsetY) / m_magnification);
        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * m_tileBases.Length;
        if (scalePerlin >= 2)
        {
            scalePerlin = 1;
        }
        return Mathf.FloorToInt(scalePerlin);
    }
}
