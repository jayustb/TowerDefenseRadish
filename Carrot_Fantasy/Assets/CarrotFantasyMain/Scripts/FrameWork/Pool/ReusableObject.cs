using UnityEngine;

///为保证这两个函数一定在子函数中实现
/// 需要这样写上abstract并且不带函数体(即那两个大括号 '{'  && '}'
public abstract class ReusableObject : MonoBehaviour, IReusable
{
    public abstract void OnSpawn();
    public abstract void OnUnSpawn();
}