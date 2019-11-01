using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum _phase
{
    deploy = 0,
    Select,
    Move,
    Action,
    result
}

public class NetGameManager : NetworkBehaviour
{
    _phase phase = _phase.deploy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (phase)
        {
            case _phase.deploy:
                {
                    //if (true)
                    //{
                    //    phase = _phase.Select;

                    //    GameObject.Find("Camera").SetActive(false);
                    //    Camera.main.gameObject.SetActive(true);
                    //}
                }
                break;
            case _phase.Select:
                {

                }
                break;
            case _phase.Move:
                {

                }
                break;
            case _phase.Action:
                {

                }
                break;
            case _phase.result:
                {

                }
                break;
        }
    }
}
