using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //追従対象のオブジェクト
    [SerializeField]
    private GameObject targetObj;
    //マウス感度
    [SerializeField]
    private float sensitivityMouse;
    //エイムカメラ
    [SerializeField]
    private GameObject aimCamera;
    //プレイヤーのコントローラー
    [SerializeField]
    StateController state;
    //AIM後、戻るカメラの位置
    [SerializeField]
    private GameObject returnCameraPos;

    Vector3 targetPos;

    bool returnCamera;

    void Start()
    {
        //追従対象のオブジェクトのposition取得
        targetPos = targetObj.transform.position;
    }

    void Update()
    {

        SetCameraAngle();

        //エイム時である場合
        if (state.stateProcessor.State.getAimStateType() == 1)
        {
            //カメラの上下移動
            CameraVerticalRotation();
            returnCamera = true;
            //カメラの正面方向を向かせる　のちにプレイヤークラスへ記述
            targetObj.transform.forward = this.transform.forward;
            //AIMカメラに向けて徐々に近づく
            transform.position = Vector3.Lerp(transform.position, aimCamera.transform.position, 5f * Time.deltaTime);
            //カメラの回転
            transform.rotation = Quaternion.Lerp(transform.rotation, targetObj.transform.rotation, 4f * Time.deltaTime);

        }
        else
        {
            //エイム時ではなく通常カメラへ切り替えが行われていない場合
            if (returnCamera)
            {
                //切り替えのフラグをfalseへ
                returnCamera = false;
                //カメラの位置をキャラクターの後ろ側に移動させる
                transform.position = returnCameraPos.transform.position;
                //カメラの角度も通常カメラ角度へ変更
                transform.rotation = returnCameraPos.transform.rotation;
            }
        }
    }

    //デフォルトのカメラアングル
    public void SetCameraAngle()
    {
        // targetの移動量分カメラで追従　ターゲットを追いかける
        transform.position += targetObj.transform.position - targetPos;
        //戻るカメラの位置
        returnCameraPos.transform.position += targetObj.transform.position - targetPos;
        //targetの現在位置を格納
        targetPos = targetObj.transform.position;
        //カメラの回転処理
        CameraRotation();
    }

    //マウス動作に合わせたカメラの回転処理
    public void CameraRotation()
    {
        // マウスの移動量
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");
        // targetの位置のY軸を中心に回転する
        transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * sensitivityMouse);
        //戻るカメラの位置
        returnCameraPos.transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * sensitivityMouse);
        //キャラクターもカメラの視点に合わせ動かす
        targetObj.transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * sensitivityMouse);
    }

    //Aim時カメラの上下移動
    public void CameraVerticalRotation()
    {
        transform.RotateAround(targetPos, transform.right, Input.GetAxis("Mouse Y") * Time.deltaTime * -40f);
    }
}
