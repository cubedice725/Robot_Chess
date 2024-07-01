using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSupporter : MonoBehaviour
{
    public enum map2dObject
    {
        noting = 0,
        wall = 1,
        player = 2,
        moster = 3
    }
    public int[,] Map2D { get; set; }
    public int MapX { get; set; }
    public int MapZ { get; set; }
}
