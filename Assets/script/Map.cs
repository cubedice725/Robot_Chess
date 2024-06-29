using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    MiniMap miniMap;
    GameSupporter gameSupporter;

    // odb: object Detection Box
    public List<GameObject> odbList;
    public List<GameObject> mapBlockInstList;

    // 맵크기
    private int m_width;
    private int m_length;

    public int X { get { return m_width; } set { m_width = value; } }
    public int Z { get { return m_length; } set { m_length = value; } }

    // 맵에 사용할 블록 생성
    public void SetMap()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        miniMap = FindObjectOfType<MiniMap>();

        GameObject mapParent = GameObject.Find("Map");
        GameObject odbParent = GameObject.Find("Object Detection Box");

        GameObject odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;
        GameObject mapBlockPrefab = Resources.Load("Prefab/Map Block Black", typeof(GameObject)) as GameObject;

        for (int i = 0; i < m_width * m_length; i++)
        {
            odbList.Add(Instantiate(odbPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), odbParent.transform));
            odbList[i].SetActive(false);
        }
        for (int i = 0; i < m_width * m_length; i++)
        {
            mapBlockInstList.Add(Instantiate(mapBlockPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), mapParent.transform));
            mapBlockInstList[i].SetActive(false);
        }
        // 2차원 배열 생성
        for (int i = 0; i < m_width * m_length; i++)
        {
            if (i % m_length == 0)
            {
                gameSupporter.map2D.Add(new List<int>());
            }
            gameSupporter.map2D[i / m_length].Add((int)GameSupporter.map2dObject.noting);
        }
    }

    // 실제로 맵 구현
    public void CreateMap()
    {
        for (int i = 0; i < m_width * m_length; i++)
        {
            mapBlockInstList[i].transform.position = new Vector3(i / m_length, 0, i % m_length);
            mapBlockInstList[i].SetActive(true);
        }   
    }

    // 실제로 충돌된 값을 확인하여 map2D에 넣어줌
    public void CheckBox()
    {
        // 0으로 초기화
        for (int i = 0; i < m_width * m_length; i++)
        {
            gameSupporter.map2D[i / m_length][i % m_length] = (int)GameSupporter.map2dObject.noting;
        }
        // 2차원 데이터로 변환
        for (int i = 0; i < m_width * m_length; i++)
        {
            Collision boxC = odbList[i].GetComponent<ObjectDetectionBox>().Box;
            try
            {
                if (boxC.transform.name.StartsWith("Map Block"))
                {
                    gameSupporter.map2D[i / m_length][i % m_length] = (int)GameSupporter.map2dObject.wall;
                }
                else if (boxC.transform.name == "Monster")
                {
                    gameSupporter.map2D[i / m_length][i % m_length] = (int)GameSupporter.map2dObject.moster;
                }
                else if (boxC.transform.name == "Player")
                {
                    gameSupporter.map2D[i / m_length][i % m_length] = (int)GameSupporter.map2dObject.player;
                }
                else
                {
                    gameSupporter.map2D[i / m_length][i % m_length] = (int)GameSupporter.map2dObject.noting;
                }
                odbList[i].GetComponent<ObjectDetectionBox>().Box = null;
            }
            catch /*(NullReferenceException error)*/
            {
                //Debug.Log((i / m_length) + "," + (i % m_length) + "는 충돌되지 않아 NullReferenceException오류 발생, 정상 작동임을 알림");
                //Debug.Log(error);
            }
        }
        // 원위치
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
        miniMap.UpdateMiniMap(m_width, m_length);
        //잘 들어갔는지 확인
        //for (int i = 0; i < m_width * m_length; i++)
        //{
        //    print(i / m_length + "," + i % m_length + ":" + gameSupporter.map2D[i / m_length][i % m_length]);
        //}
    }
    // CheckBox를 생성하여 확인할 준비를 함
    // 해당 함수는 필수적으로 Unity life cycle CollisionXXX 이전에 생성해야함
    public void SetCheckBox()
    {
        for (int i = 0; i < m_width * m_length; i++)
        {
            odbList[i].transform.position = new Vector3(i / m_length, 1, i % m_length);
            odbList[i].SetActive(true);
        }
    }
}
