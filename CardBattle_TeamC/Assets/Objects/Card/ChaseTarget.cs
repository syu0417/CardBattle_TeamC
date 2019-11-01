using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UniRx;
using UniRx.Triggers;
public class ChaseTarget : MonoBehaviour
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
    [SerializeField,Header("距離計測用")]
    float distance_two;

    [SerializeField]
    Vector3 Vec;
    [SerializeField]
    private bool isActive = false;
    private Vector3 WorkPos;


    private float PosY=1.0f;

    [SerializeField]
    private bool sts = false;//積み状態管理用フラグ

    [SerializeField]
    private bool StopFlg = false;

    private Vector3 oldPos;

    // 子供にしなくても位置座標を追従してくれるもの
    private ConstraintSource myConstraintSource;
    private PositionConstraint myPositionConstaint;

    private Transform ChildTransform;

    void Start()
    {
        //プレイヤー(カード)の初期値が入ってから実行したいのでUpdateで1回のみ実行
        this.LateUpdateAsObservable().Take(1).Subscribe(_ => SetInitPosition());
        

        //isActiveがfalseならその場で待機
        this.UpdateAsObservable().Where(_=>!isActive).
            Subscribe(_ => this.transform.position = new Vector3(WorkPos.x, PosY, WorkPos.z));

        // 追従用の初期化
        this.gameObject.AddComponent<PositionConstraint>();
        this.myPositionConstaint = GetComponent<PositionConstraint>();

        oldPos = Vector3.zero;

        ChildTransform = null;
    }

    void Update()
    {
        if(isActive|| distance_two >= 1.0)
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

            /* ここにY軸を変更する処理を入れる */



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

            if (GetisFollowing() == true)
                if (GetParentTransform().GetComponent<ChaseTarget>().GetisFollowing() == true)
                    PosY = 5.0f;
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
        
        
        if(transform.localPosition.x < (oldPos.x + 0.1f) && transform.localPosition.x > (oldPos.x - 0.1f) &&
            transform.localPosition.y < (oldPos.y + 0.1f) && transform.localPosition.y > (oldPos.y - 0.1f) &&
            transform.localPosition.z < (oldPos.z + 0.1f) && transform.localPosition.z > (oldPos.z - 0.1f) )//前フレームとポジションが一緒なら
        {
            StopFlg = true;
        }
        else
        {
            StopFlg = false;
            oldPos = transform.localPosition;
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
        this.transform.position = new Vector3(Vec.x, 1.0f, Vec.z);
    }
    
    public void SetStatus(bool Status)
    {
        sts = Status;
    }

    public bool GetisStop()
    {
        return StopFlg;
    }

    public void SetObjectConstraintTransform(Transform parent)
    {
        // もしすでに追従していた場合はそれを削除する（バグ処理）
        this.DeleteSource();

        // Constraint の参照元を設定
        this.myConstraintSource.sourceTransform = parent;
        this.myConstraintSource.weight = 1.0f;  // 影響度：1に近いほど支配度が高い　初期値は0なので修正する

        //#if POSITION_CONSTRAINT
        this.myPositionConstaint.AddSource(this.myConstraintSource);
        this.myPositionConstaint.translationOffset = new Vector3(0,parent.transform.localScale.y,0);
        this.myPositionConstaint.enabled = true;
        this.myPositionConstaint.constraintActive = true;
    }

    public void DeleteSource()
    {
        if (this.myPositionConstaint.sourceCount > 0)
        {
            this.myPositionConstaint.RemoveSource(0);
            this.myPositionConstaint.constraintActive = false;
            //this.myPositionConstaint.enabled = false;
        }
    }

    // 親がいるか
    public bool GetisFollowing()
    {
        return (this.myPositionConstaint.sourceCount > 0);
    }

    public Transform GetParentTransform()
    {
        if (GetisFollowing())
        {
            return this.myPositionConstaint.GetSource(0).sourceTransform;
        }

        return null;
    }
    
    // 子供をセット
    public void SetChildTransform(Transform child)
    {
        ChildTransform = child;
    }

    // 子供を取ってくる
    public Transform GetChildTransform()
    {
        return ChildTransform;
    }
}
