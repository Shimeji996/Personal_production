using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSlime : MonoBehaviour
{
    public GameObject Slime;
    public Animator Ani;
    public Rigidbody Rb;
    static public bool isStageChange = false;
    bool isSlimeRotate = true;
    static public bool isExeEnd = false;
    public AudioSource JumpAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(197.0f, -156.0f, 300.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (TitleScript.isStart) 
        {
            if (isSlimeRotate)
            {
                // 方向を取得して何度曲げるかの関数
                transform.RotateAround(transform.position, Vector3.up, 90);
                isSlimeRotate = false;
            }
            Rb.AddForce(transform.forward * 100);
            if(transform.position.x < -500.0f)
            {
                //次のステージに進む
                TitleScript.isEnd = true;
            }

            if (isStageChange)
            {
                transform.position = new Vector3(197.0f, -156.0f, 300.0f);
                isStageChange = false;
            }
        }

        AnimatorStateInfo stateInfo = Ani.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo stateInfo1 = Ani.GetCurrentAnimatorStateInfo(0);

        //アニメーションのステート名と一致する場合
        if (stateInfo.IsName("End") && stateInfo.normalizedTime >= 1.0f)
        {
            Ani.SetBool("isGameEnd", false);
            isExeEnd = true;
        }

        if (stateInfo1.IsName("Jump") && stateInfo1.normalizedTime >= 1.0f)
        {
            // ジャンプのアニメーションを最初に戻す
            Ani.Play("Jump", 0, 0.0f);
            // オーディオの最初に戻す
            JumpAudioSource.time = 0;
            // オーディオを鳴らす
            JumpAudioSource.Play();
        }
    }

    public void GameEnd()
    {
        Ani.SetBool("isGameEnd", true);
    }

    void AlertObservers()
    {

    }
}
