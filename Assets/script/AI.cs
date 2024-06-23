using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    // odb: object Detection Box
    public List<GameObject> odbList;
    public List<List<Vector3>> check2DList;
    public List<List<int>> map2D = new List<List<int>>();

    // 맵크기
    private int m_width;
    private int m_length;

    public int Width { get { return m_width; } set { m_width = value; } }
    public int Length{ get { return m_length; } set { m_length = value; } }

    // CheckBox를 생성
    public void Setodb(int odbSetCount)
    {
        
        GameObject odbParent = GameObject.Find("Object Detection Box");
        GameObject odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        for (int i = 0; i < odbSetCount; i++)
        {
            odbList.Add(Instantiate(odbPrefab, odbParent.transform));
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
    }

    // 실제로 충돌된 값을 확인하여 map2D에 넣어줌
    public void CheckBox()
    {
        // 값 0을 넣어줌
        for (int i = 0; i < m_width * m_length; i++)
        {
            if (i % m_length == 0)
            {
                map2D.Add(new List<int>());
            }
            map2D[i / m_length].Add(0);
        }

        for (int i = 0; i < m_width * m_length; i++)
        {
            if (odbList[i].GetComponent<ObjectDetectionBox>().Box != Vector3.zero)
            {
                Vector3 boxP = odbList[i].GetComponent<ObjectDetectionBox>().Box;
                map2D[Mathf.FloorToInt(boxP.x)][Mathf.FloorToInt(boxP.z)] = 1;
            }
        }
        //잘 들어갔는지 확인
        for (int i = 0; i < m_width * m_length; i++)
        {
            print(i / m_length + "," + i % m_length + ":" + map2D[i / m_length][i % m_length]);
        }
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
        for (int i = 0; i < m_width * m_length; i++)
        {
            odbList[i].transform.position = new Vector3(i / m_length, 1, i % m_length);
            odbList[i].SetActive(true);
        }
    }



}
