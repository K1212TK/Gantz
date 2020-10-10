using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステートの実行を管理するクラス
public class StateProcessor
{
    //ステート本体
    private State _State;
    //プロパティ
    public State State
    {
        set { _State = value; }
        get { return _State; }
    }

    // 実行
    public void Execute()
    {
        State.Execute();
    }
}
