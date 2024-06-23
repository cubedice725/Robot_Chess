using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Map : MonoBehaviour
{
    public List<GameObject> mapBlockInstList;

    // �ʿ� ����� ��� ����
    public void SetMap(int mapSetCount)
    {
        GameObject mapParent = GameObject.Find("Map");
        GameObject mapBlockPrefab = Resources.Load("Prefab/Map Block Black", typeof(GameObject)) as GameObject;

        for (int i = 0; i < mapSetCount; i++)
        {
            mapBlockInstList.Add(Instantiate(mapBlockPrefab, mapParent.transform));
            mapBlockInstList[i].transform.position = new Vector3(0, -100, 0);
            mapBlockInstList[i].SetActive(false);
        }
    }

    // ������ �� ����
    public void CreateMap(int width, int length)
    {
        for (int i = 0; i < width * length; i++)
        {
            mapBlockInstList[i].transform.position = new Vector3(i / length, 0, i % length);
            mapBlockInstList[i].SetActive(true);
        }   
    }
}
