using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomPerlinMap : MonoBehaviour
{
    [SerializeField] private int m_randomSeed = 1;

    [SerializeField] private Tilemap[] m_tileMaps = new Tilemap[2];
    //[SerializeField] private Sprite[] m_tilemapImg = new Sprite[2];
    [SerializeField] private TileBase[] m_tileBases = new TileBase[2];

    private int m_chunkSize = 20;

    private float m_magnification = 7.0f;
    private int m_offsetX = 0;
    private int m_offsetY = 0;

    private Vector3Int m_recordedChunk;

    [SerializeField] private GameObject m_player;

    private void Start()
    {
        Random.InitState(m_randomSeed);   //Seed값에 따라 초기화 시킴
        m_offsetX = Random.Range(-10000, 10000);
        m_offsetY = Random.Range(-10000, 10000);
        SetPlayer();
        GenerateMapWithoutCheck();
    }

    private void Update()
    {
        GenerateMap();
    }

    private void SetPlayer()
    {
        if (m_player != null)
        {
            GameObject player = Instantiate(m_player);
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
            m_player = null;
        }
    }

    private void GenerateMap()
    {
        var camPos = Camera.main.transform.position;
        if (m_recordedChunk != new Vector3Int((int)camPos.x / m_chunkSize, (int)camPos.y / m_chunkSize))
        {
            camPos.x /= m_chunkSize;
            camPos.y /= m_chunkSize;
            for (int x = (int)camPos.x - 1; x <= (int)camPos.x + 1; x++)
            {
                for (int y = (int)camPos.y - 1; y <= (int)camPos.y + 1; y++)
                {
                    LoadChunk(x, y);
                }
            }
            m_recordedChunk = new Vector3Int((int)camPos.x / m_chunkSize, (int)camPos.y / m_chunkSize);
        }
    }
    private void GenerateMapWithoutCheck()
    {
        var camPos = Camera.main.transform.position;
        camPos.x /= m_chunkSize;
        camPos.y /= m_chunkSize;
        for (int x = (int)camPos.x - 1; x <= (int)camPos.x + 1; x++)
        {
            for (int y = (int)camPos.y - 1; y <= (int)camPos.y + 1; y++)
            {
                LoadChunk(x, y);
            }
        }
        m_recordedChunk = new Vector3Int((int)camPos.x / m_chunkSize, (int)camPos.y / m_chunkSize);
    }


    public void LoadChunk(int pX, int pY)
    {
        for(int x = pX * (int)m_chunkSize - ((int)m_chunkSize / 2); x < pX * (int)m_chunkSize + ((int)m_chunkSize / 2); x++)
        {
            for(int y = pY * (int)m_chunkSize - ((int)m_chunkSize / 2); y < pY * (int)m_chunkSize + ((int)m_chunkSize / 2); y++)
            {
                //m_tilemaps[GetIdUsingPerlin(x, y)].CellToLocal(new Vector3Int(x, y));
                m_tileMaps[GetIdUsingPerlin(x, y)].SetTile(m_tileMaps[GetIdUsingPerlin(x, y)].LocalToCell(new Vector3Int(x, y)), m_tileBases[GetIdUsingPerlin(x, y)]);
            }
        }
    }

    public void UnloadChunk(int pX, int pY)
    {
        for (int x = pX * (int)m_chunkSize - ((int)m_chunkSize / 2); x < pX * (int)m_chunkSize + ((int)m_chunkSize / 2); x++)
        {
            for (int y = pY * (int)m_chunkSize - ((int)m_chunkSize / 2); y < pY * (int)m_chunkSize + ((int)m_chunkSize / 2); y++)
            {
                m_tileMaps[GetIdUsingPerlin(x, y)].LocalToCell(new Vector3Int(x, y));
            }
        }
    }

    private int GetIdUsingPerlin(int pX, int pY)
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
