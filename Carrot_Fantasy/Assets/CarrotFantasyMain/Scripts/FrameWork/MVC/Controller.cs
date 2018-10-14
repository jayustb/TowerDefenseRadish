//协调VIew和Model之间的交互
//被动态创建

using System;

namespace CarrotFantasyMain.Scripts.FrameWork.MVC
{
    public abstract class Controller
    {
        //1. 获取模型
        protected Model GetModel<T>()
            where T : Model
        {
            return MVC.GetModel<T>();
        }

        //2. 获取视图
        public static View GetView<T>()
            where T : View
        {
            return MVC.GetView<T>();
        }

        //3. 注册模型    from MVC
        protected void RegisterModel(Model model)
        {
            MVC.RegisterModel(model);
        }

        //4. 注册视图    from MVC
        protected void RegisterView(View view)
        {
            MVC.RegisterView(view);
        }

        //5. 注册控制器 from MVC
        protected void RegisterController(string eventName, Type controllerType)
        {
            MVC.RegisterControllerl(eventName, controllerType);
        }

        //6. 处理系统消息
        public abstract void Execute(object data);
    }
}