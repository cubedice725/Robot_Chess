using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    protected GameSupporter gameSupporter;
    protected GameObject odbParent;
    protected GameObject odbPrefab;

    // odb: object Detection Box
    protected List<GameObject> odbList = new List<GameObject>();
    
    // �ʿ� ����� ��� ����
    public void SetMap()
    {
        // �ʿ��� ������Ʈ, ������ ����
        gameSupporter = FindObjectOfType<GameSupporter>();
        odbParent = GameObject.Find("Object Detection Box");
        odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        // object Detection Box �� ũ�⸸ŭ ����
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            odbList.Add(Instantiate(odbPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), odbParent.transform));
            odbList[i].SetActive(false);
        }
        gameSupporter.Map2D = new int[gameSupporter.MapX, gameSupporter.MapZ];
    }

    // ������ �浹�� ���� Ȯ���Ͽ� map2D�� �־���
    public void CheckBox()
    {
        // 0���� �ʱ�ȭ
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ] = (int)GameSupporter.map2dObject.noting;
        }
        // 2���� �����ͷ� ��ȯ
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            Collision boxC = odbList[i].GetComponent<ObjectDetectionBox>().Box;
            try
            {
                if (boxC.transform.name.StartsWith("Map Block"))
                {
                    gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ] = (int)GameSupporter.map2dObject.wall;
                }
                else if (boxC.transform.name == "Monster")
                {
                    gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ] = (int)GameSupporter.map2dObject.moster;
                }
                else if (boxC.transform.name == "Player")
                {
                    gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ] = (int)GameSupporter.map2dObject.player;
                }
                else
                {
                    gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ] = (int)GameSupporter.map2dObject.noting;
                }
                odbList[i].GetComponent<ObjectDetectionBox>().Box = null;
            }
            catch /*(NullReferenceException error)*/
            {
                //Debug.Log((i / gameSupporter.MapZ) + "," + (i % gameSupporter.MapZ) + "�� �浹���� �ʾ� NullReferenceException���� �߻�, ���� �۵����� �˸�");
                //Debug.Log(error);
            }
        }
        // ����ġ
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
    }
    // CheckBox�� �����Ͽ� Ȯ���� �غ� ��
    // �ش� �Լ��� �ʼ������� Unity life cycle CollisionXXX ������ �����ؾ���
    public void SetCheckBox()
    {
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            odbList[i].transform.position = new Vector3(i / gameSupporter.MapZ, 1, i % gameSupporter.MapZ);
            odbList[i].SetActive(true);
        }
    }
}
