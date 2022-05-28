using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData : MonoBehaviour {
    [SerializeField] private float blood; // 主角血量
    //[SerializeField] private float exp; // 主角经验值
    //[SerializeField] private Dictionary<int, string> package; // 背包物品
    [SerializeField] private int scene; // 游戏进度
    [SerializeField] private List<float> score; // 每关的分数

    public float GetBlood() {
        return blood;
    }

    /*public float GetExp() {
        return exp;
    }

    public Dictionary<int, string> GetPackage() {
        return packages;
    }*/

    public int GetScene() {
        return scene;
    }

    public List<float> GetScore() {
        return score;
    }

    public void SetBlood(float blood) {
        this.blood = blood;
    }

    /*public void SetExp(float exp) {
        this.exp = exp;
    }

    public void SetPackage(Dictionary<int, string> packages) {
        this.packages = packages;
    }*/

    public void SetScene(int scene) {
        this.scene = scene;
    }

    public void SetScore(List<float> score) {
        this.score = score;
    }
}
