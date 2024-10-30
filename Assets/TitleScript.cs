using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TitleScript : MonoBehaviour
{

    public TextMeshProUGUI StartText;
    public TextMeshProUGUI EndText;

    public static float FlashingInterval = 0.6f;//点滅間隔

    private Coroutine GameStartCoroutine;//ゲーム開始フォントの点滅
    private Coroutine EndCoroutine;//ゲーム終了フォントの点滅

    public static bool GameStart = true;
    public static bool End = true;
    public static bool isInput = true;
    int coolTime = 0;
    //スライミーがタイトル画面で移動するフラグ
    public static bool isStart = false;
    public static bool isEnd = false;

    // ゲーム終了時のスライム挙動関連
    public GameObject titleSlime;
    private TitleSlime Gameend;

    // Start is called before the first frame update
    void Start()
    {
        Gameend = titleSlime.GetComponent<TitleSlime>();
        FlashingInterval = 0.6f;
        isInput = true;
        isStart = false;
        isEnd = false;
    }

    void SelectInputUp(float verticalInput)
    {
        //上入力されたとき
        if (verticalInput > 0 && End && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && End && coolTime >= 30 && isInput)
        {
            GameStart = true;
            End = false;
            coolTime = 0;
        }
        else if (verticalInput > 0 && GameStart && coolTime >= 30 && isInput || Input.GetKey(KeyCode.W) && GameStart && coolTime >= 30 && isInput)
        {
            GameStart = false;
            End = true;
            coolTime = 0;
        }
    }

    void SelectInputDown(float verticalInput)
    {
        //下入力されたとき
        if (verticalInput < 0 && GameStart && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && GameStart && coolTime >= 30 && isInput)
        {
            GameStart = false;
            End = true;
            coolTime = 0;
        }
        else if (verticalInput < 0 && End && coolTime >= 30 && isInput || Input.GetKeyDown(KeyCode.S) && End && coolTime >= 30 && isInput)
        {
            GameStart = true;
            End = false;
            coolTime = 0;
        }
    }

    void Blinking()
    {
        if (GameStart)
        {

            if (GameStartCoroutine == null)
            {
                GameStartCoroutine = StartCoroutine(BlinkText(StartText));
            }
            StopBlinkCoroutine(ref EndCoroutine, EndText);
        }
        else if (End)
        {
            if (EndCoroutine == null)
            {
                EndCoroutine = StartCoroutine(BlinkText(EndText));
            }
            StopBlinkCoroutine(ref GameStartCoroutine, StartText);
        }
      
    }

    private void StopBlinkCoroutine(ref Coroutine coroutine, TextMeshProUGUI textMeshPro)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        textMeshPro.enabled = true; // テキストを再表示
    }

    void SceneChange()
    {
        //スペースを押したらシーンを変更する
        if (Input.GetKeyDown(KeyCode.Space) && GameStart || Input.GetButtonDown("Fire1") && GameStart)
        {
            isInput = false;
            FlashingInterval = 0.1f;
            isStart = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && End || Input.GetButtonDown("Fire1") && End)
        {
            isInput = false;
            FlashingInterval = 0.1f;

            Gameend.GameEnd();
        }
    }

    private IEnumerator BlinkText(TextMeshProUGUI textMeshPro)
    {
        while (true)
        {
            //テキストを非表示にする
            textMeshPro.enabled = false;
            yield return new WaitForSeconds(FlashingInterval);

            //テキストを表示する
            textMeshPro.enabled = true;
            yield return new WaitForSeconds(FlashingInterval);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (coolTime <= 120)
        {
            coolTime++;
        }

        if (isEnd)
        {
            SceneManager.LoadScene("SampleScene");
            isStart = false;
            isEnd = false;
        }

        if (TitleSlime.isExeEnd == true)
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
            #else
                    Application.Quit();//ゲームプレイ終了
            #endif
        }

        //縦の入力待ち
        float verticalInput = Input.GetAxis("Vertical");

        //上下の入力した際の処理の関数
        SelectInputUp(verticalInput);
        SelectInputDown(verticalInput);

        //文字の点滅をさせる関数
        Blinking();

        //決定をした際にシーンを変更する処理の関数
        SceneChange();

        
    }

}
