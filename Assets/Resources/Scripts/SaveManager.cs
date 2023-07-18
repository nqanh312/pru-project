using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{   
    public static SaveManager instance;
    //private const string SAVE_1 = "save_1";

    private void Awake() {
        // if (SaveManager.instance != null) 
        //     Debug.LogError("Only 1 SaveManage allow");
        SaveManager.instance = this;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("speed", 300f);
    }


    // public void LoadSaveSpeed() 
    // {
    //     float speed = PlayerPrefs.GetFloat("speed");
    // }

    public void SaveGameEasy()
    {
        PlayerPrefs.SetFloat("speed", 100f);
    }

        public void SaveGameNormal()
    {
        PlayerPrefs.SetFloat("speed", 300f);
    }

        public void SaveGameFast()
    {
        PlayerPrefs.SetFloat("speed", 800f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
