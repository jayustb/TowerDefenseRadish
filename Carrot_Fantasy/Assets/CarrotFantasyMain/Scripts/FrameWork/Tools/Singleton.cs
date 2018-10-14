using UnityEngine;

//单例组件
public abstract class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get { return _instance; }
    }

    //protected 可以被继承
    //virtual       可以被复写
    protected virtual void Awake()
    {
        _instance = this as T;
    }
}