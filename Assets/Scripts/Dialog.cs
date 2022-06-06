using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [Header("UI���")]
    public Text textLabel;
    public Text name;
    public GameObject DialogUI;
    
    public TextAsset[] textFile;
    private int choseFile=0;//ѡ��Ի��ı�
    private static int choseMax = 1;//�ı�����-1

    public int index;
    List<string> textList = new List<string>();

    private void Awake()
    {   
        GetTextFromFile(textFile[choseFile]);
    }

    private void OnEnable()
    {
        name.text = textList[index];
        index++;
        textLabel.text = textList[index];
        index++;
    }
    // Update is called once per frame
    void Update()      
    {
        if (DialogUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return)){
            NextBtn();
        }

        }
        
    }

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var line= file.text.Split('\n');//���ջس��и�

        foreach(var data in line)
        {
            textList.Add(data);
        }
    }

    public void NextBtn()
    {
        
        if (index == textList.Count)//�Ի��������رնԻ���
        {
            DialogUI.SetActive(false);
            index = 0;
            choseFile++;
            if (choseFile <= choseMax)
            {
                GetTextFromFile(textFile[choseFile]);
                name.text = textList[index];
                index++;
                textLabel.text = textList[index];
                index++;
            }
            return;
        }
        name.text = textList[index];
        index++;
        textLabel.text = textList[index];
        index++;
    }
}
