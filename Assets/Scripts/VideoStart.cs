using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoStart : MonoBehaviour
{
    public RawImage rw;
    public VideoPlayer vp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(playVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator playVideo()
    {
        vp.Prepare();
        while(!vp.isPrepared)
        {
            yield return new WaitForSeconds(1f);
            break;
        }
        rw.texture = vp.texture;
        vp.Play();
    }
}
