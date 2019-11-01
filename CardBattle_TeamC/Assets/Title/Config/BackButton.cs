using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField, Header("親オブジェクト")]
    GameObject MyParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        if(MyParent!=null)
        {
            AudioManager.Instance.Play_SE_Click();
            MyParent.SetActive(false);
        }
    }
}
