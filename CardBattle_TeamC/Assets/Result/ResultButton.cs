using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{
    [Header("ボタンを押した時に飛ぶシーン名")]
    public string SceneName;
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
        if(SceneName!=null)
        {
            SceneManager.LoadScene(SceneName);//登録したシーンに飛ぶ
        }
    }
}
