using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClosePanel : MonoBehaviour
{
    public GameObject panel;
   
    public void Panel()
    {
        if(panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
        }
    }
}
