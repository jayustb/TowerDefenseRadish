using CarrotFantasyMain.Scripts.FrameWork.MVC;
using UnityEngine;
using UnityEngine.SceneManagement;

// 手动添加依赖,这样会自动添加组件(类),没有的话就自动报错了
[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : ApplicationBase<Game>
{
    //全局访问功能

    //对象池(虽然是单例,但是为了访问简单就全局使用game控制)
    public ObjectPool _ObjectPool;

    //声音控制
    public Sound _Sound;

    //静态数据
    public StaticData _StaticData;


    #region 全局方法

    public void LoadScene(int level)
    {
        //------退出旧场景
        //事件参数
        var e = new SceneArgs {_Scene = SceneManager.GetActiveScene()};

        //发布事件:退出场景使用的是ApplicationBase的方法
        SendEvent(Const.E_ExitScene, e);

        //---加载新场景 听说是加载新的会把之前的删除掉
        //todo:搞懂这个新的API,说是可以叠加多个场景
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        print("Game Start!");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion


    // 游戏入口
    private void Start()
    {
        //防止删除的时候本脚本销毁,本脚本下有许多单例
        DontDestroyOnLoad(gameObject);

        //全局单例赋值
        _ObjectPool = ObjectPool.Instance;
        _Sound = Sound.Instance;
        _StaticData = StaticData.Instance;

        //注册启动命令,启动之前一定注册
        RegisterController(Const.E_StartUp, typeof(StartUpCommand));

        //启动游戏
        SendEvent(Const.E_StartUp);
    }

    //场景加载后运行函数
    private void OnSceneLoaded(Scene scence, LoadSceneMode mod)
    {
        print($"OnSceneLoaded: \t{scence.buildIndex} + {scence.name}");

        //事件参数
        var e = new SceneArgs {_Scene = scence};

        //发布事件:进入场景
        SendEvent(Const.E_EnterScene, e);
    }
}