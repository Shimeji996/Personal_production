using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject Charcter; //�L�����N�^�[�Q�[���I�u�W�F�N�g
    public Rigidbody rb;
    public Animator animator;
    public AudioSource JumpAudioSource;

    float VectorX = 0.0f;
    float VectorZ = 0.0f;
    // �ړ��t���O
    bool isFront = false;   // �O�Ɉړ�
    bool isBack = false;    // ���Ɉړ�
    bool isLeft = false;    //�@���Ɉړ�
    bool isRight = false;   //  �E�Ɉړ�
    float Speed = 10.0f;
    float mapMax = 23.0f;
    float mapMin = -23.0f;

    float length;

    float newVectorX;
    float newVectorZ;

    private const float RotateSpeed = 720f;

    // �U���t���O
    bool isAtk = false;

    // ��]�Œ�p�t���O
    bool isFreezeRot = false;

    // ��]�U���������ԃJ�E���g
    int Count = 0;

    // �ˌ��N�[���_�E���^�C�}�[
    int bulletTimer = 0;

    // HP�֌W
    static public int Hp = 100;
    // �G��HP�o�[
    public Slider EnemySlider;
    static public Slider SpeedSlider;

    public GameObject bullet;

    public GameObject ShotPoint;

    // ���G����
    static public bool isInvincible;

    // ��]�ۊǗp�ϐ�
    Quaternion tmp = Quaternion.identity;

    Vector3 dir = Vector3.zero;

    // ����̎��
    public enum WeaponTypes : int
    {
        SWORD,//��
        GUN,//�e
        NUM,//��
    }

    private int m_weaponNo = 0; //����ԍ�

    // ����v���n�u�̃t�@�C���p�X
    private string[] m_weaponsPath = new string[]
    {
        "MagicSword_Ice",
        "SciFiGun_Diffuse"
    };

    private float m_keyInterval = 0f;// �A���ŃL�[��������Ȃ��悤��

    // ����ύX�X�N���v�g
    private EquipmentManager m_charcterEquipmentManager;

    // ���̕����
    private void NextWeapon()
    {
        m_weaponNo++;
        if (m_weaponNo >= (int)WeaponTypes.NUM)
        {
            m_weaponNo = 0;
        }
        isFreezeRot = false;
        isAtk = false;
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[m_weaponNo]);
    }

    // �O�̕����
    private void PrevWeapon()
    {
        m_weaponNo--;
        if (m_weaponNo < 0)
        {
            m_weaponNo = (int)WeaponTypes.NUM - 1;
        }
        isFreezeRot = false;
        isAtk = false;
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[m_weaponNo]);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_charcterEquipmentManager = Charcter.GetComponent<EquipmentManager>();
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[(int)WeaponTypes.SWORD]);
        ShotPoint = transform.GetChild(0).gameObject;
        Hp = 100;
        EnemySlider.value = 1000;
        SpeedSlider = EnemySlider;
    }
    void FixedUpdate()
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // �㉺
        if (isFront == true)
        {
            VectorZ = 1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // �W�����v�̃A�j���[�V�������ŏ��ɖ߂�
                animator.Play("Run", 0, 0.0f);
                // �I�[�f�B�I�̍ŏ��ɖ߂�
                JumpAudioSource.time = 0;
                // �I�[�f�B�I��炷
                JumpAudioSource.Play();
            }
        }
        else if (isBack == true)
        {
            VectorZ = -1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // �W�����v�̃A�j���[�V�������ŏ��ɖ߂�
                animator.Play("Run", 0, 0.0f);
                // �I�[�f�B�I�̍ŏ��ɖ߂�
                JumpAudioSource.time = 0;
                // �I�[�f�B�I��炷
                JumpAudioSource.Play();
            }
        }
        else
        {
            VectorZ = 0.0f;
        }

        // ���E
        if (isLeft == true)
        {
            VectorX = -1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // �W�����v�̃A�j���[�V�������ŏ��ɖ߂�
                animator.Play("Run", 0, 0.0f);
                // �I�[�f�B�I�̍ŏ��ɖ߂�
                JumpAudioSource.time = 0;
                // �I�[�f�B�I��炷
                JumpAudioSource.Play();
            }
        }
        else if (isRight == true)
        {
            VectorX = 1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // �W�����v�̃A�j���[�V�������ŏ��ɖ߂�
                animator.Play("Run", 0, 0.0f);
                // �I�[�f�B�I�̍ŏ��ɖ߂�
                JumpAudioSource.time = 0;
                // �I�[�f�B�I��炷
                JumpAudioSource.Play();
            }
        }
        else
        {
            VectorX = 0.0f;
        }

        // �΂�
        length = Mathf.Sqrt(VectorX * VectorX * VectorZ * VectorZ);

        newVectorX = VectorX;
        newVectorZ = VectorZ;

        // �΂߂Ɉړ����Ă���Ƃ�
        if (length != 0.0f)
        {
            // �ړ��x�N�g�����΂߂ɕύX
            newVectorX = VectorX / (length * 1.25f);
            newVectorZ = VectorZ / (length * 1.25f);
        }

        // �ړ��x�N�g���Ɉړ����x�萔���|�����킷
        rb.velocity = new Vector3(newVectorX * Speed, 0, newVectorZ * Speed);

        // ���W���}�b�v����𒴂��Ȃ��悤�Ɉړ������ݒ�
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, mapMin, mapMax), 0, Mathf.Clamp(rb.position.z, mapMin, mapMax));

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP" + Hp);

        if (Time.time - m_keyInterval > 0.5f)
        {
            // �E�����L�[���������Ƃ�
            if (Input.GetKey(KeyCode.RightArrow) == true)
            {
                // �L�[���A���ŉ�����Ȃ��悤�ɐݒ�
                m_keyInterval = Time.time;
                // ���̕����
                NextWeapon();
            }
            // �������L�[���������Ƃ�
            else if (Input.GetKey(KeyCode.LeftArrow) == true)
            {
                // �L�[���A���ŉ�����Ȃ��悤�ɐݒ�
                m_keyInterval = Time.time;
                // �O�̕����
                PrevWeapon();
            }
        }

        // �L�[���͊֘A�֐�
        InputToDirection();
        
        // ����������Ă���Ƃ�
        if (m_weaponNo == (int)WeaponTypes.SWORD || m_weaponNo == (int)WeaponTypes.GUN)
        {
            // �X�y�[�X�L�[���������Ƃ�
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // �U���t���O��ON�ɂ���
                isAtk = true;
                // ��]��ۊ�
                tmp = transform.rotation;
                // ��]�U���������ԃJ�E���g��0�Ƀ��Z�b�g
                Count = 0;
            }
        }

        // �U�����Ȃ�
        if (isAtk)
        {
            // ���������Ă���Ƃ�
            if (m_weaponNo == (int)WeaponTypes.SWORD)
            {
                // ��]���Œ肷��
                isFreezeRot = true;
                // �ړ��A�j���[�V�����t���O��OFF�ɂ���
                animator.SetBool("isRun", false);
                // �����𒆐S�ɉ��
                transform.Rotate(0, 900 * Time.deltaTime, 0, Space.World);

                // 120�t���[���o������
                if (Count++ >= 120)
                {
                    // �U���t���O��OFF�ɂ���
                    isAtk = false;
                    // ��]�Œ����������
                    isFreezeRot = false;
                    // ��]�ۊǗl�ϐ�����]�ɑ��
                    transform.rotation = tmp;
                }
            }
            // �e�������Ă���Ƃ�
            else if(m_weaponNo == (int)WeaponTypes.GUN)
            {
                // �ˌ��N�[���^�C����0�̂Ƃ�
                if (bulletTimer == 0)
                {
                    // �e����
                    if (Input.GetKey(KeyCode.Space))
                    {
                        // �e��F��
                        GameObject ball = (GameObject)Instantiate(bullet, ShotPoint.transform.position, Quaternion.identity);
                        // �e�����W�b�h�{�f�B��F��
                        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
                        // �e��O�����ɔ�΂�
                        ballRigidbody.AddForce(transform.forward * 1000);

                        // ���˂�����^�C�}�[��1�ɂ���
                        bulletTimer = 1;
                        isAtk = false;
                    }

                }
                // �ˌ��N�[���^�C���^�C�}�[��1���傫���Ƃ�
                else
                {
                    // �ˌ��̃N�[���^�C���^�C�}�[���J�E���g�A�b�v
                    bulletTimer++;
                    // 20�t���[���o������
                    if (bulletTimer > 20)
                    {
                        // �ˌ��̃N�[���^�C�������Z�b�g
                        bulletTimer = 0;
                    }
                }
            }
        }
        // �U��������Ȃ��Ȃ�
        else
        {
            // �G�̔�e�A�j���[�V�����t���O��OFF�ɂ���
            Enemy.EnemyAnimator.SetBool("isHit", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // �G�ɓ��������Ƃ�
        if(other.gameObject.tag == "Enemy")
        {
            // �U�����Ă���Ƃ��Ȃ�
            if (isAtk)
            {
                // �G��HP�����炷
                Enemy.Hp -= 10;
                // �G��HP�o�[�̌���
                EnemySlider.value = (float)Enemy.Hp;
                // �G�̔�e�A�j���[�V�����t���O��ON�ɂ���
                Enemy.EnemyAnimator.SetBool("isHit", true);
            }
        }
    }

    // �v���C���[�̈ړ��ƌ����̊֐�����
    private void InputToDirection()
    {
        // ��]��
        Vector3 direction = new Vector3(0f, 0f, 0f);

        // W�L�[�������Ă���Ƃ�
        if (Input.GetKey(KeyCode.W))
        {
            // �O�����t���O��ON�ɂ���
            isFront = true;
            // �������t���O��OFF�ɂ���
            isBack = false;
            // z���̉�]�ʂ�+�ɂ���
            direction.z += 1f;
        }
        // W�L�[�������Ă��Ȃ��Ƃ�
        else
        {
            // �O�����t���O��OFF�ɂ���
            isFront = false;
        }

        // S�L�[�������Ă���Ƃ�
        if (Input.GetKey(KeyCode.S))
        {
            // �O�����t���O��OFF�ɂ���
            isFront = false;
            // �������t���O��ON�ɂ���
            isBack = true;
            // z���̉�]�ʂ�-�ɂ���
            direction.z -= 1f;
        }
        // S�L�[�������Ă��Ȃ��Ƃ�
        else
        {
            // �������t���O��OFF�ɂ���
            isBack = false;
        }

        // A�L�[�������Ă���Ƃ�
        if (Input.GetKey(KeyCode.A))
        {
            // �������t���O��ON�ɂ���
            isLeft = true;
            // �E�����t���O��OFF�ɂ���
            isRight = false;
            // x���̉�]�ʂ�-�ɂ���
            direction.x -= 1f;
        }
        // A�L�[�������Ă��Ȃ��Ƃ�
        else
        {
            // �������t���O��OFF�ɂ���
            isLeft = false;
        }

        // D�L�[�������Ă���Ƃ�
        if (Input.GetKey(KeyCode.D))
        {
            // �������t���O��OFF�ɂ���
            isLeft = false;
            // �E�����t���O��ON�ɂ���
            isRight = true;
            // x���̉�]�ʂ�+�ɂ���
            direction.x += 1f;
        }
        // D�L�[�������Ă��Ȃ��Ƃ�
        else
        {
            // �E�����t���O��OFF�ɂ���
            isRight = false;
        }

        // �ړ����Ă��Ȃ��Ƃ�
        if (!isRight && !isLeft && !isFront && !isBack)
        {
            // �ړ��A�j���[�V�������~�߂�
            animator.SetBool("isRun", false);
        }

        // x��z�̉�]�ʂ�0�ɂ���
        transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);

        // ��]�ʂ𐳋K������
        dir = direction.normalized;

        // ���K�����ꂽ��]�ʂ̃x�N�g���̒���
        float magnitude = dir.magnitude;

        // �x�N�g���̒�����0����Ȃ��Ƃ�
        if (!Mathf.Approximately(magnitude, 0f))
        {
            // ��]���Œ肳��Ă��Ȃ����
            if (!isFreezeRot)
            {
                // ��]�X�V����
                UpdateRotation(dir);
            }
        }
    }
    private void LateUpdate()
    {
    }

    private void UpdateRotation(Vector3 direction)
    {
        // ���݂̌���
        Quaternion from = transform.rotation;
        // ���ꂩ���������
        Quaternion to = Quaternion.LookRotation(direction);
        // ���݂̌������炱�ꂩ������֕⊮�ړ�����
        Quaternion rot = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);

        // x��z�̉�]�ʂ�0�ɂ���
        rot.x = 0.0f;
        rot.z = 0.0f;

        // ���݂̉�]�ʂ��X�V
        transform.rotation = rot;

    }

    void AlertObservers()
    {

    }

}
