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

    // ��ũ��
    private int m_width;
    private int m_length;

    public int X { get { return m_width; } set { m_width = value; } }
    public int Z { get { return m_length; } set { m_length = value; } }

    // �ʿ� ����� ��� ����
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
        // 2���� �迭 ����
        for (int i = 0; i < m_width * m_length; i++)
        {
            if (i % m_length == 0)
            {
                gameSupporter.map2D.Add(new List<int>());
            }
            gameSupporter.map2D[i / m_length].Add((int)GameSupporter.map2dObject.noting);
        }
    }

    // ������ �� ����
    public void CreateMap()
    {
        for (int i = 0; i < m_width * m_length; i++)
        {
            mapBlockInstList[i].transform.position = new Vector3(i / m_length, 0, i % m_length);
            mapBlockInstList[i].SetActive(true);
        }   
    }

    // ������ �浹�� ���� Ȯ���Ͽ� map2D�� �־���
    public void CheckBox()
    {
        // 0���� �ʱ�ȭ
        for (int i = 0; i < m_width * m_length; i++)
        {
            gameSupporter.map2D[i / m_length][i % m_length] = (int)GameSupporter.map2dObject.noting;
        }
        // 2���� �����ͷ� ��ȯ
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
                //Debug.Log((i / m_length) + "," + (i % m_length) + "�� �浹���� �ʾ� NullReferenceException���� �߻�, ���� �۵����� �˸�");
                //Debug.Log(error);
            }
        }
        // ����ġ
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
        miniMap.UpdateMiniMap(m_width, m_length);
        //�� ������ Ȯ��
        //for (int i = 0; i < m_width * m_length; i++)
        //{
        //    print(i / m_length + "," + i % m_length + ":" + gameSupporter.map2D[i / m_length][i % m_length]);
        //}
    }
    // CheckBox�� �����Ͽ� Ȯ���� �غ� ��
    // �ش� �Լ��� �ʼ������� Unity life cycle CollisionXXX ������ �����ؾ���
    public void SetCheckBox()
    {
        for (int i = 0; i < m_width * m_length; i++)
        {
            odbList[i].transform.position = new Vector3(i / m_length, 1, i % m_length);
            odbList[i].SetActive(true);
        }
    }
}
