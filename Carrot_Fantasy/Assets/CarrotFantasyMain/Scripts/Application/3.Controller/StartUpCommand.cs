using System;
using CarrotFantasyMain.Scripts.FrameWork.MVC;
using UnityEngine.SceneManagement;

public class StartUpCommand : Controller
{
    public override void Execute(object data)
    {
        // 注册模型_Model

        // 注册命令_Command(Controller)
        RegisterController(Const.E_EnterScene, typeof(EnterSceneCommand));
        RegisterController(Const.E_ExitScene, typeof(ExitSceneCommand));

        // 进入开始界面
        Game.Instance.LoadScene(1);
    }
}