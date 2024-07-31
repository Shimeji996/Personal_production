using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    public GameObject Charcter; //�L�����N�^�[�Q�[���I�u�W�F�N�g
    public Rigidbody rb;
    public Animator animator;
    float VectorX = 0.0f;
    float VectorZ = 0.0f;
    bool isFront = false;
    bool isBack = false;
    bool isLeft = false;
    bool isRight = false;
    float Speed = 5.0f;

    float length;

    float newVectorX;
    float newVectorZ;

    Vector3 tmp;

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

    private EquipmentManager m_charcterEquipmentManager;

    // ���̕����
    private void NextWeapon()
    {
        m_weaponNo++;
        if(m_weaponNo >= (int)WeaponTypes.NUM)
        {
            m_weaponNo = 0;
        }
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[m_weaponNo]);
    }

    private void PrevWeapon()
    {
        m_weaponNo--;
        if(m_weaponNo < 0)
        {
            m_weaponNo = (int)WeaponTypes.NUM - 1;
        }
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[m_weaponNo]);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_charcterEquipmentManager = Charcter.GetComponent<EquipmentManager>();

        tmp = GetComponent<Transform>().position;
    }

    void FixedUpdate()
    {

        // �㉺
        if (isFront == true)
        {
            VectorZ = 1.0f;
            animator.SetBool("isRun", true);
        }
        else if (isBack == true)
        {
            VectorZ = -1.0f;
            animator.SetBool("isRun", true);
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
        }
        else if (isRight == true)
        {
            VectorX = 1.0f;
            animator.SetBool("isRun", true);
        }
        else
        {
            VectorX = 0.0f;
        }

        // �΂�
        length = Mathf.Sqrt(VectorX * VectorX * +VectorZ * VectorZ);

        newVectorX = VectorX;
        newVectorZ = VectorZ;

        if(length != 0.0f)
        {
            newVectorX = VectorX / length;
            newVectorZ = VectorZ / length;
        }

        rb.velocity = new Vector3(newVectorX * Speed, 0, newVectorZ * Speed);

        // �ړ������Ɍ���
        float rot = newVectorX * newVectorZ * 25;
        rb.rotation = Quaternion.Euler(0, rot * 12.0f, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - m_keyInterval > 0.5f)
        {
            if(Input.GetKey(KeyCode.RightArrow) == true) 
            {
                m_keyInterval = Time.time;
                NextWeapon();
            }
            else if(Input.GetKey(KeyCode.LeftArrow) == true)
            {
                m_keyInterval = Time.time;
                PrevWeapon();
            }
        }

        if (!isRight && !isLeft && !isFront && !isBack)
        {
            animator.SetBool("isRun", false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            isFront = true;
            isBack = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isFront = false;
            isBack = true;
        }
        else
        {
            isFront = false;
            isBack = false;
        }

        if(Input.GetKey(KeyCode.A))
        {
            isLeft = true;
            isRight = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            isLeft = false;
            isRight = true;
        }
        else
        {
            isLeft = false;
            isRight = false;
        }
    }

    void AlertObservers()
    {

    }
}
