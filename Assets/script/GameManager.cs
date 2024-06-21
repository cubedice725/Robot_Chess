using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Map map;
    void Start()
    {
        map = GetComponent<Map>();
        map.CreateMap(100, 100);
    }
}

