using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    [SerializeField] private int exp; // 主角经验值
    [SerializeField] private int level; // 主角经验值
    //[SerializeField] private Dictionary<int, string> package; // 背包物品
    [SerializeField] private int scene; // 游戏进度
    //[SerializeField] private List<float> score; // 每关的分数

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
