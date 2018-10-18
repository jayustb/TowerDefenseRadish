using System;
using System.Collections.Generic;

//解决所有的Model \ View的存储问题
//顺便解决一些注册 和 获取 的功能
//最后有 发送事件 的功能
namespace CarrotFantasyMain.Scripts.FrameWork.MVC
{
    public static class MVC
    {
        //1. 存储MVC


        //名字     -> 模型
        public static Dictionary<string, Model> Models = new Dictionary<string, Model>();

        //名字     -> 视图
        public static Dictionary<string, View> Views = new Dictionary<string, View>();

        //事件名 -> 控制器类型
        public static Dictionary<string, Type> CommandMap = new Dictionary<string, Type>();

        //1.1 注册Model
        public static void RegisterModel(Model model)
        {
            Models[model.Name] = model;
        }

        //1.2 注册View
        public static void RegisterView(View view)
        {
            Views[view.Name] = view;
        }

        //1.3 注册Controller
        public static void RegisterControllerl(string eventName, Type controllerType)
        {
            CommandMap[eventName] = controllerType;
        }

        //1.4 获取Model
        public static Model GetModel<T>()
            where T : Model
        {
            foreach (Model model in Models.Values)
            {
                if (model is T)
                {
                    return model;
                }
            }

            return null;
        }

        //1.5 获取View
        public static View GetView<T>()
            where T : View
        {
            foreach (View view in Views.Values)
            {
                if (view is T)
                {
                    return view;
                }
            }

            return null;
        }

        //2. 发送事件
        public static void SendEvent(string eventName, object data = null)
        {
            //控制器响应事件
            if (CommandMap.ContainsKey(eventName))
            {
                Type t = CommandMap[eventName];
                Controller c = Activator.CreateInstance(t) as Controller;

                //控制器执行
                c.Execute(data);
            }

            //视图    响应事件
            foreach (View view in Views.Values)
            {
                if (view.AttentionEvent.Contains(eventName))
                {
                    //若视图包含事件名字
                    //视图响应事件
                    view.HandleEvent(eventName, data);
                }
            }
        }
    }
}