using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Map : MonoBehaviour
{
    public List<GameObject> myInstance;

    public void SetMap(int mapSetCount)
    {
        GameObject prefab = Resources.Load("Prefab/mapBlock", typeof(GameObject)) as GameObject;
        for (int i = 0; i < mapSetCount; i++)
        {
            myInstance.Add(Instantiate(prefab));
            myInstance[i].transform.position = new Vector3(0, -100, 0);
            myInstance[i].SetActive(false);
        }
    }
    public void CreateMap(int width, int length)
    {
        SetMap(width * length);
        int createMapcount = 0;
        for (int i = 0;i < length; i++) {
            for (int j = 0; j < width; j++)
            {
                myInstance[createMapcount].transform.position = new Vector3(j, 0, i);
                myInstance[createMapcount].SetActive(true);
                createMapcount++;
            }
        }   
    }
}
