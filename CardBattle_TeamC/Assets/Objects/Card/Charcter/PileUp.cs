using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileUp : MonoBehaviour
{
    [Header("親オブジェクト")]
    public GameObject MyParent;
    private Transform MyTrans;

    private bool sts;
    // Start is called before the first frame update
    void Start()
    {
        if(MyParent == null)
        {
            MyParent = this.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //リジッドボディでバグらないように常に位置、回転角度は親の物と同期
        MyTrans = MyParent.transform;
        this.transform.position = MyParent.transform.position;
        this.transform.rotation = MyParent.transform.rotation;

        //当たった時の処理
        //もし自分のisActiveがtrueなら積み上げ(ParentのGetisActive使用)
        //falseだった場合下のままで。
        //1段目Yは1.0
        //2段目は1.5(仮)
        //3段目は2.0(仮)
        //それぞれに対応するY座標に更新(ParentのSetPosition使用)

        //当たっていなければそのまま
        //MyParent.GetComponent<ChaseTarget>().SetStatus(sts);

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit(collision)");
        sts = true;
        if(MyParent.GetComponent<ChaseTarget>().GetisFollowing())//後にきたほうが乗るように＆すでに上に載っているときも
        {
            MyParent.GetComponent<ChaseTarget>().SetStatus(sts);
        }
        else if(MyParent.GetComponent<ChaseTarget>().GetisActive())
        {
            MyParent.GetComponent<ChaseTarget>().SetStatus(!sts);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Remove");
        sts = false;

        if (MyParent.GetComponent<ChaseTarget>().GetisFollowing() == false || MyParent.GetComponent<ChaseTarget>().GetisActive() &&
            MyParent.GetComponent<ChaseTarget>().GetParentTransform().GetComponent<ChaseTarget>().GetisFollowing() != false)    // 今、上に載っているときはフラグを切り替えない
            MyParent.GetComponent<ChaseTarget>().SetStatus(sts);
    }
}
