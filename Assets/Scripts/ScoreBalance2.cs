using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Calculates the score in percentage for the second game, and creates a graph to show the score.
/// </summary>
public class ScoreBalance2 : MonoBehaviour
{
    public Image scorePercent;
    public Text text;
    public bool showed = false;
    float percent = 0f;
    // Start is called before the first frame update
    void Start()
    {
        scorePercent.fillAmount = 0f;        

        percent = (float)DriveRailway.nrCoins / (float)DriveRailway.totalNrCoins;
    }



    // Update is called once per frame
    void Update()
    {
        if(!showed)
        {
            Debug.Log(percent);
            text.text = Mathf.Round(percent * 100) + "%";
            StartCoroutine(createDiagram());
        }
    }

    /// <summary>
    /// Creates the diagram with the score-presentage.
    /// </summary>
    /// <returns>Seconds to wait between each percentage in the diagram</returns>
    IEnumerator createDiagram()
    {
        showed = true;
        yield return new WaitForSeconds(0.1f);
        for(float i = 0f; i<percent; i+=0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            scorePercent.fillAmount = i;
        }
    }

    /// <summary>
    /// If the user press the play again button the game starts again.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("Balance 2");
    }


}
