using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : Weapon
{

    Ray ray;
    // outパラメータ用に、Rayのヒット情報を取得するための変数を用意
    RaycastHit hit;
    // 照準のImageをインスペクターから設定
    [SerializeField]
    private GameObject aimPointObject;
    //射程距離
    [SerializeField]
    private float rangeDistance;
    //レティクルの画像
    private SpriteRenderer aimPointImage;


    // Start is called before the first frame update
    void Start()
    {
        aimPointImage = aimPointObject.GetComponent<SpriteRenderer>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //現在のステータスが銃の構え　または　射撃時 または　構え歩きの場合
        if (state.stateProcessor.State.getAimStateType() == 1)
        {
            //レティクルの表示
            aimPointObject.SetActive(true);
            //rayのあたり判定を行う
            CollisionRay();
        }
        else
        {
            //レティクルの非表示
            aimPointObject.SetActive(false);
        }
    }

    //銃口のray衝突判定
    public void CollisionRay()
    {
        // Rayを飛ばす（第1引数がRayの発射座標、第2引数がRayの向き）
        ray = new Ray(transform.position, transform.forward);
        // シーンビューにRayを可視化するデバッグ用
        Debug.DrawRay(ray.origin, ray.direction * rangeDistance, Color.red, 0.0f);

        // Rayのhit情報を取得する　ヒットしていた場合
        if (Physics.Raycast(ray, out hit, rangeDistance) && hit.collider.tag =="Enemy")
        {
            //衝突対象のタグがEnemyの場合レティクルの色を緑へ
            aimPointImage.color = new Color(255f, 255f, 255f, 255f);
            
            //ステータスがshootingである　かつ　武器がオーバーヒートしていない
            if (state.stateProcessor.State.getStateName() == state.shootingState.getStateName()
                && !HasOverHeat
                && !isNextAttack)
            {
                StartCoroutine("NextAttack",0.2f);
                //武器の使用回数を減算
                MinusWeaponNum();
                //衝突した対象のオブジェクトのHpを削る
                Debug.Log("銃撃!");
            }
        }
        //何にもヒットしていない場合
        else
        {
            //衝突対象がEnemyでない場合、レティクルの色を黒へ
            aimPointImage.color = new Color(0f, 0f, 0f, 255f);
            
            //ステータスがshootingであった場合
            if (state.stateProcessor.State.getStateName() == state.shootingState.getStateName()
                && !HasOverHeat
                && !isNextAttack)
            {
                StartCoroutine("NextAttack", 0.2f);
                //武器の使用回数を減算
                MinusWeaponNum();
            }
        }
    }
}
