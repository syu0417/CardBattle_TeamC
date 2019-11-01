using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class ChangeModel : MonoBehaviour
{
    [Header("ゴブリンのオブジェクト")]
    public GameObject GobrinObj;

    [Header("マレフィセント(仮)のオブジェクト")]
    public GameObject MarefiObj;

    [Header("八和和の八和子のオブジェクト")]
    public GameObject HawawaObj;

    [Header("根黒男差のオブジェクト")]
    public GameObject NecroObj;

    [Header("敵モデルのオブジェクト")]
    public GameObject EnemyObj;

    [Header("カードセット")]
    public GameObject CardSetObj;

    [SerializeField]
    private int Power;

    [SerializeField, Header("ホストかクライアントか")]
    private int TeamId;

    [Header("スモークオブジェクト")]
    public GameObject SmokeObj;

    // Start is called before the first frame update
    void Start()
    {
        if(SmokeObj!=null)//出現時に爆発
        {
            SmokeObj.SetActive(true);
        }

        if(CardSetObj!=null)
        {
            Power = CardSetObj.GetComponent<CardSet>().GetPower();
        }


        switch(Power)
        {
            case 1://ゴブリン
                if(GobrinObj!=null)
                {
                    GobrinObj.SetActive(true);
                }
                break;

            case 2://八和子
                if(HawawaObj!=null)
                {
                    HawawaObj.SetActive(true);
                }
                break;

            case 3://根黒男差
                if(NecroObj!=null)
                {
                    NecroObj.SetActive(true);
                }
                break;

            case 4://マレフィ
                if(MarefiObj!=null)
                {
                    MarefiObj.SetActive(true);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(自分と違うやつなら)
        //SetEnemyModel();
    }

    public void SetEnemyModel()//エネミーのモデルに切り替え
    {
        GobrinObj.SetActive(false);
        HawawaObj.SetActive(false);
        NecroObj.SetActive(false);
        MarefiObj.SetActive(false);
        EnemyObj.SetActive(true);
    }
    public void SetTeamId(int Id)//ホストかクライアントか設定
    {
        TeamId = Id;
    }
}
