using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tips : MonoBehaviour
{
    [Header("1つ前のページ")]
    public GameObject BeforeTutorial;
    [Header("1つ次のページ")]
    public GameObject NextTutorial;

    [Header("最初のページ")]
    public GameObject StartTutorial;

    [Header("表示状態を切り替えたいオブジェクト群の親")]
    public GameObject Parent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))//右クリック
        {
            AudioManager.Instance.Play_SE_Click();
            //一つ前のページがあれば
            if (BeforeTutorial != null)
            {
                //一つ前のページへ
                BeforeTutorial.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }
        else if (Input.GetMouseButtonDown(0))//左クリック
        {
            AudioManager.Instance.Play_SE_Click();
            //次のページがあれば
            if (NextTutorial != null)
            {
                //次のページへ
                NextTutorial.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else//次のページがなければ
            {
                //モードセレクトへ
                //SceneManager.LoadScene("ModeSelect");
                //自分を非表示に
                if(Parent!=null)
                {
                    Parent.SetActive(false);
                    this.gameObject.SetActive(false);
                    StartTutorial.SetActive(true);//初期状態に
                }
            }
        }
    }
    //bool GetBeforeKeyDown()//戻るボタンが押されたのを取得
    //{
    //    Debug.Log("beforedown");
    //    if (Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}
    //bool GetNextKeyDown()//次へボタンが押されたのを取得
    //{
    //    Debug.Log("nextdown");
    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}
}
