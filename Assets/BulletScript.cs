using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // 5�b��ɃI�u�W�F�N�g������
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnCollisionEnter(Collision other)
    {
        // �G�ɓ��������Ƃ�
        if (other.gameObject.tag == "Enemy")
        {
            // �G��HP�����炷
            Enemy.Hp -= 5;
            // �G��HP�o�[�����炷
            PlayerScript.SpeedSlider.value = (float)Enemy.Hp;
            // �G�̔�e�A�j���[�V������ON�ɂ���
            Enemy.EnemyAnimator.SetBool("isHit", true);
            // ���̃I�u�W�F�N�g������
            Destroy(this.gameObject);
        }
        else
        {
            return;
        }
    }
}