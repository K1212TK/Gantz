using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //武器のコライダーがオブジェクトと衝突した場合
    void OnTriggerEnter(Collider col)
    {
        //当たった対象がenemy　かつ　攻撃ステータスの場合
        if (col.tag == "Enemy" && state.stateProcessor.State.getAttackStateType() == 1)
        {
            //オーバーヒートしていない場合
            if (!HasOverHeat && !isNextAttack)
            {
                //武器の使用回数を減算
                MinusWeaponNum();
                StartCoroutine("NextAttack",0.6f);
                Debug.Log("斬撃!");
                //衝突した対象のゲームオブジェクトの取得
                //オブジェクトのhpを減らす
            }
        }
    }
}
