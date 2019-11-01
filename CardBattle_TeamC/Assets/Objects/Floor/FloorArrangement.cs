using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorArrangement : MonoBehaviour
{
    //マス表示カウント用
    private int cnt_x = 0;
    private int cnt_z = 0;
    private int FloorNum = 0;//マスの番号

    //マスオブジェクト生成用座標
    private float PosX = 0;
    private float PosZ = 0;

    [Header("横のマスの数")]
    public uint X_FloorNum;
    private uint WorkFloorNum = 0;

    [Header("縦のマスの数")]
    public uint Z_FloorNum;

    [Header("1マスのサイズ(プレハブのサイズも変更してください)")]
    public float SquareSize;

    [Header("床オブジェクト")]
    public GameObject ChildObj;
    

    // Start is called before the first frame update
    void Start()
    {
        WorkFloorNum = X_FloorNum;//初期値を退避
        //初期段階でマスを一気に生成
        while (cnt_z<Z_FloorNum)
        {
            if (Z_FloorNum % 2 != 0)//奇数の時のみ飛び出る処理を実行
            {
                if (cnt_z == ((Z_FloorNum + 1) / 2) - 1)//中心なら1マス飛び出させる
                {
                    Debug.Log("true");
                    cnt_x -= 1;
                    X_FloorNum += 1;
                    PosX -= SquareSize;
                }
                else//中心でなければ通常通りの配置
                {
                    X_FloorNum = WorkFloorNum;
                }
            }
            else
            {
                Debug.Log("奇数ではありません。");
            }
            while (cnt_x < X_FloorNum)
            {
                //オブジェクト生成処理
                if(ChildObj!=null)
                {
                    GenerationChild();
                }
                else
                {
                    Debug.Log("オブジェクトがアタッチされていません");
                }
                PosX += SquareSize;//マスのサイズ分X座標をずらす

                cnt_x++;//Xカウントプラス
            }
            //X関係の変数リセット
            PosX = 0.0f;
            cnt_x = 0;

            PosZ += SquareSize;//マスのサイズ分Y座標をずらす

            cnt_z++;//Yカウントプラス
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerationChild()//子オブジェクトとしてプレハブを動的に生成
    {
        Instantiate(ChildObj, new Vector3(PosX, 0.0f, PosZ), Quaternion.identity,this.transform);
        //Debug.Log(FloorNum);
        FloorNum++;
    }

    public int GetNowNumber()
    {
        return FloorNum;
    }
}

