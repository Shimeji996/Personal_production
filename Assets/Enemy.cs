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
    private GameObject goal; 　//②←目的地になるオブジェクトを取得するための変数

    static public Animator EnemyAnimator;
    //敵のHP
    public static int Hp = 1000;
    //自機のHPバー
    public Slider slider;

    int Count = 0;

    bool isHit = false;

    int AtkCount = 150;

    static public bool isAtk = false;

    /*private NavMeshAgent agent;　//③コンポーネント取得用の変*/

    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();　//③コンポーネントの取得
        goal = GameObject.Find("Player");　//②ここで目的地を取得
        transform.position = new Vector3(0, 0, 8);
        EnemyAnimator = GetComponent<Animator>();
        Hp = 1000;
        slider.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.position = new Vector3(Mathf.Clamp(rb.position.x, mapMin, mapMax), 0, Mathf.Clamp(rb.position.z, mapMin, mapMax));

        // プレイヤーとエネミーの座標の差が5.0fのとき
        if (Vector3.Distance(transform.position, goal.transform.position) < 5.0f)
        {
            // 移動アニメーションをOFFにする
            EnemyAnimator.SetBool("isMove", false);
        }
        // 5.0fよりも大きいとき
        else
        {
            // プレイヤーの方向に進む
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(goal.transform.position.x, 0, goal.transform.position.z),
                5f * Time.deltaTime);

            // プレイヤーの方向を向く
            transform.LookAt(goal.transform);

            // 移動アニメーションをONにする
            EnemyAnimator.SetBool("isMove", true);
        }

        // 座標がマップ上限を超えないように移動上限を設定
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapMin, mapMax), 0, Mathf.Clamp(transform.position.z, mapMin, mapMax));

        // HPが0になったら
        if (Hp <= 0)
        {
            // オブジェクトを消す
            Destroy(gameObject, 5);
        }
        // 被弾したとき
        if (isHit)
        {
            // 60フレーム経ったら
            if (Count++ >= 60)
            {
                // 被弾アニメーションをOFFにする
                EnemyAnimator.SetBool("isHit", false);
                // 被弾フラグをOFFにする
                isHit = false;
                // 被弾クールタイムカウントを0にする
                Count = 0;
            }
        }

        // 攻撃したとき
        if (isAtk)
        {
            // 200フレーム経ったら
            if (AtkCount++ >= 200)
            {
                // 攻撃アニメーションフラグをOFFにする
                EnemyAnimator.SetBool("isAttack", false);
                // 攻撃フラグをOFFにする
                isAtk = false;
                // プレイヤーの無敵を解除
                PlayerScript.isInvincible = false;
                // 攻撃クールタイムカウントを0にする
                AtkCount = 0;
            }
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        // 弾に当たったとき
        if (other.gameObject.tag == "Bullet")
        {
            // 被弾フラグをONにする
            isHit = true;
            // 被弾アニメーションをONにする
            EnemyAnimator.SetBool("isHit", true);
        }

        // プレイヤーに攻撃が当たったとき
        if (other.gameObject.name == "Player")
        {
            // 攻撃フラグがONなら
            if (isAtk)
            {
                // 無敵中は処理しない
                if (PlayerScript.isInvincible)
                {
                    return;
                }
                // プレイヤーの無敵をONにする
                PlayerScript.isInvincible = true;
                // プレイヤーのHPを減らす
                PlayerScript.Hp -= 20;
                // プレイヤーのHPバーを減らす
                slider.value = (float)PlayerScript.Hp;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 攻撃範囲にプレイヤーが入ったとき
        if (other.gameObject.name == "Player")
        {
            // 攻撃フラグがOFFなら
            if (!isAtk)
            {
                // 前回の攻撃から150フレーム経ったら
                if (AtkCount++ >= 150)
                {
                    // 攻撃アニメーションフラグをONにする
                    EnemyAnimator.SetBool("isAttack", true);
                    // 攻撃フラグをONにする
                    isAtk = true;
                    // 攻撃クールタイムカウントを0にする
                    AtkCount = 0;
                }
            }
        }
    }
}
