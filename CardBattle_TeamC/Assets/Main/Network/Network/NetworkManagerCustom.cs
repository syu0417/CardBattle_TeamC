using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
#pragma warning disable 0618 //警告無視

public class NetworkManagerCustom : NetworkManager
{


    ///////////////////////////
    //シングルプレイー
    ///////////////////////////
    public static void SingleGame()
    {
        //サーバーとクライアントとして起動(プレイー人数は1人)
        singleton.StartHost(singleton.connectionConfig, 1);

    }



    ///////////////////////////
    //ローカルプレイー
    ///////////////////////////
    public static void LanGame()
    {
        singleton.StartCoroutine((singleton as NetworkManagerCustom).DiscoveryNetwork());

    }

    public IEnumerator DiscoveryNetwork()
    {
        //Discoveryコンポーネントを取得する
        NetworkDiscoverCustom discovery = GetComponent<NetworkDiscoverCustom>();

        //コンポーネントを初期化
        discovery.Initialize();

        //ローカル内のサーバーを検査(すでにサーバーがあれば参加する)
        discovery.StartAsClient();

        yield return new WaitForSeconds(2);//2秒まつ

        //検索した結果サーバーがなければサーバーを構築する
        if (discovery.running)
        {
            discovery.StopBroadcast();            //アナウンスをストップさせる
            yield return new WaitForSeconds(0.5f);//0.5秒まつ

            discovery.StartAsServer();//サーバーとしてアナウンス
            StartHost();              //サーバーとクライアントとして実行
                                      // StartClient();            //クライアントとして実行    
                                      //StartServer();            //サーバーとして実行
        }
    }

    ///////////////////////////
    //マルチプレイー
    ///////////////////////////
    public static void NetGame()
    {
        singleton.StartMatchMaker();//UNETマルチプレイー機能を実行
        singleton.matchMaker.ListMatches(0,　//Listの番号
                                         20,
                                         " ",//検索する部屋の名前
                                         false,
                                          0,
                                          0,
                                          singleton.OnMatchList);

    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (!success)
        {
            return;//失敗
        }

        if (matchList != null)
        {
            List<MatchInfoSnapshot> availableMatcher = new List<MatchInfoSnapshot>();
            foreach (MatchInfoSnapshot match in matchList)
            {
                //プレイヤー人数     //最大プレイヤー人数
                if (match.currentSize < match.maxSize)
                {
                    //プレイヤー人数が満たされていない場合を保存
                    availableMatcher.Add(match);
                }

            }

            //Listの数は0ならサーバーを構築する,
            //0ではないならサーバーに参加する
            if (availableMatcher.Count == 0)
            {
                //サーバーを構築する
                CreatMatch();

            }
            else
            {
                //サーバーに参加
                matchMaker.JoinMatch(availableMatcher[Random.Range(0, availableMatcher.Count - 1)].networkId, " ", " ", " ", 0, 0, OnMatchJoined);

            }


        }// if(matchList!=null)

    }// void OnMatchList()


    void CreatMatch()//UNETにサーバーを構築すると伝える
    {
        matchMaker.CreateMatch(" ",      //部屋の名前
                               matchSize,//プレイヤー人数
                               true,
                               " ",
                               " ",
                               " ",
                               0,
                               0,
                               OnMatchCreate);
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (!success)
        {
            return;//失敗
        }

        StartHost(matchInfo);//Unetから返されたmatchInfoでサーバーを構築する

    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (!success)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
            return;

        }

        StartClient(matchInfo);//Unetから返されたmatchInfoでクライアントとして実行

    }

    private void FixedUpdate()
    {


        GameObject player = GameObject.FindGameObjectWithTag("Player");//Playerタグをゲット

        //CardManager cardManager = player.GetComponent<CardManager>();
        if(player!=null)
        {
            CardManager cardManager = player.GetComponent<CardManager>();
            if (cardManager.StopHostFlag == true)
            {
                StopHost();
            }
        }
        

    }

    //PlayerObjectをGameManagerの設定に従って生成する
    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    int teamIndex = PlayerNoManager.GetInstance().GetTeamFill();

    //    GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);//ないとキャラが操作出来ない

    //    CardManager cardManager = player.GetComponent<CardManager>();

    //    cardManager.teamIndex = teamIndex;

    //    PlayerNoManager.GetInstance().size[teamIndex]++;
    //    // 2人目のクライアントが接続されたら、
    //    // プレイヤーのタグを切り替える
    //    foreach (NetworkConnection netconn in NetworkServer.connections)
    //    {
    //        if (conn != netconn)
    //        {
    //            player.tag = "Enemy";
    //        }
    //    }
    //}
    //終了処理
    private void OnApplicationQuit()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");//Playerタグをゲット

        //CardManager cardManager = player.GetComponent<CardManager>();
        if (player != null)
        {
            CardManager cardManager = player.GetComponent<CardManager>();
            if (cardManager.StopHostFlag == true)
            {
                StopHost();
            }
        }
    }
}
