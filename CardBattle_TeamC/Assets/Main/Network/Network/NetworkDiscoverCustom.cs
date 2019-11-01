using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#pragma warning disable 0618 //警告無視

public class NetworkDiscoverCustom : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        StopBroadcast();
        NetworkManagerCustom.singleton.networkAddress = fromAddress;
        NetworkManagerCustom.singleton.StartClient();
    }



}
