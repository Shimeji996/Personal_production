using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[��HP��0�ɂȂ����Ƃ�
        if (PlayerScript.Hp <= 0)
        {
            // �^�C�g���V�[���ɑJ��
            SceneManager.LoadScene("Title Scene");
            // �X�e�[�W�`�F���W�t���O��ON�ɂ���
            TitleSlime.isStageChange = true;
        }

        // �G��HP��0�ɂȂ����Ƃ�
        if (Enemy.Hp <= 0)
        {
            // �^�C�g���V�[���ɑJ��
            SceneManager.LoadScene("Title Scene");
            // �X�e�[�W�`�F���W�t���O��ON�ɂ���
            TitleSlime.isStageChange = true;
        }
    }
}
