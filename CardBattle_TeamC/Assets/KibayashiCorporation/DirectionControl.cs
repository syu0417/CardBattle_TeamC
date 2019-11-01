using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionControl : MonoBehaviour
{
    private Vector3 workDiff;//距離計測用
    [Header("親")]
    public GameObject MyParent;

    private Quaternion MyRotation;
    private ModelAnimation modelanimation;
    // Start is called before the first frame update
    void Start()
    {
        MyRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        workDiff = MyParent.GetComponent<ModelControl>().Getdiff();
        //進行方向を向く
        if (workDiff.magnitude > 0.01f)//動作中
        {
            this.transform.rotation = Quaternion.LookRotation(workDiff);
            MyRotation = this.transform.rotation;
            this.transform.rotation = new Quaternion(MyRotation.x, MyRotation.y, MyRotation.z, MyRotation.w);
            //歩行モーション
            this.GetComponent<ModelAnimation>().Walk();
        }

        if (!MyParent.GetComponent<ModelControl>().GetisMove())//停止中
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            //待機モーション
            this.GetComponent<ModelAnimation>().Idle();
        }
        

        
    }
}
