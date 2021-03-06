﻿using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * 地图编辑的约定:
 * 1. 鼠标右键创建或者取消寻路点
 * 2. 鼠标左键创建或者取消放塔点
 */

/*
 * 编辑器在哪个脚本里写:
 * MapEditor只能在Scene下起作用
 * 我们的编辑需求只能在那个Game下操作
 * 所以写在MapEditor里面是没有用的
 * 只能写在Map里面
 */


/// <summary>
/// 用于描述一个关卡地图的状态
/// </summary>
public class Map : MonoBehaviour
{
    #region 常量

    //行列数
    private const int RowCount = 8; //行数
    private const int ColumnCount = 12; //列数

    #endregion

    #region 字段

    private float _mapWidth; //地图宽
    private float _mapHeight; //地图高

    private float _tileWidth; //格子宽
    private float _tileHeight; //格子高

    public bool DrawGizmos = true; //是否绘制网格

    List<Tile> _grid = new List<Tile>(); //格子集合
    List<Tile> _road = new List<Tile>(); //路径集合

    [SerializeField] Level _level; //关卡数据

    #endregion

    #region 属性

    public Level Level => _level;

    public List<Tile> Grid => _grid;

    public List<Tile> Road => _road;

    //背景图片
    //加载背景的调用set,加载完成之后创建协程自动加载
    public string BackgroundImage
    {
        set
        {
            var render = transform.Find("BackGround").GetComponent<SpriteRenderer>();
            StartCoroutine(Tool.LoadImage(value, render));
        }
    }

    //路径图片
    //加载背景的调用set
    public string RoadImage
    {
        set
        {
            var render = transform.Find("Road").GetComponent<SpriteRenderer>();
            StartCoroutine(Tool.LoadImage(value, render));
        }
    }

    //怪物的寻路路径
    public Vector3[] Path
    {
        get
        {
            var path = new List<Vector3>();
            for (var i = 0; i < _road.Count; i++)
            {
                var t = _road[i];
                var point = GetPosition(t);
                path.Add(point);
            }

            return path.ToArray();
        }
    }

    #endregion

    //todo:这里需要去补充C#的基础知识
    //事件是委托的实例


    #region 方法

    //Step: 0 加载场景总方法
    public void LoadLevel(Level level)
    {
        //清除当前状态
        Clear();

        //保存
        this._level = level;

        //加载图片
        this.BackgroundImage = "file://" + Const.ConDir_Map + "/" + level.Background;
        this.RoadImage = "file://" + Const.ConDir_Map + "/" + level.Road;

        //加载寻路点
        for (int i = 0; i < level.Paths.Count; i++)
        {
            Point p = level.Paths[i];
            Tile t = GetTile(p.X, p.Y);
            _road.Add(t);
            print("添加一个寻路点");
        }

        //加载塔位信息
        for (int i = 0; i < level.Holder.Count; i++)
        {
            var p = level.Holder[i];
            var t = GetTile(p.X, p.Y);
            t.CanHold = true;
        }
    }

    //Step: 1 清除所有信息
    public void Clear()
    {
        _level = null;
        ClearHolder();
        ClearRoad();
    }

    //Step: 1.1 清除塔位信息
    public void ClearHolder()
    {
        foreach (Tile t in _grid)
        {
            if (t.CanHold)
                t.CanHold = false;
        }
    }

    //Step: 1.2 清除寻路格子集合
    public void ClearRoad()
    {
        _road.Clear();
    }

    #endregion

    #region 帮助方法

    //todo:这是Kaike的方法
    //计算地图大小，格子大小
    void CalculateSize()
    {
        //获取摄像头视角
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightUp = new Vector3(1, 1);

        if (Camera.main != null)
        {
            Vector3 p1 = Camera.main.ViewportToScreenPoint(leftDown);
            Vector3 p2 = Camera.main.ViewportToScreenPoint(rightUp);
            //计算地图大小，格子大小
            _mapWidth = (p2.x - p1.x);
            _mapHeight = (p2.y - p1.y);
        }
        else
        {
            Debug.LogError($"Camera.Main为空了!!!");
        }

        _tileWidth = _mapWidth / ColumnCount;
        _tileHeight = _mapHeight / RowCount;
    }

    //获取格子中心点所在的世界坐标
    Vector3 GetPosition(Tile t)
    {
        return new Vector3(
            -_mapWidth / 2 + (t.X + 0.5f) * _tileWidth,
            -_mapHeight / 2 + (t.Y + 0.5f) * _tileHeight,
            0
        );
    }

    //根据格子索引号获得格子
    Tile GetTile(int tileX, int tileY)
    {
        var index = tileX + tileY * ColumnCount;

        if (index < 0 || index >= _grid.Count)
            return null;

        return _grid[index];
    }

    //获取鼠标下面的格子
    Tile GetTileUnderMouse()
    {
        Vector2 wordPos = GetWorldPosition();
        var col = (int) ((wordPos.x + _mapWidth / 2) / _tileWidth);
        var row = (int) ((wordPos.y + _mapHeight / 2) / _tileHeight);
        return GetTile(col, row);
    }

    //获取鼠标所在位置的世界坐标
    Vector3 GetWorldPosition()
    {
        Vector3 worldPos = new Vector3(0, 0, 0);
        //todo:可以一步到位,尝试新的api???
        if (Camera.main != null)
        {
            var viewPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        }
        else
        {
            Debug.LogError("Camera.Main Lost!!!");
        }

        return worldPos;
    }

    #endregion

    #region Unity回调(UnityEngine的相关函数)

    //只在运行期起作用
    private void Awake()
    {
        //计算地图和格子大小
//        CalculateSize(MainCamera);
        CalculateSize();

        //创建所有的格子
        for (var i = 0; i < RowCount; i++)
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                _road.Add(new Tile(j, i));
                _grid.Add(new Tile(j, i));
            }
        }

        //监听鼠标点击事件
        OnTileClick += Map_OnTileClick;
    }

    //只在编辑器里起作用(由DrawGizmos提供效果)
    void OnDrawGizmos()
    {
        //根据全局判断是否需要绘制这个GIzmo
        if (!DrawGizmos)
            return;

        //计算地图和格子大小
//        CalculateSize(MainCamera);
        CalculateSize();

        //绘制格子
        Gizmos.color = Color.green;

        //绘制行
        for (var row = 0; row <= RowCount; row++)
        {
            var from = new Vector2(-_mapWidth / 2, -_mapHeight / 2 + row * _tileHeight);
            var to = new Vector2(-_mapWidth / 2 + _mapWidth, -_mapHeight / 2 + row * _tileHeight);
            Gizmos.DrawLine(from, to);
        }

        //绘制列
        for (var col = 0; col <= ColumnCount; col++)
        {
            var from = new Vector2(-_mapWidth / 2 + col * _tileWidth, _mapHeight / 2);
            var to = new Vector2(-_mapWidth / 2 + col * _tileWidth, -_mapHeight / 2);
            Gizmos.DrawLine(from, to);
        }

        //加载_Grid 以及 _Grid的贴图
        foreach (var t in _grid)
        {
            if (t.CanHold)
            {
                var pos = GetPosition(t);
                Gizmos.DrawIcon(pos, "holder.png", true);
            }
        }

        //设置Gizmo的颜色
        Gizmos.color = Color.red;

        //路途点的绘制
        for (var i = 0; i < _road.Count; i++)
        {
            //起点
            if (i == 0)
            {
                Gizmos.DrawIcon(GetPosition(_road[i]), "start.png", true);
            }

            //终点
            if (_road.Count > 1 && i == _road.Count - 1)
            {
                Gizmos.DrawIcon(GetPosition(_road[i]), "end.png", true);
            }

            //红色的连线
            if (_road.Count > 1 && i != 0)
            {
                var from = GetPosition(_road[i - 1]);
                var to = GetPosition(_road[i]);
                Gizmos.DrawLine(from, to);
            }
        }
    }

    private void Update()
    {
        //鼠标点击事件函数
        MouseClickEvent();
    }

    #endregion

    #region 事件

    //鼠标点击事件
    public event EventHandler<TileClickEventArgs> OnTileClick;

    //Update中的鼠标点击事件函数体
    //todo:在这里实现类似编辑器的连拖编辑
    private void MouseClickEvent()
    {
        //鼠标左键检测
        if (Input.GetMouseButtonDown(0))
//        if (Input.GetMouseButton(0))
        {
            var t = GetTileUnderMouse();

            if (t != null)
            {
                //触发鼠标左键点击事件
                var e = new TileClickEventArgs(0, t);

                //判断是否为空,否则执行
                OnTileClick?.Invoke(this, e);
            }
        }

        //鼠标右键检测
        if (Input.GetMouseButtonDown(1))
//        if (Input.GetMouseButton(1))
        {
            var t = GetTileUnderMouse();

            if (t != null)
            {
                //触发鼠标右键点击事件
                var e = new TileClickEventArgs(1, t);
                OnTileClick?.Invoke(this, e);
            }
        }
    }

    #endregion

    #region 事件回调

    //鼠标点击事件
    void Map_OnTileClick(object sender, TileClickEventArgs e)
    {
        if (Level == null)
            return;

        //鼠标左键对 <放塔点编辑> 的实现,并且点击到的地图不在寻路点中
        if (e.MouseButton == 0 && !Road.Contains(e.Tile))
        {
            e.Tile.CanHold = !e.Tile.CanHold;
        }

        //鼠标右键对 <路径点编辑> 的实现,点击到的地图不能是一个放塔点
        if (e.MouseButton == 1 && !e.Tile.CanHold)
        {
            if (_road.Contains(e.Tile))
            {
                _road.Remove(e.Tile);
            }
            else
            {
                _road.Add(e.Tile);
            }
        }
    }

    #endregion
}

// 鼠标点击参数类
public class TileClickEventArgs : EventArgs
{
    //鼠标点击状态: 0 左键 1 右键
    public int MouseButton;

    //当前的方块格子
    public Tile Tile;

    public TileClickEventArgs(int mouseButton, Tile tile)
    {
        MouseButton = mouseButton;
        Tile = tile;
    }
}