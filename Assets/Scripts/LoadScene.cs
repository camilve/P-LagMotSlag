using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kinect = Windows.Kinect;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    private bool loadScene = false;

    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;    

    [SerializeField]
    private int scene;
    [SerializeField]
    private Text loadingText;


    // Updates once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            Debug.Log("1");
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            Debug.Log("2");
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            Debug.Log("3 " + _BodyManager);
            return;
        }


        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                // ...set the loadScene boolean to true to prevent loading a new scene more than once...
                loadScene = true;

                // ...change the instruction text to read "Loading..."
                loadingText.text = "Loading...";

                // ...and start a coroutine that will load the desired scene.
                StartCoroutine(LoadNewScene());
            }
        }

        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }

    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 2 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(2);

      
        //Uses the loadScene parameter from SwitchScene script to know which scene to load.
        SceneManager.LoadScene(SwitchScene.loadScene); 
        //DontDestroyOnLoad(BodySourceManager);    

    }

}
