using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 0618 //警告無視

public class PlayerNoManager : NetworkBehaviour
{

    private static PlayerNoManager instance;
    public static PlayerNoManager GetInstance()
    {
        return instance;
    }


    public Team[] teams;//チームの配列

    [HideInInspector]
    public SyncListInt size = new SyncListInt();//チームの情報の保存用

    void Awake()
    {
        instance = this;
    }

    //サーバーが実行した時sizeの初期化をする
    public override void OnStartServer()
    {
      if(size.Count!=teams.Length)
        {
            for(int i=0;i<teams.Length;i++)
            {
                size.Add(0);
            }
        }
    }

    //チームごとのナンバー(NetworkManager用)
    public int GetTeamFill()
    {
        int teamNo = 0;
        int min = size[0];
        for(int i=0;i<teams.Length;i++)
        {
            if(size[i]<min)
            {
                min = size[i];
                teamNo = i;
            }

        }

        return teamNo;

    }//GetTeamFill()

 


}

[System.Serializable]
public class Team
{
    public string name;      //チーム名
    public Color color;      //チームごとのカラー


}
