using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetTexture : MonoBehaviour
{
    [Header("ストックのテクスチャ")]
    public Sprite[] TexList = new Sprite[5];

    private Image image;

    [Header("残数を所持している親")]
    public GameObject MyParent;

    [Header("パワーを所持している親")]
    public GameObject MyPowerParent;
    private int Power;//自分のパワー
    // Start is called before the first frame update
    void Start()
    {
        Power = MyPowerParent.GetComponentInParent<SelectCard>().GetPower();
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetTextureNum(MyParent.GetComponent<CurrentSelected>().GetTexNum(Power));
    }

    public void SetTextureNum(int TexNum)
    {
        image.sprite = TexList[TexNum];
    }
}
