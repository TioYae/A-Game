using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public GameObject Settings;
    public GameObject Tips;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void BackToMenu() {
        SceneManager.LoadScene(0);
    }

    // 开始游戏
    public void Next() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // 退出游戏
    public void Exit() {
        Application.Quit();
    }

    // 读取进度
    public void Load() {
        var path = Path.Combine(Application.dataPath, "Savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        SceneManager.LoadScene(saveData.GetScene());
    }

    // 选择关卡
    public void Choose(int scene) {
        SceneManager.LoadScene(scene);
    }

    // 设置
    public void Setting() {
        if (Settings.activeSelf) Settings.SetActive(false);
        else Settings.SetActive(true);
    }

    // 删除存档
    public void DeleteData() {
        var path = Path.Combine(Application.dataPath, "Savedata/data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
            fileInfo.Delete();
        Tips.SetActive(true);
        Invoke(nameof(TipsFin), 5f);
    }

    // 提示完毕
    public void TipsFin() {
        Tips.SetActive(false);
    }
}
