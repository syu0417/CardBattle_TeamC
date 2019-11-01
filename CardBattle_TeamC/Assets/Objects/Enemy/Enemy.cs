using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UniRx;
using UniRx.Triggers;

public class Enemy : MonoBehaviour
{
    private GameObject Obj;
    [SerializeField]
    Vector3 Vec;
    [SerializeField, Header("1回で動く量")]
    public float Speed;

    [Header("初期位置(X:横,Z:縦)")]
    public int PosX;
    public int PosZ;

    private int Array_X_Num;
    private int Array_Y_Num;

    [SerializeField]
    private bool isActive = false;
    private bool MySts = true;//trueなら動ける

    public bool PileSts = false;

    private bool FastFlame = false;//最初のフレームに当たってる判定を取らない
    [SerializeField]
    private bool BattleFlg = false;
    // Start is called before the first frame update
    void Start()
    {
        //マスの番号ごとに座標を設定
        Vec = new Vector3((float)PosX * 10.0f, 0.1f, (float)PosZ * 10.0f);
        //Vec = GameObject.Find("Floor").transform.position;
        //this.transform.position = Vec;

        //初期位置を元に番号セット
        Array_X_Num = PosX;
        Array_Y_Num = PosZ * 7;

        
        this.UpdateAsObservable().Take(1).Subscribe(_ => FastFlame = true);
        //UpdateでY座標のみセット(1回のみ)
        this.UpdateAsObservable().Take(1).
            Subscribe(_ => this.transform.position = new Vector3(Vec.x, 0.1f, Vec.z));


        //// 追従用の初期化
        //this.gameObject.AddComponent<PositionConstraint>();
        //this.myPositionConstaint = GetComponent<PositionConstraint>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isActive)
        //{
        //    Debug.Log("false");
        //}
        //else
        //{
        //    Debug.Log("true");
        //}
    }


    

    public void SetisActive(bool flg)
    {
        isActive = flg;
    }

    public void SetStatus(bool sts)
    {
        MySts = sts;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Card" && FastFlame)
        {
            Debug.Log("Hit(Card)");
            PileSts = true;
        }
        if (collision.collider.tag == "Card" && FastFlame)
        {
            Debug.Log("Battle");
            BattleFlg = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Card" && FastFlame)
        {
            Debug.Log("Hit(Card)");
            PileSts = false;
        }
        if (collision.collider.tag == "Card" && FastFlame)
        {
            BattleFlg = false;
        }
    }

    public bool GetPileSts()
    {
        return PileSts;
    }

    public bool GetBattleFlg()
    {
        return BattleFlg;
    }
}
