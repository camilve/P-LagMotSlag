using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    static public string loadScene = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoBalance1Scene()
    {
        loadScene = "Balance 1";
        SceneManager.LoadScene("PauseScreen");
    }

    public void GotoBalance2Scene()
    {
        loadScene = "Balance 2";
        SceneManager.LoadScene("PauseScreen");
    }

    public void GoToMenu()
    {
        loadScene = "Menu";
        SceneManager.LoadScene("Menu");
    }
}
