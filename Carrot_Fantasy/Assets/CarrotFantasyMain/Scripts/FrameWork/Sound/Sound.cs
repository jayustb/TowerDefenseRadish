using UnityEngine;

public class Sound : Singleton<Sound>
{
#region 生命周期

    protected override void Awake()
    {
        base.Awake();

        //bgm原生不播放
        _bgSound = this.gameObject.AddComponent<AudioSource>();
        _bgSound.playOnAwake = false;
        _bgSound.loop = true;

        //efx原生挂载
        _efxSound = this.gameObject.AddComponent<AudioSource>();
    }

#endregion

    //资源路径:相对于Resources
    public readonly string RelaDirOfResources = null;

    private AudioSource _bgSound; //BGM音源
    private AudioSource _efxSound; //EFX音源

    //音乐音量大小
    public float BgVolume
    {
        get { return _bgSound.volume; }
        set { _bgSound.volume = value; }
    }

    //音效音量大小
    public float EfxVolume
    {
        get { return _efxSound.volume; }
        set { _efxSound.volume = value; }
    }

    //播放音乐
    public void PlayBg(string audioname)
    {
        //正在播放的音乐
        string oldName;

        //有无判断
        if (_bgSound.clip == null)
        {
            oldName = "";
        }
        else
        {
            oldName = _bgSound.clip.name;
        }

        if (oldName != audioname)
        {
            //1.音乐路径判断
            string path;

            if (string.IsNullOrEmpty(RelaDirOfResources))
            {
                path = audioname;
            }
            else
            {
                path = string.Format("{0}/{1}", RelaDirOfResources, audioname);
            }

            //2.加载音乐
            AudioClip clip = Resources.Load<AudioClip>(path);

            //3.播放
            if (clip != null)
            {
                _bgSound.clip = clip;
                _bgSound.Play();
            }
        }
    }

    //停止音乐
    public void StopBg()
    {
        _bgSound.Stop();
        _bgSound.clip = null;
    }

    //播放特效
    public void PlayEfx(string audioName)
    {
        //1.get路径
        string path;

        if (string.IsNullOrEmpty(RelaDirOfResources))
        {
            path = "";
        }
        else
        {
            path = string.Format("{0}/{1}", RelaDirOfResources, audioName);
        }

        //2.get音频
        AudioClip clip = Resources.Load<AudioClip>(path);

        //3.let播放
        if (clip != null)
        {
            _efxSound.PlayOneShot(clip);
        }
    }
}