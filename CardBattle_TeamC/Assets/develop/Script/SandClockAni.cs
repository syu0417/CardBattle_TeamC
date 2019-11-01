using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandClockAni : MonoBehaviour
{
    private Animator animator;

    private const string key_isAni = "isAni";

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
