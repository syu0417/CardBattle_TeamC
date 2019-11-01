using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class Floor : MonoBehaviour
{
    [SerializeField] int Num;//自分の番号

    private void Awake()//生成時に実行
    {
        if(this.transform.parent.GetComponent<FloorArrangement>())//自動生成なら
        {
            SetNum(this.GetComponentInParent<FloorArrangement>().GetNowNumber());//ナンバーセット
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public int GetNum()//自分の番号を返す
    {
        return Num;
    }

    public void SetNum(int num)//番号をセット
    {
        Num = num;
    }
}
