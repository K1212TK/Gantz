using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBone : MonoBehaviour
{
    public StateController state;
    public Animator animator;
    //キャラクターの脊椎のボーン
    [SerializeField]
    private Transform spine;
    //public Transform AimCamera;
    float xRotation;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        //エイムカメラがActiveの場合
        if (state.stateProcessor.State.getAimStateType() == 1)
        {
            RotateWaistBone();
        }

    }

    //腰のBoneを動かす
    public void RotateWaistBone()
    {
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;
        xRotation -= mouseY;

        //spine.rotation = Quaternion.Euler(spine.eulerAngles.x, spine.eulerAngles.y, spine.eulerAngles.z + Camera.main.transform.localEulerAngles.x);
        //キャラクターの腰の角度をマウス操作に合わせて動かす
        spine.rotation = Quaternion.Euler(spine.eulerAngles.x, spine.eulerAngles.y, spine.eulerAngles.z + xRotation);
        
                
    }
}
