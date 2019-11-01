using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
public class Timer : MonoBehaviour
{
    [Header("指定秒数"), SerializeField]
    private float Second;
    // Start is called before the first frame update
    void Start()
    {
        //〇秒後に行いたい処理がある場合の書き方(この場合だとStart実行されてからSecond秒後に何かが実行されます。)
        //エラーの関係で今はコメントアウトしてますが"Subscribe(_=>やりたい処理)"という書き方にしてください。
        Observable.Timer(TimeSpan.FromSeconds(Second)).Subscribe(/*_=>ここにやりたい処理*/);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
