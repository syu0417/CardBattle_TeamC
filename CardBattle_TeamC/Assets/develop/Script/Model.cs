using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class Model : MonoBehaviour
{
    //スタートと終わりの目印
    [Header("開始地点")]
    public Transform startMarker;
    [Header("目標")]
    public Transform endMarker;

    // スピード
    [Header("スピード")]
    public float speed;

    [SerializeField]
    private float workspeed;
    //二点間の距離を入れる
    [SerializeField, Header("距離計測用")]
    float distance_two;

    [Header("ベースの高さ")]
    public float BaseHight = 2.5f;

    [SerializeField]
    Vector3 Vec;
    [SerializeField]
    private bool isActive = false;
    private Vector3 WorkPos;


    private float PosY;

    private Transform MyTrans = null;

    [SerializeField]
    private bool sts = false;//積み状態管理用フラグ
    private Quaternion Qt;

    // Animator コンポーネント
    private Animator animator;

    private const string key_isIdle = "isIdle";
    private const string key_isWalk = "isWalk";
    void Start()
    {
        // Animatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();

        PosY = BaseHight;
        //プレイヤー(カード)の初期値が入ってから実行したいのでUpdateで1回のみ実行
        this.LateUpdateAsObservable().Take(1).Subscribe(_ => SetInitPosition());


        //isActiveがfalseならその場で待機
        this.UpdateAsObservable().Where(_ => !isActive).
            Subscribe(_ => this.transform.position = new Vector3(WorkPos.x, PosY, WorkPos.z));

        MyTrans = this.GetComponent<Transform>();
        this.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);//前向ける
    }

    void Update()
    {
        if (isActive || distance_two >= 1.0f)
        {
            //二点間の距離を代入(スピード調整に使う)
            distance_two = Vector3.Distance(startMarker.position, endMarker.position);

            if (distance_two <= 1.0f)//停止中の処理をここに記述
            {
                //Debug.Log("停止中");
                this.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);

                // アニメーション状態
                this.animator.SetBool(key_isWalk, false);
                this.animator.SetBool(key_isIdle, true);

                //停止してから積んだ見た目にする
                if (!sts)//積み状態なら
                {
                    PosY = BaseHight;
                }
            }
            else
            {
                //Debug.Log("動いてます");
                // アニメーション状態
                this.animator.SetBool(key_isWalk, true);
                this.animator.SetBool(key_isIdle, false);
            }

            // 現在の位置
            float present_Location = (speed) / distance_two;
            // オブジェクトの移動
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, present_Location);
            //transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

            transform.position = new Vector3(transform.position.x, PosY, transform.position.z);
            WorkPos = transform.position;

           
            SetAngle();//向きをセット

        }
        if (sts)
        {
            PosY = 3.0f;
        }
        else
        {
            PosY = BaseHight;
        }
    }

    public void SetisActive(bool flg)
    {
        isActive = flg;
    }

    public bool GetisActive()
    {
        return isActive;
    }

    public void SetInitPosition()
    {
        Vec = endMarker.position;
        WorkPos = Vec;
        this.transform.position = new Vector3(Vec.x, BaseHight, Vec.z);
    }

    public void SetStatus(bool Status)
    {
        sts = Status;
    }

    public void SetAngle()//向きを変える
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        
    }
}
