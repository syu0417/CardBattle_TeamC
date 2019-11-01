using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;
using UniRx.Triggers;

public class CardManager : NetworkBehaviour
{
    [SerializeField, Header("自分のターンか")]
    private bool MyTurnFlg = false;
    
    private Transform OldTrans = null;

    GameObject SelectObj = null;
    GameObject EventSys = null; 

    public GameObject CardSet = null;

    [HideInInspector]
    public bool StopHostFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdTag();

        SelectObj = GameObject.Find("SetCardParent");
        EventSys = GameObject.Find("EventSystem");


        this.UpdateAsObservable().Where(_ => SelectObj.GetComponent<CurrentSelected>().GetPhaseSts()).Take(1).Subscribe(_ => EventSys.GetComponent<ChangeCamera>().ChangeCameraFunc(this.tag));
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))//ホストストップするやつ
            {
                StopHostFlag = true;
            }
            if (MyTurnFlg && Camera.main != null)
            {
                getClickObject();
            }
        }
    }


    public void SetisTurn(bool Flg)
    {
        MyTurnFlg = Flg;
    }

    public bool GetisTurn()
    {
        return MyTurnFlg;
    }

    // 左クリックしたオブジェクトを取得する関数(3D)
    public void getClickObject()
    {
        GameObject obj = null;
        bool ArrowFlg = false;
        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                // 移動マスをクリックしたとき
                if(hit.collider.gameObject.tag == "Arrow" && hit.collider.transform.parent.parent.GetComponent<CardSet>().GameManager.tag == this.tag)
                {
                    hit.collider.gameObject.transform.parent.parent.GetComponent<CardSet>().SetisSelect(false);

                    hit.collider.gameObject.transform.parent.parent.GetComponent<CardSet>().SetisMove(true,
                        hit.collider.transform.GetComponent<MoveArrowControl>().GetFloorNum());
                    

                    if(obj != null)
                    {
                        ArrowFlg = true;
                    }

                    break;
                }

                // クリックで操作カードを選択
                if (hit.collider.gameObject.tag == "Card" && hit.collider.transform.parent.GetComponent<CardSet>().GameManager.tag == this.tag &&
                    hit.collider.transform.parent.GetComponent<CardSet>().IsParentSourceOf() == false) 
                {
                    obj = hit.collider.gameObject;
                }
            }
        }

        // カードの移動の処理
        if (obj != null && ArrowFlg == false)
        {
            // 選択されていないカードの場合
            if (obj.transform.parent.GetComponent<CardSet>().GetisSelect() == false)
            {
                // 既に違うカードを選択している場合
                if (OldTrans != null)
                {
                    // カードの選択を解除する
                    OldTrans.GetComponent<CardSet>().SetisSelect(false);
                }

                obj.transform.parent.GetComponent<CardSet>().SetisSelect(true);

                OldTrans = obj.transform.parent;
            }
            // 既に選択しているカードの場合
            else
            {
                obj.transform.parent.GetComponent<CardSet>().SetisSelect(false);

                OldTrans = null;
            }
        }
    }


    public bool CallisLocaPlayer()
    {
        return isLocalPlayer;
    }
    

    public void CallSpawnCard(int floorNum, int SelectCardPower)
    {
        if(!isLocalPlayer)
        {
            return;
        }

        CmdSpawnCard(floorNum, SelectCardPower);
    }

    void SpawnSelectCard()
    {
        GameObject target = GameObject.Find("SetCardParent");

        target.GetComponent<CurrentSelected>().parentObj = this.gameObject;

        if (this.tag == "Player")
        {
            print("Player");

            // 親子関係にしてから位置を調整する(Unityが勝手に数値を変更してくれやがる)
            target.transform.localPosition = new Vector3(0.0f, -155.0f, 0.0f);
            target.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            target.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            target.GetComponent<SelectMath>().SubCamera = GameObject.Find("Camera").GetComponent<Camera>();
        }
        else
        {
            print("enemy");

            // 親子関係にしてから位置を調整する(Unityが勝手に数値を変更してくれやがる)
            target.transform.localPosition = new Vector3(0.0f, 405.0f, 0.0f);
            target.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            target.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            target.GetComponent<SelectMath>().SubCamera = GameObject.Find("Camera").GetComponent<Camera>();
            target.GetComponent<SelectMath>().SubCamera.transform.position = new Vector3(60.0f, 45.0f, 20.0f);
            target.GetComponent<SelectMath>().SubCamera.transform.localRotation = Quaternion.Euler(90.0f, -90.0f, 0.0f);
        }

    }

    /* タグを切り替える */
    [Command]
    void CmdTag()
    {
        // 2人目のクライアントが接続されたら、
        // プレイヤーのタグを切り替える
        foreach (NetworkConnection netconn in NetworkServer.connections)
        {
            if (connectionToClient != netconn)
            {
                if (this.tag == "Player")
                {
                    this.tag = "Enemy";
                    break;
                }
            }
        }

        RpcTag(this.tag);
    }

    [ClientRpc]
    void RpcTag(string tag)
    {
        if (isLocalPlayer)
        {
            this.tag = tag;

            SpawnSelectCard();
        }
    }
          /* end */

    [Command]
    public void CmdSpawnCard(int floorNum, int SelectCardPower)
    {
        GameObject obj = Instantiate(CardSet, new Vector3((floorNum % 7 * 10.0f), 0.1f, (floorNum / 7 * 10.0f)), Quaternion.identity);

        NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);

        if (isServer)
        {
            if(this.tag != "Player")
            {
                obj.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }

            obj.GetComponent<CardSet>().GameManager = this.gameObject;
            obj.GetComponent<CardSet>().SetPower(SelectCardPower);
            obj.GetComponent<CardSet>().CardNum = floorNum;

            obj.transform.GetChild(0).GetComponent<CardColtrol>().SetNowFloorNum(floorNum);
        }

        RpcSpawnCard(obj.GetComponent<CardSet>().netId, floorNum, SelectCardPower);
    }

    [ClientRpc]
    void RpcSpawnCard(NetworkInstanceId NetId, int floorNum, int SelectCardPower)
    {
        GameObject target = ClientScene.FindLocalObject(NetId);


        target.GetComponent<CardSet>().GameManager = this.gameObject;
        target.GetComponent<CardSet>().SetPower(SelectCardPower);
        target.GetComponent<CardSet>().CardNum = floorNum;

        target.transform.GetChild(0).GetComponent<CardColtrol>().SetNowFloorNum(floorNum);

    }

    [TargetRpc]
    public void TargetMove(NetworkConnection target, int CardSetNum)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

        CardSet cardSet = null;

        // 重なるカードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.tag)
            {
                if (cardset.GetComponent<CardSet>().CardNum == CardSetNum)
                {
                    // 検索結果を一時格納
                    cardSet = cardset.GetComponent<CardSet>();

                    // 無駄にループさせない
                    break;
                }
            }
        }

        cardSet.Move();
    }


    [TargetRpc]
    public void TargetOverlap(NetworkConnection target, int CardSetNum, int RideCardNum)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

        CardSet cardSet = null;

        CardSet RideCardSet = null;

        // 重なるカードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.tag)
            {
                if (cardset.GetComponent<CardSet>().CardNum == CardSetNum)
                {
                    // 検索結果を一時格納
                    cardSet = cardset.GetComponent<CardSet>();

                    // 無駄にループさせない
                    break;
                }
            }
        }

        // 重なるカードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.tag)
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

        
        cardSet.MoveOverlap(RideCardSet.transform);
    }

    [TargetRpc]
    public void TargetBattle(NetworkConnection target, int CardSetNum, int EnemyCardNum)
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("CardSet");

        CardSet cardSet = null;

        CardSet EnemyCardSet = null;

        // 重なるカードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.tag)
            {
                if (cardset.GetComponent<CardSet>().CardNum == CardSetNum)
                {
                    // 検索結果を一時格納
                    cardSet = cardset.GetComponent<CardSet>();

                    // 無駄にループさせない
                    break;
                }
            }
        }

        // 重なるカードを検索
        foreach (GameObject cardset in obj)
        {
            // 検索出来たら処理をする
            if (cardset.GetComponent<CardSet>().GameManager.tag == this.tag)
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


        cardSet.MoveOverlap(EnemyCardSet.transform);
    }

    //終了処理
    private void OnApplicationQuit()
    {
        StopHostFlag = true;
    }
}
