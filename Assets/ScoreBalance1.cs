using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBalance1 : MonoBehaviour
{
    public Image scorePercent;
    public Text text;
    public bool showed = false;
    float percent = 0f;
    // Start is called before the first frame update
    void Start()
    {
        scorePercent.fillAmount = 0f;

        percent = (float)MovementBoxes.score / (float)Balance1Script.totalBoxes;
    }

    // Update is called once per frame
    void Update()
    {
        if (!showed)
        {
            Debug.Log(percent);
            text.text = Mathf.Round(percent * 100) + "%";
            StartCoroutine(createDiagram());
        }
    }
    IEnumerator createDiagram()
    {
        showed = true;
        yield return new WaitForSeconds(0.1f);
        for (float i = 0f; i < percent; i += 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            scorePercent.fillAmount = i;
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene("Balance 1");
    }

}
