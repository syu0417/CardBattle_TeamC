using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexUI : MonoBehaviour
{
    public GameObject LoadingObj;
    public void SingleButtonClick()//シングルプレイーボータン
    {
        NetworkManagerCustom.SingleGame();
    }

    public void NetButtonClick()//マルチプレイーボータン
    {
        if(LoadingObj!=null)
        {
            LoadingObj.SetActive(true);
        }
        NetworkManagerCustom.NetGame();
    }


    public void LanButtonClick()//ローカルボータン
    {
        NetworkManagerCustom.LanGame();
    }
 



}
