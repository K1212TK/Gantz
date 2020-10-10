using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;


//ステートのクラス
public abstract class State
{
    //デリゲート
    public delegate void executeState();
    public executeState execDelegate;

    //実行処理
    public virtual void Execute()
    {
        if (execDelegate != null)
        {
            execDelegate();
        }
    }

    //ステート名を取得するメソッド
    public abstract string getStateName();
    //Aim時のステータスかを取得するメソッド 
    //0:通常時　1:AIM時 
    public abstract int getAimStateType();

    //攻撃のステータスかを判定する用のメソッド
    //0:攻撃ステータスではない 1:攻撃ステータス
    public abstract int getAttackStateType();
}