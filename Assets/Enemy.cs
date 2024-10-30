using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //public Rigidbody rb;
    float mapMax = 23.0f;
    float mapMin = -23.0f;
    private GameObject goal; �@//�A���ړI�n�ɂȂ�I�u�W�F�N�g���擾���邽�߂̕ϐ�

    static public Animator EnemyAnimator;
    //�G��HP
    public static int Hp = 1000;
    //���@��HP�o�[
    public Slider slider;

    int Count = 0;

    bool isHit = false;

    int AtkCount = 150;

    static public bool isAtk = false;

    /*private NavMeshAgent agent;�@//�B�R���|�[�l���g�擾�p�̕�*/

    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();�@//�B�R���|�[�l���g�̎擾
        goal = GameObject.Find("Player");�@//�A�����ŖړI�n���擾
        transform.position = new Vector3(0, 0, 8);
        EnemyAnimator = GetComponent<Animator>();
        Hp = 1000;
        slider.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.position = new Vector3(Mathf.Clamp(rb.position.x, mapMin, mapMax), 0, Mathf.Clamp(rb.position.z, mapMin, mapMax));

        // �v���C���[�ƃG�l�~�[�̍��W�̍���5.0f�̂Ƃ�
        if (Vector3.Distance(transform.position, goal.transform.position) < 5.0f)
        {
            // �ړ��A�j���[�V������OFF�ɂ���
            EnemyAnimator.SetBool("isMove", false);
        }
        // 5.0f�����傫���Ƃ�
        else
        {
            // �v���C���[�̕����ɐi��
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(goal.transform.position.x, 0, goal.transform.position.z),
                5f * Time.deltaTime);

            // �v���C���[�̕���������
            transform.LookAt(goal.transform);

            // �ړ��A�j���[�V������ON�ɂ���
            EnemyAnimator.SetBool("isMove", true);
        }

        // ���W���}�b�v����𒴂��Ȃ��悤�Ɉړ������ݒ�
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapMin, mapMax), 0, Mathf.Clamp(transform.position.z, mapMin, mapMax));

        // HP��0�ɂȂ�����
        if (Hp <= 0)
        {
            // �I�u�W�F�N�g������
            Destroy(gameObject, 5);
        }
        // ��e�����Ƃ�
        if (isHit)
        {
            // 60�t���[���o������
            if (Count++ >= 60)
            {
                // ��e�A�j���[�V������OFF�ɂ���
                EnemyAnimator.SetBool("isHit", false);
                // ��e�t���O��OFF�ɂ���
                isHit = false;
                // ��e�N�[���^�C���J�E���g��0�ɂ���
                Count = 0;
            }
        }

        // �U�������Ƃ�
        if (isAtk)
        {
            // 200�t���[���o������
            if (AtkCount++ >= 200)
            {
                // �U���A�j���[�V�����t���O��OFF�ɂ���
                EnemyAnimator.SetBool("isAttack", false);
                // �U���t���O��OFF�ɂ���
                isAtk = false;
                // �v���C���[�̖��G������
                PlayerScript.isInvincible = false;
                // �U���N�[���^�C���J�E���g��0�ɂ���
                AtkCount = 0;
            }
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        // �e�ɓ��������Ƃ�
        if (other.gameObject.tag == "Bullet")
        {
            // ��e�t���O��ON�ɂ���
            isHit = true;
            // ��e�A�j���[�V������ON�ɂ���
            EnemyAnimator.SetBool("isHit", true);
        }

        // �v���C���[�ɍU�������������Ƃ�
        if (other.gameObject.name == "Player")
        {
            // �U���t���O��ON�Ȃ�
            if (isAtk)
            {
                // ���G���͏������Ȃ�
                if (PlayerScript.isInvincible)
                {
                    return;
                }
                // �v���C���[�̖��G��ON�ɂ���
                PlayerScript.isInvincible = true;
                // �v���C���[��HP�����炷
                PlayerScript.Hp -= 20;
                // �v���C���[��HP�o�[�����炷
                slider.value = (float)PlayerScript.Hp;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // �U���͈͂Ƀv���C���[���������Ƃ�
        if (other.gameObject.name == "Player")
        {
            // �U���t���O��OFF�Ȃ�
            if (!isAtk)
            {
                // �O��̍U������150�t���[���o������
                if (AtkCount++ >= 150)
                {
                    // �U���A�j���[�V�����t���O��ON�ɂ���
                    EnemyAnimator.SetBool("isAttack", true);
                    // �U���t���O��ON�ɂ���
                    isAtk = true;
                    // �U���N�[���^�C���J�E���g��0�ɂ���
                    AtkCount = 0;
                }
            }
        }
    }
}
