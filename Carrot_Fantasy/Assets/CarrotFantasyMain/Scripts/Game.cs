using CarrotFantasyMain.Scripts.FrameWork.MVC;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using static System.Diagnostics.Debug;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : ApplicationBase<Game>
{
    //全局访问功能
    public ObjectPool _ObjectPool = null; //对象池
    public Sound _Sound = null; //声音控制
    public StaticData _StaticData = null; //静态数据


#region 全局方法

    public void LoadScene(int level)
    {
        //---退出旧场景
        //事件参数
        SceneArgs e = new SceneArgs {_Scene = SceneManager.GetActiveScene()};

        //发布事件:退出场景
        SendEvent(Const.E_ExitScene, e);

        //---加载新场景 听说是加载新的会把之前的删除掉
        //todo:搞懂这个新的API
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    void OnEnable()
    {
        print("Game Start!");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

#endregion


    //游戏入口
    void Start()
    {
        //全局单例赋值
        _ObjectPool = ObjectPool.Instance;
        _Sound = Sound.Instance;
        _StaticData = StaticData.Instance;

        //注册启动命令
        RegisterController(Const.E_StartUp, typeof(StartUpCommand));

        //启动游戏
        SendEvent(Const.E_StartUp);
    }

    //场景加载后运行函数
    void OnSceneLoaded(Scene scence, LoadSceneMode mod)
    {
        print($"OnSceneLoaded: \t{scence.buildIndex} + {scence.name}");

        //事件参数
        SceneArgs e = new SceneArgs();
        e._Scene = scence;

        //发布事件:进入场景
        SendEvent(Const.E_EnterScene, e);
    }
}

public class SceneArgs
{
    //场景索引号(0 1 2 3 4)
    public Scene _Scene;

    public SceneArgs()
    {
    }
}