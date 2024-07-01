using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniMap : MonoBehaviour
{
    GameSupporter gameSupporter;
    GameObject miniMapParent;

    // Prefab
    public GameObject wallPlanePrefab;
    public GameObject playerPlanePrefab;
    public GameObject monsterPlanePrefab;

    // Prefab List
    public List<GameObject> wallPlanePrefabList;
    public List<GameObject> monsterPlanePrefabList;
    public GameObject PlayerPlane;

    private int wallPlanCount = 0;
    private int mosterCount = 0;

    int m_wall;
    int m_monster;
    public int Wall { get { return m_wall; } set { m_wall = value; } }
    public int Monster { get { return m_monster; } set { m_monster = value; } }
    public void SetMiniMap()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        miniMapParent = GameObject.Find("Mini Map");

        GameObject odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        wallPlanePrefab = Resources.Load("Prefab/Wall Plane", typeof(GameObject)) as GameObject;
        playerPlanePrefab = Resources.Load("Prefab/Player Plane", typeof(GameObject)) as GameObject;
        monsterPlanePrefab = Resources.Load("Prefab/Monster Plane", typeof(GameObject)) as GameObject;

        for (int i = 0; i < m_wall; i++)
        {
            wallPlanePrefabList.Add(Instantiate(wallPlanePrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), miniMapParent.transform));
            wallPlanePrefabList[i].SetActive(false);
        }
        for (int i = 0; i < m_monster; i++)
        {
            monsterPlanePrefabList.Add(Instantiate(monsterPlanePrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), miniMapParent.transform));
            monsterPlanePrefabList[i].SetActive(false);
        }
        PlayerPlane = Instantiate(playerPlanePrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), miniMapParent.transform);
    }
    public void UpdateMiniMap()
    {
        for (int i = 0; i < wallPlanCount; i++) { 
            wallPlanePrefabList[i].SetActive(false);
        }
        wallPlanCount = 0;

        for (int i = 0; i < mosterCount; i++)
        {
            wallPlanePrefabList[i].SetActive(false);
        }
        mosterCount = 0;

        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            int objactNum = gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ];
            if (objactNum == (int)GameSupporter.map2dObject.wall)
            {
                wallPlanePrefabList[wallPlanCount].transform.localPosition = new Vector3(i / gameSupporter.MapZ + 0.5f, 1, i % gameSupporter.MapZ + 0.5f);
                wallPlanePrefabList[wallPlanCount].SetActive(true);

                wallPlanCount++;
            }
            else if (objactNum == (int)GameSupporter.map2dObject.moster)
            {
                monsterPlanePrefabList[mosterCount].transform.localPosition = new Vector3(i / gameSupporter.MapZ + 0.5f, 1, i % gameSupporter.MapZ + 0.5f);
                monsterPlanePrefabList[mosterCount].SetActive(true);

                mosterCount++;
            }
            else if (objactNum == (int)GameSupporter.map2dObject.player)
            {
                PlayerPlane.transform.localPosition = new Vector3(i / gameSupporter.MapZ + 0.5f, 1, i % gameSupporter.MapZ + 0.5f);
            }
            else if (objactNum == (int)GameSupporter.map2dObject.noting)
            {

            }
        }
    }
}
