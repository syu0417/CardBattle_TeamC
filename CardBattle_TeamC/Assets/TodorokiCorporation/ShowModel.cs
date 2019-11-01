using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModel : MonoBehaviour
{
    [Header("回転スピード")]
    public float Speed;
    GameObject ModelObj;
    Transform MyTrans;
    float RotY = 0;

    // Start is called before the first frame update
    void Start()
    {
        ModelObj = this.GetComponent<GameObject>();
        MyTrans = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MyTrans.rotation = Quaternion.Euler(MyTrans.rotation.x, RotY, MyTrans.rotation.z);
        RotY+=Speed;
        if(RotY>=360.0f)
        {
            RotY = 0f;
        }
        if (this.GetComponent<ModelAnimation>())
        {
            this.GetComponent<ModelAnimation>().Idle();

        }
    }
}
