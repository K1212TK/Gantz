using PlayerState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    //プレイヤーの情報をインスペクターから取得
    [SerializeField]
    private Player player;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SoundManager sound;
    [SerializeField]
    private ParticleManager particle;
    [SerializeField]
    private WeaponManager weapon;


    //現在他のアニメーションが再生されているか否かのチェック
    public bool animationPlayFlg;

    //変更前のステート名
    private string _beforeStateName;
    //各ステータスのインスタンスを生成
    public StateProcessor stateProcessor = new StateProcessor();
    public StandState standState = new StandState();
    public MoveState moveState = new MoveState();
    public JumpState jumpState = new JumpState();
    public AimGunState aimGunState = new AimGunState();
    public ShootingState shootingState = new ShootingState();
    public SlashSwordState slashSwordState = new SlashSwordState();
    public RollingState rollingState = new RollingState();
    public AimMoveState aimMoveState = new AimMoveState();
    public SlideState slideState = new SlideState();

    public string BeforeStateName
    {
        get => _beforeStateName;
        private set => _beforeStateName = value;
    }

    // Use this for initialization
    void Start()
    {
        //立ちステータスをデリゲートへセット
        standState.execDelegate = Stand;
        //歩きステータスをデリゲートへセット
        moveState.execDelegate = Move;
        //ジャンプステータスをデリゲートへセット
        jumpState.execDelegate = Jump;
        //AimGunステータスをデリゲートへセット
        aimGunState.execDelegate = StandAimGun;
        //shootingステータスをデリゲートへセット
        shootingState.execDelegate = Shooting;
        //rollingステータスをデリゲートへセット
        rollingState.execDelegate = Rolling;
        //slashSwordステータスをデリゲートへセット
        slashSwordState.execDelegate = SlashSword;
        //aimMoveステータスをデリゲートへセット
        aimMoveState.execDelegate = MoveAimGun;
        //slideステータスをデリゲートへセット
        slideState.execDelegate = Slide;
    }

    // Update is called once per frame
    void Update()
    {
        //stateがnullの場合は処理しない
        if (stateProcessor.State == null)
        {
            return;
        }

        //Aim中でない場合はAimのアニメーションを解除
        if (stateProcessor.State.getAimStateType() != 1)
        {
            animator.SetBool(aimGunState.getStateName(), false);
        }

        //前回のステータスとは異なる場合
        if (BeforeStateName != stateProcessor.State.getStateName())
        {
            //前回のステータスから現在のステータスへ書き換え
            BeforeStateName = stateProcessor.State.getStateName();
            //現在格納されているステートのデリゲートを実行する
            stateProcessor.Execute();
        }
    }

    //ステート名を返す
    public string GetStateName()
    {
        return BeforeStateName;
    }

    //ステータスがstandになったら実行される
    private void Stand()
    {
        animator.Play(stateProcessor.State.getStateName());
    }
    //ステータスがMoveになったら実行される
    private void Move()
    {
        animator.Play(stateProcessor.State.getStateName());
    }
    //ステータスがjumpになったら実行される
    private void Jump()
    {
        player.Jump();
        StartCoroutine("AnimationFlow");
    }
    //ステータスがAimGunになったら実行される
    private void StandAimGun()
    {
        animator.Play("Idle");
        //構えのアニメーションを呼び出し
        animator.SetBool(stateProcessor.State.getStateName(), true);
    }
    //ステータスがshootingになったら実行される
    private void Shooting()
    {
        //オーバーヒートしていない場合
        if (!weapon.IsOverHeatNowWeapon())
        {
            //銃撃音の再生
            sound.PlaySound(sound.shootingSound);
            //パーティクルの出力
            particle.PlayParticle(particle.shootParticle);
        }
        else
        {
            sound.PlaySound(sound.noneBulletSound);
        }
    }

    //ステータスがSlashSwordになったら実行される
    private void SlashSword()
    {
        //斬る音の再生
        sound.PlaySound(sound.slashSwordSound);
        //パーティクルの出力
        particle.PlayParticle(particle.slashSwordParticle);

        //通常攻撃
        if (!animator.GetBool("SlashSword"))
        {
            animator.SetBool("SlashSword", true);
        }
    }
    //ステータスがRollingになったら実行される
    private void Rolling()
    {
        player.Rolling();
        //ローリングアニメーションの実行
        StartCoroutine("AnimationFlow");
    }

    //ステータスがSlideになったら実行される
    private void Slide()
    {
        //スライドアニメーションの実行
        StartCoroutine("AnimationFlow");
        player.Slide();
    }

    //ステータスがAimMoveになったら実行される
    private void MoveAimGun()
    {
        //構えのアニメーションを呼び出し
        animator.SetBool(aimGunState.getStateName(), true);
        //歩くアニメーションを呼び出し
        animator.Play(stateProcessor.State.getStateName());

    }
    //何も入力がない場合呼び出し
    //ステートをstandに変更する
    public void ChangeStateStand()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステータスを立ちに変更
            stateProcessor.State = standState;
        }
    }
    //移動入力時呼び出し
    //ステートを歩きに変更
    public void ChangeStateMove()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステータスを走りに変更
            stateProcessor.State = moveState;
            //プレイヤーを動かす
            player.Move(moveState.inputHorizontal,moveState.inputVertical);
        }
    }
    //ジャンプボタンが押されたら呼び出し 
    //ステートをジャンプに変更する
    public void ChangeStateJump()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステートをジャンプに変更する
            stateProcessor.State = jumpState;
        }
    }
    //銃を装備した状態で右クリックされたら呼び出し
    //ステートをAimへ変更
    public void ChangeStateAimGun()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステートをAimに変更する
            stateProcessor.State = aimGunState;
        }
    }

    //剣を装備時左マウスクリック入力時呼び出し
    //ステートをSlashSwordに変更
    public void ChangeStateSlashSword()
    {
        //武器がオーバーヒートしていない場合
        if (!weapon.IsOverHeatNowWeapon())
        {
            //コンボ攻撃発生時は攻撃が連鎖するように設定
            if (animator.GetBool("ComboChance"))
            {
                //斬る音の再生
                sound.PlaySound(sound.slashSwordSound);
                //パーティクルの出力
                particle.PlayParticle(particle.slashSwordParticle);
                animator.SetTrigger("Attack");
            }

            //アニメーションが実行中でなければ
            if (!animationPlayFlg)
            {
                //現在のステートをslashSwordに変更する
                stateProcessor.State = slashSwordState;
            }
        }
    }
    //銃を装備した状態で右左クリックされたら呼び出し
    //ステートをshootingへ変更
    public void ChangeStateShooting()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステートをAimに変更する
            stateProcessor.State = shootingState;
        }
    }
    //ローリングボタン入力時呼び出し
    //ステートをローリングに変更
    public void ChangeStateRolling()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステートをローリングに変更する
            stateProcessor.State = rollingState;
        }
    }

    //スライディング入力時呼び出し
    //ステートをスライディングに変更
    public void ChangeStateSlide()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステートをローリングに変更する
            stateProcessor.State = slideState;
        }
    }

    //Aim時に移動ボタンが入力されたら呼び出し
    //ステートをAim移動に変更
    public void ChangeStateAimMove()
    {
        //アニメーションが実行中でなければ
        if (!animationPlayFlg)
        {
            //現在のステートをAim移動に変更する
            stateProcessor.State = aimMoveState;
        }
    }

    IEnumerator AnimationFlow()
    {
        animationPlayFlg = true;
        //現在のステータスのアニメーション再生
        animator.Play(stateProcessor.State.getStateName());
        // ステートの反映に1フレーム
        yield return null;
        yield return new WaitForAnimation(animator, 0);
        animationPlayFlg = false;
        //ChangeStateStand();   
    }
}
