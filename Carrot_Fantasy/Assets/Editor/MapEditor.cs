using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    //像其他的Component一样隐藏脚本文件
    [HideInInspector] Map map = null;

    //将关卡文件列出来
    List<FileInfo> _files = new List<FileInfo>();

    //正在编辑的关卡索引
    private int _selectedIndex = -1;


    //绘制 Inspector 的主进程
    public override void OnInspectorGUI()
    {
        //先继承再搞事
        base.OnInspectorGUI();

        //这个if控制的是只有在 运行模式 才打开
        if (Application.isPlaying)
        {
            //关联脚本组件
            map = target as Map;

            //step: 0_示例
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();

            //step: 1_读取关卡列表 & 选择并读取关卡布局
            EditorGUILayout.BeginHorizontal();

            //1_1_新建关卡
            if (GUILayout.Button("新建列表"))
            {
                //新建关卡文件入口函数
            }

            //1_2_读取关卡列表
            if (GUILayout.Button("读取列表"))
            {
                //读取关卡文件入口函数
                LoadLevelFilesEntry();
            }

            //1_3_选择并读取关卡布局
            int currentSelectedIndex = EditorGUILayout.Popup(_selectedIndex, getNames(_files));
            if (currentSelectedIndex != _selectedIndex)
            {
                _selectedIndex = currentSelectedIndex;
                // 选择后Load
                LoadLevel();
            }

            EditorGUILayout.EndHorizontal();

            //step: 2_清除组件集合


            //2_1_清除塔点
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清除塔点"))
            {
                map.ClearHolder();
            }

            //2_2_清除路径
            if (GUILayout.Button("清除路径"))
            {
                map.ClearRoad();
            }

            EditorGUILayout.EndHorizontal();

            //step: 3_保存路径数据
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存路径"))
            {
                SaveLevel();
            }

            EditorGUILayout.EndHorizontal();
        }

        // 实时更新:
        if (GUI.changed)
        {
            //这个Dirty的意思是 ready to get clear updated
            EditorUtility.SetDirty(target);
        }
    }


#region 工具函数集合

    // 获取关卡名 : 从关卡文件xml获取关卡名
    string[] getNames(List<FileInfo> fileInfos)
    {
        List<string> Names = new List<string>();
        foreach (var info in fileInfos)
        {
            Names.Add(info.Name);
        }

        return Names.ToArray();
    }

    // 读取关卡文件入口函数
    void LoadLevelFilesEntry()
    {
        //清除关卡列表
        Clear();

        //真正加载列表
        _files = Tool.GetLevelFiles();

        if (_files.Count > 0)
        {
            _selectedIndex = 0;
            LoadLevel();
        }
    }

    // 读取关卡执行函数
    private void LoadLevel()
    {
        //读取 关卡 file
        FileInfo file = _files[_selectedIndex];

        Level level = new Level();
        Tool.FillLevel(file.FullName, ref level);

        map.LoadLevel(level);
    }

    // 清除关卡列表
    private void Clear()
    {
        _files.Clear();
        _selectedIndex = -1;
    }

    // 保存设计的关卡路径数据
    private void SaveLevel()
    {
        // 获取当前加载的关卡
        Level level = map.Level;

        // tmp 索引点
        List<Point> list;

        // 收集塔点
        list = new List<Point>();
        for (int i = 0; i < map.Grid.Count; i++)
        {
            Tile t = map.Grid[i];
            if (t.CanHold)
            {
                Point p = new Point(t.X, t.Y);
                list.Add(p);
            }
        }

        level.Holder = list;

        // 收集路径
        list = new List<Point>();
        for (int i = 0; i < map.Road.Count; i++)
        {
            Tile t = map.Road[i];
            Point p = new Point(t.X, t.Y);
            list.Add(p);
        }

        level.Paths = list;


        // 获取路径
        string fileName = _files[_selectedIndex].FullName;

        // 保存
        Tool.SaveLevel(fileName, level);

        // 弹框提示
        EditorUtility.DisplayDialog("保存关卡数据", "保存成功", "确定");
    }

#endregion
}