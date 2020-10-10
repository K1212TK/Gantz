using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Weapon : MonoBehaviour
{

    [Header("プレイヤーステータス")]
    [SerializeField]
    protected StateController state;
    [Header("オーバーヒート時に表示するスライダー")]
    [SerializeField]
    protected ParameterSlider overHeatSlider;
    [Header("武器の攻撃力")]
    [SerializeField]
    protected int power;
    [Header("クールタイムにかかる時間")]
    [SerializeField]
    protected int coolTime;
    [Header("連続攻撃できる回数")]
    [SerializeField]
    protected int continuousAttackNum;

    //次の攻撃に遷移できるかのフラグ
    protected bool isNextAttack;
    //武器のオーバーヒートの判定
    private bool hasOverHeat;
    //使用回数を減算する際の変数
    private int useWeaponNum;

    private float coolValue;

    public bool HasOverHeat { get => hasOverHeat; private set => hasOverHeat = value; }

    // Start is called before the first frame update
    protected void Start()
    {
        //初期使用数のセット
        useWeaponNum = continuousAttackNum;
        coolValue = 1.0f / continuousAttackNum;
    }

    // Update is called once per frame
    void Update()
    {

    }


    //武器の使用回数を減算する 攻撃実行時に呼び出し
    protected void MinusWeaponNum()
    {
        //オーバーヒートしていない場合は減算
        if (!HasOverHeat)
        {
            useWeaponNum = useWeaponNum - 1;
            //オーバーヒートのスライダーを減算
            overHeatSlider.MinusSlideParameter(coolValue);
        }

        //useWeaponNum　0以下かつオーバーヒートしていない場合
        if (useWeaponNum <= 0 && !HasOverHeat)
        {
            //オーバーヒートさせる
            HasOverHeat = true;
            //クールダウン処理の実行
            StartCoroutine("CoolDown");
        }
    }

    //coolTime秒間経過後,オーバーヒートのフラグを戻す
    IEnumerator CoolDown()
    {
        for (int i = 0; i < continuousAttackNum; i++)
        {
            //1秒ごとにオーバーヒートの解除処理を行う
            yield return new WaitForSeconds(1);
            //スライダーのvalueへcoolValueをadd
            overHeatSlider.AddSliderParameter(coolValue);
        }
        //オーバーヒートのフラグを元に戻す
        HasOverHeat = false;
        //useWeaponNumの初期化
        useWeaponNum = continuousAttackNum;
    }

    //一回攻撃したら秒間攻撃の制御を行う
    protected IEnumerator NextAttack(float waitTime)
    {
        isNextAttack = true;
        yield return new WaitForSeconds(waitTime);
        isNextAttack = false;
    }

    //ゲームオブジェクトが非アクティブになった時に実行
    void OnDisable()
    {
        //オーバーヒートのフラグがtrueの場合コルーチンの中断
        if (hasOverHeat)
        {
            //コルーチンの停止
            StopCoroutine("CoolDown");
        }
    }
    //ゲームオブジェクトがアクティブになった時に実行
    void OnEnable()
    {
        //オーバーヒートのフラグがtrueの場合コルーチンの再開
        if (hasOverHeat)
        {
            //コルーチンの再開
            StartCoroutine("CoolDown");
        }
    }
}
