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
    [SerializeField] private List<int> itemsAmount;//��������Ʒ������
    //[SerializeField] private List<float> score; // ÿ�صķ���

    public SaveData(int exp, int level, int scene, List<Item> packages, List<int> itemsAmount) {
        this.exp = exp;
        this.level = level;
        this.scene = scene;
        this.packages = packages;
        this.itemsAmount = itemsAmount;
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

    public List<int> GetItemsAmount() {
        return itemsAmount;
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

    public void SetItemsAmount(List<int> itemsAmount) {
        this.itemsAmount = itemsAmount;
    }

    public void SetScene(int scene) {
        this.scene = scene;
    }

    /*public void SetScore(List<float> score) {
        this.score = score;
    }*/
}
