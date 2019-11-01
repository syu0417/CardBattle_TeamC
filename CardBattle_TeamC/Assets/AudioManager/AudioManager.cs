using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField, Range(0, 1), Header("BGMの音量")]
    float bgmVolume = 1;

    [SerializeField, Range(0, 1), Header("SEの音量")]
    float seVolume = 1;


    [SerializeField, Header("現在アクティブなシーン")]
    private string NowSceneName;
    private Scene NowScene;

    [Header("BGMとSEの音源")]
    public AudioClip Bgm_Title;
    public AudioClip Bgm_Mode;
    public AudioClip Bgm_Main;

    public AudioClip Bgm_Win;
    public AudioClip Bgm_Lose;

    public AudioClip Se_Start;
    public AudioClip Se_Click;
    public AudioClip Se_Movecard;
    public AudioClip Se_Movemodel;
    public AudioClip Se_P1atk;
    public AudioClip Se_P2atk;
    public AudioClip Se_P3atk;
    public AudioClip Se_P4atk;
    public AudioClip Se_Jump;
    public AudioClip Se_Dawn;
    public AudioClip Se_Change;
    public AudioClip Se_Goal;
    public AudioClip Se_Put;
    public AudioClip Se_Smoke;
    public AudioClip Se_Cardtouch;
    public AudioClip Se_Pile;
    public AudioClip Se_Cardbreak;
    public AudioClip Se_Timehurry;
    public AudioClip Se_Timeend;
    public AudioClip Se_Turnstart;
    public AudioClip Se_Turnend;
    public AudioClip Se_Matching;
    public AudioClip Se_Error;
    public AudioClip Se_Cancel;
    public AudioClip Se_Win;
    public AudioClip Se_Lose;

    [Header("BGMのオーディオソース")]
    public AudioSource Bgm_audioSource;

    [Header("SEのオーディオソース")]
    public AudioSource Se_audioSource;

    [Header("マネージャオブジェクト")]
    public GameObject gameobject;

    public float Bgm_Volume
    {
        set//volumeセット
        {
            bgmVolume = Mathf.Clamp01(value);
            Bgm_audioSource.volume = bgmVolume;
        }
        get
        {
            return bgmVolume;
        }
    }

    public float Se_Volume
    {
        set
        {
            seVolume = Mathf.Clamp01(value);
            Se_audioSource.volume = seVolume;
        }
        get
        {
            return seVolume;
        }
    }

    //どこからスタートしても読み込む
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]

    //どこからスタートしても読み込む
    
    void Awake()
    {        
        DontDestroyOnLoad(gameobject);//ずっと残しておく

        SceneManager.activeSceneChanged += ActiveSceneChanged;
        NowScene = SceneManager.GetActiveScene();//現在有効なシーンを取得
        NowSceneName = NowScene.name;
        NowSceneChangeBgm();
    }


    void ActiveSceneChanged(Scene thisScene, Scene nextScene)//アクティブシーンが切り替わったら
    {
        NowScene = nextScene;
        NowSceneName = NowScene.name;
        NowSceneChangeBgm();
    }

    

    public void Play_BGM_Win()//勝利した時のBGM(フラグが立ったら1回だけ実行するようにしてください)
    {
        Bgm_audioSource.clip = Bgm_Win;
        Bgm_audioSource.loop = true;
        Bgm_audioSource.volume = bgmVolume;//BGMのボリューム設定
        Bgm_audioSource.Play();
    }

    public void Play_BGM_Lose()//敗北した時のBGM(フラグが立ったら1回だけ実行するようにしてください)
    {
        Bgm_audioSource.loop = false;
        Bgm_audioSource.volume = bgmVolume;//BGMのボリューム設定
        Bgm_audioSource.clip = Bgm_Lose;
        Bgm_audioSource.Play();
    }

    public void Play_SE_Start()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Start);
    }

    public void Play_SE_Click()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Click);
    }

    public void Play_SE_Movecard()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Movecard);
    }

    public void Play_SE_Movemodel()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Movemodel);
    }

    public void Play_SE_P1atk()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_P1atk);
    }

    public void Play_SE_P2atk()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_P2atk);
    }

    public void Play_SE_P3atk()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_P3atk);
    }

    public void Play_SE_P4atk()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_P4atk);
    }

    public void Play_SE_Jump()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Jump);
    }

    public void Play_SE_Dawn()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Dawn);
    }

    public void Play_SE_Change()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Change);
    }

    public void Play_SE_Goal()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Goal);
    }

    public void Play_SE_Put()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Put);
    }

    public void Play_SE_Smoke()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Smoke);
    }

    public void Play_SE_Cardtouch()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Cardtouch);
    }

    public void Play_SE_Pile()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Pile);
    }

    public void Play_SE_Cardbreak()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Cardbreak);
    }

    public void Play_SE_Timehurry()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Timehurry);
    }

    public void Play_SE_Timeend()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Timeend);
    }

    public void Play_SE_Matching()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Matching);
    }

    public void Play_SE_Error()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Error);
    }

    public void Play_SE_Cancel()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Cancel);
    }

    public void Play_SE_Win()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Win);
    }

    public void Play_SE_Lose()
    {
        Se_audioSource.volume = seVolume;//SEの音量
        Se_audioSource.PlayOneShot(Se_Lose);
    }


    public void NowSceneChangeBgm()
    {
        if (NowScene.name == "Title")
        {
            Bgm_audioSource.clip = Bgm_Title;
        }
        else if (NowScene.name == "ModeSelect")
        {
            Bgm_audioSource.clip = Bgm_Mode;
        }
        else if (NowScene.name == "GameMain"|| NowScene.name == "MainCopy")
        {
            Bgm_audioSource.clip = Bgm_Main;
        }
        else
        {
            Bgm_audioSource.clip = null;
        }
        Bgm_audioSource.loop = true;
        Bgm_audioSource.volume = bgmVolume;//BGMのボリューム設定
        Bgm_audioSource.Play();
    }
}
