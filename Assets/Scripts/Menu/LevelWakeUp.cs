using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelWakeUp : MonoBehaviour {
    public List<Button> levels;

    // Start is called before the first frame update
    void Start() {
        Load();
    }

    // Update is called once per frame
    void Update() {

    }

    void Load() {
        var path = Path.Combine(Application.dataPath, "Savedata");
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) {
            WakeUp(1);
            return;
        }

        path = Path.Combine(path, "data.json");
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists) {
            WakeUp(1);
            return;
        }

        var str = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(str);
        int scene = saveData.GetScene();
        WakeUp(scene);
    }

    void WakeUp(int scene) {
        foreach (Button i in levels) {
            int sc;
            int.TryParse(i.name.Substring(6, 1), out sc);
            if (sc <= scene) {
                i.transform.GetChild(1).gameObject.SetActive(false);
                i.interactable = true;
            }
            else {
                i.transform.GetChild(1).gameObject.SetActive(true);
                i.interactable = false;
            }
        }
    }
}
