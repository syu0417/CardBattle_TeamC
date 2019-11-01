using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManeger : MonoBehaviour
{
    //３つのPanelを格納する変数
    //インスペクターウィンドウからゲームオブジェクトを設定する
    [SerializeField] GameObject Win;
    [SerializeField] GameObject Lose;
    [SerializeField] GameObject Menu;
    [SerializeField] GameObject Option;


    // Start is called before the first frame update
    void Start()
    {
        //BackToMenuメソッドを呼び出す
        BackToMenu();
    }


    //勝利した時に呼び出される処理
    public void SelectWinDescription()
    {
        Option.SetActive(false);
        Menu.SetActive(false);
        Win.SetActive(true);
    }

    //敗北した時に呼び出される処理
    public void SelectLoseDescription()
    {
        Option.SetActive(false);
        Menu.SetActive(false);
        Win.SetActive(false);
        Lose.SetActive(true);
    }

    //Optionボタンを押したときの処理
    public void SelectMenuDescription()
    {
        Option.SetActive(false);
        Menu.SetActive(true);
    }


    //2つのDescriptionPanelでBackButtonが押されたときの処理
    //MenuPanelをアクティブにする
    public void BackToMenu()
    {
        Option.SetActive(true);
        Win.SetActive(false);
        Menu.SetActive(false);
    }
}
