using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboAttack : StateMachineBehaviour
{
    private StateController state;
    float deltaTime;
    float waitTime = 0.1f;  // 連続攻撃の受付開始時間（秒）
    float overTime = 1.2f;  // 連続攻撃の受付終了時間（秒）

    //MonoBehaviourでのStart()メソッドと同じ
    //新しいステートに移り変わった時に実行
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //プレイヤーのステートコントローラーの取得
        state = GameObject.Find("Player").GetComponent<StateController>();
        deltaTime = 0f;
    }

    //MonoBehaviourでのUpdate()メソッドと同じ
    //スクリプトをアタッチしたステートメントが実行中に呼び出される
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //アニメーションを切り替えられないように制御
        state.animationPlayFlg = true;
        deltaTime += Time.deltaTime;

        // 連続攻撃の受付開始
        if (deltaTime > waitTime && !animator.GetBool("ComboChance"))  
        {
            animator.SetBool("ComboChance", true);
        }
        // 連続攻撃の受付終了
        if (deltaTime > overTime && animator.GetBool("ComboChance"))   
        {
            animator.SetBool("ComboChance", false);
            animator.SetBool("SlashSword", false);
        }
    }

    //MonoBehaviourでのOnDestroy()メソッドと同じ ステートが終了したら呼び出される
    //ステートが次のステートに移り変わる直前に実行
    //スクリプトが貼り付けられたステートマシンから出て行く時に実行
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        // 連続攻撃の受付終了
        animator.SetBool("ComboChance", false); 
        //攻撃終了時アニメーションを切り替えられるように
        state.animationPlayFlg = false;
    }


}