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
    
    // 맵에 사용할 블록 생성
    public void SetMap()
    {
        // 필요한 컴포넌트, 프리펩 생성
        gameSupporter = FindObjectOfType<GameSupporter>();
        odbParent = GameObject.Find("Object Detection Box");
        odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        // object Detection Box 맵 크기만큼 생성
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            odbList.Add(Instantiate(odbPrefab, new Vector3(0, -100, 0), Quaternion.Euler(Vector3.zero), odbParent.transform));
            odbList[i].SetActive(false);
        }
        gameSupporter.Map2D = new int[gameSupporter.MapX, gameSupporter.MapZ];
    }

    // 실제로 충돌된 값을 확인하여 map2D에 넣어줌
    public void CheckBox()
    {
        // 0으로 초기화
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            gameSupporter.Map2D[i / gameSupporter.MapZ, i % gameSupporter.MapZ] = (int)GameSupporter.map2dObject.noting;
        }
        // 2차원 데이터로 변환
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
                //Debug.Log((i / gameSupporter.MapZ) + "," + (i % gameSupporter.MapZ) + "는 충돌되지 않아 NullReferenceException오류 발생, 정상 작동임을 알림");
                //Debug.Log(error);
            }
        }
        // 원위치
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
    }
    // CheckBox를 생성하여 확인할 준비를 함
    // 해당 함수는 필수적으로 Unity life cycle CollisionXXX 이전에 생성해야함
    public void SetCheckBox()
    {
        for (int i = 0; i < gameSupporter.MapX * gameSupporter.MapZ; i++)
        {
            odbList[i].transform.position = new Vector3(i / gameSupporter.MapZ, 1, i % gameSupporter.MapZ);
            odbList[i].SetActive(true);
        }
    }
}
