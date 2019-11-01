using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
public class ShowCamera : MonoBehaviour
{
    [Header("X座標の限界")]
    public float Max_X;

    [Header("スタートのX座標")]
    public float Start_X;

    private Camera Mycamera;
    private Transform MyTrans;
    private float PosX = 0;
    // Start is called before the first frame update
    void Start()
    {
        Mycamera = this.GetComponent<Camera>();
        MyTrans = Mycamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            PosX -= 20.0f;
            if(PosX<Max_X)
            {
                PosX = Start_X;
            }
            MyTrans.position = new Vector3(PosX, MyTrans.position.y, MyTrans.position.z);
        }
    }
}
