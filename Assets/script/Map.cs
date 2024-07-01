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

    // �ʿ� ����� ��� ����
    public void SetMap()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        miniMap = FindObjectOfType<MiniMap>();

        GameObject mapParent = GameObject.Find("Map");
        GameObject odbParent = GameObject.Find("Object Detection Box");

        GameObject odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;
        GameObject mapBlockPrefab = Resources.Load("Prefab/Map Block Black", typeof(GameObject)) as GameObject;

        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            odbList.Add(Instantiate(odbPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), odbParent.transform));
            odbList[i].SetActive(false);
        }
        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            mapBlockInstList.Add(Instantiate(mapBlockPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), mapParent.transform));
            mapBlockInstList[i].SetActive(false);
        }
        // 2���� �迭 ����
        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            if (i % gameSupporter.Length == 0)
            {
                gameSupporter.map2D.Add(new List<int>());
            }
            gameSupporter.map2D[i / gameSupporter.Length].Add((int)GameSupporter.map2dObject.noting);
        }
    }

    // ������ �� ����
    public void CreateMap()
    {
        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            mapBlockInstList[i].transform.position = new Vector3(i / gameSupporter.Length, 0, i % gameSupporter.Length);
            mapBlockInstList[i].SetActive(true);
        }   
    }

    // ������ �浹�� ���� Ȯ���Ͽ� map2D�� �־���
    public void CheckBox()
    {
        // 0���� �ʱ�ȭ
        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            gameSupporter.map2D[i / gameSupporter.Length][i % gameSupporter.Length] = (int)GameSupporter.map2dObject.noting;
        }
        // 2���� �����ͷ� ��ȯ
        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            Collision boxC = odbList[i].GetComponent<ObjectDetectionBox>().Box;
            try
            {
                if (boxC.transform.name.StartsWith("Map Block"))
                {
                    gameSupporter.map2D[i / gameSupporter.Length][i % gameSupporter.Length] = (int)GameSupporter.map2dObject.wall;
                }
                else if (boxC.transform.name == "Monster")
                {
                    gameSupporter.map2D[i / gameSupporter.Length][i % gameSupporter.Length] = (int)GameSupporter.map2dObject.moster;
                }
                else if (boxC.transform.name == "Player")
                {
                    gameSupporter.map2D[i / gameSupporter.Length][i % gameSupporter.Length] = (int)GameSupporter.map2dObject.player;
                }
                else
                {
                    gameSupporter.map2D[i / gameSupporter.Length][i % gameSupporter.Length] = (int)GameSupporter.map2dObject.noting;
                }
                odbList[i].GetComponent<ObjectDetectionBox>().Box = null;
            }
            catch /*(NullReferenceException error)*/
            {
                //Debug.Log((i / gameSupporter.Length) + "," + (i % gameSupporter.Length) + "�� �浹���� �ʾ� NullReferenceException���� �߻�, ���� �۵����� �˸�");
                //Debug.Log(error);
            }
        }
        // ����ġ
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
        miniMap.UpdateMiniMap();
        //�� ������ Ȯ��
        //for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        //{
        //    print(i / gameSupporter.Length + "," + i % gameSupporter.Length + ":" + gameSupporter.map2D[i / gameSupporter.Length][i % gameSupporter.Length]);
        //}
    }
    // CheckBox�� �����Ͽ� Ȯ���� �غ� ��
    // �ش� �Լ��� �ʼ������� Unity life cycle CollisionXXX ������ �����ؾ���
    public void SetCheckBox()
    {
        for (int i = 0; i < gameSupporter.Width * gameSupporter.Length; i++)
        {
            odbList[i].transform.position = new Vector3(i / gameSupporter.Length, 1, i % gameSupporter.Length);
            odbList[i].SetActive(true);
        }
    }
}
