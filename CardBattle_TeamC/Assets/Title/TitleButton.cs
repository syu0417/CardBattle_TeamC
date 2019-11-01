using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ButtonSts
{
    START,
    CONFIG,
    EXIT
}
public class TitleButton : MonoBehaviour
{
    private Button MyButton;
    
    [Header("ボタンの種類")]
    public ButtonSts MySts;

    [SerializeField, Header("コンフィグオブジェクト")]
    GameObject ConfigObj;

    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();
        
        MyButton.onClick.AddListener(OnClickButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        switch (MySts)
        {
            case ButtonSts.START:
                AudioManager.Instance.Play_SE_Click();
                SceneManager.LoadScene("ModeSelect");
                break;

            case ButtonSts.CONFIG:
                if(ConfigObj!=null)
                {
                    AudioManager.Instance.Play_SE_Click();
                    ConfigObj.SetActive(true);
                }
                Debug.Log("コンフィグ");
                break;

            case ButtonSts.EXIT:
                AudioManager.Instance.Play_SE_Click();
                this.GetComponent<ExitFunc>().Quit();//終了処理
                break;
        }
    }

    
}
