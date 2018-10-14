using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    //字段---------------------------------
    //对象预设
    private GameObject m_prefab;

    //集合
    List<GameObject> m_objects = new List<GameObject>();

    //名字标识
    public string Name
    {
        get { return m_prefab.name; }
    }

    //方法---------------------------------
    //构造
    public SubPool(GameObject mPrefab)
    {
        this.m_prefab = mPrefab;
    }

    //1.取对象

    public GameObject Spawn()
    {
        GameObject go = null;
        //先看池子里有没有:池子里有就拿
        if (m_objects.Count != 0)
        {
            for (int i = 0; i < m_objects.Count; i++)
            {
                if (!m_objects[i].activeSelf)
                {
                    go = m_objects[i];
                    break;
                }
            }
        }

        //先看池子里有没有:池子里没有就新建
        else
        {
            go = GameObject.Instantiate<GameObject>(m_prefab);
            m_objects.Add(go);
        }

        go.SetActive(true);

        //todo:这点在这里值得商榷:想想怎么改
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        //todo:手段一:获取物件直接的脚本的 OnSpawn


        return go;
    }

    //2.回收对象

    public void Unspawn(GameObject go)
    {
        if (m_objects.Contains(go))
        {
            go.SendMessage("OnUnSpawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    //3.回收池中所有对象
    public void SpawnAll()
    {
        for (int i = 0; i < m_objects.Count; i++)
        {
            if (m_objects[i].activeSelf)
            {
                Unspawn(m_objects[i]);
            }
        }
    }

    //Tools:是List否包含此对象
    //这个函数的意义应该是给外部调用
    //对内部这个函数没什么意义
    public bool Contains(GameObject go)
    {
        return m_objects.Contains(go);
    }
}