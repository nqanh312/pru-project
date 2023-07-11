using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour
{
    // Start is called before the first frame update
   public void ShowPausePanel()
   {
        GameManager.instance.ShowPausePanel();
   }

   public void HidePausePanel()
   {
        GameManager.instance.HidePausePanel();
   }
}
