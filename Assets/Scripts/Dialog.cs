using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
    [Header("UI组件")]
    public Text textLabel;
    public Text personName;
    public GameObject DialogUI;

    public TextAsset[] textFile;
    private int choseFile = 0;//选择对话文本
    private static int choseMax = 1;//文本数量-1

    public int index;
    List<string> textList = new List<string>();

    private void Awake() {
        GetTextFromFile(textFile[choseFile]);
    }

    private void OnEnable() {
        personName.text = textList[index];
        index++;
        textLabel.text = textList[index];
        index++;
    }
    // Update is called once per frame
    void Update() {
        if (DialogUI.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) {
                NextBtn();
            }

        }
    }

    void GetTextFromFile(TextAsset file) {
        textList.Clear();
        index = 0;

        var line = file.text.Split('\n');//按照回车切割

        foreach (var data in line) {
            textList.Add(data);
        }
    }

    public void NextBtn() {

        if (index == textList.Count)//对话结束，关闭对话窗
        {
            DialogUI.SetActive(false);
            index = 0;
            choseFile++;
            if (choseFile <= choseMax) {
                GetTextFromFile(textFile[choseFile]);
                personName.text = textList[index];
                index++;
                textLabel.text = textList[index];
                index++;
            }
            return;
        }
        personName.text = textList[index];
        index++;
        textLabel.text = textList[index];
        index++;
    }
}
