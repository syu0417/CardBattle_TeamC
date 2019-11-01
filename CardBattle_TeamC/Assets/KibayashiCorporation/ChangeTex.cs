using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTex : MonoBehaviour
{
    public string TexName;
    public Texture tex;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().material.SetTexture("_BumpMap", tex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
