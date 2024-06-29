using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSupporter : MonoBehaviour
{
    public List<List<int>> map2D = new List<List<int>>();

    public enum map2dObject
    { 
        noting = 0,
        wall = 1,
        player = 2,
        moster = 3
    }
}
