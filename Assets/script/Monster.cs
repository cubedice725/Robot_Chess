using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : AStar
{
    protected int HP { get; set; }
    protected int ATK { get; set; }
    protected int Defense { get; set; }
    protected int AttackDistance { get; set; }
    protected int MovingDistance { get; set; }

    private void Awake()
    {
        SetMonster();
    }
    private void Update()
    {
        if (gameSupporter.TurnStart)
        {
            Move();
        }
    }
    public virtual void SetMonster()
    {
        SetAStar();
        HP = 4;
        ATK = 1;
        Defense = 1;
        AttackDistance = 1;
        MovingDistance = 1;
    }
    
    public virtual void Move()
    {
        gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.noting;
        PathFinding();
        if (FinalNodeList.Count < AttackDistance + 3 && FinalNodeList.Count != 0)
        {
            print(FinalNodeList.Count);
            attack();
        }
        else if(FinalNodeList.Count != 0)
        {
            for(int i = 1; i <= MovingDistance; i++)
            {
                transform.position = new Vector3Int(FinalNodeList[i].x, (int)transform.position.y, FinalNodeList[i].z);
                gameSupporter.Map2D[(int)transform.position.x, (int)transform.position.z] = (int)GameSupporter.map2dObject.moster;
            }
        }
        else
        {
            Debug.Log("플레이어를 찾을수 없음");
        }
    }
    public virtual void attack()
    {
        print("공격");
    }

}
