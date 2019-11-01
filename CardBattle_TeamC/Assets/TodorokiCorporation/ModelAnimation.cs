using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimation : MonoBehaviour
{
    private Animator animator;

    private const string key_isIdle01 = "isIdle01";
    private const string key_isIdle02 = "isIdle02";
    private const string key_isWalk01 = "isWalk01";
    private const string key_isWalk02 = "isWalk02";
    private const string key_isJump = "isJump";
    private const string key_isAttack = "isAttack";

    public bool Female = false;

    // Start is called before the first frame update
    void Start()
    {
        // Animatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /////////////////////////////////////////////////
    /// 外部から以下の関数実行でアニメーション再生
    /////////////////////////////////////////////////
    public void Idle()
    {
        if (Female)
        {
            this.GetComponent<ModelAnimation>().Idle01();
        }
        else
        {
            this.GetComponent<ModelAnimation>().Idle02();
        }
    }

   
    public void Attack()
    {
        //攻撃アニメーション
        this.animator.SetBool(key_isJump, false);
        this.animator.SetBool(key_isAttack, true);
        this.animator.SetBool(key_isWalk01, false);
        this.animator.SetBool(key_isWalk02, false);
        this.animator.SetBool(key_isIdle01, false);
        this.animator.SetBool(key_isIdle02, false);
    }

    public void Jump()
    {
        //ジャンプアニメーション
        this.animator.SetBool(key_isJump, true);
        this.animator.SetBool(key_isAttack, false);
        this.animator.SetBool(key_isWalk01, false);
        this.animator.SetBool(key_isWalk02, false);
        this.animator.SetBool(key_isIdle01, false);
        this.animator.SetBool(key_isIdle02, false);
    }
    public void Walk()
    {
        if (Female)
        {
            this.GetComponent<ModelAnimation>().Walk01();
        }
        else
        {
            this.GetComponent<ModelAnimation>().Walk02();
        }
    }


    private void Walk01()
    {
        //移動アニメーション
        this.animator.SetBool(key_isJump, false);
        this.animator.SetBool(key_isAttack, false);
        this.animator.SetBool(key_isWalk01, true);
        this.animator.SetBool(key_isWalk02, false);
        this.animator.SetBool(key_isIdle01, false);
        this.animator.SetBool(key_isIdle02, false);
    }
    private void Walk02()
    {
        //移動アニメーション
        this.animator.SetBool(key_isJump, false);
        this.animator.SetBool(key_isAttack, false);
        this.animator.SetBool(key_isWalk01, false);
        this.animator.SetBool(key_isWalk02, true);
        this.animator.SetBool(key_isIdle01, false);
        this.animator.SetBool(key_isIdle02, false);
    }

    private void Idle01()
    {
        //移動アニメーション
        this.animator.SetBool(key_isJump, false);
        this.animator.SetBool(key_isAttack, false);
        this.animator.SetBool(key_isWalk01, false);
        this.animator.SetBool(key_isWalk02, false);
        this.animator.SetBool(key_isIdle01, true);
        this.animator.SetBool(key_isIdle02, false);
    }
    private void Idle02()
    {
        //移動アニメーション
        this.animator.SetBool(key_isJump, false);
        this.animator.SetBool(key_isAttack, false);
        this.animator.SetBool(key_isWalk01, false);
        this.animator.SetBool(key_isWalk02, false);
        this.animator.SetBool(key_isIdle01, false);
        this.animator.SetBool(key_isIdle02, true);
    }
}
