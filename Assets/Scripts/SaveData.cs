using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData : MonoBehaviour
{
    [SerializeField] private float blood; // ����Ѫ��
    //[SerializeField] private float exp; // ���Ǿ���ֵ
    //[SerializeField] private Dictionary<int, string> package; // ������Ʒ
    [SerializeField] private int scene; // ��Ϸ����

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
}