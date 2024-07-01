using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{
    public List<GameObject> movePlaneInstList;
    RaycastHit hit;
    public RaycastHit Hit { set{hit = value;} }

    // Player�� �����϶� Ŭ���� �� �غ�
    public void SetMovePlane(int movePlaneSetCount)
    {
        GameObject movePlaneprefab = Resources.Load("Prefab/Move Plane", typeof(GameObject)) as GameObject;
        for (int i = 0; i < movePlaneSetCount; i++)
        {
            movePlaneInstList.Add(Instantiate(movePlaneprefab, transform));
            movePlaneInstList[i].transform.position = new Vector3(0, -100, 0);
            movePlaneInstList[i].SetActive(false);
        }
    }

    // Player�� ������ ������
    public void PlayerMove()
    {

        transform.position = new Vector3(hit.transform.position.x, 1, hit.transform.position.z);
    }

    // Player�� �����Ҽ� �ִ� ������ �� ����
    public void ReadyPlayerMove(int radius)
    {
        // ���� ���
        int diameter = radius * 2 + 1; 
        for (int i = 0; i < diameter * diameter; i++)
        {
            // ���̳ʽ� ��ǥ�� ���� ���� ����
            int width = (i % diameter) - radius;
            int length = (i / diameter) - radius;

            if (width != 0 || length != 0)
            {
                if (Mathf.FloorToInt(pythagoras(width, length)) <= radius)
                {
                    movePlaneInstList[i].transform.localPosition = new Vector3(width, -0.49f, length);
                    movePlaneInstList[i].SetActive(true);
                }
            }
        }
    }

    private float pythagoras(int pythA, int pythB)
    {
        return Mathf.Sqrt((pythA * pythA) + (pythB * pythB));
    }
}