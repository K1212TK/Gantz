using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    //銃ナンバー
    const int MAIN_WEAPON_NUM = 0;
    //刀ナンバー
    const int SUB_WEAPON_NUM = 1;


    
    [Header("武器のオブジェクト")]
    [SerializeField]
    private Weapon[] weapons;
    [Header("武器のspriteGameObject")]
    [SerializeField]
    private Image[] weaponImage;
    //playerのステータス情報
    [Header("playerのステータス情報")]
    [SerializeField]
    StateController state;
    [Header("パーティクル")]
    [SerializeField]
    ParticleManager particle;
    [Header("サウンド")]
    [SerializeField]
    SoundManager sound;

    //現在の武器番号を格納
    private int nowWeaponNum;
    public int NowWeaponNum { get => nowWeaponNum; private set => nowWeaponNum = value; }

    // Start is called before the first frame update
    void Start()
    {
        //デフォルトの武器をセット
        weapons[MAIN_WEAPON_NUM].gameObject.SetActive(true);
        //武器画像のセット
        weaponImage[MAIN_WEAPON_NUM].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //武器切り替え
        CheckInputKey();
    }

    //1なら銃へ切り替え　2なら刀へ切り替え
    public void CheckInputKey()
    {
        //キー入力が1である　かつ　ステータスが立ち状態である場合 銃へ切り替え
        if (Input.GetKeyDown("1") && state.BeforeStateName == state.standState.getStateName()){
            //現在の武器ナンバー
            NowWeaponNum = MAIN_WEAPON_NUM;
            //武器の切り替え
            ChangeWeapon();
        }
        //キー入力が2である　かつ　ステータスが立ち状態である場合 銃へ切り替え
        else if (Input.GetKeyDown("2") && state.BeforeStateName == state.standState.getStateName())
        {
            //現在の武器ナンバー
            NowWeaponNum = SUB_WEAPON_NUM;
            //武器の切り替え
            ChangeWeapon();
        }
    }

    //武器の切り替え
    public void ChangeWeapon()
    {
        for (int i = 0; i < weapons.Length;i++)
        {
            //現在の武器ナンバーと同じであった場合
            if (i == NowWeaponNum)
            {
                if (!weapons[i].gameObject.activeSelf)
                {
                    //武器を切り替える
                    weapons[i].gameObject.SetActive(true);
                    //武器画像の変更
                    weaponImage[i].gameObject.SetActive(true);
                    //エフェクトを出力する
                    particle.PlayParticle(particle.WarpWeaponParticle);
                    //ワープ音の再生
                    sound.PlaySound(sound.warpWeaponSound);

                }
            }
            //ナンバーと異なる場合
            else
            {
                //武器を非表示に変更する
                weapons[i].gameObject.SetActive(false);
                //武器画像を非表示に変更する
                weaponImage[i].gameObject.SetActive(false);
            }
        }
    }

    //現在所持している武器がオーバーヒートしているかを返す
    public bool IsOverHeatNowWeapon()
    {
        bool result;

        //オーバーヒートしていたら
        if (weapons[nowWeaponNum].HasOverHeat)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
}
