using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCard : MonoBehaviour
{
    [Header("カードパワー"),Range(1,4)]
    public int Power;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPower()//パワーを返す
    {
        return Power;
    }
}
