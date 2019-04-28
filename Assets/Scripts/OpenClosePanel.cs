using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Open or closes the panel where you can chose level in the menu
/// </summary>
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
