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
    [Space]
    public GameObject PauseMenu;
    public Inventory mybag;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // �������˵�
    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // ��ʼ��Ϸ
    public void Next() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // ���¿�ʼ��Ϸ
    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // �˳���Ϸ
    public void Exit() {
        Application.Quit();
    }

    // ��ȡ����
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

    // ѡ��ؿ�
    public void Choose(int scene) {
        SceneManager.LoadScene(scene);
    }

    // ����
    public void Setting() {
        if (Settings.activeSelf) Settings.SetActive(false);
        else Settings.SetActive(true);
    }

    // ɾ���浵
    public void DeleteData() {
        var path = Path.Combine(Application.dataPath, "Savedata/data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
            fileInfo.Delete();
        Tips.SetActive(true);
        Invoke(nameof(TipsFin), 5f);
        mybag.itemList = new List<Item>(new Item[18]);
        
    }

    // ��ʾ���
    public void TipsFin() {
        Tips.SetActive(false);
    }

    // ��Ϸ��ͣ
    public void Pause() {
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    // ������Ϸ
    public void UnPause() {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
