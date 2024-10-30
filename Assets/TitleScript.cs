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

    public static float FlashingInterval = 0.6f;//�_�ŊԊu

    private Coroutine GameStartCoroutine;//�Q�[���J�n�t�H���g�̓_��
    private Coroutine EndCoroutine;//�Q�[���I���t�H���g�̓_��

    public static bool GameStart = true;
    public static bool End = true;
    public static bool isInput = true;
    int coolTime = 0;
    //�X���C�~�[���^�C�g����ʂňړ�����t���O
    public static bool isStart = false;
    public static bool isEnd = false;

    // �Q�[���I�����̃X���C�������֘A
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
        //����͂��ꂽ�Ƃ�
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
        //�����͂��ꂽ�Ƃ�
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

        textMeshPro.enabled = true; // �e�L�X�g���ĕ\��
    }

    void SceneChange()
    {
        //�X�y�[�X����������V�[����ύX����
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
            //�e�L�X�g���\���ɂ���
            textMeshPro.enabled = false;
            yield return new WaitForSeconds(FlashingInterval);

            //�e�L�X�g��\������
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
                    UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
            #else
                    Application.Quit();//�Q�[���v���C�I��
            #endif
        }

        //�c�̓��͑҂�
        float verticalInput = Input.GetAxis("Vertical");

        //�㉺�̓��͂����ۂ̏����̊֐�
        SelectInputUp(verticalInput);
        SelectInputDown(verticalInput);

        //�����̓_�ł�������֐�
        Blinking();

        //����������ۂɃV�[����ύX���鏈���̊֐�
        SceneChange();

        
    }

}
