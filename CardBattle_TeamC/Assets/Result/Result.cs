using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField,Header("勝ってるか負けてるか(false:Lose,true:Win)")]
    private bool ResultStatus;

    [Header("Winのオブジェクト")]
    public GameObject WinObj;

    [Header("Loseのオブジェクト")]
    public GameObject LoseObj;


    // Start is called before the first frame update
    void Start()
    {
        if (WinObj != null && LoseObj != null)
        {
            //勝利してたらWinを、敗北してたらLoseを表示
            if (ResultStatus)
            {
                WinObj.SetActive(true);
                LoseObj.SetActive(false);
            }
            else
            {
                LoseObj.SetActive(true);
                WinObj.SetActive(false);
                AudioManager.Instance.Play_BGM_Lose();
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    public void SetResult(bool sts)//ステータスセット
    {
        ResultStatus = sts;
    }
}
