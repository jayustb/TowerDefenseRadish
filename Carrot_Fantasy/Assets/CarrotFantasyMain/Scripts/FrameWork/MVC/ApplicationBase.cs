using System;
using UnityEngine;


//这个类的目的:Unknown
namespace CarrotFantasyMain.Scripts.FrameWork.MVC
{
    public abstract class ApplicationBase<T> : Singleton<T>
        where T : MonoBehaviour
    {
        //注册控制器
        protected void RegisterController(string eventName, Type controllerType)
        {
            MVC.RegisterControllerl(eventName, controllerType);
        }

        //执行控制器
        protected void SendEvent(string eventName)
        {
            MVC.SendEvent(eventName);
        }
    }
}