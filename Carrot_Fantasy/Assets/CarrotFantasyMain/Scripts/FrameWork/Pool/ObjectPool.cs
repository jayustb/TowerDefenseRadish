using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    //资源路径:相对于Resources
    public readonly string RelaDirOfResources = null;

    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();


    //取池子对象
    public GameObject Spawn(string name)
    {
        SubPool pool = null;

        //1.判断有没有, 没有这个子对象池就新建
        if (!m_pools.ContainsKey(name))
        {
            RegisterNew(name);
        }

        //2.肯定有了, 有这个子对象池就用
        pool = m_pools[name];

        return pool.Spawn();
    }


    //回收
    public void Unspawn(GameObject go)
    {
        SubPool pool = null;

        //一次看看各个字典值有没有这个
        //有就拿出来
        foreach (SubPool sp in m_pools.Values)
        {
            if (sp.Contains(go))
            {
                pool = sp;
                break;
            }
        }

        pool.Unspawn(go);
    }


    //回收所有对象
    public void SpawnAll()
    {
        foreach (SubPool sp in m_pools.Values)
        {
            sp.SpawnAll();
        }
    }

    //创建新的子池
    void RegisterNew(string name)
    {
        //1. 得到预设路径
        string path = "";
        if (string.IsNullOrEmpty(RelaDirOfResources))
        {
            path = name;
        }
        else
        {
            path = string.Format("{0}/{1}", RelaDirOfResources, name);
        }

        //2. 加载预设
        GameObject prefab = Resources.Load<GameObject>(path);

        //3. 创建子对象池
        SubPool pool = new SubPool(prefab);
        m_pools.Add(pool.Name, pool);
    }
}