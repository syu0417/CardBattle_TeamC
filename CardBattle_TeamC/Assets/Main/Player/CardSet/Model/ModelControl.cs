using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControl : MonoBehaviour
{
    private bool OnMoveFlg = false;

    private bool MovingFlg = false;

    private Vector3 OldPos  = new Vector3();
    public Vector3 NewCardPos { get; set; } = new Vector3();

    [SerializeField, Header("数値が大きいほど遅くなる")]
    private float MoveSpeed = 100.0f;

    [SerializeField, Header("数値が大きいほど止まる距離が遠くなる(0以上)")]
    private float MoveStopDis = 2.0f;

    // バトルor鏡餅の場合に切り替わるフラグ
    public bool EventFlg { get; set; } = false;
    // 意図的に停止したとき（バトルや鏡餅の直前）に切り替わる
    public bool StopFlg { get; set; }

    private float RemoveTimer = 0.0f;


    private Vector3 diff;//距離図る用

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OnMoveFlg == true)
        {
            Move();
        }
    }
    


    public void Move()
    {
        if(MovingFlg == false)
        {
            OldPos = this.transform.position;

            MovingFlg = true;
        }

        // 移動先に何かがいないか
        if (EventFlg == false)
        {
            // 移動先のカードまで移動する
            if ((this.transform.position - NewCardPos).magnitude > 0.25f)
            {
                this.transform.position += (NewCardPos - OldPos) / MoveSpeed;
                diff = this.transform.position - OldPos;
            }
            else
            {
                this.transform.position = NewCardPos;

                MovingFlg = false;
                OnMoveFlg = false;
            }
        }
        else
        {
            // とある距離まで移動する
            if ((this.transform.position - NewCardPos).magnitude > MoveStopDis)
            {
                this.transform.position += (NewCardPos - OldPos) / MoveSpeed;
            }
            else
            {
                if (RemoveTimer > 300)
                {
                    StopFlg = true;
                    EventFlg = false;
                    MovingFlg = false;
                    OnMoveFlg = false;
                }
                else
                {
                    RemoveTimer++;
                }
            }
        }
    }

    public void SetisMove(bool Flg)
    {
        OnMoveFlg = Flg;
    }
    public void SetisMove(bool Flg, int floorNum)
    {
        OnMoveFlg = Flg;
        
        NewCardPos = new Vector3(((floorNum % 7) * 10.0f), 0.1f, ((floorNum / 7) * 10.0f));
    }

    public bool GetisMove()
    {
        return OnMoveFlg;
    }

    public Vector3 Getdiff()
    {
        return diff;
    }
}
