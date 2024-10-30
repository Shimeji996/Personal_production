using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // 5秒後にオブジェクトを消す
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnCollisionEnter(Collision other)
    {
        // 敵に当たったとき
        if (other.gameObject.tag == "Enemy")
        {
            // 敵のHPを減らす
            Enemy.Hp -= 5;
            // 敵のHPバーを減らす
            PlayerScript.SpeedSlider.value = (float)Enemy.Hp;
            // 敵の被弾アニメーションをONにする
            Enemy.EnemyAnimator.SetBool("isHit", true);
            // このオブジェクトを消す
            Destroy(this.gameObject);
        }
        else
        {
            return;
        }
    }
}