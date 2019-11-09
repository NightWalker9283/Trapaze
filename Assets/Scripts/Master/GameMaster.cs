using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public GameMode gameMode;
   


    void Awake()
    {
        
        // 以降破棄しない
        DontDestroyOnLoad(gameObject);

    }

    
    public void GameStart()
    {
        SceneManager.LoadScene("Main");
        
    }
   

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
