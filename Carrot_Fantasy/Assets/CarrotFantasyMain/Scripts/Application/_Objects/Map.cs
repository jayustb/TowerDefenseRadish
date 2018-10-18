using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 用于描述一个关卡地图的状态
/// </summary>
public class Map : MonoBehaviour
{
#region 常量

    public const int RowCount = 8; //行数
    public const int ColumnCount = 12; //列数

#endregion

#region 字段

    float _mapWidth; //地图宽
    float _mapHeight; //地图高

    float _tileWidth; //格子宽
    float _tileHeight; //格子高

    public bool DrawGizmos = true; //是否绘制网格

    List<Tile> _grid = new List<Tile>(); //格子集合
    List<Tile> _road = new List<Tile>(); //路径集合

    Level _level; //关卡数据

#endregion

#region 属性

    public Level Level
    {
        get { return _level; }
    }

    public List<Tile> Grid
    {
        get { return _grid; }
    }

    public List<Tile> Road
    {
        get { return _road; }
    }

    //背景图片
    //加载背景的调用set
    public string BackgroundImage
    {
        set
        {
            SpriteRenderer render = transform.Find("BackGround").GetComponent<SpriteRenderer>();
            StartCoroutine(Tool.LoadImage(value, render));
        }
    }

    //路径图片
    //加载背景的调用set
    public string RoadImage
    {
        set
        {
            SpriteRenderer render = transform.Find("Road").GetComponent<SpriteRenderer>();
            StartCoroutine(Tool.LoadImage(value, render));
        }
    }

    //怪物的寻路路径
    public Vector3[] Path
    {
        get
        {
            List<Vector3> path = new List<Vector3>();
            for (int i = 0; i < _road.Count; i++)
            {
                Tile t = _road[i];
                Vector3 point = GetPosition(t);
                path.Add(point);
            }

            return path.ToArray();
        }
    }

#endregion

#region 事件

#endregion

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
        }

        //加载塔位信息
        for (int i = 0; i < level.Holder.Count; i++)
        {
            Point p = level.Holder[i];
            Tile t = GetTile(p.X, p.Y);
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

#region Unity回调

    private Camera _mainCamera;

    //只在运行期起作用
    private void Start()
    {
        //是否要话辅助线
        DrawGizmos = true;

        //主摄像头赋值
        _mainCamera = Camera.main;

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

#endregion

#region 事件回调

#endregion

#region 帮助方法

    //todo:这是DashJay给的方法
    //计算地图大小，格子大小
//    private void CalculateSize(Camera MainCamera)
//    {
//        //获取摄像头视角
//        var halfFov = MainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad;
//        var aspect = MainCamera.aspect;
//        var height = MainCamera.transform.position.z * Mathf.Tan(halfFov);
//        var width = height * aspect;

//        var LeftDown = MainCamera.transform.position - MainCamera.transform.right * width;
//        LeftDown -= MainCamera.transform.up * height;
//        LeftDown += MainCamera.transform.forward * MainCamera.transform.position.z;
//
//        var RightUp = MainCamera.transform.position + MainCamera.transform.right * width;
//        RightUp += MainCamera.transform.up * height;
//        RightUp += MainCamera.transform.forward * MainCamera.transform.position.z;
//
//计算地图大小，格子大小
//        _mapWidth = RightUp.x - LeftDown.x;
//        _mapHeight = RightUp.y - LeftDown.y;
//
//        _tileWidth = _mapWidth / ColumnCount;
//        _tileHeight = _mapHeight / RowCount;
//    }


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
}