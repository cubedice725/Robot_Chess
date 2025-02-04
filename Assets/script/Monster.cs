using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected GameSupporter gameSupporter;
    protected int HP { get; set; }
    protected int ATK { get; set; }
    protected int Defense { get; set; }
   

    public void Awake()
    {
        gameSupporter = FindObjectOfType<GameSupporter>();
        HP = 4;
        ATK = 1;
        Defense = 1;
    }
}
