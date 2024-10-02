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
        if (PlayerScript.Hp <= 0)
        {
            SceneManager.LoadScene("Title Scene");
        }

        if(Enemy.Hp <= 0)
        {
            SceneManager.LoadScene("Title Scene");
        }
    }
}
