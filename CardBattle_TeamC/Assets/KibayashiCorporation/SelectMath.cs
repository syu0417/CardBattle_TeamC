using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMath : MonoBehaviour
{
    [Header("配置用カメラ")]
    public Camera SubCamera;

    [SerializeField,Range(-1,36)]
    private int SelectFloorNum = -1;
    //Ray ray = new Ray(new Vector3());
    // Start is called before the first frame update
    void Start()
    {
        if(SubCamera == null)
        {
            SubCamera = GameObject.Find("Camera").GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void getClickObject()
    {
        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = SubCamera.ScreenPointToRay(Input.mousePosition);
            SelectFloorNum = -1;
            foreach (RaycastHit hit in Physics.RaycastAll(ray))
            {
                
                //マスの座標取得
                if (hit.collider.tag=="Floor")
                {
                    SelectFloorNum=hit.collider.GetComponent<FloorControl>().GetFloorNum();
                }
            }
        }
    }

    public int GetSelectFloorNum()//現在選択しているマスのNoを返す
    {
        return SelectFloorNum;
    }
}
