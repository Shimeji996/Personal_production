using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    public GameObject Charcter; //キャラクターゲームオブジェクト
    public Rigidbody rb;
    public Animator animator;

    // 注視する敵キャラクター
    public Rigidbody enemy;

    float VectorX = 0.0f;
    float VectorZ = 0.0f;
    bool isFront = false;
    bool isBack = false;
    bool isLeft = false;
    bool isRight = false;
    float Speed = 10.0f;
    float mapMax = 23.0f;
    float mapMin = -23.0f;

    float length;

    float newVectorX;
    float newVectorZ;

    private const float RotateSpeed = 720f;

    bool isAtk = false;

    bool isFreezeRot = false;

    int Count = 0;

    int bulletTimer = 0;

    static public int Hp = 100;

    public GameObject bullet;

    public GameObject ShotPoint;

    // 無敵時間
    static public bool isInvincible;

    Quaternion tmp = Quaternion.identity;

    Vector3 dir = Vector3.zero;

    // 武器の種類
    public enum WeaponTypes : int
    {
        SWORD,//剣
        GUN,//銃
        NUM,//数
    }

    private int m_weaponNo = 0; //武器番号

    // 武器プレハブのファイルパス
    private string[] m_weaponsPath = new string[]
    {
        "MagicSword_Ice",
        "SciFiGun_Diffuse"
    };

    private float m_keyInterval = 0f;// 連続でキーが押されないように

    private EquipmentManager m_charcterEquipmentManager;

    // 次の武器へ
    private void NextWeapon()
    {
        m_weaponNo++;
        if (m_weaponNo >= (int)WeaponTypes.NUM)
        {
            m_weaponNo = 0;
        }
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[m_weaponNo]);
    }

    private void PrevWeapon()
    {
        m_weaponNo--;
        if (m_weaponNo < 0)
        {
            m_weaponNo = (int)WeaponTypes.NUM - 1;
        }
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[m_weaponNo]);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_charcterEquipmentManager = Charcter.GetComponent<EquipmentManager>();
        m_charcterEquipmentManager.EquipWeapon(m_weaponsPath[(int)WeaponTypes.SWORD]);
        ShotPoint = transform.GetChild(0).gameObject;
        Hp = 100;
    }

    void FixedUpdate()
    {

        // 上下
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

        // 左右
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

        // 斜め
        length = Mathf.Sqrt(VectorX * VectorX * VectorZ * VectorZ);

        newVectorX = VectorX;
        newVectorZ = VectorZ;

        if (length != 0.0f)
        {
            newVectorX = VectorX / (length * 1.25f);
            newVectorZ = VectorZ / (length * 1.25f);
        }

        rb.velocity = new Vector3(newVectorX * Speed, 0, newVectorZ * Speed);

        rb.position = new Vector3(Mathf.Clamp(rb.position.x, mapMin, mapMax), 0, Mathf.Clamp(rb.position.z, mapMin, mapMax));

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP" + Hp);

        if (Time.time - m_keyInterval > 0.5f)
        {
            if (Input.GetKey(KeyCode.RightArrow) == true)
            {
                m_keyInterval = Time.time;
                NextWeapon();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) == true)
            {
                m_keyInterval = Time.time;
                PrevWeapon();
            }
        }

        InputToDirection();
        
        if (m_weaponNo == (int)WeaponTypes.SWORD || m_weaponNo == (int)WeaponTypes.GUN)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isAtk = true;
                tmp = transform.rotation;
                Count = 0;
            }
        }

        if (isAtk)
        {
            if (m_weaponNo == (int)WeaponTypes.SWORD)
            {
                isFreezeRot = true;
                animator.SetBool("isRun", false);
                transform.Rotate(0, 900 * Time.deltaTime, 0, Space.World);
                if (Count++ >= 120)
                {
                    isAtk = false;
                    isFreezeRot = false;
                    transform.rotation = tmp;
                }
            }
            else if(m_weaponNo == (int)WeaponTypes.GUN)
            {
                if (bulletTimer == 0)
                {
                    // 弾発射
                    if (Input.GetKey(KeyCode.Space))
                    {

                        GameObject ball = (GameObject)Instantiate(bullet, ShotPoint.transform.position, Quaternion.identity);
                        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
                        ballRigidbody.AddForce(transform.forward * 1000);

                        // 発射したらタイマーを1にする
                        bulletTimer = 1;
                    }

                }
                else
                {
                    bulletTimer++;
                    if (bulletTimer > 20)
                    {
                        bulletTimer = 0;
                    }
                }
            }
        }
        else
        {
            Enemy.EnemyAnimator.SetBool("isHit", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if (isAtk)
            {
                Enemy.Hp -= 10;
                Enemy.EnemyAnimator.SetBool("isHit", true);
            }
        }
    }

    // プレイヤーの移動と向きの関数処理
    private void InputToDirection()
    {
        Vector3 direction = new Vector3(0f, 0f, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            isFront = true;
            isBack = false;
            direction.z += 1f;
        }
        else
        {
            isFront = false;
        }

        if (Input.GetKey(KeyCode.S))
        {
            isFront = false;
            isBack = true;
            direction.z -= 1f;
        }
        else
        {
            isBack = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            isLeft = true;
            isRight = false;
            direction.x -= 1f;
        }
        else
        {
            isLeft = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            isLeft = false;
            isRight = true;
            direction.x += 1f;
        }
        else
        {
            isRight = false;
        }

        if (!isRight && !isLeft && !isFront && !isBack)
        {
            animator.SetBool("isRun", false);
        }

        transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);
        dir = direction.normalized;


        float magnitude = dir.magnitude;

        if (!Mathf.Approximately(magnitude, 0f))
        {
            if (!isFreezeRot)
            {
                UpdateRotation(dir);
            }
        }
    }
    private void LateUpdate()
    {
    }

    private void UpdateRotation(Vector3 direction)
    {
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(direction);
        Quaternion rot = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);

        rot.x = 0.0f;
        rot.z = 0.0f;

        transform.rotation = rot;

    }

    void AlertObservers()
    {

    }

}
