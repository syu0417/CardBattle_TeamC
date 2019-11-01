using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class LineUp : MonoBehaviour
{
    [Header("高さ")]
    public float high;

    [Header("オブジェクト間の幅")]
    public float width;

    [Header("上から見て縦、Z軸のオブジェクトの量")]
    public int vertical;

    [Header("上から見て横、X軸のオブジェクトの量")]
    public int horizontal;

    [Header("Prefabを入れる欄を作る")]
    public GameObject cube;

    [Header("位置を入れる変数")]
    public Vector3 pos;

    private int num = 0;

    [Header("秒数")]
    public float Sec;

    [Header("ゲームオブジェクトたち")]
    public GameObject[] gameObjects = new GameObject[5];
    void Start()
    {
        //このスクリプトを入れたオブジェクトの位置
        pos = transform.position;

        Observable.Timer(TimeSpan.FromSeconds(Sec)).Subscribe(_ => LineUpFunc());
    }

    void LineUpFunc()
    {
        //Z軸にverticalの数だけ並べる
        for (int vi = 0; vi < vertical; vi++)
        {
            //X軸にhorizontalの数だけ並べる
            for (int hi = 0; hi < horizontal; hi++)
            {
                gameObjects[num].transform.position = new Vector3(
                        //X軸
                        pos.x + horizontal * width / 2 - hi * width - width / 2,
                        //Y軸
                        high,
                        //Z軸
                        pos.z + vertical * width / 2 - vi * width - width / 2);

                num++;
            }
        }
    }
}
