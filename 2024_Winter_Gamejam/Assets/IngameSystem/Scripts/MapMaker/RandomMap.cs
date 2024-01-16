using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [SerializeField] private int randomSeed = 1;

    [SerializeField] private GameObject m_backgroundPrefab;
    [SerializeField] private GameObject m_wallPrefab;

    private Dictionary<int, GameObject> m_tileSet;
    private Dictionary<int, GameObject> m_tileGroups;

    private int m_mapWidth = 160;
    private int m_mapHeight = 90;
    private List<List<int>> m_noiseGrid = new List<List<int>>();
    private List<List<GameObject>> m_tileGrid = new List<List<GameObject>>();

    private float m_magnification = 7.0f;
    private int m_offsetX = 0;
    private int m_offsetY = 0;

    private int m_camX = -1;
    private int m_camY = -1;

    //[0] : BackGround [1] : Wall
    private int[] m_right;
    private int[] m_up;
    private int[] m_left;
    private int[] m_down;

    [SerializeField] private GameObject m_player;

    private void Awake()
    {
        m_right = new int[2] { m_mapWidth, m_mapWidth };
        m_up = new int[2] { m_mapHeight, m_mapHeight };
        m_left = new int[2] { 0, 0 };
        m_down = new int[2] { 0, 0 };
    }

    private void Start()
    {
        Random.InitState(randomSeed);   //Seed값에 따라 초기화 시킴
        m_offsetX = Random.Range(-10000, 10000);
        m_offsetY = Random.Range(-10000, 10000);
        CreateTileSet();
        CreateTileGroups();
        GenerateMap();
        GameObject player = Instantiate(m_player);
        //플레이어가 벽에 부딪히면 안 되기 때문에 벽 외의 공간에 생성
        if (m_tileGrid[m_mapWidth / 2][m_mapHeight / 2].transform.parent.name == m_tileSet[0].name)
        {
            player.transform.position = new Vector3(m_mapWidth / 2, m_mapHeight / 2);
        }
        else
        {
            int width = m_mapWidth / 2;
            int height = m_mapHeight / 2;
            if(Random.Range(0.0f, 1.0f) < 0.5f)
            {
                while (++width >= 0 && m_tileGrid[width][height].transform.parent.name != m_tileSet[0].name)
                {
                    continue;
                }
            }
            else
            {
                while (++width < m_mapWidth && m_tileGrid[width][height].transform.parent.name != m_tileSet[0].name)
                {
                    continue;
                }
            }
            player.transform.position = new Vector3(width, height);
        }
        //카메라 이슈로 임시조치
        player.transform.localScale = new Vector3(1, 1, 1) * 3;
    }

    private void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        if(m_camX == -1)
        {
            m_camX = (int)camPos.x;
            m_camY = (int)camPos.y;
        }
        if(Mathf.Abs(camPos.x - m_camX) >= 0.5f)
        {
            //left
            //바꿀 수 있다면 지금이라도 늦지 않았다면............(제가 타일맵을 거의 안 써봐서)
            if(camPos.x - m_camX < 0)
            {
                //int right = m_tileGrid.Count - 1;
                
                for(int i = 0; i < m_mapHeight; i++)
                {
                    if(GameObject.Find($"tile({(int)camPos.x - 8}, {i})"))
                    {
                        continue;
                    }
                    else
                    {
                        int right;
                        int tempID = GetIdUsingPerlin((int)camPos.x - 8, i);
                        for(int j = 0; j < m_mapHeight; j++)
                        {
                            //m_right[0] = m_tileGrid.Where(n => )

                            right = (tempID == 0? m_right[0] : m_right[1]) - 1;
                            if (m_tileGrid[right][j] != null && m_tileGrid[right][j].transform.position.x > (int)camPos.x - 8
                                && (tempID == 0? !m_tileGrid[right][j].GetComponent<BoxCollider2D>() : m_tileGrid[right][j].GetComponent<BoxCollider2D>()))
                            {
                                m_tileGrid[right][j].name = string.Format($"tile({(int)camPos.x - 8}, {i})");
                                m_tileGrid[right][j].transform.position = new Vector3((int)camPos.x - 8, i);
                                
                                break;
                            }
                            else if((m_tileGrid[right][j] == null && m_tileGrid[right - 1][j] != null) /*&& m_tileGrid[right - 1][j].transform.position.x > (int)camPos.x - 8*/
                                && (tempID == 0 ? !m_tileGrid[right - 1][j].GetComponent<BoxCollider2D>() : m_tileGrid[right - 1][j].GetComponent<BoxCollider2D>()))
                            {
                                m_tileGrid[right - 1][j].name = string.Format($"tile({(int)camPos.x - 8}, {i})");
                                m_tileGrid[right - 1][j].transform.position = new Vector3((int)camPos.x - 8, i);
                            }
                            if (m_tileGrid[right].Last(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x
                                < m_tileGrid[right - 1].Last(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x)
                            {
                                Debug.Log("End of Line");
                                if(tempID == 0)
                                {
                                    m_right[0] -= 1;
                                }
                                else
                                {
                                    m_right[1] -= 1;
                                }
                            }
                            //else
                            //{
                            //    Debug.Log($"m_tileGrid[right].All(n => n != null) == {m_tileGrid[right].All(n => n != null)}");
                            //    Debug.Log($"m_tileGrid[right].First(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x = " +
                            //        $"{m_tileGrid[right].First(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x}");
                            //    Debug.Log($"m_tileGrid[right - 1].First(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x = " +
                            //        $"{m_tileGrid[right - 1].First(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x}");
                            //    Debug.Log($"m_tileGrid[right].First(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x" +
                            //        $" < m_tileGrid[right - 1].First(n => tempID == 0 ? !n.GetComponent<BoxCollider2D>() : n.GetComponent<BoxCollider2D>()).gameObject.transform.position.x");
                            //}
                        }
                    }
                }
            }
            //right
            else
            {

            }
            m_camX = (int)camPos.x;
            m_camY = (int)camPos.y;
        }
        if(Mathf.Abs(camPos.y - m_camY) >= 1)
        {
            //up
            //down
        }
    }

    private void CreateTileSet()
    {
        m_tileSet = new Dictionary<int, GameObject>();
        m_tileSet.Add(0, m_backgroundPrefab);
        m_tileSet.Add(1, m_wallPrefab);
    }

    private void CreateTileGroups()
    {
        m_tileGroups = new Dictionary<int, GameObject>();
        foreach(var prefab in m_tileSet)
        {
            GameObject tileGroup = new GameObject(prefab.Value.name);
            tileGroup.transform.parent = gameObject.transform;
            tileGroup.transform.localPosition = new Vector3(0, 0, 0);
            m_tileGroups.Add(prefab.Key, tileGroup);
        }
    }

    private void GenerateMap()
    {
        for(int x = 0; x < m_mapWidth; x++)
        {
            m_noiseGrid.Add(new List<int>());
            m_tileGrid.Add(new List<GameObject>());

            for (int y = 0; y < m_mapHeight; y++)
            {
                int tileID = GetIdUsingPerlin(x, y);
                m_noiseGrid[x].Add(tileID);
                CreateTile(tileID, x, y);
            }
        }
    }

    private int GetIdUsingPerlin(int pX, int pY)
    {
        float rawPerlin = Mathf.PerlinNoise((pX - m_offsetX) / m_magnification, (pY - m_offsetY) / m_magnification);
        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * m_tileSet.Count;
        if(scalePerlin >= 2)
        {
            scalePerlin = 1;
        }
        return Mathf.FloorToInt(scalePerlin);
    }

    private void CreateTile(int pTileID, int pX, int pY)
    {
        GameObject tilePrefab = m_tileSet[pTileID];
        GameObject tileGroup = m_tileGroups[pTileID];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name = string.Format($"tile({pX}, {pY})");
        tile.transform.localPosition = new Vector3(pX, pY, 0);

        m_tileGrid[pX].Add(tile);
    }
}
