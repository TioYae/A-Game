using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    [SerializeField] private int exp; // ���Ǿ���ֵ
    [SerializeField] private int level; // ���Ǿ���ֵ
    //[SerializeField] private Dictionary<int, string> package; // ������Ʒ
    [SerializeField] private int scene; // ��Ϸ����
    //[SerializeField] private List<float> score; // ÿ�صķ���

    public SaveData(int exp, int level, int scene) {
        this.exp = exp;
        this.level = level;
        this.scene = scene;
    }

    public int GetExp() {
        return exp;
    }

    public int GetLevel() {
        return level;
    }

    /*public Dictionary<int, string> GetPackage() {
        return packages;
    }*/

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

    /*public void SetPackage(Dictionary<int, string> packages) {
        this.packages = packages;
    }*/

    public void SetScene(int scene) {
        this.scene = scene;
    }

    /*public void SetScore(List<float> score) {
        this.score = score;
    }*/
}
