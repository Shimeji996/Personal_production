using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy.Hp -= 5;
            PlayerScript.SpeedSlider.value = (float)Enemy.Hp;
            Enemy.EnemyAnimator.SetBool("isHit", true);
            Destroy(this.gameObject);
        }
        else
        {
            return;
        }
    }
}