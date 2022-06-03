using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test_Menu : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void BackToMenu() {
        SceneManager.LoadScene(0);
    }

    public void Next() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit() {
        Application.Quit();
    }

    public void Load() {
        var path = Path.Combine(Application.dataPath, "savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 

        path = Path.Combine(path, "data.json"); 
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        SceneManager.LoadScene(saveData.GetScene());
    }
}
