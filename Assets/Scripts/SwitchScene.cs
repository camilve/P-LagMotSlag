using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public static string loadScene = "";

    [SerializeField]
    private int level;

    public static int lvl;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GotoBalance1Scene()
    {
        loadScene = "Balance 1";
        lvl = level;
        Debug.Log(lvl);
        SceneManager.LoadScene("PauseScreen");
    }

    public void GotoBalance2Scene()
    {
        loadScene = "Balance 2";
        lvl = level;
        Debug.Log(level);
        SceneManager.LoadScene("PauseScreen");
    }

    public void GoToMenu()
    {
        loadScene = "Menu";
        SceneManager.LoadScene("Menu");
    }
}
