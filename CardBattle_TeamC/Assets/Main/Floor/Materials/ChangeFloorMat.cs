using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFloorMat : MonoBehaviour
{
    [Header("マテリアル達")]
    public Material[] _material = new Material[4];           // 割り当てるマテリアル


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetMaterial(int Num)
    {
        this.GetComponent<Renderer>().material = _material[Num-1];
    }
}
