using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //A,Dキー入力値取得　
    float inputHorizontal;
    //W,Sキー入力値取得
    float inputVertical;
    //入力キー管理用Dictionary
    Dictionary<string, int> inputKeyBoardMap;


    private string nowKeyValue = "";
    public string NowKeyValue { get => nowKeyValue; private set => nowKeyValue = value; }

    //インスペクターから設定
    [SerializeField]
    private Player player;
    [SerializeField]
    private StateController stateController;
    [SerializeField]
    private CollisionDetection collision;
    [SerializeField]
    private WeaponManager weapon;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";



    // Start is called before the first frame update
    void Start()
    {
        //Dictionaryの初期化
        inputKeyBoardMap = new Dictionary<string, int>
        {
            {"inputHorizontal",0},
            {"inputVertical",0},
            {"inputCKey",0},
            {"inputSpace",0},
            {"inputMouseRight",0},
            {"inputMouseLeft",0},
        };
    }

    //入力キーの取得
    void Update()
    {
        //A,Dキー入力 左A(-1),右D(1) 入力がない場合0
        inputHorizontal = Input.GetAxisRaw(HORIZONTAL);
        //W,Sキー入力 上W(1),下S(-1) 入力がない場合0
        inputVertical = Input.GetAxisRaw(VERTICAL);

        //A,Dキー入力 左A(-1),右D(1) 入力がない場合0
        if (inputHorizontal != 0)
        {
            inputKeyBoardMap["inputHorizontal"] = 1;
        }
        else
        {
            inputKeyBoardMap["inputHorizontal"] = 0;
        }
        //W,Sキー入力 上W(1),下S(-1) 入力がない場合0
        if (inputVertical != 0)
        {
            inputKeyBoardMap["inputVertical"] = 1;
        }
        else
        {
            inputKeyBoardMap["inputVertical"] = 0;
        }

        //Cキー入力
        if (Input.GetKeyDown(KeyCode.C))
        {
            inputKeyBoardMap["inputCKey"] = 1;
        }
        //スペースキー入力
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputKeyBoardMap["inputSpace"] = 1;
        }
        //マウス右クリック
        if (Input.GetMouseButton(1))
        {
            inputKeyBoardMap["inputMouseRight"] = 1;
        }
        else
        {
            inputKeyBoardMap["inputMouseRight"] = 0;
        }
        //マウス左クリック
        if (Input.GetMouseButton(0))
        {
            inputKeyBoardMap["inputMouseLeft"] = 1;
        }
        else
        {
            inputKeyBoardMap["inputMouseLeft"] = 0;
        }
    }

    void FixedUpdate()
    {
        JoinInputValue();

        //何も入力がない場合
        if (NowKeyValue == "000000")
        {
            //ステータスをスタンドへ切り替え
            stateController.ChangeStateStand();
            return;
        }

        switch (NowKeyValue)
        {
            //移動 縦横斜め
            case "100000":
            case "010000":
            case "110000":
                //移動方向の値をセット
                stateController.moveState.inputHorizontal = inputHorizontal;
                stateController.moveState.inputVertical = inputVertical;
                //ステータスを歩きに変更
                stateController.ChangeStateMove();
                break;
            //ジャンプ
            case "000100":
            //移動時のジャンプ
            case "100100":
            case "010100":
            case "110100":
                //地面に触れている場合
                if (collision.IsGround)
                {
                    //移動方向の値をセット
                    stateController.moveState.inputHorizontal = inputHorizontal;
                    stateController.moveState.inputVertical = inputVertical;
                    //ステータスをジャンプに変更
                    stateController.ChangeStateJump();
                    //入力値の初期化
                    inputKeyBoardMap["inputSpace"] = 0;
                }
                break;
            //AIM 右クリック
            case "000010":
                //現在装備している武器が銃(MainWeapon)であった場合
                if (weapon.NowWeaponNum == 0)
                {
                    stateController.ChangeStateAimGun();
                }
                break;
            //左クリック
            case "000001":
                //現在装備している武器が刀(SubWeapon)であった場合
                if (weapon.NowWeaponNum == 1)
                {
                    stateController.ChangeStateSlashSword();
                }
                break;
            //左右クリック　射撃
            case "000011":
                //現在装備している武器が銃(MainWeapon)であった場合
                if (weapon.NowWeaponNum == 0)
                {
                    //ステータスを変更
                    stateController.ChangeStateShooting();
                }
                break;
            //ローリング
            case "001000":
                stateController.ChangeStateRolling();
                inputKeyBoardMap["inputCKey"] = 0;
                break;
            //スライディング
            case "101000":
            case "011000":
            case "111000":
                stateController.ChangeStateSlide();
                inputKeyBoardMap["inputCKey"] = 0;
                break;
            //移動時Aim
            case "100010":
            case "010010":
            case "110010":
                //現在装備している武器が銃(MainWeapon)であった場合
                if (weapon.NowWeaponNum == 0)
                {
                    //呼び出しステータスを変更する
                    stateController.ChangeStateAimMove();
                    player.Move(inputHorizontal, inputVertical);
                }
                break;
            //移動時AIM射撃
            case "100011":
            case "010011":
            case "110011":
                //現在装備している武器が銃(MainWeapon)であった場合
                if (weapon.NowWeaponNum == 0)
                {
                    //ステータスを変更
                    stateController.ChangeStateShooting();
                    player.Move(inputHorizontal, inputVertical);
                }
                break;
            //その他キー入力
            default:
                break;
        }
    }

    //Dictionaryのvalueを結合
    public void JoinInputValue()
    {
        NowKeyValue = "";

        foreach (KeyValuePair<string, int> inputKey in inputKeyBoardMap)
        {
            NowKeyValue = NowKeyValue + inputKey.Value;
        }
    }
}