using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [Header("動的生成用スクリプトです。")]
    public GameObject obj;

    [SerializeField, Header("床の横幅")]
    const int FloorWidth = 7;
    [SerializeField, Header("床の縦幅")]
    const int FloorHeight = 5;

    const float FloorScale = 10.0f;

    //床のマテリアルナンバー
    private int[,] FloorMatNum = new int[,]{{1,2,1,3,1,2,1},{2,1,2,1,2,1,2}};
    bool MatSts = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject prehub;

        // 床を動的に生成
        for (int heightCnt = 0; heightCnt < FloorHeight; heightCnt++) 
        {
            for (int widthCnt = 0; widthCnt < FloorWidth; widthCnt++) 
            {
                // 生成処理
                prehub = Instantiate(obj, new Vector3((FloorScale * widthCnt), 0.0f, (FloorScale * heightCnt)), Quaternion.identity);

                // マネージャーを親にする
                prehub.transform.parent = this.transform;

                // 生成した後に必要な数値を持たせる
                // 今回は「床の通し番号」のみ
                prehub.GetComponent<FloorControl>().SetFloorNum(FloorWidth * heightCnt + widthCnt);

                //マテリアルを設定された数値に基づいて動的切り替え
                prehub.GetComponent<ChangeFloorMat>().SetMaterial(FloorMatNum[System.Convert.ToInt32(MatSts),widthCnt]);
            }
            MatSts = !MatSts;
        }

        // 追加で床を生成
        {
            /* こっちは手前のゴール */
            // 生成処理
            prehub = Instantiate(obj, new Vector3((FloorScale * -1), 0.0f, (FloorScale * (FloorHeight / 2))), Quaternion.identity);

            // マネージャーを親にする
            prehub.transform.parent = this.transform;

            // 生成した後に必要な数値を持たせる
            // 今回は「床の通し番号」のみ
            prehub.GetComponent<FloorControl>().SetFloorNum(FloorHeight * FloorWidth);

            //ゴールは特殊なマテリアルに
            prehub.GetComponent<ChangeFloorMat>().SetMaterial(4);

            //テクスチャの関係で回転
            prehub.transform.rotation = Quaternion.Euler(0, 90, 0);

            /* こっちは奥のゴール */
            // 生成処理
            prehub = Instantiate(obj, new Vector3((FloorScale * FloorWidth), 0.0f, (FloorScale * (FloorHeight / 2))), Quaternion.identity);

            // マネージャーを親にする
            prehub.transform.parent = this.transform;

            // 生成した後に必要な数値を持たせる
            // 今回は「床の通し番号」のみ
            prehub.GetComponent<FloorControl>().SetFloorNum(FloorHeight * FloorWidth + 1);

            //ゴールは特殊なマテリアルに
            prehub.GetComponent<ChangeFloorMat>().SetMaterial(4);

            //テクスチャの関係で回転
            prehub.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.enabled = false;
    }

   
}
