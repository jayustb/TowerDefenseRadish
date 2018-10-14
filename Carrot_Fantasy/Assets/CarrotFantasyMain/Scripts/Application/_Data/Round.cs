using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//关卡的定义类
internal class Round
{
    //怪物类型ID
    public int Monster;

    //怪物数量
    public int Count;

    //构造关卡
    public Round(int monster, int count)
    {
        Monster = monster;
        Count = count;
    }
}