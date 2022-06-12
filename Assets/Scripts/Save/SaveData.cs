using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    [SerializeField] private int exp; // ���Ǿ���ֵ
    [SerializeField] private int level; // ���Ǿ���ֵ
    [SerializeField] private int scene; // ��Ϸ����
    [SerializeField] private List<Item> packages; // ������Ʒ
    [SerializeField] private List<int> ItemsAmount ;//��������Ʒ������
    //[SerializeField] private List<float> score; // ÿ�صķ���

    public SaveData(int exp, int level, int scene, List<Item> packages , List<int> ItemsAmount) {
        this.exp = exp; 
        this.level = level;
        this.scene = scene; 
        this.packages = packages;
        this.ItemsAmount = ItemsAmount;
    }

    public int GetExp() {
        return exp;
    }

    public int GetLevel() {
        return level;
    }

    public List<Item> GetPackage() {
        return packages;
    }
    public List<int> GetAmount()
    {
        return ItemsAmount;
    }



    public int GetScene() {
        return scene;
    }

    /*public List<float> GetScore() {
        return score;
    }*/

    public void SetExp(int exp) {
        this.exp = exp;
    }

    public void SetLevel(int level) {
        this.level = level;
    }

    public void SetPackage(List<Item> packages) {
        this.packages = packages;
    }
    public void SetItemsAmount(List<int> ItemsAmount)
    {
        this.ItemsAmount = ItemsAmount;
    }

    public void SetScene(int scene) {
        this.scene = scene;
    }



    /*public void SetScore(List<float> score) {
        this.score = score;
    }*/
}
