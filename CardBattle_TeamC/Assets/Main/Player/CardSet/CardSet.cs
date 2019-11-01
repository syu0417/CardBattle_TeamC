using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Networking;

public class CardSet : NetworkBehaviour
{
    [SerializeField]
    private GameObject gameManager = null;
    public GameObject GameManager
    {
        get
        {
            return gameManager;
        }
        set
        {
            gameManager = value;
        }
    }


    // 選択されているか
    [SerializeField]
    private bool SelectFlg = false;

    // 移動しているか
    private bool OnMoveFlg = false;

    // 次の移動先
    [SyncVar]
    private int NewFloorNum = -1;

    // 所持している力
    [SyncVar]
    private int Power = 1;

    // カードを判別する一意の数値。同じ強さでも違う数値になっている
    [SerializeField]
    private int cardNum = 0;

    public int CardNum
    {
        get
        {
            return cardNum;
        }
        set
        {
            cardNum = value;
        }
    }


    // 親子関係(追従)用
    private ConstraintSource MyConstraintSource;
    private PositionConstraint MyPositionConstraint;

    private Transform ChildTrans = null;

    // 使う力を格納
    [SerializeField, Header("実際に使う力（合計値）")]
    private int TotalPower = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<PositionConstraint>();
        MyPositionConstraint = GetComponent<PositionConstraint>();

        TotalPower = Power;
    }

    // Update is called once per frame
    void Update()
    {
        // マネージャーから移動の通知
        if (OnMoveFlg)
        {
            CallCmdFunction();

            OnMoveFlg = false;
        }
    }

    public void Move()
    {
        this.transform.GetChild(0).GetComponent<CardColtrol>().SetisMove(true, NewFloorNum);
        this.transform.GetChild(1).GetComponent<ModelControl>().SetisMove(true, NewFloorNum);
        
    }

    private void MoveEvent()
    {
        this.transform.GetChild(0).GetComponent<CardColtrol>().SetisMove(true, NewFloorNum);
        this.transform.GetChild(1).GetComponent<ModelControl>().SetisMove(true, NewFloorNum);

        this.transform.GetChild(1).GetComponent<ModelControl>().EventFlg = true;
        
    }

    public void SetisSelect(bool Flg)
    {
        SelectFlg = Flg;
    }

    public bool GetisSelect()
    {
        return SelectFlg;
    }

    public int GetPower()
    {
        return Power;
    }

    public void SetPower(int power)
    {
        Power = power;

        TotalPower = Power;
    }

    public int GetTotalPower()
    {
        return TotalPower;
    }

    public void SetisMove(bool Flg)
    {
        OnMoveFlg = Flg;
    }

    public void SetisMove(bool Flg, int floorNum)
    {
        OnMoveFlg = Flg;

        NewFloorNum = floorNum;
    }

    public bool GetisMove()
    {
        return OnMoveFlg;
    }

    /* 子供を持っているか */
    // 格納されている子供情報を読み出す(いないならNULL)
    public Transform GetChildTrans()
    {
        return ChildTrans;
    }

    // 子供情報をセットする
    public void SetChildTrans(Transform child)
    {
        ChildTrans = child;

        // 子供にしたカードの合計の力を追加
        TotalPower = Power + child.GetComponent<CardSet>().GetTotalPower();
    }
    /*        end          */

    // 追従する親をセット
    public void SetConstraintTransform(Transform parent)
    {
        // 対象のオブジェクトに追従する
        MyConstraintSource.sourceTransform = parent;
        MyConstraintSource.weight = 1.0f;

        MyPositionConstraint.AddSource(MyConstraintSource);
        MyPositionConstraint.translationOffset = Vector3.zero;
        MyPositionConstraint.enabled = true;
        MyPositionConstraint.constraintActive = true;
    }

    // 追従している親がいるか
    public bool IsParentSourceOf()
    {
        return this.MyPositionConstraint.sourceCount > 0;
    }

    // 追従を解除する
    public void DeleteSource()
    {
        if (this.MyPositionConstraint.sourceCount > 0)
        {
            this.MyPositionConstraint.RemoveSource(0);
            this.MyPositionConstraint.constraintActive = false;
        }
    }

    private void CallCmdFunction()
    {
        // 移動するかを判定
        bool MoveFlg = false;

        // 移動予定の床のオブジェクトを検索
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (obj.GetComponent<FloorControl>().GetFloorNum() == NewFloorNum)
            {
                // レイを床の上方向に出す
                Ray ray = new Ray(new Vector3(obj.transform.position.x, obj.transform.position.y - 5.0f, obj.transform.position.z),
                    new Vector3(0.0f, 10.0f, 0.0f));


                // 移動予定の床の上に他のカードがあるか検索
                foreach (RaycastHit hit in Physics.RaycastAll(ray, 50.0f))
                {
                    // カードがあるなら判定
                    if (hit.collider.tag == "Card")
                    {
                        // 相手が追従しているオブジェクトなら処理しない(ループに戻る)
                        if (hit.collider.transform.parent.GetComponent<CardSet>().IsParentSourceOf())
                        {
                            // ループに戻る
                            continue;
                        }


                        // 同じクライアントの場合(重なる処理)
                        if (hit.collider.transform.parent.GetComponent<CardSet>().GameManager.tag == GameManager.tag)
                        {
                            print("鏡餅！！！");

                            /* 重なる処理 */
                            AudioManager.Instance.Play_SE_Pile();

                            // [CardSet]のtransformを送る
                            //Overlap(hit.collider.transform);
                            //RpcMoveOverlap(hit.collider.transform.parent);

                            CmdMoveOverlap(hit.collider.transform.parent.GetComponent<CardSet>().CardNum);
                            
                            //MoveOverlap(hit.collider.transform.parent);

                            MoveFlg = true;

                            break;
                        }
                        // 違うクライアントの場合(バトル処理)
                        else
                        {
                            print("バトル！！！");

                            /* バトル処理 */

                            // [CardSet]のtransformを送る
                            // Battle(hit.collider.transform);
                            //RpcMoveBattle(hit.collider.transform.parent);


                            CmdMoveBattle(hit.collider.transform.parent.GetComponent<CardSet>().CardNum, NewFloorNum);
                            //MoveBattle(hit.collider.transform.parent);

                            MoveFlg = true;

                            break;
                        }
                    }
                }

                // 重なるのもバトルもしないなら移動処理をする
                if (MoveFlg == false)
                {
                    CmdMove(NewFloorNum);
                    Move();
                }

                break;
            }
        }
    }

    [Command]
    private void CmdMove(int newFloorNum)
    {
        print("COMMAND");

        // 移動先の床番号を同期
        NewFloorNum = newFloorNum;

        if (isServer)
        {
            // 移動処理
            Move();
        }



        // 各接続に対して情報を送信する
        foreach (var conn in NetworkServer.connections)
        {
            // 無効なConnectionは無視する
            if (conn == null || !conn.isReady)
                continue;

            // 自分（情報発信元）に送り返してもしょうがない（というかむしろ有害）なので、
            // 自分へのConnectionは無視する
            //if (conn == connectionToClient)
            //    continue;

            // このConnectionに対して位置情報を送信する
            //this.MoveOverlap(RideCardSet.transform);
            this.GameManager.GetComponent<CardManager>().TargetMove(conn, this.CardNum);
        }

        //// クライアントでも処理をする
        //RpcMove(newFloorNum);
    }

    [Command]
    private void CmdMoveOverlap(int RideCardNum)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

        CardSet RideCardSet = null;

        // 重なるカードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.GameManager.tag)
            {
                if (cardset.GetComponent<CardSet>().CardNum == RideCardNum)
                {
                    // 検索結果を一時格納
                    RideCardSet = cardset.GetComponent<CardSet>();

                    // 無駄にループさせない
                    break;
                }
            }
        }

        // 各接続に対して情報を送信する
        foreach (var conn in NetworkServer.connections)
        {
            // 無効なConnectionは無視する
            if (conn == null || !conn.isReady)
                continue;

            // 自分（情報発信元）に送り返してもしょうがない（というかむしろ有害）なので、
            // 自分へのConnectionは無視する
            //if (conn == connectionToClient)
            //    continue;

            // このConnectionに対して位置情報を送信する
            //this.MoveOverlap(RideCardSet.transform);
            this.GameManager.GetComponent<CardManager>().TargetOverlap(conn, this.CardNum, RideCardNum);
        }
        

        // クライアントでも処理をする
        //RpcMoveOverlap(RideCardNum, newFloorNum);
    }

    [Command]
    private void CmdMoveBattle(int EnemyCardNum, int newFloorNum)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

        CardSet EnemyCardSet = null;

        // 移動先の床番号を同期
        NewFloorNum = newFloorNum;

        // 敵カードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.GameManager.tag)
            {
                if (cardset.GetComponent<CardSet>().CardNum == EnemyCardNum)
                {
                    // 検索結果を一時格納
                    EnemyCardSet = cardset.GetComponent<CardSet>();

                    // 無駄にループさせない
                    break;
                }
            }
        }


        // 各接続に対して情報を送信する
        foreach (var conn in NetworkServer.connections)
        {
            // 無効なConnectionは無視する
            if (conn == null || !conn.isReady)
                continue;

            // 自分（情報発信元）に送り返してもしょうがない（というかむしろ有害）なので、
            // 自分へのConnectionは無視する
            //if (conn == connectionToClient)
            //    continue;

            // このConnectionに対して位置情報を送信する
            //this.MoveOverlap(RideCardSet.transform);
            this.GameManager.GetComponent<CardManager>().TargetOverlap(conn, this.CardNum, EnemyCardNum);
        }

        // クライアントでも処理をする
        //RpcMoveBattle(EnemyCardNum, newFloorNum);
    }


    //[Server]
    //private void RpcMove(int newFloorNum)
    //{
    //    print("CLIENTRPC");

    //    // 移動先の床番号を同期
    //    NewFloorNum = newFloorNum;

    //    // 移動処理
    //    //Move();

    //}

    //[Server]
    //private void RpcMoveOverlap(int RideCardNum, int newFloorNum)
    //{
    //    GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

    //    CardSet RideCardSet = null;

    //    // 移動先の床番号を同期
    //    NewFloorNum = newFloorNum;

    //    // 重なるカードを検索
    //    foreach (GameObject cardset in obj)
    //    {
    //        // 検索出来たら処理をする
    //        if (cardset.GetComponent<CardSet>().GameManager.tag == this.GameManager.tag)
    //        {
    //            if (cardset.GetComponent<CardSet>().CardNum == RideCardNum)
    //            {
    //                // 検索結果を一時格納
    //                RideCardSet = cardset.GetComponent<CardSet>();

    //                // 無駄にループさせない
    //                break;
    //            }
    //        }
    //    }
        
    //    this.MoveOverlap(RideCardSet.transform);
    //}

    //[Server]
    //private void RpcMoveBattle(int EnemyCardNum, int newFloorNum)
    //{
    //    GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

    //    CardSet EnemyCardSet = null;

    //    // 移動先の床番号を同期
    //    NewFloorNum = newFloorNum;

    //    // 敵カードを検索
    //    foreach (GameObject cardset in obj)
    //    {
    //        // 検索出来たら処理をする
    //        if (cardset.GetComponent<CardSet>().GameManager.tag == this.GameManager.tag)
    //        {
    //            if (cardset.GetComponent<CardSet>().CardNum == EnemyCardNum)
    //            {
    //                // 検索結果を一時格納
    //                EnemyCardSet = cardset.GetComponent<CardSet>();

    //                // 無駄にループさせない
    //                break;
    //            }
    //        }
    //    }

    //    this.MoveBattle(EnemyCardSet.transform);
    //}



    public void MoveOverlap(Transform Parent)
    {
        Overlap(Parent);
        MoveEvent();
    }

    public void MoveBattle(Transform Enemy)
    {
        Battle(Enemy);
        MoveEvent();
    }

    // 重なる処理
    private void Overlap(Transform Parent)
    {
        // 子供がいる同士、片方でも孫がいる場合は重ならない
        if ((Parent.GetComponent<CardSet>().GetChildTrans() == null || this.GetChildTrans() == null) &&
            (Parent.GetComponent<CardSet>().GetChildTrans() == null || Parent.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().GetChildTrans() == null) ||
            (this.GetChildTrans() == null || this.GetChildTrans().GetComponent<CardSet>().GetChildTrans() == null))
        {
            // 親に既に子供がいる場合
            if (Parent.GetComponent<CardSet>().GetChildTrans() != null)
            {
                // 移動先カードの子供をを親として登録する
                this.SetConstraintTransform(Parent.GetComponent<CardSet>().GetChildTrans().GetChild(0));

                // 親カードに自分を子供として登録する(処理内でカードの力の合計値を変更している)
                Parent.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().SetChildTrans(this.transform);


                // 頭にキャラを追加表示
                for (int child = 0; child < Parent.GetChild(1).childCount; child++)
                {
                    if (Parent.GetChild(1).GetChild(child).gameObject.activeInHierarchy)
                    {
                        Parent.GetChild(1).GetChild(child).GetComponent<ChangeHead>().ChangeHeads3(this.GetPower() - 1);
                    }
                }
            }
            // 親に子供がいない場合
            else
            {
                // 移動先カードを親として登録する
                this.SetConstraintTransform(Parent.GetChild(0));

                // 親カードに自分を子供として登録する(処理内でカードの力の合計値を変更している)
                Parent.GetComponent<CardSet>().SetChildTrans(this.transform);

                // 自分に子供がいる場合
                if (this.GetChildTrans() != null)
                {
                    // 頭にキャラを追加表示
                    for (int child = 0; child < Parent.GetChild(1).childCount; child++)
                    {
                        if (Parent.GetChild(1).GetChild(child).gameObject.activeInHierarchy)
                        {
                            Parent.GetChild(1).GetChild(child).GetComponent<ChangeHead>().ChangeHeads2(this.GetPower() - 1);
                            Parent.GetChild(1).GetChild(child).GetComponent<ChangeHead>().ChangeHeads3(this.GetChildTrans().GetComponent<CardSet>().GetPower() - 1);
                        }
                    }
                }
                // 子供がいない場合
                else
                {
                    // 頭にキャラを追加表示
                    for (int child = 0; child < Parent.GetChild(1).childCount; child++)
                    {
                        if (Parent.GetChild(1).GetChild(child).gameObject.activeInHierarchy)
                        {
                            Parent.GetChild(1).GetChild(child).GetComponent<ChangeHead>().ChangeHeads2(this.GetPower() - 1);
                        }
                    }
                }
            }

            // モデルを消す
            this.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    // バトル処理
    private void Battle(Transform Enemy)
    {
        if (TotalPower == 3 && Enemy.GetComponent<CardSet>().GetTotalPower() == 10 &&
            this.GetChildTrans() != null && this.GetChildTrans().GetComponent<CardSet>().GetChildTrans() != null)
        {
            //Destroy(Enemy.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().GetChildTrans().gameObject);
            //Destroy(Enemy.GetComponent<CardSet>().GetChildTrans().gameObject);
            //Destroy(Enemy.gameObject);

            CmdDestroyCard(Enemy.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().CardNum);
            CmdDestroyCard(Enemy.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().CardNum);
            CmdDestroyCard(Enemy.GetComponent<CardSet>().CardNum);
        }
        else if (TotalPower == 10 && Enemy.GetComponent<CardSet>().GetTotalPower() == 3 &&
            Enemy.GetComponent<CardSet>().GetChildTrans() != null &&
            Enemy.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().GetChildTrans() != null)
        {
            //Destroy(this.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().GetChildTrans().gameObject);
            //Destroy(this.GetComponent<CardSet>().GetChildTrans().gameObject);
            //Destroy(this.gameObject);

            CmdDestroyCard(this.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().CardNum);
            CmdDestroyCard(this.GetComponent<CardSet>().GetChildTrans().GetComponent<CardSet>().CardNum);
            CmdDestroyCard(this.GetComponent<CardSet>().CardNum);
        }
        // 操作カードが負ける場合
        else if (TotalPower < Enemy.GetComponent<CardSet>().GetTotalPower())
        {
            // 操作カードに追従しているカードがいる場合
            if (this.GetChildTrans() != null)
            {
                // 追従を解除させる
                ChildCancelFoliowing(this);
            }

            // 親のカードを削除
            //Destroy(this.gameObject);
            CmdDestroyCard(this.CardNum);
        }
        // 操作カードが勝つ場合
        else if (TotalPower > Enemy.GetComponent<CardSet>().GetTotalPower())
        {
            // 相手カードに追従しているカードがいる場合
            if (Enemy.GetComponent<CardSet>().GetChildTrans() != null)
            {
                // 追従を解除させる
                ChildCancelFoliowing(Enemy.GetComponent<CardSet>());
            }

            // 親のカードを削除
            //Destroy(Enemy.gameObject);
            CmdDestroyCard(Enemy.GetComponent<CardSet>().CardNum);
        }
        // 同士うちの場合
        else
        {
            /* 操作カードの処理 */
            // 操作カードに追従しているカードがいる場合
            if (this.GetChildTrans() != null)
            {
                // 追従を解除させる
                ChildCancelFoliowing(this);
            }

            // 親のカードを削除
            //Destroy(this.gameObject);
            CmdDestroyCard(this.GetComponent<CardSet>().CardNum);


            /* 相手カードの処理 */
            // 相手カードに追従しているカードがいる場合
            if (Enemy.GetComponent<CardSet>().GetChildTrans() != null)
            {
                // 追従を解除させる
                ChildCancelFoliowing(Enemy.GetComponent<CardSet>());
            }

            // 親のカードを削除
            //Destroy(Enemy.gameObject);
            CmdDestroyCard(Enemy.GetComponent<CardSet>().CardNum);
        }
    }

    [Command]
    void CmdDestroyCard(int CardNum)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

        GameObject CardSet = null;

        // 敵カードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.GameManager.tag)
            {
                if (cardset.GetComponent<CardSet>().CardNum == CardNum)
                {
                    // 検索結果を一時格納
                    CardSet = cardset;

                    // 無駄にループさせない
                    break;
                }
            }
        }

        NetworkServer.Destroy(CardSet);
    }


    // 追従を解除する
    private void ChildCancelFoliowing(CardSet cardSet)
    {
        // カードの位置情報を親と同じにする(位置の調整)
        cardSet.GetChildTrans().GetChild(0).GetComponent<CardColtrol>().
            SetNowFloorNum(cardSet.transform.GetChild(0).GetComponent<CardColtrol>().GetNowFloorNum());

        // 子供の追従を解除させる
        cardSet.GetChildTrans().GetComponent<CardSet>().DeleteSource();

        // 子供のモデルを表示させる
        cardSet.GetChildTrans().GetChild(1).gameObject.SetActive(true);

        // 子供のモデルの位置を調整
        cardSet.GetChildTrans().GetChild(1).localPosition = new Vector3(0.0f, 0.1f, 0.0f);
        cardSet.GetChildTrans().GetChild(1).GetComponent<ModelControl>().NewCardPos = cardSet.transform.GetChild(1).position;
    }

    
}
