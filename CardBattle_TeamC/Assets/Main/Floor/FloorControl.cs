using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControl : MonoBehaviour
{
    [SerializeField]
    private int FloorNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetFloorNum(int num)
    {
        FloorNum = num;
    }

    public int GetFloorNum()
    {
        return FloorNum;
    }
}
