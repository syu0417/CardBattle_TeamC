using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material[] _material;           // 割り当てるマテリアル.
    private int Power;
    public GameObject MyParent;
    // Start is called before the first frame update
    void Start()
    {
        Power = MyParent.GetComponent<CardSet>().GetPower();
        this.GetComponent<Renderer>().material = _material[Power-1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
