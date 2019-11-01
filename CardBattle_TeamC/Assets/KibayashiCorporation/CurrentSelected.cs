using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CurrentSelected : MonoBehaviour
{
    [SerializeField]
    private GameObject ParentObj = null;
    public GameObject parentObj
    {
        get
        {
            return ParentObj;
        }

        set
        {
            ParentObj = value;
        }
    }


    // eventSystemを取得するための変数宣言
    [SerializeField]
    EventSystem eventSystem;

    [SerializeField,Header("現在選択されてるカードパワー")]
    int NowSelectCardPower;

    private GameObject selectedObj;

    [SerializeField,Header("カード選択状態か")]
    bool SelectCardState;

    [SerializeField, Header("配置できるカードの残数")]
    int[] CardStock = new int[4];

    private int NowFloorNum = -1;

    [SerializeField, Header("配置フェーズか(trueで終了)")]
    private bool PhaseSts;
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        CardStock[0] = 4;
        CardStock[1] = 2;
        CardStock[2] = 2;
        CardStock[3] = 1;


    }



    // Update is called once per frame
    void Update()
    {
        OnClickCheck();
        if(CardStock[0]+ CardStock[1]+ CardStock[2]+ CardStock[3]==0)
        {
            PhaseSts = true;
        }
    }

    public void OnClickCheck()
    {
        // クリックされたタイミングで判定する
        if (Input.GetMouseButtonDown(0))
        {
            // TryCatch文でNull回避
            try
            {
                // 子供のコンポーネントにアクセスしたいのでいったん変数に格納
                selectedObj = eventSystem.currentSelectedGameObject.gameObject;
                //現在選択してるカードのパワー取得
                NowSelectCardPower = selectedObj.GetComponentInChildren<SelectCard>().GetPower();

                //選択したカードのストックがあれば選択状態に
                if (CardStock[NowSelectCardPower - 1] > 0)
                {
                    SelectCardState = true;
                }
                else
                {
                    SelectCardState = false;
                }

            }
            // 例外処理的なやつ
            catch (NullReferenceException ex)
            {
                if (SelectCardState)
                {
                    //選択したマスを取得
                    this.GetComponent<SelectMath>().getClickObject();

                    NowFloorNum = this.GetComponent<SelectMath>().GetSelectFloorNum();

                    if (NowFloorNum != 35&&NowFloorNum>=0)//出っ張りには置かせない
                    {
                        //Floorのちょっと下からRayを真上に飛ばしてカードに当たればbreak
                        //if()
                        //{
                        //    break;
                        //}

                        //カードを配置する処理

                        //現在選択してるカードの残り枚数を減算
                        CardStock[NowSelectCardPower - 1] -= 1;

                        parentObj.GetComponent<CardManager>().CallSpawnCard(NowFloorNum, NowSelectCardPower);
                    }
   
                }
                //Debug.Log("何も選択されてない");
                NowSelectCardPower = 0;
                SelectCardState = false;
                
            }
        }
    }

    public int GetTexNum(int Num)//渡された数値番目の配列の要素を返す
    {
        return CardStock[Num-1];
    }

    public bool GetPhaseSts()
    {
        return PhaseSts;
    }

    public bool GetSelectCardState()
    {
        return SelectCardState;
    }

    public int GetNowSelectCardPower()
    {
        return NowSelectCardPower;
    }
}