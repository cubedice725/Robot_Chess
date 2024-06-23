using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Map : MonoBehaviour
{
    public List<GameObject> mapBlockInstList;

    // 맵에 사용할 블록 생성
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

    // 실제로 맵 구현
    public void CreateMap(int width, int length)
    {
        for (int i = 0; i < width * length; i++)
        {
            mapBlockInstList[i].transform.position = new Vector3(i / length, 0, i % length);
            mapBlockInstList[i].SetActive(true);
        }   
    }
}
