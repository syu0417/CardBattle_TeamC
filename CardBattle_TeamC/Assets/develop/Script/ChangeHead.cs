using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHead : MonoBehaviour
{
    
    public GameObject[] Heads2;
    public GameObject[] Heads3;
    [SerializeField]
    private int equipment2;
    private int equipment3;

    // Start is called before the first frame update
    void Start()
    {
        //　初期装備設定
        equipment2 = 0;
        for(int i = 0; i < Heads2.Length; i++)
        {
            if(Heads2[i]!=null)
            {
                Heads2[i].SetActive(false);
            }
        }
        equipment3 = 0;
        for (int i = 0; i < Heads3.Length; i++)
        {
            if (Heads3[i] != null)
            {
                Heads3[i].SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHeads2(int power)
    {
        equipment2 = power;
        for (var i = 0; i < Heads2.Length; i++)
        {
            if (i == equipment2)
            {
                if(Heads2[i]!=null)
                {
                    Heads2[i].SetActive(true);
                }

            }
            else
            {
                if(Heads2[i]!=null)
                {
                    Heads2[i].SetActive(false);
                }
            }
        }
    }
    public void ChangeHeads3(int power)
    {
        equipment3 = power;
        for (var i = 0; i < Heads3.Length; i++)
        {
            if (i == equipment3)
            {
                if(Heads3[i]!=null)
                {
                    Heads3[i].SetActive(true);
                }

            }
            else
            {
                if(Heads3[i]!=null)
                {
                    Heads3[i].SetActive(false);
                }
            }
        }
    }
}
