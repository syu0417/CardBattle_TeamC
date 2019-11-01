using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPileUp : MonoBehaviour
{
    [Header("親オブジェクト")]
    public GameObject MyParent;
    private Transform MyTrans;

    private bool sts;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //リジッドボディでバグらないように常に位置、回転角度は親の物と同期
        MyTrans = MyParent.transform;
        this.transform.position = MyParent.transform.position;
        this.transform.rotation = MyParent.transform.rotation;

       

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit(collision)");
        sts = true;
        if (MyParent.GetComponent<ChaseTarget>().GetisActive())//後にきたほうが乗るように
        {
            MyParent.GetComponent<ChaseTarget>().SetStatus(sts);
        }
        else
        {
            MyParent.GetComponent<ChaseTarget>().SetStatus(!sts);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Remove");
        sts = false;
        MyParent.GetComponent<ChaseTarget>().SetStatus(sts);
    }
}
