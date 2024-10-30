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
        // プレイヤーのHPが0になったとき
        if (PlayerScript.Hp <= 0)
        {
            // タイトルシーンに遷移
            SceneManager.LoadScene("Title Scene");
            // ステージチェンジフラグをONにする
            TitleSlime.isStageChange = true;
        }

        // 敵のHPが0になったとき
        if (Enemy.Hp <= 0)
        {
            // タイトルシーンに遷移
            SceneManager.LoadScene("Title Scene");
            // ステージチェンジフラグをONにする
            TitleSlime.isStageChange = true;
        }
    }
}
