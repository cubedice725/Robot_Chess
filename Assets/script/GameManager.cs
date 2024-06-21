using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    Map map;
    public void Awake()
    {
        map = GetComponent<Map>();
        map.SetMap(10000);
        map.CreateMap(100, 100);
    }
}

