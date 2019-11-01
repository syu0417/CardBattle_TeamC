using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public enum Direction
{
    RIGHT = 1,
    BOTTOM,
    LEFT,
    TOP
}
public class MoveArrow : MonoBehaviour
{
    [Header("右:1,下:2,左:3,上:4")]
    public Direction DirecNum;

    public GameObject MyParent;

    // 移動用
    private bool MoveFlg;

    // Start is called before the first frame update
    void Start()
    {
       if(MyParent == null && this.transform.parent != null)
        {
            MyParent = this.transform.parent.gameObject;
        }

        MoveFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        // クリックされた時の処理
        if (MyParent != null && MoveFlg == true)
        {
            switch (DirecNum)
            {
                case Direction.RIGHT:
                    //右に移動する処理
                    MyParent.GetComponent<Player>().MoveControl((int)Direction.RIGHT);
                    Debug.Log("右です");
                    break;
                case Direction.BOTTOM:
                    //下に移動する処理
                    MyParent.GetComponent<Player>().MoveControl((int)Direction.BOTTOM);
                    Debug.Log("下です");
                    break;
                case Direction.LEFT:
                    //左に移動する処理
                    MyParent.GetComponent<Player>().MoveControl((int)Direction.LEFT);
                    Debug.Log("左です");
                    break;
                case Direction.TOP:
                    //上に移動する処理
                    MyParent.GetComponent<Player>().MoveControl((int)Direction.TOP);
                    Debug.Log("上です");
                    break;
            }

            // 移動が終わったので切る
            MoveFlg = false;
        }
    }

    //public void OnClicked()//クリックされた時の処理
    //{
    //    if(MyParent!=null)
    //    {
    //        switch (DirecNum)
    //        {
    //            case Direction.RIGHT:
    //                //右に移動する処理
    //                MyParent.GetComponent<Player>().MoveControl((int)Direction.RIGHT);
    //                Debug.Log("右です");
    //                break;
    //            case Direction.BOTTOM:
    //                //下に移動する処理
    //                MyParent.GetComponent<Player>().MoveControl((int)Direction.BOTTOM);
    //                Debug.Log("下です");
    //                break;
    //            case Direction.LEFT:
    //                //左に移動する処理
    //                MyParent.GetComponent<Player>().MoveControl((int)Direction.LEFT);
    //                Debug.Log("左です");
    //                break;
    //            case Direction.TOP:
    //                //上に移動する処理
    //                MyParent.GetComponent<Player>().MoveControl((int)Direction.TOP);
    //                Debug.Log("上です");
    //                break;
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)//当たったら消す
    {
        if(MyParent==null)//ここには3積みの状態かどうかを記述(3積みならif分の中に入るように)
        {
            this.gameObject.SetActive(false);//表示を消して疑似的に移動できないように
        }
    }

    public void SetMove(bool Flg)
    {
        MoveFlg = Flg;
    }

    public Direction GetDirection()
    {
        return DirecNum;
    }
}
