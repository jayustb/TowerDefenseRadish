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

        //关联脚本组件
        map = target as Map;

        //step: 0_示例
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        //step: 1_读取关卡列表 & 选择并读取关卡布局
        //1_1_读取关卡列表
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("读取列表"))
        {
            //读取关卡文件入口函数
            LoadLevelFilesEntry();
        }

        //1_2_选择并读取关卡布局
        int currentSelectedIndex = EditorGUILayout.Popup(0, getNames(_files));
        if (currentSelectedIndex != _selectedIndex)
        {
            _selectedIndex = currentSelectedIndex;
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

    //读取关卡文件入口函数
    void LoadLevelFilesEntry()
    {
        //清除关卡列表
        Clear();

        _files = Tool.GetLevelFiles();

        if (_files.Count > 0)
        {
            _selectedIndex = 0;
            LoadLevel();
        }
    }

    //读取关卡执行函数
    private void LoadLevel()
    {
    }

    //清除关卡列表
    private void Clear()
    {
    }

    //保存设计的关卡路径数据
    private void SaveLevel()
    {
    }

#endregion
}