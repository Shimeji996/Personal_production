using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //public Rigidbody rb;
    float mapMax = 23.0f;
    float mapMin = -23.0f;
    private GameObject goal; 　//②←目的地になるオブジェクトを取得するための変数

    static public Animator EnemyAnimator;

    public static int Hp = 1000;

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
    }

    // Update is called once per frame
    void Update()
    {
        //rb.position = new Vector3(Mathf.Clamp(rb.position.x, mapMin, mapMax), 0, Mathf.Clamp(rb.position.z, mapMin, mapMax));

        if (Vector3.Distance(transform.position, goal.transform.position) < 5.0f)
        {
            EnemyAnimator.SetBool("isMove", false);
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(goal.transform.position.x, 0, goal.transform.position.z),
                8f * Time.deltaTime);

            transform.LookAt(goal.transform);

            EnemyAnimator.SetBool("isMove", true);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapMin, mapMax), 0, Mathf.Clamp(transform.position.z, mapMin, mapMax));

        if (Hp <= 0)
        {
            Destroy(gameObject, 5);
        }
        if (isHit)
        {
            if (Count++ >= 60)
            {
                EnemyAnimator.SetBool("isHit", false);
                isHit = false;
                Count = 0;
            }
        }

        if (isAtk)
        {
            if (AtkCount++ >= 200)
            {
                EnemyAnimator.SetBool("isAttack", false);
                isAtk = false;
                PlayerScript.isInvincible = false;
                AtkCount = 0;
            }
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            isHit = true;
            EnemyAnimator.SetBool("isHit", true);
        }

        if (other.gameObject.name == "Player")
        {
            if (isAtk)
            {
                // 無敵中は処理しない
                if (PlayerScript.isInvincible)
                {
                    return;
                }

                PlayerScript.isInvincible = true;
                PlayerScript.Hp -= 20;
                
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (!isAtk)
            {
                if (AtkCount++ >= 150)
                {
                    EnemyAnimator.SetBool("isAttack", true);
                    isAtk = true;
                    AtkCount = 0;
                }
            }
        }
    }
}
