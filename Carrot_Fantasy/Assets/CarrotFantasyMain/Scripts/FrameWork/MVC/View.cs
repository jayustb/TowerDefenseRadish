using System.Collections.Generic;
using UnityEngine;

//View需要挂载在对象上
namespace CarrotFantasyMain.Scripts.FrameWork.MVC
{
    public abstract class View : MonoBehaviour
    {
        //视图标识
        public abstract string Name { get; }

        //关心的事件列表
        public List<string> AttentionEvent = new List<string>();

        //处理事件函数
        public abstract void HandleEvent(string eventName, object obj = null);

        //获取模型
        protected Model GetModel<T>()
            where T : Model
        {
            return Mvc.GetModel<T>();
        }

        //发送消息
        protected void SendEvent(string eventName, object data = null)
        {
            Mvc.SendEvent(eventName, data);
        }
    }
}