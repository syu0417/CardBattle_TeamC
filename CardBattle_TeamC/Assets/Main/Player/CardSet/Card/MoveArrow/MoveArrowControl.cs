using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrowControl : MonoBehaviour
{
    private int FloorX = 0;
    private int FloorY = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFloorNum(int fX,int fY)
    {
        FloorX = fX;
        FloorY = fY;
    }

    public int GetFloorNum()
    {
        return FloorY * 7 + FloorX;
    }
}
