using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Map : MonoBehaviour
{
    public List<GameObject> mapBlockInstList;

    // 멥에 사용할 블록 생성
    public void SetMap(int mapSetCount)
    {
        GameObject mapParent = GameObject.Find("Map");
        GameObject mapBlockprefab = Resources.Load("Prefab/Map Block", typeof(GameObject)) as GameObject;

        for (int i = 0; i < mapSetCount; i++)
        {
            mapBlockInstList.Add(Instantiate(mapBlockprefab, mapParent.transform));
            mapBlockInstList[i].transform.position = new Vector3(0, -100, 0);
            mapBlockInstList[i].SetActive(false);
        }
    }

    // 실제로 멥 구현
    public void CreateMap(int width, int length)
    {
        int createMapcount = 0;
        for (int i = 0;i < length; i++) {
            for (int j = 0; j < width; j++)
            {
                mapBlockInstList[createMapcount].transform.position = new Vector3(j, 0, i);
                mapBlockInstList[createMapcount].SetActive(true);
                createMapcount++;
            }
        }   
    }
}
