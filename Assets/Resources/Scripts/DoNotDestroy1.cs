using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy1 : MonoBehaviour
{
    private void Awake()
    {   
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1) 
        {
            Destroy(musicObj[0]);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
