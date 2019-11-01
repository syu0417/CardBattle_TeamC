using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class EnemyObj : MonoBehaviour
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

    [SerializeField]
    Vector3 Vec;
    [SerializeField]
    private bool isActive = false;
    private Vector3 WorkPos;


    private float PosY = 1.0f;

    [SerializeField]
    private bool sts = false;//積み状態管理用フラグ
    void Start()
    {
        //プレイヤー(カード)の初期値が入ってから実行したいのでUpdateで1回のみ実行
        this.LateUpdateAsObservable().Take(1).Subscribe(_ => SetInitPosition());


        //isActiveがfalseならその場で待機
        this.UpdateAsObservable().Where(_ => !isActive).
            Subscribe(_ => this.transform.position = new Vector3(WorkPos.x, PosY, WorkPos.z));
    }

    void Update()
    {
        if (isActive || distance_two >= 1.0)
        {
            //二点間の距離を代入(スピード調整に使う)
            distance_two = Vector3.Distance(startMarker.position, endMarker.position);
            if (distance_two <= 1.0)
            {
                //Debug.Log("停止中");

                //停止してから積んだ見た目にする
                if (!sts)//積み状態なら
                {
                    PosY = 1.0f;
                }
            }
            else
            {
                //Debug.Log("動いてます");
            }

            // 現在の位置
            float present_Location = (speed) / distance_two;
            // オブジェクトの移動
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, present_Location);
            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

            transform.position = new Vector3(transform.position.x, PosY, transform.position.z);
            WorkPos = transform.position;
            //if(!sts)
            //{
            //    PosY = 1.0f;
            //}
        }
        if (sts)
        {
            PosY = 3.0f;
        }
        else
        {
            PosY = 1.0f;
        }
        //if(isActive)
        //{

        //}
        //else
        //{
        //    if (!sts)
        //    {
        //        PosY = 1.0f;
        //    }
        //}
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
        this.transform.position = new Vector3(Vec.x, 1.0f, Vec.z);
    }

    public void SetStatus(bool Status)
    {
        sts = Status;
    }
}

