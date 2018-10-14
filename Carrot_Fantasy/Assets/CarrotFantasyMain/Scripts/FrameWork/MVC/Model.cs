namespace CarrotFantasyMain.Scripts.FrameWork.MVC
{
    public abstract class Model
    {
        public abstract string Name { get; }

        //发送事件:委托 MVC 完成事件
        protected void SendEvent(string eventName, object data = null)
        {
            MVC.SendEvent(eventName, data);
        }
    }
}