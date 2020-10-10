using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private RaycastHit hit;
    private const string GROUND_TAG = "Ground";

    //地面着地判定
    private bool isGround;
    public bool IsGround { 
        get => isGround; 
        private set => isGround = value; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //下方向に向けたrayがオブジェクトに衝突していたら
        //レイの始点 , レイの方向 , ヒットしたターゲット情報 , レイの長さ
        if (Physics.Raycast(new Vector3(transform.position.x, (float)(transform.position.y + 0.8), transform.position.z), Vector3.down, out hit,1))
        {
            //オブジェクトの触れたタグがgroundか判定
            if (hit.collider.tag == GROUND_TAG)
            {
                //Debug.Log("着地中");
                IsGround = true;
            }
        }
        //rayに何も衝突していない
        else
        {
            //Debug.Log("何も衝突していない");
            IsGround = false;
        }
    }

}
