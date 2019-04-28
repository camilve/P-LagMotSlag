using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Switch to another scene
/// </summary>
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

    /// <summary>
    /// Start the first game by first going to the loading scene.
    /// </summary>
    public void GotoBalance1Scene()
    {
        loadScene = "Balance 1";
        lvl = level;
        Debug.Log(lvl);
        SceneManager.LoadScene("PauseScreen");
    }

    /// <summary>
    /// Start the second game by first going to the loading scene.
    /// </summary>
    public void GotoBalance2Scene()
    {
        loadScene = "Balance 2";
        lvl = level;
        Debug.Log(level);
        SceneManager.LoadScene("PauseScreen");
    }

    /// <summary>
    /// Goes to the menu.
    /// </summary>
    public void GoToMenu()
    {
        FindObjectOfType<AudioManager>().Stop("Balance1");
        FindObjectOfType<AudioManager>().Stop("Balance2");
        FindObjectOfType<AudioManager>().Play("Theme");
        loadScene = "Menu";
        SceneManager.LoadScene("Menu");
    }
}
