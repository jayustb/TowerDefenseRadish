using CarrotFantasyMain.Scripts.FrameWork.MVC;

public class StartUpCommand : Controller
{
    /// <summary>
    /// 对于该方法的执行
    /// </summary>
    /// <param name="data"></param>
    public override void Execute(object data)
    {
        // 注册模型_Model

        // 注册命令_Command(Controller)
        RegisterController(Const.E_EnterScene, typeof(ExitSceneCommand));
        RegisterController(Const.E_ExitScene, typeof(ExitSceneCommand));


        // 进入开始界面,先销毁当前场景,并且生成新场景
        Game.Instance.LoadScene(1);
    }
}