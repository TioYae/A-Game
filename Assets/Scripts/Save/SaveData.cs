using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData {
    [SerializeField] private int exp; // 主角经验值
    [SerializeField] private int level; // 主角经验值
    [SerializeField] private int scene; // 游戏进度
    [SerializeField] private List<Item> packages; // 背包物品
    //[SerializeField] private List<float> score; // 每关的分数

    public SaveData(int exp, int level, int scene, List<Item> packages) {
        this.exp = exp; 
        this.level = level;
        this.scene = scene; 
        this.packages = packages;
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

    public void SetScene(int scene) {
        this.scene = scene;
    }

    /*public void SetScore(List<float> score) {
        this.score = score;
    }*/
}
