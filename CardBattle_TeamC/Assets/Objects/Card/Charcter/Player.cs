using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UniRx;
using UniRx.Triggers;

public class Player : MonoBehaviour
{
    private GameObject Obj;
    [SerializeField] Vector3 Vec;
    [SerializeField, Header("1回で動く量")]
    public float Speed;

    [Header("初期位置(X:横,Z:縦)")]
    public int PosX;
    public int PosZ;
    
    [SerializeField]
    private int Array_X_Num;
    [SerializeField]
    private int Array_Y_Num;

    private int Old_Array_X_Num;
    private int Old_Array_Y_Num;

    private Vector3 OldPos;

    [SerializeField]private bool isActive = false;
    private bool MySts = true;//trueなら動ける

    public bool PileSts=false;

    private bool FastFlame = false;//最初のフレームに当たってる判定を取らない
    [SerializeField]
    private bool BattleFlg = false;

    [Header("強さ(1～4)"),Range(1,4)]
    public int Power;

    private int MoveSpd;

    // 子供にしなくても位置座標を追従してくれるもの
    private ConstraintSource myConstraintSource;
    private PositionConstraint myPositionConstaint;

    // Start is called before the first frame update
    void Start()
    {
        //マスの番号ごとに座標を設定
        Vec =new Vector3((float)PosX*10.0f,0.1f,(float)PosZ * 10.0f );
        //Vec = GameObject.Find("Floor").transform.position;
        //this.transform.position = Vec;

        //初期位置を元に番号セット
        Array_X_Num = PosX;
        Array_Y_Num = PosZ*7;

        Old_Array_X_Num = 0;
        Old_Array_Y_Num = 0;

        this.UpdateAsObservable().Where(_=>this.isActive&&this.MySts).
            Subscribe(_ => Controller());
        this.UpdateAsObservable().Take(1).Subscribe(_ => FastFlame = true);
        //UpdateでY座標のみセット(1回のみ)
        this.UpdateAsObservable().Take(1).
            Subscribe(_ => this.transform.position = new Vector3(Vec.x, 0.1f, Vec.z));

        // 追従用の初期化
        this.gameObject.AddComponent<PositionConstraint>();
        this.myPositionConstaint = GetComponent<PositionConstraint>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isActive)
        //{
        //    Debug.Log("false");
        //}
        //else
        //{
        //    Debug.Log("true");
        //}

        //if(transform.childCount > 3)
        //{
        //    Vector3 vec = transform.position;

        //    transform.GetChild(2).transform.localPosition = new Vector3(vec.x, transform.GetChild(2).transform.localPosition.y, vec.z);
        //}

        if(isActive && this.transform.GetChild(0).gameObject.activeInHierarchy == false &&
            this.transform.parent.GetChild(1).GetComponent<ChaseTarget>().GetisStop() == true)
        {
            for (int cnt = 0; cnt < this.transform.childCount; cnt++)
            {
                this.transform.GetChild(cnt).gameObject.SetActive(true);
            }
        }
        else if(isActive == false && this.transform.GetChild(0).gameObject.activeInHierarchy == true &&
            this.transform.parent.GetChild(1).GetComponent<ChaseTarget>().GetisStop() == false)
        {
            for (int cnt = 0; cnt < this.transform.childCount; cnt++)
            {
                this.transform.GetChild(cnt).gameObject.SetActive(false);
            }
        }
    }


    void Controller()
    {
        if (this.transform.parent.GetChild(1).GetComponent<ChaseTarget>().GetisStop())
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))//右
            {
                OldPos = transform.position;
                transform.Translate(Speed, 0, 0);
                if (Array_X_Num != 36)
                {
                    if (Array_X_Num == 35)
                    {
                        Old_Array_X_Num = Array_X_Num;
                        Array_X_Num = -1;
                    }

                    if (Array_X_Num + Array_Y_Num != 20)
                    {
                        Old_Array_X_Num = Array_X_Num;
                        Array_X_Num += 1;
                        if (Array_X_Num > 6)
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num = 6;
                            OldPos = transform.position;
                            transform.Translate(-Speed, 0, 0);
                        }
                    }
                    else
                    {
                        Old_Array_X_Num = Array_X_Num;
                        Array_X_Num = 36;
                    }
                }
                else
                {
                    OldPos = transform.position;
                    transform.Translate(-Speed, 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))//左
            {
                OldPos = transform.position;
                transform.Translate(-Speed, 0, 0);
                if (Array_X_Num != 35)
                {
                    if (Array_X_Num == 36)
                    {
                        Old_Array_X_Num = Array_X_Num;
                        Array_X_Num = 7;
                    }

                    if (Array_X_Num + Array_Y_Num != 14)
                    {
                        Old_Array_X_Num = Array_X_Num;
                        Array_X_Num -= 1;
                        if (Array_X_Num < 0)
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num = 0;
                            OldPos = transform.position;
                            transform.Translate(Speed, 0, 0);
                        }
                    }
                    else
                    {
                        Old_Array_X_Num = Array_X_Num;
                        Array_X_Num = 35;
                    }
                }
                else
                {
                    OldPos = transform.position;
                    transform.Translate(Speed, 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))//上
            {
                OldPos = transform.position;
                transform.Translate(0, 0, Speed);
                Old_Array_Y_Num = Array_Y_Num;
                Array_Y_Num += 7;
                if (Array_Y_Num > 28)
                {
                    Old_Array_Y_Num = Array_Y_Num;
                    Array_Y_Num = 28;
                    OldPos = transform.position;
                    transform.Translate(0, 0, -Speed);
                }
                else if(Array_X_Num >= 35)
                {
                    Old_Array_Y_Num = Array_Y_Num;
                    Array_Y_Num -= 7;
                    OldPos = transform.position;
                    transform.Translate(0, 0, -Speed);
                }

            }
            if (Input.GetKeyDown(KeyCode.DownArrow))//下
            {
                OldPos = transform.position;
                transform.Translate(0, 0, -Speed);
                Old_Array_Y_Num = Array_Y_Num;
                Array_Y_Num -= 7;
                if (Array_Y_Num < 0)
                {
                    Old_Array_Y_Num = Array_Y_Num;
                    Array_Y_Num = 0;
                    OldPos = transform.position;
                    transform.Translate(0, 0, Speed);
                }
                else if (Array_X_Num >= 35)
                {
                    Old_Array_Y_Num = Array_Y_Num;
                    Array_Y_Num += 7;
                    OldPos = transform.position;
                    transform.Translate(0, 0, Speed);
                }
            }
        }
    }

    public void SetisActive(bool flg)
    {
        isActive = flg;
    }

    public void SetStatus(bool sts)
    {
        MySts = sts;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == this.transform.tag && FastFlame)
        {
            Debug.Log("Hit(Card)");
            PileSts = true;
        }
        if(collision.collider.tag != this.transform.tag && FastFlame)
        {
            Debug.Log("Battle");
            BattleFlg = true;

            Array_X_Num = Old_Array_X_Num;
            Array_Y_Num = Old_Array_Y_Num;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == this.transform.tag && FastFlame)
        {
            Debug.Log("Hit(Card)");
            PileSts = false;
        }
        if (collision.collider.tag != this.transform.tag && FastFlame)
        {
            BattleFlg = false;
        }
    }

    public bool GetPileSts()
    {
        return PileSts;
    }

    public bool GetBattleFlg()
    {
        return BattleFlg;
    }

    public void SetBattleFlg(bool Flg)
    {
        BattleFlg = Flg;
    }

    public void Move()//動くスピードセット
    {
        switch(Power)
        {
            case 1:
                MoveSpd = 1;
                break;

            case 2:
                MoveSpd = 1;
                break;

            case 3:
                MoveSpd = 2;
                break;

            case 4:
                MoveSpd = 1;
                break;
        }
    }

    public void SetArrayNum(int ax,int ay)
    {
        Old_Array_X_Num = Array_X_Num;
        Old_Array_Y_Num = Array_Y_Num;

        Array_X_Num = ax;
        Array_Y_Num = ay;
    }

    public int GetArrayNumX()
    {
        return Array_X_Num;
    }

    public int GetArrayNumY()
    {
        return Array_Y_Num;
    }

    public int GetOldArrayNumX()
    {
        return Old_Array_X_Num;
    }

    public int GetOldArrayNumY()
    {
        return Old_Array_Y_Num;
    }

    public int GetPower()//カードのパワーを取得
    {
        return Power;
    }

    public void SetObjectConstraintTransform(Transform parent)
    {
        // もしすでに追従していた場合はそれを削除する（バグ処理）
        this.DeleteSource();

        // Constraint の参照元を設定
        this.myConstraintSource.sourceTransform = parent;
        this.myConstraintSource.weight = 1.0f;  // 影響度：1に近いほど支配度が高い　初期値は0なので修正する

        //#if POSITION_CONSTRAINT
        this.myPositionConstaint.AddSource(this.myConstraintSource);
        this.myPositionConstaint.translationOffset = Vector3.zero;
        this.myPositionConstaint.enabled = true;
        this.myPositionConstaint.constraintActive = true;
    }

    public void DeleteSource()
    {
        if (this.myPositionConstaint.sourceCount > 0)
        {
            this.myPositionConstaint.RemoveSource(0);
            this.myPositionConstaint.enabled = false;
        }
    }

    public void MoveControl(int MoveNum)
    {
        if (this.transform.parent.GetChild(1).GetComponent<ChaseTarget>().GetisStop())
        {
            switch (MoveNum)
            {
                case 1://右
                    transform.Translate(Speed, 0, 0);
                    if (Array_X_Num != 36)
                    {
                        if (Array_X_Num == 35)
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num = -1;
                        }

                        if (Array_X_Num + Array_Y_Num != 20)
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num += 1;
                            if (Array_X_Num > 6)
                            {
                                Old_Array_X_Num = Array_X_Num;
                                Array_X_Num = 6;
                                transform.Translate(-Speed, 0, 0);
                            }
                        }
                        else
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num = 36;
                        }
                    }
                    else
                    {
                        transform.Translate(-Speed, 0, 0);
                    }
                    break;

                case 2://下
                    transform.Translate(0, 0, -Speed);
                    Old_Array_Y_Num = Array_Y_Num;
                    Array_Y_Num -= 7;
                    if (Array_Y_Num < 0)
                    {
                        Old_Array_Y_Num = Array_Y_Num;
                        Array_Y_Num = 0;
                        transform.Translate(0, 0, Speed);
                    }
                    else if (Array_X_Num >= 35)
                    {
                        Old_Array_Y_Num = Array_Y_Num;
                        Array_Y_Num += 7;
                        transform.Translate(0, 0, Speed);
                    }
                    break;

                case 3://左
                    transform.Translate(-Speed, 0, 0);
                    if (Array_X_Num != 35)
                    {
                        if (Array_X_Num == 36)
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num = 7;
                        }

                        if (Array_X_Num + Array_Y_Num != 14)
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num -= 1;
                            if (Array_X_Num < 0)
                            {
                                Old_Array_X_Num = Array_X_Num;
                                Array_X_Num = 0;
                                transform.Translate(Speed, 0, 0);
                            }
                        }
                        else
                        {
                            Old_Array_X_Num = Array_X_Num;
                            Array_X_Num = 35;
                        }
                    }
                    else
                    {
                        transform.Translate(Speed, 0, 0);
                    }
                    break;

                case 4://上
                    transform.Translate(0, 0, Speed);
                    Old_Array_Y_Num = Array_Y_Num;
                    Array_Y_Num += 7;
                    if (Array_Y_Num > 28)
                    {
                        Old_Array_Y_Num = Array_Y_Num;
                        Array_Y_Num = 28;
                        transform.Translate(0, 0, -Speed);
                    }
                    else if (Array_X_Num >= 35)
                    {
                        Old_Array_Y_Num = Array_Y_Num;
                        Array_Y_Num -= 7;
                        transform.Translate(0, 0, -Speed);
                    }
                    break;
            }
        }
    }
    
    //public void OnClicked()
    //{
    //    if(isActive == false)
    //    {
    //        print("select!!");

    //        isActive = true;
    //        //this.transform.parent.GetChild(1).GetComponent<ChaseTarget>().SetisActive(true);

    //        // マネージャーの方でコントロール対象を切り替える
    //        // 一回送るだけだからそんなに重くないはず
    //        GameObject.Find("GameManager").GetComponent<GameManager>().ControlFlg = true;
    //    }
    //}
    
}
