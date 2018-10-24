using System.Collections.Generic;

//关卡的总定义类
public class Level
{
    //名字
    public string Name;

    //背景
    public string Background;

    //路径
    public string Road;

    //金币
    public int InitScore;

    //炮塔可放置位置
    public List<Point> Holder;

    //怪物行走路径
    public List<Point> Paths;

    //出怪回合信息
    public List<Round> Rounds;

    public Level()
    {
        Holder = new List<Point>();
        Paths = new List<Point>();
        Rounds = new List<Round>();
    }
}

//回合的定义类
public class Round
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

//格子坐标定义类
public class Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

//地图格子的定义类
public class Tile
{
    public int X;
    public int Y;
    public bool CanHold; //是否可以防止塔
    public object Data; //格子所保存的数据

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    //重写ToString(虽然没什么可重写的)
    public override string ToString()
    {
        return string.Format("[X:{},Y:{},CanHold:{}]", this.X, this.Y, this.CanHold);
    }
}