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

    // ��ũ��
    private int m_width;
    private int m_length;

    public int Width { get { return m_width; } set { m_width = value; } }
    public int Length{ get { return m_length; } set { m_length = value; } }

    // CheckBox�� ����
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

    // ������ �浹�� ���� Ȯ���Ͽ� map2D�� �־���
    public void CheckBox()
    {
        // �� 0�� �־���
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
        //�� ������ Ȯ��
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
