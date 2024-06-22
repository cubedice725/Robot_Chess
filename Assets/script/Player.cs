using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        int createMovePlanecount = 0;
        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                if (i != 0 || j != 0)
                {
                    if (Mathf.FloorToInt(pythagoras(i,j)) <= radius)
                    {
                        movePlaneInstList[createMovePlanecount].transform.localPosition = new Vector3(i, -0.49f, j);
                        movePlaneInstList[createMovePlanecount].SetActive(true);
                        createMovePlanecount++;
                    }
                }
            }
        }
    }

    private float pythagoras(int pythA, int pythB)
    {
        return Mathf.Sqrt((pythA * pythA) + (pythB * pythB));
    }
}