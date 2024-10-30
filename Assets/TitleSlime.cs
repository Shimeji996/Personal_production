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
                // �������擾���ĉ��x�Ȃ��邩�̊֐�
                transform.RotateAround(transform.position, Vector3.up, 90);
                isSlimeRotate = false;
            }
            Rb.AddForce(transform.forward * 100);
            if(transform.position.x < -500.0f)
            {
                //���̃X�e�[�W�ɐi��
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

        //�A�j���[�V�����̃X�e�[�g���ƈ�v����ꍇ
        if (stateInfo.IsName("End") && stateInfo.normalizedTime >= 1.0f)
        {
            Ani.SetBool("isGameEnd", false);
            isExeEnd = true;
        }

        if (stateInfo1.IsName("Jump") && stateInfo1.normalizedTime >= 1.0f)
        {
            // �W�����v�̃A�j���[�V�������ŏ��ɖ߂�
            Ani.Play("Jump", 0, 0.0f);
            // �I�[�f�B�I�̍ŏ��ɖ߂�
            JumpAudioSource.time = 0;
            // �I�[�f�B�I��炷
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
