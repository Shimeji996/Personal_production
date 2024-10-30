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
    public GameObject Charcter; //キャラクターゲームオブジェクト
    public Rigidbody rb;
    public Animator animator;
    public AudioSource JumpAudioSource;

    float VectorX = 0.0f;
    float VectorZ = 0.0f;
    // 移動フラグ
    bool isFront = false;   // 前に移動
    bool isBack = false;    // 後ろに移動
    bool isLeft = false;    //　左に移動
    bool isRight = false;   //  右に移動
    float Speed = 10.0f;
    float mapMax = 23.0f;
    float mapMin = -23.0f;

    float length;

    float newVectorX;
    float newVectorZ;

    private const float RotateSpeed = 720f;

    // 攻撃フラグ
    bool isAtk = false;

    // 回転固定用フラグ
    bool isFreezeRot = false;

    // 回転攻撃持続時間カウント
    int Count = 0;

    // 射撃クールダウンタイマー
    int bulletTimer = 0;

    // HP関係
    static public int Hp = 100;
    // 敵のHPバー
    public Slider EnemySlider;
    static public Slider SpeedSlider;

    public GameObject bullet;

    public GameObject ShotPoint;

    // 無敵時間
    static public bool isInvincible;

    // 回転保管用変数
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

    // 武器変更スクリプト
    private EquipmentManager m_charcterEquipmentManager;

    // 次の武器へ
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

    // 前の武器へ
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

        // 上下
        if (isFront == true)
        {
            VectorZ = 1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // ジャンプのアニメーションを最初に戻す
                animator.Play("Run", 0, 0.0f);
                // オーディオの最初に戻す
                JumpAudioSource.time = 0;
                // オーディオを鳴らす
                JumpAudioSource.Play();
            }
        }
        else if (isBack == true)
        {
            VectorZ = -1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // ジャンプのアニメーションを最初に戻す
                animator.Play("Run", 0, 0.0f);
                // オーディオの最初に戻す
                JumpAudioSource.time = 0;
                // オーディオを鳴らす
                JumpAudioSource.Play();
            }
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

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // ジャンプのアニメーションを最初に戻す
                animator.Play("Run", 0, 0.0f);
                // オーディオの最初に戻す
                JumpAudioSource.time = 0;
                // オーディオを鳴らす
                JumpAudioSource.Play();
            }
        }
        else if (isRight == true)
        {
            VectorX = 1.0f;
            animator.SetBool("isRun", true);

            if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1.0f)
            {
                // ジャンプのアニメーションを最初に戻す
                animator.Play("Run", 0, 0.0f);
                // オーディオの最初に戻す
                JumpAudioSource.time = 0;
                // オーディオを鳴らす
                JumpAudioSource.Play();
            }
        }
        else
        {
            VectorX = 0.0f;
        }

        // 斜め
        length = Mathf.Sqrt(VectorX * VectorX * VectorZ * VectorZ);

        newVectorX = VectorX;
        newVectorZ = VectorZ;

        // 斜めに移動しているとき
        if (length != 0.0f)
        {
            // 移動ベクトルを斜めに変更
            newVectorX = VectorX / (length * 1.25f);
            newVectorZ = VectorZ / (length * 1.25f);
        }

        // 移動ベクトルに移動速度定数を掛け合わす
        rb.velocity = new Vector3(newVectorX * Speed, 0, newVectorZ * Speed);

        // 座標がマップ上限を超えないように移動上限を設定
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, mapMin, mapMax), 0, Mathf.Clamp(rb.position.z, mapMin, mapMax));

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP" + Hp);

        if (Time.time - m_keyInterval > 0.5f)
        {
            // 右方向キーを押したとき
            if (Input.GetKey(KeyCode.RightArrow) == true)
            {
                // キーが連続で押されないように設定
                m_keyInterval = Time.time;
                // 次の武器へ
                NextWeapon();
            }
            // 左方向キーを押したとき
            else if (Input.GetKey(KeyCode.LeftArrow) == true)
            {
                // キーが連続で押されないように設定
                m_keyInterval = Time.time;
                // 前の武器へ
                PrevWeapon();
            }
        }

        // キー入力関連関数
        InputToDirection();
        
        // 武器を持っているとき
        if (m_weaponNo == (int)WeaponTypes.SWORD || m_weaponNo == (int)WeaponTypes.GUN)
        {
            // スペースキーを押したとき
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 攻撃フラグをONにする
                isAtk = true;
                // 回転を保管
                tmp = transform.rotation;
                // 回転攻撃持続時間カウントを0にリセット
                Count = 0;
            }
        }

        // 攻撃中なら
        if (isAtk)
        {
            // 剣を持っているとき
            if (m_weaponNo == (int)WeaponTypes.SWORD)
            {
                // 回転を固定する
                isFreezeRot = true;
                // 移動アニメーションフラグをOFFにする
                animator.SetBool("isRun", false);
                // 自分を中心に回る
                transform.Rotate(0, 900 * Time.deltaTime, 0, Space.World);

                // 120フレーム経ったら
                if (Count++ >= 120)
                {
                    // 攻撃フラグをOFFにする
                    isAtk = false;
                    // 回転固定を解除する
                    isFreezeRot = false;
                    // 回転保管様変数を回転に代入
                    transform.rotation = tmp;
                }
            }
            // 銃を持っているとき
            else if(m_weaponNo == (int)WeaponTypes.GUN)
            {
                // 射撃クールタイムが0のとき
                if (bulletTimer == 0)
                {
                    // 弾発射
                    if (Input.GetKey(KeyCode.Space))
                    {
                        // 弾を認定
                        GameObject ball = (GameObject)Instantiate(bullet, ShotPoint.transform.position, Quaternion.identity);
                        // 弾をリジッドボディを認定
                        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
                        // 弾を前方向に飛ばす
                        ballRigidbody.AddForce(transform.forward * 1000);

                        // 発射したらタイマーを1にする
                        bulletTimer = 1;
                        isAtk = false;
                    }

                }
                // 射撃クールタイムタイマーが1より大きいとき
                else
                {
                    // 射撃のクールタイムタイマーをカウントアップ
                    bulletTimer++;
                    // 20フレーム経ったら
                    if (bulletTimer > 20)
                    {
                        // 射撃のクールタイムをリセット
                        bulletTimer = 0;
                    }
                }
            }
        }
        // 攻撃中じゃないなら
        else
        {
            // 敵の被弾アニメーションフラグをOFFにする
            Enemy.EnemyAnimator.SetBool("isHit", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 敵に当たったとき
        if(other.gameObject.tag == "Enemy")
        {
            // 攻撃しているときなら
            if (isAtk)
            {
                // 敵のHPを減らす
                Enemy.Hp -= 10;
                // 敵のHPバーの減少
                EnemySlider.value = (float)Enemy.Hp;
                // 敵の被弾アニメーションフラグをONにする
                Enemy.EnemyAnimator.SetBool("isHit", true);
            }
        }
    }

    // プレイヤーの移動と向きの関数処理
    private void InputToDirection()
    {
        // 回転量
        Vector3 direction = new Vector3(0f, 0f, 0f);

        // Wキーを押しているとき
        if (Input.GetKey(KeyCode.W))
        {
            // 前方向フラグをONにする
            isFront = true;
            // 後ろ方向フラグをOFFにする
            isBack = false;
            // z軸の回転量を+にする
            direction.z += 1f;
        }
        // Wキーを押していないとき
        else
        {
            // 前方向フラグをOFFにする
            isFront = false;
        }

        // Sキーを押しているとき
        if (Input.GetKey(KeyCode.S))
        {
            // 前方向フラグをOFFにする
            isFront = false;
            // 後ろ方向フラグをONにする
            isBack = true;
            // z軸の回転量を-にする
            direction.z -= 1f;
        }
        // Sキーを押していないとき
        else
        {
            // 後ろ方向フラグをOFFにする
            isBack = false;
        }

        // Aキーを押しているとき
        if (Input.GetKey(KeyCode.A))
        {
            // 左方向フラグをONにする
            isLeft = true;
            // 右方向フラグをOFFにする
            isRight = false;
            // x軸の回転量を-にする
            direction.x -= 1f;
        }
        // Aキーを押していないとき
        else
        {
            // 左方向フラグをOFFにする
            isLeft = false;
        }

        // Dキーを押しているとき
        if (Input.GetKey(KeyCode.D))
        {
            // 左方向フラグをOFFにする
            isLeft = false;
            // 右方向フラグをONにする
            isRight = true;
            // x軸の回転量を+にする
            direction.x += 1f;
        }
        // Dキーを押していないとき
        else
        {
            // 右方向フラグをOFFにする
            isRight = false;
        }

        // 移動していないとき
        if (!isRight && !isLeft && !isFront && !isBack)
        {
            // 移動アニメーションを止める
            animator.SetBool("isRun", false);
        }

        // xとzの回転量を0にする
        transform.rotation = new Quaternion(0.0f, transform.rotation.y, 0.0f, transform.rotation.w);

        // 回転量を正規化する
        dir = direction.normalized;

        // 正規化された回転量のベクトルの長さ
        float magnitude = dir.magnitude;

        // ベクトルの長さが0じゃないとき
        if (!Mathf.Approximately(magnitude, 0f))
        {
            // 回転が固定されていなければ
            if (!isFreezeRot)
            {
                // 回転更新処理
                UpdateRotation(dir);
            }
        }
    }
    private void LateUpdate()
    {
    }

    private void UpdateRotation(Vector3 direction)
    {
        // 現在の向き
        Quaternion from = transform.rotation;
        // これから向く方向
        Quaternion to = Quaternion.LookRotation(direction);
        // 現在の向きからこれから向くへ補完移動する
        Quaternion rot = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);

        // xとzの回転量を0にする
        rot.x = 0.0f;
        rot.z = 0.0f;

        // 現在の回転量を更新
        transform.rotation = rot;

    }

    void AlertObservers()
    {

    }

}
