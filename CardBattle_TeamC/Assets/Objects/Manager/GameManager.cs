using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class GameManager : MonoBehaviour
{
    [SerializeField, Header("キャラの数")]
    const int characterNum = 8;

    // Start is called before the first frame update
    public GameObject[] CardObjects = new GameObject[characterNum];

    [SerializeField]
    private int ArrayNum = 0;//現在の配列の番号
    [SerializeField]
    private int WorkArrayNum = 0;//前の番号保存用

    [SerializeField]
    int[] NowPow = new int[characterNum];

    //[SerializeField]
    //private bool[] worknum = new bool[5];

    [SerializeField]
    int TargetNum = -1;

    private bool BattleFlg = false;

    // 移動先のオブジェクト
    [SerializeField]
    GameObject DestinationObj;
    
    void Start()
    {
        //一番目の配列のenableをtrueに、それ以外をfalseにして操作可能オブジェクトを1つに
        for(int cnt = 0;cnt<CardObjects.Length;cnt++)
        {
            CardObjects[cnt].transform.GetChild(0).gameObject.GetComponent<Player>().SetisActive(false);
            CardObjects[cnt].transform.GetChild(1).gameObject.GetComponent<ChaseTarget>().SetisActive(false);
            
        }
       
        CardObjects[0].transform.GetChild(0).gameObject.GetComponent<Player>().SetisActive(true);
        CardObjects[0].transform.GetChild(1).gameObject.GetComponent<ChaseTarget>().SetisActive(true);

        //ArrayNumが変化した時に実行
        this.UpdateAsObservable().
            Select(_ => ArrayNum).
            DistinctUntilChanged(x => x).
            Subscribe(_ => ChangeActive());

        TargetNum = -1;

        DestinationObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCharacter();

        ChangePower();

        Following();

        Battle();
        
    }

    // クリックすると操作キャラが変更される
    private void ChangeCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WorkArrayNum = ArrayNum;
            ArrayNum++;
            for (; ArrayNum >= CardObjects.Length ||
                CardObjects[ArrayNum] == null || CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing();)
            {
                if (ArrayNum >= CardObjects.Length)
                    ArrayNum = 0;
                ArrayNum++;
            }
        }

        /* クリックで操作変更 */
        GameObject obj = getClickObject();

        if (obj != null)
        {
            for (int cnt = 0; cnt < CardObjects.Length; cnt++)
            {
                // 操作キャラ以外で操作可能なキャラを検索
                if (cnt != ArrayNum && CardObjects[cnt] != null && CardObjects[cnt].transform.GetChild(0).gameObject == obj)
                {
                    print("click!!");

                    // 操作可能になっているキャラに切り替える
                    WorkArrayNum = ArrayNum;
                    ArrayNum = cnt;

                    break;
                }
            }
        }
        /* クリックで操作変更 */
    }

    // 力を変更させる
    private void ChangePower()
    {
        /* キャラのパワーの処理 S */
        for (int cnt = 0; cnt < CardObjects.Length; cnt++)
        {
            if (CardObjects[cnt] == null)
                continue;

            // キャラに追従しているキャラがいるか
            if (CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() != null)
            {
                // 追従しているキャラに追従しているキャラはいるか
                if (CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                    GetComponent<ChaseTarget>().GetChildTransform() != null)
                {
                    NowPow[cnt] = CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetPower() +

                        CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                        parent.GetChild(0).GetComponent<Player>().GetPower() +

                        CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                        GetComponent<ChaseTarget>().GetChildTransform().parent.GetChild(0).GetComponent<Player>().GetPower();
                }
                else
                {
                    NowPow[cnt] = CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetPower() +

                        CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                        parent.GetChild(0).GetComponent<Player>().GetPower();
                }
            }
            else
            {
                NowPow[cnt] = CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetPower();
            }
        }
        /* キャラのパワーの処理 E */
    }

    // 追従の処理
    private void Following()
    {
        /* 操作中のものの処理 */
        if (DestinationObj != null)
        {
            for (int cnt = 0; cnt < CardObjects.Length; cnt++)
            {
                if (CardObjects[cnt] != null && cnt != ArrayNum)
                {
                    // 重なる処理
                    if (CardObjects[cnt].transform.GetChild(0).tag == CardObjects[ArrayNum].transform.GetChild(0).tag)
                    {
                        if (CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetPileSts())
                        {
                            if (DestinationObj == CardObjects[cnt])
                            {
                                /* 追従 */

                                // 操作キャラが停止しているか
                                if (CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetisStop())
                                {
                                    // 対象キャラ（親予定）に親がいないか
                                    if (!CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing())
                                    {
                                        // 操作キャラに子供がいない、或いは
                                        // 操作キャラの子供に子供（つまり孫）がいないか
                                        // 対象キャラも同じ制約がかけられる
                                        if (CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() == null ||
                                        CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                                        GetComponent<ChaseTarget>().GetChildTransform() == null ||
                                        CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() == null ||
                                        CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                                        GetComponent<ChaseTarget>().GetChildTransform() == null)
                                        {
                                            // モデルに追従させる
                                            CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().
                                                SetObjectConstraintTransform(CardObjects[cnt].transform.GetChild(1).transform);

                                            // カードに追従させる
                                            CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().
                                                SetObjectConstraintTransform(CardObjects[cnt].transform.GetChild(0).transform);

                                            // 追従させたモデルを子供情報として保持
                                            CardObjects[cnt].transform.GetChild(1).GetComponent<ChaseTarget>().
                                                SetChildTransform(CardObjects[ArrayNum].transform.GetChild(1).transform);

                                            // 操作する対象が変わるので、位置をセットする
                                            CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().SetArrayNum(
                                                CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().GetArrayNumX(),
                                                CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().GetArrayNumY());

                                            // 操作対象の変更
                                            WorkArrayNum = ArrayNum;
                                            ArrayNum = cnt;

                                            // 対象の情報を消す
                                            DestinationObj = null;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    // バトルの処理
    private void Battle()
    {
        // バトル開始
        if (TargetNum != -1 && BattleFlg == true)
        {
            print("BattleStart!");

            // 操作カードが弱い場合
            if (NowPow[ArrayNum] < NowPow[TargetNum])
            {
                BattleEnemyWin();
            }
            // 操作カードが強い場合
            else if (NowPow[ArrayNum] > NowPow[TargetNum])
            {
                BattlePlayerWin();
            }
            // どちらのカードも強さが同じ場合
            else
            {
                BattleEnemyWin();
                BattlePlayerWin();
            }

            TargetNum = -1;
            BattleFlg = false;
        }
        else
        {
            TargetNum = -1;
            BattleFlg = false;
        }
    }

    // プレイヤーが勝つ場合の処理
    private void BattlePlayerWin()
    {
        // 敵に子供がいる場合
        if (CardObjects[TargetNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() != null)
        {
            int childNum = -1;

            // 追従しているカードを検索
            for (int cnt = 0; cnt < CardObjects.Length; cnt++)
            {
                if (CardObjects[cnt] == null)
                    continue;

                if (CardObjects[TargetNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() ==
                    CardObjects[cnt].transform.GetChild(1).transform)
                {
                    childNum = cnt;

                    break;
                }
            }

            // 敵カードの子供情報を消す
            CardObjects[TargetNum].transform.GetChild(1).GetComponent<ChaseTarget>().SetChildTransform(null);
            // 子供カード（次に敵カードになる）の追従を消す
            CardObjects[childNum].transform.GetChild(0).GetComponent<Player>().DeleteSource();
            CardObjects[childNum].transform.GetChild(1).GetComponent<ChaseTarget>().DeleteSource();

            // 子供カード（次期敵カード）に位置をセット
            CardObjects[childNum].transform.GetChild(0).GetComponent<Player>().SetArrayNum(
                CardObjects[TargetNum].transform.GetChild(0).GetComponent<Player>().GetArrayNumX(),
                CardObjects[TargetNum].transform.GetChild(0).GetComponent<Player>().GetArrayNumY());

            int oldTargetNum = TargetNum;
            TargetNum = -1;

            // 残ったキャラのバトルフラグを切る
            CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().SetBattleFlg(false);

            Destroy(CardObjects[oldTargetNum]);

            // スタート位置を調整
            CardObjects[childNum].transform.GetChild(1).GetComponent<ChaseTarget>().startMarker.position =
                CardObjects[childNum].transform.GetChild(1).position =
                CardObjects[childNum].transform.GetChild(1).GetComponent<ChaseTarget>().endMarker.position;
        }
        else
        {
            int oldTargetNum = TargetNum;
            TargetNum = -1;

            // 残ったキャラのバトルフラグを切る
            CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().SetBattleFlg(false);

            Destroy(CardObjects[oldTargetNum]);
            //CardObjects[TargetNum].SetActive(false);
        }
    }

    // 敵が勝つ場合
    private void BattleEnemyWin()
    {
        // 操作カードに追従しているカードがある場合
        if (CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() != null)
        {
            int childNum = -1;

            // 追従しているカードを検索
            for (int cnt = 0; cnt < CardObjects.Length; cnt++)
            {
                if (CardObjects[cnt] == null)
                    continue;

                if (CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() ==
                    CardObjects[cnt].transform.GetChild(1).transform)
                {
                    childNum = cnt;

                    break;
                }
            }

            // 操作カードの子供情報を消す
            CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().SetChildTransform(null);
            // 子供カード（次に操作カードになる）の追従を消す
            CardObjects[childNum].transform.GetChild(0).GetComponent<Player>().DeleteSource();
            CardObjects[childNum].transform.GetChild(1).GetComponent<ChaseTarget>().DeleteSource();

            // 子供カード（次期操作カード）に位置をセット
            CardObjects[childNum].transform.GetChild(0).GetComponent<Player>().SetArrayNum(
                CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().GetArrayNumX(),
                CardObjects[ArrayNum].transform.GetChild(0).GetComponent<Player>().GetArrayNumY());

            WorkArrayNum = ArrayNum;
            ArrayNum = childNum;

            // 残ったキャラのバトルフラグを切る
            CardObjects[TargetNum].transform.GetChild(0).GetComponent<Player>().SetBattleFlg(false);

            TargetNum = -1;
            Destroy(CardObjects[WorkArrayNum]);

            // スタート位置を調整
            CardObjects[childNum].transform.GetChild(1).GetComponent<ChaseTarget>().startMarker.position =
                CardObjects[childNum].transform.GetChild(1).position =
                CardObjects[childNum].transform.GetChild(1).GetComponent<ChaseTarget>().endMarker.position;

            //CardObjects[WorkArrayNum].SetActive(false);
        }
        else
        {
            // 一時的な格納用
            int tem = ArrayNum;

            WorkArrayNum = ArrayNum;
            tem++;
            for (; tem >= CardObjects.Length ||
            CardObjects[tem] == null || CardObjects[tem].transform.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing();)
            {
                if (tem >= CardObjects.Length)
                    tem = 0;
                tem++;
            }
            ArrayNum = tem;

            // 残ったキャラのバトルフラグを切る
            CardObjects[TargetNum].transform.GetChild(0).GetComponent<Player>().SetBattleFlg(false);

            TargetNum = -1;
            Destroy(CardObjects[WorkArrayNum]);
            //CardObjects[WorkArrayNum].SetActive(false);
        }
    }

    public void ChangeActive()
    {
        //操作可能オブジェクト切り替え
        if (CardObjects[WorkArrayNum] != null)
        {
            CardObjects[WorkArrayNum].transform.GetChild(0).gameObject.GetComponent<Player>().SetisActive(false);
            CardObjects[WorkArrayNum].transform.GetChild(1).gameObject.GetComponent<ChaseTarget>().SetisActive(false);
        }
        

        CardObjects[ArrayNum].transform.GetChild(0).gameObject.GetComponent<Player>().SetisActive(true);
        CardObjects[ArrayNum].transform.GetChild(1).gameObject.GetComponent<ChaseTarget>().SetisActive(true);
    }


    // 左クリックしたオブジェクトを取得する関数(3D)
    public GameObject getClickObject()
    {
        GameObject result = null;
        bool breakFlg = false ;
        bool hitFlg = false;
        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                if (hit.collider.gameObject.tag == "MoveArrow")
                {
                    result = null;

                    // 移動先の判定
                    for (int cnt = 0; cnt < CardObjects.Length; cnt++)
                    {
                        if (cnt == ArrayNum || CardObjects[cnt] == null)
                            continue;


                        switch (hit.collider.gameObject.transform.GetComponent<MoveArrow>().GetDirection())
                        {
                            case Direction.TOP:     // 上
                                {
                                    // 移動後の予測で処理
                                    if (hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumX() ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumX() &&
                                        (hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumY() + 7) ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumY())
                                    {
                                        hitFlg = ClickMove(hit, CardObjects[cnt], cnt);
                                    }
                                    else
                                    {
                                        hitFlg = true;
                                    }
                                }
                                break;

                            case Direction.BOTTOM:  // 下
                                {
                                    // 移動後の予測で処理
                                    if (hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumX() ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumX() &&
                                        (hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumY() - 7) ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumY())
                                    {
                                        hitFlg = ClickMove(hit, CardObjects[cnt], cnt);
                                    }
                                    else
                                    {
                                        hitFlg = true;
                                    }

                                }
                                break;

                            case Direction.LEFT:    // 左
                                {
                                    // 移動後の予測で処理
                                    if ((hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumX() - 1) ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumX() &&
                                        hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumY() ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumY())
                                    {
                                        hitFlg = ClickMove(hit, CardObjects[cnt], cnt);
                                    }
                                    else
                                    {
                                        hitFlg = true;
                                    }

                                }
                                break;

                            case Direction.RIGHT:   // 右
                                {
                                    // 移動後の予測で処理
                                    if ((hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumX() + 1) ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumX() &&
                                        hit.collider.gameObject.transform.parent.GetComponent<Player>().GetArrayNumY() ==
                                        CardObjects[cnt].transform.GetChild(0).GetComponent<Player>().GetArrayNumY())
                                    {
                                        hitFlg = ClickMove(hit, CardObjects[cnt], cnt);
                                    }
                                    else
                                    {
                                        hitFlg = true;
                                    }

                                }
                                break;

                            default:
                                break;
                        }

                        if (hitFlg)
                            break;
                    }

                    if (hitFlg == true) 
                    {
                        hit.collider.gameObject.transform.GetComponent<MoveArrow>().SetMove(true);
                    }

                    break;
                }

                else if (hit.collider.gameObject.tag == "Card" || hit.collider.gameObject.tag == "Enemy")
                {
                    if (hit.collider.gameObject.transform.parent.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing() == false &&
                        CardObjects[ArrayNum].transform.GetChild(1).GetComponent<ChaseTarget>().GetisStop() == true)
                    {
                        result = hit.collider.gameObject;

                        //break;
                    }
                }
            }
        }
        return result;
    }

    // クリックした後の処理(getClickObject内で使用)
    private bool ClickMove(RaycastHit hit, GameObject obj, int cnt)
    {
        // タグで判定
        // 同じ場合（味方同士）
        if (hit.collider.gameObject.transform.parent.tag == obj.transform.GetChild(0).tag)
        {
            // 移動しても問題ないか
            // 選択したキャラに親がいないか
            if (hit.collider.gameObject.transform.parent.parent.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing() == false)
            {
                // 検索した対象キャラに親がいないか
                if (obj.transform.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing() == false)
                {
                    // 選択キャラと対象キャラの両方に子供がいない
                    // 或いは、選択キャラに孫がいない
                    // 或いは、対象キャラに孫がいない
                    // の3つともクリアすれば移動する
                    if (hit.collider.gameObject.transform.parent.parent.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() == null &&
                        obj.transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() == null ||
                        hit.collider.gameObject.transform.parent.parent.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() != null &&
                        hit.collider.gameObject.transform.parent.parent.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().
                        GetComponent<ChaseTarget>().GetChildTransform() == null ||
                        obj.transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform() != null &&
                        obj.transform.GetChild(1).GetComponent<ChaseTarget>().GetChildTransform().GetComponent<ChaseTarget>().GetChildTransform() == null)
                    {
                        DestinationObj = obj;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        // 違う場合（敵同士）
        else
        {
            if (hit.collider.gameObject.transform.parent.parent.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing() == false)
            {
                // 検索した対象キャラに親がいないか
                if (obj.transform.GetChild(1).GetComponent<ChaseTarget>().GetisFollowing() == false)
                {
                    print("バトル");

                    BattleFlg = true;

                    TargetNum = cnt;
                    return false;
                }
            }

            return false;
        }
    }
}
