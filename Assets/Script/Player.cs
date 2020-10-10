using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //インスペクターから設定

    //通常移動スピード
    [SerializeField]
    private float defaultWalkSpeed;
    //AIM時移動スピード
    [SerializeField]
    private float aimWalkSpeed;

    //ジャンプ力
    [SerializeField]
    private float jumpForce;
    //ローリング力
    [SerializeField]
    private float rollingForce;
    //スライディング力
    [SerializeField]
    private float slideForce;
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private CollisionDetection collision;
    [SerializeField]
    private GameObject aimCamera;
    [SerializeField]
    private StateController state;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //キャラクターを動かす
    public void Move(float inputHorizontal, float inputVertical)
    {
        Vector3 cameraForward;
        Vector3 moveForward;
        //AIM時
        if (state.stateProcessor.State.getAimStateType() == 1)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            cameraForward = Vector3.Scale(aimCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とエイムカメラの向きから移動方向を設定
            moveForward = cameraForward * inputVertical + aimCamera.transform.right * inputHorizontal;
            // 移動方向にスピードを掛ける
            rigidBody.velocity = moveForward * aimWalkSpeed + new Vector3(0, rigidBody.velocity.y, 0);
        }
        else
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            // 方向キーの入力値とメインカメラの向きから移動方向を設定
            moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;
            // 移動方向にスピードを掛ける
            rigidBody.velocity = moveForward * defaultWalkSpeed + new Vector3(0, rigidBody.velocity.y, 0);
        }


        //Aimカメラでない場合はカメラをキャラクターの向きに合わせる
        if (state.stateProcessor.State.getAimStateType() == 0)
        {
            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }
    //キャラクタージャンプ
    public void Jump()
    {
        //地面に触れていたら
        if (collision.IsGround)
        {
            rigidBody.AddForce(transform.up * jumpForce);

            //移動時にジャンプした場合
            if (state.moveState.inputHorizontal != 0 || state.moveState.inputVertical != 0)
            {
                rigidBody.AddForce(transform.forward * rollingForce);
            }
        }
    }

    //キャラクターローリング
    public void Rolling()
    {
        //地面に触れていたら
        if (collision.IsGround)
        {
            rigidBody.AddForce(transform.forward * rollingForce);
        }
    }

    //キャラクタースライディング
    public void Slide()
    {
        //地面に触れていたら
        if (collision.IsGround)
        {
            //slideForceの設定を行う
            rigidBody.AddForce(transform.forward * slideForce);
        }
    }

}
